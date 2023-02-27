using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShipController : MonoBehaviour
{
    public GameObject Tilemap_Base;
    public GameObject Tilemap_Floor;
    public GameObject Tilemap_Ship;
    public GameObject Tilemap_Collision;

    PlayerShipStat _playershipstat;

    GameObject go;

    UI_InGame _inGame;

    GameObject _mainPowerwire;

    GameObject _ShieldsBtn;
    GameObject _EnginesBtn;
    GameObject _MedbayBtn;
    GameObject _OxygenBtn;
    GameObject _WeaponsBtn;

    public UI_InGame InGame { get { return _inGame; } set { _inGame = value; } }

    public GameObject Shields { get { return _ShieldsBtn; } set { _ShieldsBtn = value; } }
    public GameObject Engines { get { return _EnginesBtn; } set { _EnginesBtn = value; } }
    public GameObject Medbay { get { return _MedbayBtn; } set { _MedbayBtn = value; } }
    public GameObject Oxygen { get { return _OxygenBtn; } set { _OxygenBtn = value; } }
    public GameObject Weapons { get { return _WeaponsBtn; } set { _WeaponsBtn = value; } }

    void Start()
    {
        _ShieldsBtn = InGame.ShieldsPowerBtn;
        _EnginesBtn = InGame.EnginesPowerBtn;
        _MedbayBtn = InGame.MedbayPowerBtn;
        _OxygenBtn = InGame.OxygenPowerBtn;
        _WeaponsBtn = InGame.WeaponsPowerBtn;
        _mainPowerwire = InGame.MainPower;
        _playershipstat = GetComponent<PlayerShipStat>();
        //ShipPowerControl();
    }

    void Update()
    {
        GenerateTotalPowerBar();

        foreach (var propertyInfo in _playershipstat.ShipMain.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            // do stuff here
            string name = propertyInfo.Name;
            GenerateMainSystemPowerBar($"{name}");
        }

        ShipPowerControl();
    }

    void ShipPowerControl()
    {
        _playershipstat.power = (_playershipstat.Reactor - _playershipstat.PowerUsage);

        for (int i = 0; i < _playershipstat.Reactor; i++)
        {
            _mainPowerwire.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 0; i < _playershipstat.power; i++)
        {
            _mainPowerwire.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void GenerateSystemButton()
    {

    }

    void GenerateTotalPowerBar()
    {
        if (_mainPowerwire.transform.childCount == _playershipstat.Reactor)
            return;

        for (int i = 0; i < _playershipstat.Reactor; i++)
        {
            GameObject powerbar = Managers.Resource.Instantiate("UI/SubItem/TotalPower", _mainPowerwire.transform);
            powerbar.name = $"MainPower_{i + 1}";

            Vector3 pos = new Vector3()
            {
                x = powerbar.transform.position.x,
                y = powerbar.transform.position.y + i * 0.18f
            };

            powerbar.transform.position = pos;
        }
    }

    void GenerateMainSystemPowerBar(string System)
    {
        int systemlevel;

        PropertyInfo propertyInfo = _playershipstat.ShipMain.GetType().GetProperty(System);

        systemlevel = (int)propertyInfo.GetValue(_playershipstat.ShipMain);

        GameObject _systemButton = (GameObject)GetType().GetProperty(System).GetValue(this);

        if (_systemButton.transform.childCount == systemlevel)
            return;

        GameObject powerbar = Managers.Resource.Instantiate("UI/SubItem/SystemPower", _systemButton.transform);
        powerbar.name = $"{System}Power_1";

        Vector3 pos = new Vector3()
        {
            x = _systemButton.transform.position.x,
            y = _systemButton.transform.position.y + 0.30f
        };

        powerbar.transform.position = pos;

        for (int i = 1; i < systemlevel; i++)
        {
            powerbar = Managers.Resource.Instantiate("UI/SubItem/SystemPower", _systemButton.transform);
            powerbar.name = $"{System}Power_{i + 1}";

            pos = new Vector3()
            {
                x = _systemButton.transform.position.x,
                y = _systemButton.transform.position.y + 0.30f + i * 0.17f
            };

            powerbar.transform.position = pos;
        }
    }
}
