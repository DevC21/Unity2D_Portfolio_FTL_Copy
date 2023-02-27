using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EnemyShipController : MonoBehaviour
{
    [SerializeField]
    ShipStat _enemyShipStat;

    GameObject _playership;

    WeaponController[] weapons;

    void Start()
    {
        _playership = GameObject.FindWithTag("PlayerShip");
        weapons = _playership.GetComponentsInChildren<WeaponController>();
        _enemyShipStat = GetComponent<ShipStat>();
        Managers.Sound.Clear();
        Managers.Sound.Play("music/bp_MUS_CivilBATTLE", Sound.Bgm);
    }

    void Update()
    {
        OnDestroyed();
        UpdateTargetHp();
    }

    void UpdateTargetHp()
    {
        for(int i = 0; i < weapons.Length; i++)
        {
            weapons[i].TargetHp = _enemyShipStat.Hp;
        }
    }

    void OnDestroyed()
    {
        if (_enemyShipStat.Hp <= 0)
        {
            Managers.Sound.Play("music/bp_MUS_CivilEXPLORE", Sound.Bgm);
            Destroy(gameObject);
        }
    }
}
