using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterModel : MonoBehaviour
{
    [field : SerializeField] public float _moveSpd { get; set; }
    [field : SerializeField] public float _attack { get; set; }
    [field: SerializeField] public float _attackRange { get; set; }
    [field : SerializeField] public float _maxHP { get; set; }

    [field: SerializeField] public ObservableProperty<float> _curHP { get; private set; } = new();
}
