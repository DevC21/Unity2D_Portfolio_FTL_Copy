using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_InGame : UI_Popup
{
    enum Buttons
    {
        Weapon_1,
        Weapon_2,
        Weapon_3,
        Weapon_4,
        FTLButton
    }

    [SerializeField]
    Image _bg;

    [SerializeField]
    GameObject _playership;

    [SerializeField]
    GameObject _mainPower;

    [SerializeField]
    GameObject _shieldsPowerBtn;
    [SerializeField]
    GameObject _enginesPowerBtn;
    [SerializeField]
    GameObject _medbayPowerBtn;
    [SerializeField]
    GameObject _oxygenPowerBtn;
    [SerializeField]
    GameObject _weaponsPowerBtn;
    [SerializeField]
    Button _FTLBtn;
    [SerializeField]
    Image _FTLCharge;
    [SerializeField]
    Text Fuel;
    [SerializeField]
    Text Missiles;
    [SerializeField]
    Text Drones;
    [SerializeField]
    Text Scrap;

    float _ftlChargetime = 0;
    UI_Map _map;
    PlayerShipStat _playershipstat;
    GameObject _enemyShip;

    public Image BackgroundImage { get { return _bg; } set { _bg = value; } }
    public GameObject PlayerShip { get { return _playership; }  set { _playership = value; } }
    public GameObject MainPower { get { return _mainPower; } set { _mainPower = value; } }
    public GameObject ShieldsPowerBtn { get { return _shieldsPowerBtn; } set { _shieldsPowerBtn = value; } }
    public GameObject EnginesPowerBtn { get { return _enginesPowerBtn; } set { _enginesPowerBtn = value; } }
    public GameObject MedbayPowerBtn { get { return _medbayPowerBtn; } set { _medbayPowerBtn = value; } }
    public GameObject OxygenPowerBtn { get { return _oxygenPowerBtn; } set { _oxygenPowerBtn = value; } }
    public GameObject WeaponsPowerBtn { get { return _weaponsPowerBtn; } set { _weaponsPowerBtn = value; } }
    public PlayerShipStat Playershipstat { get { return _playershipstat; } set { _playershipstat = value; } }
    public GameObject EnemyShip { get { return _enemyShip; } set { _enemyShip = value; } }
    public float FTLChargeTime { get { return _ftlChargetime; } set { _ftlChargetime = value; } }

    public override void Init()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;

        _playershipstat = _playership.GetComponent<PlayerShipStat>();

        Bind<Button>(typeof(Buttons));

        foreach (var propertyInfo in _playershipstat.ShipMain.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            String System = propertyInfo.Name;
            GameObject PowerBtn = (GameObject)GetType().GetProperty($"{System}PowerBtn").GetValue(this);
            if (PowerBtn == null)
                continue;
            PowerBtn.GetComponent<Button>().gameObject.BindEvent(OnMouseEnter, UIEvent.PointerEnter);
            PowerBtn.GetComponent<Button>().gameObject.BindEvent(OnMouseExit, UIEvent.PointerExit);
        }


        foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
        {
            GetButton((int)button).gameObject.BindEvent(OnMouseEnter, UIEvent.PointerEnter);
            GetButton((int)button).gameObject.BindEvent(OnMouseExit, UIEvent.PointerExit);
        }

        GetButton((int)Buttons.FTLButton).gameObject.BindEvent(OnClickFTLButton);

        Managers.Sound.Clear();
        Managers.Sound.Play("music/bp_MUS_CivilEXPLORE", Sound.Bgm);
        _map = Managers.UI.ShowPopupUI<UI_Map>();
        _map.GetComponent<UI_Map>().InGame = this;
        _map.GetComponent<UI_Map>().playerShipStat = _playershipstat;
        _map.gameObject.SetActive(false);
        Managers.UI.ShowPopupUI<UI_TextBox>();
    }

    private void Update()
    {
        ResourceUpdate();
        FTLButtonUpdate();
    }

    void ResourceUpdate()
    {
        Fuel.text = _playershipstat.Fuel.ToString();

        Missiles.text = _playershipstat.Missiles.ToString();

        Drones.text = _playershipstat.Droneparts.ToString();

        Scrap.text = _playershipstat.Scrap.ToString();
    }

    void FTLButtonUpdate()
    {
        if (EnemyShip != null)
        {
            _FTLBtn.interactable = false;
        }
        else if (EnemyShip == null)
        {
            _FTLBtn.interactable = true;
        }
    }

    private void OnMouseEnter(PointerEventData obj)
    {
        if (CursorController.instance.State == CursorState.TargetMode)
            return;
        CursorController.instance.BasicValidCursor();
        Managers.Sound.Play("waves/ui/select_light1", Sound.Effect);
    }

    private void OnMouseExit(PointerEventData obj)
    {
        if (CursorController.instance.State == CursorState.TargetMode)
            return;
        CursorController.instance.BasicCursor();
    }



    private void OnClickFTLButton(PointerEventData obj)
    {
        if (EnemyShip != null)
            return;
        else if(EnemyShip == null)
            _map.gameObject.SetActive(true);
    }

    //private void OnClickStartButton(PointerEventData obj)
    //{

    //}
}
