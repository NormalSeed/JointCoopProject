using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterView : MonoBehaviour
{
    public Animator _animator;

    private void Awake() => Init();
    private void Init()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimation(int hash)
    {
        _animator.Rebind();
        _animator.Update(0f);
        _animator.Play(hash, 0, 0f);
    }
}
