using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipStat : ShipStat
{

    [SerializeField]
    int _fuel;
    [SerializeField]
    int _missiles;
    [SerializeField]
    int _droneparts;
    [SerializeField]
    int _scrap;

    public int Fuel { get { return _fuel; } set { _fuel = value; } }
    public int Missiles { get { return _missiles; } set { _missiles = value; } }
    public int Droneparts { get { return _droneparts; } set { _droneparts = value; } }
    public int Scrap { get { return _scrap; } set { _scrap = value; } }

    //public int Exp 
    //{ 
    //    get { return _exp; } 
    //    set 
    //    { 
    //        _exp = value;
    //        //레벨업 체크

    //        int level = 1;
    //        while (true)
    //        {
    //            Data.Stat stat;
    //            if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)
    //                break;
    //            if (_exp < stat.totalExp)
    //                break;
    //            level++;
    //        }

    //        if (level != Level)
    //        {
    //            Debug.Log("Level Up!!");
    //            Level = level;
    //            SetStat(Level);
    //        }
    //    } 
    //}
    //public int Gold { get { return _gold; } set { _gold = value; } }

    public void Start()
    {
        _shipCode = 1;
        _fuel = 16;
        _missiles = 8;
        _droneparts = 2;
        _scrap = 30;

        SetStat(_shipCode);
    }

    //protected override void OnDead(Stat attacker)
    //{
    //    Debug.Log("Player Dead");
    //}
}
