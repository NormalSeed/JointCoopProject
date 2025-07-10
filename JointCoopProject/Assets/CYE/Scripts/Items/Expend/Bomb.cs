using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomb : Item, IPickable, IInstallable
{
    [Header("Item Data")]
    [SerializeField]
    private float _explosiveDelay;
    [SerializeField]
    private int explosiveDamage;
    public int _explosiveDamage { get { return explosiveDamage; } }

    [SerializeField]
    private GameObject _explosionFx;
    

    public bool _isSetUp;

    private Coroutine _explodeRoutine = null;

    private Animator _animator;

    private bool _isExplode;

    private LayerMask _playerLayer;


    #region // Unity Message Function
    void Awake()
    {
        Init();
    }
    void Update()
    {
        if (_isSetUp && _explodeRoutine == null)
        {
            _explodeRoutine = StartCoroutine(Explode());
        }
    }
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
    //     {
    //         PickUp(collision.transform);
    //     }
    // }
    #endregion

    #region // Item Class
    protected override void Init()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = _itemData._itemIcon;
        _spriteRenderer.color = Color.white;

        _animator = transform.GetChild(0).GetComponent<Animator>();

        _playerLayer = LayerMask.NameToLayer("Player");
    }
    #endregion

    #region // IPickable
    public void PickUp(Transform pickupPos)
    {
        if (!_isSetUp)
        {
            ItemManager.inventory.GetBomb(1);
            Destroy(gameObject);
        }
    }
    public void Drop(Transform dropPos)
    {
        GameObject itemObject = Instantiate(gameObject, dropPos.position, Quaternion.identity);
        itemObject.GetComponent<Rigidbody2D>().AddForce(0.5f * transform.forward);
    }
    #endregion

    #region // IInstallable
    public void Install(Transform setUpPos)
    {
        GameObject setUpBomb = Instantiate(gameObject, setUpPos.position, Quaternion.identity);
        setUpBomb.GetComponent<Bomb>()._isSetUp = true;
    }
    #endregion

    private IEnumerator Explode()
    {
        _isExplode = false;
        _animator.SetBool("IsSetUp", true);
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(_explosiveDelay);

        _isExplode = true;
        _animator.SetBool("IsExplode", _isExplode);
        _spriteRenderer.enabled = true;
        _explosionFx.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        _isExplode = false;
        _explosionFx.SetActive(false);
        if (_explodeRoutine != null)
        {
            StopCoroutine(_explodeRoutine);
            _explodeRoutine = null;
        }
        Destroy(gameObject);
    }

    private void TakeDamage(IDamagable target, LayerMask targetLayer)
    {
        int realDamage = (targetLayer == _playerLayer) ? 2 : (int)_explosiveDamage;
        target?.TakeDamage(realDamage, transform.position);
    }
}
