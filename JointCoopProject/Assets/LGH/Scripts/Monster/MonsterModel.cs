using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterModel : MonoBehaviour
{
    [field : SerializeField] public float _moveSpd { get; set; }
    [field : SerializeField] public int _attack1Damage { get; set; }
    [field: SerializeField] public float _attackRange { get; set; }
    [field : SerializeField] public int _maxHP { get; set; }

    [field: SerializeField] public ObservableProperty<int> _curHP { get; private set; } = new();
}
