using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_Map : UI_Popup
{
    enum Buttons
    {
        CancelButton,
    }

    enum Beacons
    {
        BeaconIndex_1,
        BeaconIndex_2,
        BeaconIndex_3,
        BeaconIndex_4,
        BeaconIndex_5,
        BeaconIndex_6,
        BeaconIndex_7,
        BeaconIndex_8,
        BeaconIndex_9,
        BeaconIndex_10,
        BeaconIndex_11,
        BeaconIndex_12,
        BeaconIndex_13,
        BeaconIndex_14,
        BeaconIndex_15,
        BeaconIndex_16,
        BeaconIndex_17,
        BeaconIndex_18,
        BeaconIndex_19,
        BeaconIndex_20,
        BeaconIndex_21,
        BeaconIndex_22,
        BeaconIndex_23,
        BeaconIndex_24
    }

    Color highlighted = new Color(255 / 255f, 240 / 255f, 0 / 255f);
    Color normal = new Color(235 / 255f, 245 / 255f, 229 / 255f);

    int i = 1;

    int StartPoint = 1;
    int ExitPoint = 6;

    UI_InGame _ingame;

    PlayerShipStat _playerShipStat;

    [SerializeField]
    ShipRotateAround _playerShipIcon;
    [SerializeField]
    GameObject _exitPoint;

    public UI_InGame InGame { get { return _ingame; } set { _ingame = value; } }
    public PlayerShipStat playerShipStat { get { return _playerShipStat; } set { _playerShipStat = value; } }
    public ShipRotateAround PlayerShipIcon { get { return _playerShipIcon; } set { _playerShipIcon = value; } }

    public override void Init()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;

        Bind<Button>(typeof(Buttons));

        Bind<GameObject>(typeof(Beacons));

        RandomeStartPoint();

        RandomGenerateNodes();

        ConnectNodes();

        foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
        {
            GetButton((int)button).gameObject.BindEvent(OnMouseEnter, UIEvent.PointerEnter);
            GetButton((int)button).gameObject.BindEvent(OnMouseExit, UIEvent.PointerExit);
        }


        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(OnClickCancelButton);

    }

    private void OnMouseEnter(PointerEventData obj)
    {
        if(obj.pointerEnter.GetComponentInChildren<Text>())
            obj.pointerEnter.GetComponentInChildren<Text>().color = highlighted;
        CursorController.instance.BasicValidCursor();
        Managers.Sound.Play("waves/ui/select_light1", Sound.Effect);
    }

    private void OnMouseExit(PointerEventData obj)
    {
        if (obj.pointerEnter.GetComponentInChildren<Text>())
            obj.pointerEnter.GetComponentInChildren<Text>().color = normal;
        CursorController.instance.BasicCursor();
    }

    private void OnClickCancelButton(PointerEventData obj)
    {
        gameObject.SetActive(false);
    }

    private void RandomGenerateNodes()
    {
        foreach (Beacons beacon in Enum.GetValues(typeof(Beacons)))
        {
            GameObject go = Managers.Resource.Instantiate("UI/SubItem/map_icon_triangle_yellow", GetObject((int)beacon).gameObject.transform);
            go.name = $"Beacon_{i}";
            go.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(UnityEngine.Random.Range(-35.0f, 35.0f), UnityEngine.Random.Range(-35.0f, 35.0f));
            if (i != StartPoint && i != ExitPoint)
            {
                go.GetComponent<NodeStatus>().SetNodeEvent(UnityEngine.Random.Range(1, 3));
            }
            if (i == StartPoint)
            {
                go.GetComponent<NodeStatus>().IsPlayerLocated = true;
                go.GetComponent<NodeStatus>().IsPlayerVisited = true;

                go.GetComponent<Image>().sprite =
                    Managers.Resource.Load<Sprite>("Sprites/map_new/map_icon_diamond_blue");
                _playerShipIcon.Anchor = go.gameObject;
                _playerShipIcon.enabled = true;
            }
            if (i == ExitPoint)
            {
                go.GetComponent<NodeStatus>().SetNodeEvent(3);
                _exitPoint.transform.position = go.transform.position + new Vector3(0.3f, 0.2f, 0f);
            }
            go.BindEvent(OnMouseEnter, UIEvent.PointerEnter);
            go.BindEvent(OnMouseExit, UIEvent.PointerExit);
            i++;
        }
    }

    private void ConnectNodes()
    {
        foreach (Beacons beacon in Enum.GetValues(typeof(Beacons)))
        {
            NodeStatus node = GetObject((int)beacon).gameObject.transform.GetComponentInChildren<NodeStatus>();
            foreach (Beacons Otherbeacon in Enum.GetValues(typeof(Beacons)))
            {
                NodeStatus OtherNode = GetObject((int)Otherbeacon).gameObject.transform.GetComponentInChildren<NodeStatus>();
                
                node.SetNode(OtherNode.GetComponent<RectTransform>());
            }
        }
    }

    private void RandomeStartPoint()
    {
        float random = UnityEngine.Random.Range(0f, 10f);

        if(random > 0f && random <= 2.5f)
        {
            StartPoint = 1;
        }
        else if(random > 2.5f && random <= 5f)
        {
            StartPoint = 7;
        }
        else if(random > 5f && random <= 7.5f)
        {
            StartPoint = 13;
        }
        else if(random > 7.5f && random <= 10f)
        {
            StartPoint = 19;
        }
    }

    private void RandomeExitPoint()
    {
        float random = UnityEngine.Random.Range(0f, 10f);

        if (random > 0f && random <= 2.5f)
        {
            ExitPoint = 6;
        }
        else if (random > 2.5f && random <= 5f)
        {
            ExitPoint = 12;
        }
        else if (random > 5f && random <= 7.5f)
        {
            ExitPoint = 18;
        }
        else if (random > 7.5f && random <= 10f)
        {
            ExitPoint = 24;
        }
    }
}
