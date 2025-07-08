using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDamagable
{
    [Header("Player Weapon Status")]
    [SerializeField] GameObject _swordPrefab;
    [SerializeField] float _swordAttackDelay = 1f;    // ���� ���� ������

    [Header("Player KnockBack & Invincible")]
    [SerializeField] public float _hitCoolTime = 0.5f;
    [SerializeField] float _knockBackForce = 3f;
    [SerializeField] float _knockBackTime = 0.2f;

    [Header("skill Manager Reference")]
    [SerializeField] PlayerSkillManager _skillManager;

    SpriteRenderer _PlayerSprite;
    Rigidbody2D _playerRigid;
    Animator _playerAnimator;
    CapsuleCollider2D _capsuleCollider;

    public Vector2 _moveInput;
    public Vector2 _attackDirection;
    Vector2 _targetVelocity;
    Vector2 _curVelocity;
    Vector2 _dashDirection;

    float _wieldTimer;
    bool _isDash = false;
    bool _isDamaged = false;
    bool _isKnockBack = false;
    float _dashProgressTime;
    float _dashCoolTime;
    string _selectAttackDir;

    readonly int DOWN_IDLE_HASH = Animator.StringToHash("Player_Down_Idle");
    readonly int DOWN_ATTACK_HASH = Animator.StringToHash("Player_Down_Attack");
    readonly int UP_ATTACK_HASH = Animator.StringToHash("Player_Up_Attack");
    readonly int LEFT_ATTACK_HASH = Animator.StringToHash("Player_Left_Attack");
    readonly int RIGHT_ATTACK_HASH = Animator.StringToHash("Player_Right_Attack");

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (PlayerStatManager.Instance._alive)
        {
            MoveInput();

            if (_isDash && PlayerStatManager.Instance._canDash)
            {
                MoveDash();
            }
            else
            {
                Movement();
                ShotInput();
                DashInput();
                Attack();
            }
        }
        else
        {
            PlayerDeath();
        }
    }

    // First Initialize
    private void Init()
    {
        _playerRigid = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _PlayerSprite = GetComponent<SpriteRenderer>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Player Move Input
    private void MoveInput()
    {
        if (_isKnockBack) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _moveInput = new Vector2(horizontal, vertical).normalized;
    }

    // Player Movement
    private void Movement()
    {
        if (_isKnockBack) return;

        _targetVelocity = _moveInput * PlayerStatManager.Instance._moveSpeed;

        // Acceleration & Deceleration Speed Choice
        float moveSpeed = (_targetVelocity.magnitude > _curVelocity.magnitude) ? PlayerStatManager.Instance._accelerationSpeed : PlayerStatManager.Instance._decelerationSpeed;
        _curVelocity = Vector2.MoveTowards(_curVelocity, _targetVelocity, moveSpeed * Time.deltaTime);
        transform.position += (Vector3)_curVelocity * Time.deltaTime;

        Vector2 roundedMove = new Vector2(
            Mathf.Abs(_moveInput.x) < 0.1f ? 0 : Mathf.Sign(_moveInput.x),
            Mathf.Abs(_moveInput.y) < 0.1f ? 0 : Mathf.Sign(_moveInput.y)
        );

        if (_curVelocity.magnitude < 0.01f && _moveInput == Vector2.zero)
        {
            _curVelocity = Vector2.zero;
        }

        if (_moveInput != Vector2.zero)
        {
            _playerAnimator.SetBool("IsMoving", true);
            _playerAnimator.SetBool("IsDeath", false);
            _playerAnimator.SetFloat("MoveX", roundedMove.x);
            _playerAnimator.SetFloat("MoveY", roundedMove.y);
        }
        else
        {
            _playerAnimator.SetBool("IsMoving", false);
            _playerAnimator.SetBool("IsDeath", false);
        }
    }

    // Player Dash Input
    private void DashInput()
    {
        if (_isKnockBack) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_moveInput != Vector2.zero && _dashCoolTime <= 0f)
            {
                _isDash = true;
                _dashProgressTime = PlayerStatManager.Instance._dashDurationTime;
                _dashCoolTime = PlayerStatManager.Instance._dashCoolTime;
                _dashDirection = _moveInput;
            }
        }
        _dashCoolTime -= Time.deltaTime;
    }

    // Player Move Dash
    private void MoveDash()
    {
        if (_isKnockBack) return;

        gameObject.layer = 8;   // Player Layer Change
        transform.position += (Vector3)_dashDirection * PlayerStatManager.Instance._dashSpeed * Time.deltaTime;
        _dashProgressTime -= Time.deltaTime;
        _PlayerSprite.color = new Color(0.5f, 0.5f, 0.5f, 0.7f);

        if (_dashProgressTime <= 0f)
        {
            gameObject.layer = 6;   // Player Layer Change
            _isDash = false;
            _PlayerSprite.color = new Color(1, 1, 1, 1);
            
            TempManager.inventory._activeSkillData.ReleaseSkill(transform);
        }
    }

    // Player Shoot Input
    private void ShotInput()
    {
        // Direction Zero Setting
        _attackDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            _attackDirection += Vector2.up;
            _playerAnimator.speed = PlayerStatManager.Instance._attackSpeed;
            _selectAttackDir = "Up";
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _playerAnimator.speed = PlayerStatManager.Instance._attackSpeed;
            _attackDirection += Vector2.down;
            _selectAttackDir = "Down";
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _playerAnimator.speed = PlayerStatManager.Instance._attackSpeed;
            _attackDirection += Vector2.left;
            _selectAttackDir = "Left";
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _playerAnimator.speed = PlayerStatManager.Instance._attackSpeed;
            _attackDirection += Vector2.right;
            _selectAttackDir = "Right";
        }

        _attackDirection = _attackDirection.normalized;
    }

    private void Attack()
    {
        if (_isKnockBack) return;

        if (_attackDirection != Vector2.zero && _wieldTimer <= 0f)
        {
            SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_PlayerAttack);
            GameObject sword = Instantiate(_swordPrefab, transform.position, Quaternion.identity);
            // ���� ������ �ʱ�ȭ
            float calcNextDamage = CalcNextDamage();
            sword.GetComponent<PlayerSwordController>().Init(transform, _attackDirection, PlayerStatManager.Instance._attackSpeed, (int)calcNextDamage, _skillManager, PlayerStatManager.Instance._attackRange);

            // Attack Animation
            AttackDirection(_selectAttackDir);

            // ���ݼӵ��� ����Ͽ� ���� ���� ��Ÿ�� ���
            _wieldTimer = _swordAttackDelay / PlayerStatManager.Instance._attackSpeed;
        }
        _wieldTimer -= Time.deltaTime;

        // �÷��̾� ���� �ִϸ��̼� �ӵ� ����
        _playerAnimator.speed = 1f;
    }

    private void AttackDirection(string attackDir)
    {
        switch (attackDir)
        {
            case "Up":
                _playerAnimator.Play(UP_ATTACK_HASH);
                _skillManager.SwordEnergyAttack();
                break;
            case "Down":
                _playerAnimator.Play(DOWN_ATTACK_HASH);
                _skillManager.SwordEnergyAttack();
                break;
            case "Left":
                _playerAnimator.Play(LEFT_ATTACK_HASH);
                _skillManager.SwordEnergyAttack();
                break;
            case "Right":
                _playerAnimator.Play(RIGHT_ATTACK_HASH);
                _skillManager.SwordEnergyAttack();
                break;
            default:
                break;
        }
    }

    public void TakeDamage(int damage, Vector2 targetPos)
    {
        if (_isDamaged)
        {
            return;
        }

        _isDamaged = true;

        // Player Shield Discount
        int leftDamage = TakeDamageOnShield(damage);
        // Player Hp Down
        HealthDown(leftDamage);

        // Player hit Reaction
        _PlayerSprite.color = new Color(1, 0.4f, 0.1f, 0.7f);

        // Player Layer Change
        gameObject.layer = 8;

        // Player Hit KnockBack
        PlayerKnockBack(targetPos);
        Invoke("OffDamage", _hitCoolTime);
    }

    private void OffDamage()
    {
        // Player Layer Change
        gameObject.layer = 6;
        _isDamaged = false;
        _PlayerSprite.color = new Color(1, 1, 1, 1);
    }

    // Player Hp Down
    public void HealthDown(int damage)
    {
        // TODO : �÷��̾� �ǰ� ����
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_PlayerDamage);

        if (PlayerStatManager.Instance._playerHp > 1)
        {
            PlayerStatManager.Instance._playerHp -= damage;
            
        }
        else
        {
            Debug.Log("�÷��̾ ����߽��ϴ�.");
            PlayerStatManager.Instance._playerHp = 0;

            // Player Death
            SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Title);
            PlayerStatManager.Instance._alive = false;
        }
    }

    // Player Death
    public void PlayerDeath()
    {
        _playerAnimator.SetBool("IsDeath", true);
        Invoke("OnDeathUI", 3f);

    }

    private void OnDeathUI()
    {
        PlayerStatManager.Instance._alive = true;
        UIManager.Instance.OpenUi(UIKeyList.deathWindow);
        _playerAnimator.Play(DOWN_IDLE_HASH);
        CancelInvoke();
    }

    // Player KnockBack
    public void PlayerKnockBack(Vector2 targetPos)
    {
        _isKnockBack = true;
        Vector2 hitDirection = ((Vector2)transform.position - targetPos).normalized;
        _playerRigid.velocity = Vector2.zero;
        _playerRigid.AddForce(hitDirection * _knockBackForce, ForceMode2D.Impulse);

        Invoke("StopKnockBack", _knockBackTime);
    }

    private void StopKnockBack()
    {
        _playerRigid.velocity = Vector2.zero;
        _curVelocity = Vector2.zero;
        _isKnockBack = false;
    }

    private int TakeDamageOnShield(int damage)
    {
        float leftover = 0;
        if (PlayerStatManager.Instance._shield > 0)
        {
            PlayerStatManager.Instance._shield -= damage;

            if (PlayerStatManager.Instance._shield <= 0)
            {
                leftover = Mathf.Abs(PlayerStatManager.Instance._shield);
                PlayerStatManager.Instance._shield = 0;
                TempManager.inventory._activeSkillData.ReleaseSkill(transform);
            }
        }
        else
        {
            leftover = damage;
        }

        return (int)leftover;
    }

    private int CalcNextDamage()
    {
        int returnValue = PlayerStatManager.Instance._attackDamage;
        if (PlayerStatManager.Instance._attackBonus != 0)
        {
            returnValue += (int)PlayerStatManager.Instance._attackBonus;
            PlayerStatManager.Instance._attackBonus = 0;
        }
        return returnValue;
    }
}
