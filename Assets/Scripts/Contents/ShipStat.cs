using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShipMainSystem
{
    [SerializeField]
    protected int _shields;
    [SerializeField]
    protected int _engines;
    [SerializeField]
    protected int _oxygen;
    [SerializeField]
    protected int _weapons;
    [SerializeField]
    protected int _medbay;

    public int Shields { get { return _shields; } set { _shields = value; } }
    public int Engines { get { return _engines; } set { _engines = value; } }
    public int Oxygen { get { return _oxygen; } set { _oxygen = value; } }
    public int Weapons { get { return _weapons; } set { _weapons = value; } }
    public int Medbay { get { return _medbay; } set { _medbay = value; } }
}

[Serializable]
public class ShipSubSystem
{
    [SerializeField]
    protected int _piloting;
    [SerializeField]
    protected int _sensors;
    [SerializeField]
    protected int _doors;


    public int Piloting { get { return _piloting; } set { _piloting = value; } }
    public int Sensors { get { return _sensors; } set { _sensors = value; } }
    public int Doors { get { return _doors; } set { _doors = value; } }
}

public class ShipStat : MonoBehaviour
{
    [SerializeField]
    protected int _shipCode;
    [SerializeField]
    protected int _reactor;
    [SerializeField]
    protected int _power;
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _maxHp;
    [SerializeField]
    protected ShipMainSystem _shipMainSystem;
    [SerializeField]
    protected ShipSubSystem _shipSubSystem;
    [SerializeField]
    protected float _evade;

    protected int _powerusage = 0;

    public int ShipCode { get { return _shipCode; } set { _shipCode = value; } }
    public int Reactor { get { return _reactor; } set { _reactor = value; } }
    public int power { get { return _power; } set { _power = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public ShipMainSystem ShipMain { get { return _shipMainSystem; } set { _shipMainSystem = value; } }
    public ShipSubSystem ShipSub { get { return _shipSubSystem; } set { _shipSubSystem = value; } }
    public float Evade { get { return _evade; } set { _evade = value; } }
    public int PowerUsage { get { return _powerusage; } set { _powerusage = value; } }

    private void Start()
    {
        _shipCode = 2;
        SetStat(_shipCode);
        _evade = 15.0f;
    }

    public virtual void SetStat(int shipCode)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        Data.Stat stat = dict[shipCode];
        _reactor = stat.reactor;
        _hp = stat.hp;
        _maxHp = stat.maxHp;
        _shipSubSystem.Piloting = stat.piloting;
        _shipSubSystem.Doors = stat.doors;
        _shipSubSystem.Sensors = stat.sensors;
        _shipMainSystem.Medbay = stat.medbay;
        _shipMainSystem.Oxygen = stat.oxygen;
        _shipMainSystem.Shields = stat.shields;
        _shipMainSystem.Engines = stat.engines;
        _shipMainSystem.Weapons = stat.weapons;
    }

    //public virtual void OnAttacked(Stat attacker)
    //{
    //    int damage = Mathf.Max(0, attacker.Attack - Defense);
    //    Hp -= damage;
    //    if (Hp <= 0)
    //    {
    //        Hp = 0;
    //        OnDead(attacker);
    //    }
    //}

    //protected virtual void OnDead(Stat attacker)
    //{
    //    playerstat playerstat = attacker as playerstat;
    //    if (playerstat != null)
    //    {
    //        playerstat.exp += 5;
    //    }

    //    Managers.Game.Despawn(gameObject);
    //}
}
