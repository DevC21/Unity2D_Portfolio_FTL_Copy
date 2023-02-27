using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBox : MonoBehaviour
{
    WeaponController[] weapons;

    [SerializeField]
    int _remainWP;

    [SerializeField]
    int _powerOccupancy;

    [SerializeField]
    WeaponsPowerButton _weaponPwBtn;

    GameObject WC;

    GameObject _playership;

    public WeaponController[] WEAPONS { get { return weapons; } set { weapons = value; } }
    public WeaponsPowerButton WeaponPwBtn { get { return _weaponPwBtn; } set { _weaponPwBtn = value; } }
    public int RemainWeaponPower { get { return _remainWP; ; } set { _remainWP = value; } }
    public int PowerOccupancy { get { return _powerOccupancy; } set { _powerOccupancy = value; } }

    void Start()
    {
        _playership = transform.parent.GetComponent<UI_InGame>().PlayerShip;
        weapons = _playership.GetComponentsInChildren<WeaponController>();
    }

    void Update()
    {
        _remainWP = _weaponPwBtn.ConsumePower - _powerOccupancy;
    }
}
