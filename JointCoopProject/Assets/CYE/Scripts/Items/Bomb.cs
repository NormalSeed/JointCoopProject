using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private ItemDataSO _itemData;
    [SerializeField] private float _explodeDelay;
    [SerializeField] private float _explodeRange;
    [SerializeField] private GameObject _explosive;
    private Coroutine _explodeRoutine;
    private Animator _animator;
    private float _explodeTerm = 0.5f;
    void Awake()
    {
        _animator = GetComponent<Animator>();
        gameObject.GetComponent<SpriteRenderer>().sprite = _itemData._itemIcon;
        _explosive.GetComponent<CircleCollider2D>().radius = _explodeRange;
    }

    void OnEnable()
    {
        if (_explodeRoutine == null)
        { 
            _explodeRoutine = StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        _animator.SetTrigger("IsActivate");
        yield return new WaitForSeconds(_explodeDelay);

        _animator.SetTrigger("IsExplode");
        _explosive.SetActive(true);
        yield return new WaitForSeconds(_explodeTerm);

        if (_explodeRoutine != null)
        {
            StopCoroutine(_explodeRoutine);
            _explodeRoutine = null;
        }
        Destroy(gameObject);
    }
}
