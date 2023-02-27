using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_Customize : UI_Popup
{
    enum Buttons
    {
        RenameButton,
        StartButton,
        HideRoomButton,
    }

    [SerializeField]
    InputField ShipNameInputField;

    CustomizeSystemBar[] _systemBar;

    GameObject[] SystemPowerBars;

    GameObject _playership;

    PlayerShipStat _playershipstat;

    int PreShipCode = 0;

    bool hideRoom = false;

    SpriteState ss;

    public GameObject PlayerShip { get { return _playership; } set { _playership = value; } }

    public override void Init()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;

        _playership = Managers.Map.LoadShip("Kestral");

        _playership.GetComponent<PlayerShipController>().enabled = false;
        Util.FindChild(_playership, "UI_HPBar", true).SetActive(false);
        _playership.transform.position = new Vector3(2.32f, 1.94f);

        Bind<Button>(typeof(Buttons));

        _playershipstat = _playership.GetComponent<PlayerShipStat>();

        foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
        {
            GetButton((int)button).gameObject.BindEvent(OnMouseEnter, UIEvent.PointerEnter);
            GetButton((int)button).gameObject.BindEvent(OnMouseExit, UIEvent.PointerExit);
        }



        GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);
        GetButton((int)Buttons.RenameButton).gameObject.BindEvent(OnClickRenameButton);
        GetButton((int)Buttons.HideRoomButton).gameObject.BindEvent(OnClickHideRoomButton);
    }

    private void Update()
    {
        EndShipRename();
        _systemBar = transform.GetComponentsInChildren<CustomizeSystemBar>();
        CustomizeSystemPowerBarControl();
    }

    private void OnMouseEnter(PointerEventData obj)
    {
        CursorController.instance.BasicValidCursor();
        Managers.Sound.Play("waves/ui/select_light1", Sound.Effect);
    }

    private void OnMouseExit(PointerEventData obj)
    {
        CursorController.instance.BasicCursor();
    }

    private void OnClickStartButton(PointerEventData obj)
    {
        UI_InGame _ingame = Managers.UI.ShowPopupUI<UI_InGame>();

        _ingame.PlayerShip = _playership;

        _playership.GetComponent<PlayerShipController>().InGame = _ingame;
        _playership.GetComponent<PlayerShipController>().enabled = true;
        _playership.transform.position = Vector3.zero;
        _playership.GetComponentInChildren<UI_HPBar>(true).gameObject.SetActive(true);

        GameObject player = Managers.Resource.Instantiate("Creature/Player Human");
        player.name = "Player";
        Managers.Object.Add(player);

        gameObject.SetActive(false);
    }

    private void OnClickRenameButton(PointerEventData obj)
    {
        ShipNameInputField.interactable = true;
    }

    private void EndShipRename()
    {
        if (ShipNameInputField.interactable  == true)
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Escape))
            {
                ShipNameInputField.interactable  = false;
            }
        }
    }

    private void OnClickHideRoomButton(PointerEventData obj)
    {
        if (hideRoom == false)
        {
            _playership.GetComponent<PlayerShipController>().Tilemap_Base.SetActive(false);
            _playership.GetComponent<PlayerShipController>().Tilemap_Floor.SetActive(false);
            GetButton((int)Buttons.HideRoomButton).gameObject.GetComponent<Image>().sprite =
                Resources.Load<Sprite>($"Sprites/customizeUI/button_show_on");
            ss.highlightedSprite = Resources.Load<Sprite>("Sprites/customizeUI/button_show_select2");
            GetButton((int)Buttons.HideRoomButton).gameObject.GetComponent<Button>().spriteState = ss;
            hideRoom = true;
        }
        else
        {
            _playership.GetComponent<PlayerShipController>().Tilemap_Base.SetActive(true);
            _playership.GetComponent<PlayerShipController>().Tilemap_Floor.SetActive(true);
            GetButton((int)Buttons.HideRoomButton).gameObject.GetComponent<Image>().sprite =
    Resources.Load<Sprite>($"Sprites/customizeUI/button_hide_on");
            ss.highlightedSprite = Resources.Load<Sprite>("Sprites/customizeUI/button_hide_select2");
            GetButton((int)Buttons.HideRoomButton).gameObject.GetComponent<Button>().spriteState = ss;
            hideRoom = false;
        }
    }

    private void CustomizeSystemPowerBarControl()
    {
        if (_playershipstat.ShipCode != PreShipCode)
        {
            DestroyCustomzieSystemPowerBar();
            PreShipCode = _playershipstat.ShipCode;
        }
        else
        {
            GenerateCustomzieSystemPowerBar();

            foreach (var propertyInfo in _playershipstat.ShipMain.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                String System = propertyInfo.Name;
                GameObject _systemButton;

                if (transform.GetComponentInChildren<CustomizeSystemBar>() != null)
                {
                    for (int i = 0; i < _systemBar.Length; i++)
                    {
                        _systemButton = _systemBar[i].gameObject;
                        if (_systemButton.GetComponent<CustomizeSystemBar>().SystemBarName == System)
                        {
                            _systemButton.GetComponent<Button>().gameObject.BindEvent(OnMouseEnter, UIEvent.PointerEnter);
                            _systemButton.GetComponent<Button>().gameObject.BindEvent(OnMouseExit, UIEvent.PointerExit);
                        }
                    }
                }
            }

            foreach (var propertyInfo in _playershipstat.ShipSub.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                String System = propertyInfo.Name;
                GameObject _systemButton;

                if (transform.GetComponentInChildren<CustomizeSystemBar>() != null)
                {
                    for (int i = 0; i < _systemBar.Length; i++)
                    {
                        _systemButton = _systemBar[i].gameObject;
                        if (_systemButton.GetComponent<CustomizeSystemBar>().SystemBarName == System)
                        {
                            _systemButton.GetComponent<Button>().gameObject.BindEvent(OnMouseEnter, UIEvent.PointerEnter);
                            _systemButton.GetComponent<Button>().gameObject.BindEvent(OnMouseExit, UIEvent.PointerExit);
                        }
                    }
                }
            }
        }
    }

    private void GenerateCustomzieSystemPowerBar()
    {
        string System;
        int count = 0;
        foreach (var propertyInfo in _playershipstat.ShipMain.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            // do stuff here
            System = propertyInfo.Name;

            int systemlevel;

            systemlevel = (int)propertyInfo.GetValue(_playershipstat.ShipMain);

            if (systemlevel == 0)
                continue;

            Vector2 barpos = new Vector2()
            {
                x = -250 + count * 38,
                y = -70,
            };

            GameObject _systemButton;

            if (transform.GetComponentInChildren<CustomizeSystemBar>() != null)
            {
                for (int i = 0; i < _systemBar.Length; i++)
                {
                    _systemButton = _systemBar[i].gameObject;
                    if (_systemButton.GetComponent<CustomizeSystemBar>().SystemBarName == System)
                        return;
                }
            }

            _systemButton = Managers.Resource.Instantiate("UI/SubItem/CustomizeSystemBar", gameObject.transform);
            _systemButton.name = $"{System} System Bar";
            _systemButton.GetComponent<CustomizeSystemBar>().SystemBarName = System;
            _systemButton.GetComponent<RectTransform>().anchoredPosition = barpos;

            count++;


            _systemButton.transform.GetChild(0).GetComponent<Image>().sprite =
                Resources.Load<Sprite>($"Sprites/icons/s_{System}_green1");

            GameObject powerbar = Managers.Resource.Instantiate("UI/SubItem/SystemPower", _systemButton.transform);
            powerbar.name = $"{System}Power_1";

            Vector3 pos = new Vector3()
            {
                x = _systemButton.transform.GetChild(0).position.x,
                y = _systemButton.transform.GetChild(0).position.y + 0.30f
            };

            powerbar.transform.position = pos;

            for (int i = 1; i < systemlevel; i++)
            {
                powerbar = Managers.Resource.Instantiate("UI/SubItem/SystemPower", _systemButton.transform);
                powerbar.name = $"{System}Power_{i + 1}";

                pos = new Vector3()
                {
                    x = _systemButton.transform.GetChild(0).position.x,
                    y = _systemButton.transform.GetChild(0).position.y + 0.30f + i * 0.17f
                };

                powerbar.transform.position = pos;
            }
        }

        foreach (var propertyInfo in _playershipstat.ShipSub.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            // do stuff here
            System = propertyInfo.Name;

            int systemlevel;

            systemlevel = (int)propertyInfo.GetValue(_playershipstat.ShipSub);

            if (systemlevel == 0)
                continue;

            Vector2 barpos = new Vector2()
            {
                x = -250 + count * 38,
                y = -70,
            };

            GameObject _systemButton;

            if (transform.GetComponentInChildren<CustomizeSystemBar>() != null)
            {
                for (int i = 0; i < _systemBar.Length; i++)
                {
                    _systemButton = _systemBar[i].gameObject;
                    if (_systemButton.GetComponent<CustomizeSystemBar>().SystemBarName == System)
                        return;
                }
            }

            _systemButton = Managers.Resource.Instantiate("UI/SubItem/CustomizeSystemBar", gameObject.transform);
            _systemButton.name = $"{System} System Bar";
            _systemButton.GetComponent<CustomizeSystemBar>().SystemBarName = System;
            _systemButton.GetComponent<RectTransform>().anchoredPosition = barpos;

            count++;
            

            _systemButton.transform.GetChild(0).GetComponent<Image>().sprite =
                Resources.Load<Sprite>($"Sprites/icons/s_{System}_green1");

            GameObject powerbar = Managers.Resource.Instantiate("UI/SubItem/SystemPower", _systemButton.transform);
            powerbar.name = $"{System}Power_1";

            Vector3 pos = new Vector3()
            {
                x = _systemButton.transform.GetChild(0).position.x,
                y = _systemButton.transform.GetChild(0).position.y + 0.30f
            };

            powerbar.transform.position = pos;

            for (int i = 1; i < systemlevel; i++)
            {
                powerbar = Managers.Resource.Instantiate("UI/SubItem/SystemPower", _systemButton.transform);
                powerbar.name = $"{System}Power_{i + 1}";

                pos = new Vector3()
                {
                    x = _systemButton.transform.GetChild(0).position.x,
                    y = _systemButton.transform.GetChild(0).position.y + 0.30f + i * 0.17f
                };

                powerbar.transform.position = pos;
            }
        }
    }

    private void DestroyCustomzieSystemPowerBar()
    {
        string System;
        foreach (var propertyInfo in _playershipstat.ShipMain.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            System = propertyInfo.Name;

            GameObject _systemButton;

            if (transform.GetComponentInChildren<CustomizeSystemBar>() != null)
            {
                for (int i = 0; i < _systemBar.Length; i++)
                {
                    _systemButton = _systemBar[i].gameObject;
                    if (_systemButton.GetComponent<CustomizeSystemBar>().SystemBarName == System)
                        Managers.Resource.Destroy(_systemButton);
                }
            }
        }
        foreach (var propertyInfo in _playershipstat.ShipSub.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            System = propertyInfo.Name;
            GameObject _systemButton;

            if (transform.GetComponentInChildren<CustomizeSystemBar>() != null)
            {
                for (int i = 0; i < _systemBar.Length; i++)
                {
                    _systemButton = _systemBar[i].gameObject;
                    if (_systemButton.GetComponent<CustomizeSystemBar>().SystemBarName == System)
                        Managers.Resource.Destroy(_systemButton);
                }
            }
        }
    }
}

