using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MonsterModel : MonoBehaviour
{
    [field: SerializeField] public int _maxHP { get; set; }
    [field: SerializeField] public string _name { get; set; }
    [field: SerializeField] public int _bodyDamage { get; set; }
    [field : SerializeField] public int _attack1Damage { get; set; }
    [field : SerializeField] public int _attack2Damage { get; set; }
    [field : SerializeField] public int _attack3Damage { get; set; }
    [field: SerializeField] public float _attack1Range { get; set; }
    [field: SerializeField] public float _attack2Range { get; set; }
    [field: SerializeField] public float _attack3Range { get; set; }
    
    [field: SerializeField] public float _moveSpd { get; set; }

    [field: SerializeField] public ObservableProperty<int> _curHP { get; private set; } = new();

    public void ApplyData(MonsterData data)
    {
        _maxHP = data._maxHP;
        _name = data._name;
        _bodyDamage = data._bodyDamage;
        _attack1Damage = data._attack1Damage;
        _attack2Damage = data._attack2Damage;
        _attack3Damage = data._attack3Damage;
        _attack1Range = data._attack1Range;
        _attack2Range = data._attack2Range;
        _attack3Range = data._attack3Range;
        _moveSpd = data._moveSpd;
    }
}
