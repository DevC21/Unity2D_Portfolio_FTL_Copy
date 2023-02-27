using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class NodeStatus : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected RectTransform elementA;
    [SerializeField]
    protected RectTransform elementB;

    protected int i = 0;

    float myDistance;

    NodeStatus vs;

    FieldInfo[] nodes;

    [SerializeField]
    GameObject _enemyShip;

    Color PlayerLocateColor = new Color(0 / 255f, 240 / 255f, 0 / 255f);
    Color SelectedNodeColor = new Color(255 / 255f, 240 / 255f, 0 / 255f);

    UI_Map _map;

    UI_InGame _ingame;


    int _eventCode;
    String _eventText;
    EventFlag _flag = EventFlag.NonCombat;

    [SerializeField]
    bool _isPlayerLocated = false;

    bool _isPlayerVisited = false;

    public GameObject _connectedNode_1;
    public GameObject _connectedNode_2;
    public GameObject _connectedNode_3;
    public GameObject _connectedNode_4;
    public GameObject _connectedNode_5;
    public GameObject _connectedNode_6;
    public GameObject _connectedNode_7;
    public GameObject _connectedNode_8;
    public GameObject _connectedNode_9;
    public GameObject _connectedNode_10;

    public GameObject ConnectedNode_1 { get { return _connectedNode_1; } set { _connectedNode_1 = value; } }
    public GameObject ConnectedNode_2 { get { return _connectedNode_2; } set { _connectedNode_2 = value; } }
    public GameObject ConnectedNode_3 { get { return _connectedNode_3; } set { _connectedNode_3 = value; } }
    public GameObject ConnectedNode_4 { get { return _connectedNode_4; } set { _connectedNode_4 = value; } }
    public GameObject ConnectedNode_5 { get { return _connectedNode_5; } set { _connectedNode_5 = value; } }
    public GameObject ConnectedNode_6 { get { return _connectedNode_6; } set { _connectedNode_6 = value; } }
    public GameObject ConnectedNode_7 { get { return _connectedNode_7; } set { _connectedNode_7 = value; } }
    public GameObject ConnectedNode_8 { get { return _connectedNode_8; } set { _connectedNode_8 = value; } }
    public bool IsPlayerLocated { get { return _isPlayerLocated; } set { _isPlayerLocated = value; } }
    public bool IsPlayerVisited { get { return _isPlayerVisited; } set { _isPlayerVisited = value; } }

    void Awake()
    {
        elementA = transform.GetComponent<RectTransform>();
        vs = transform.GetComponent<NodeStatus>();
        _map = transform.GetComponentInParent<UI_Map>();
        _ingame = _map.InGame;
    }

    private void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        for(int index = 0; index < 10; index++)
        {
            GameObject node = (GameObject)nodes[index].GetValue(vs);
            if (node == null)
                return;
            NodeStatus PlayerPos = node.GetComponent<NodeStatus>();
            if (PlayerPos._isPlayerLocated)
            {
                PlayerPos._isPlayerLocated = false;
                foreach (Transform Child in PlayerPos.transform)
                {
                    Child.gameObject.SetActive(false);
                    Child.GetComponent<Image>().color = SelectedNodeColor;
                }

                vs._isPlayerLocated = true;
                vs.GetComponent<Image>().sprite =
                    Managers.Resource.Load<Sprite>($"Sprites/map_new/map_icon_diamond_blue");
                SetPlayerShipIcon();
                foreach (Transform Child in transform)
                {
                    Child.gameObject.SetActive(true);
                    Child.GetComponent<Image>().color = PlayerLocateColor;
                }
                if (!_isPlayerVisited)
                {
                    UI_TextBox textbox = Managers.UI.ShowPopupUI<UI_TextBox>();
                    textbox.EventText.text = _eventText;
                    if (_flag == EventFlag.Combat)
                    { 
                        _ingame.EnemyShip = Instantiate(_enemyShip);
                    }
                }
                vs._isPlayerVisited = true;
                _ingame.Playershipstat.Fuel--;

                SetRandomBackGroundImage();
                Managers.Sound.Play("Sounds/waves/ship/bp_jump_1", Sound.Effect);

                _map.gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isPlayerLocated)
            return;
        foreach(Transform Child in transform)
        {
            Child.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isPlayerLocated)
            return;
        foreach (Transform Child in transform)
        {
            Child.gameObject.SetActive(false);
        }
    }

    public void SetNode(RectTransform elementB)
    {
        this.elementB = elementB;
        Vector2 posA = elementA.position;
        Vector2 posB = elementB.position;
        Vector3 dir = elementB.position - elementA.position;
        myDistance = Vector2.Distance(posA, posB);
        //Debug.Log("Distance: " + myDistance);
        if (myDistance == 0)
            return;

        vs = gameObject.GetComponent<NodeStatus>();
        nodes = vs.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        if (myDistance <= 1.5f)
        {
            String Connectednode = $"_connectedNode_{i+1}";
            nodes[i].SetValue(vs, elementB.gameObject);
            GameObject line = Managers.Resource.Instantiate("UI/SubItem/dotted_line", elementA);
            line.transform.position = elementA.position;
            float distance = (elementB.position - elementA.position).magnitude;
            line.GetComponent<RectTransform>().sizeDelta = new Vector2(distance * 65, line.GetComponent<RectTransform>().sizeDelta.y);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            line.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, angle);
            line.gameObject.SetActive(false);
            //vs.GetType().GetField(Connectednode).SetValue(vs, elementB.gameObject);
            i++;
        }
    }

    public void SetNodeEvent(int EventCode)
    {
        Dictionary<int, Data.Event> dict = Managers.Data.EventDict;
        Data.Event evt = dict[EventCode];

        _eventCode = evt.EventCode;
        _eventText = evt.EventText;
        _flag = evt.Flag;

        if(evt.Flag == EventFlag.Combat)
        {
            _enemyShip = Managers.Resource.Load<GameObject>("Prefabs/Ship/Ship_FederationScout");
        }
    }

    void SetPlayerShipIcon()
    {
        _map.PlayerShipIcon.Anchor = gameObject;
        _map.PlayerShipIcon.transform.position = Vector3.zero;
        _map.PlayerShipIcon.transform.rotation = Quaternion.Euler(Vector3.zero);
        _map.PlayerShipIcon.transform.position = transform.position + new Vector3(0.2f, 0f, 0f);
    }

    void SetRandomBackGroundImage()
    {
        int bg = UnityEngine.Random.Range(1, 8);
        switch (bg)
        {
            case 1:
                _ingame.BackgroundImage.sprite =
                    Managers.Resource.Load<Sprite>($"Sprites/stars/bg_blueStarcluster");
                break;
            case 2:
                _ingame.BackgroundImage.sprite =
                    Managers.Resource.Load<Sprite>($"Sprites/stars/bg_darknebula");
                break;
            case 3:
                _ingame.BackgroundImage.sprite =
                    Managers.Resource.Load<Sprite>($"Sprites/stars/bg_dullstars");
                break;
            case 4:
                _ingame.BackgroundImage.sprite =
                    Managers.Resource.Load<Sprite>($"Sprites/stars/bg_dullstars2");
                break;
            case 5:
                _ingame.BackgroundImage.sprite =
                    Managers.Resource.Load<Sprite>($"Sprites/stars/bg_lonelyRedStar");
                break;
            case 6:
                _ingame.BackgroundImage.sprite =
                    Managers.Resource.Load<Sprite>($"Sprites/stars/bg_lonelystar");
                break;
            case 7:
                _ingame.BackgroundImage.sprite =
                    Managers.Resource.Load<Sprite>($"Sprites/stars/bg_smallbluenebula");
                break;
        }
    }
}
