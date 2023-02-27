using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class PowerButton_Base : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    string BtnName;

    protected int _consumepower = 0;

    protected GameObject _playership;

    protected PlayerShipStat _playershipstat;

    [SerializeField]
    protected int _systemlevel = 0;

    protected PropertyInfo propertyInfo;

    protected SpriteState ss;

    public int ConsumePower { get { return _consumepower; } set { _consumepower = value;  } }
    public PlayerShipStat Playershipstat { get { return _playershipstat; } set { _playershipstat = value; } }
    public int SystemLevel { get { return _systemlevel; } set { _systemlevel = value; } }



    protected virtual void Init()
    {
        _playership = transform.parent.GetComponent<UI_InGame>().PlayerShip;
        _playershipstat = _playership.GetComponent<PlayerShipStat>();
        BtnName = transform.name.Split(' ')[0];
        propertyInfo = _playershipstat.ShipMain.GetType().GetProperty(BtnName);
    }

    void Start()
    {
        Init();
    }

    protected virtual void Update()
    {
        _systemlevel = (int)propertyInfo.GetValue(_playershipstat.ShipMain);
        PowerButtonOnOff();
    }

    void LateUpdate()
    {
        PowerBarUpdate();
    }


    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_systemlevel <= _consumepower)
            {
                Managers.Sound.Play("waves/ui/select_b_fail1", Sound.Effect);
                return;
            }
            if (_playershipstat.PowerUsage + 1 > _playershipstat.Reactor)
            {
                Managers.Sound.Play("waves/ui/select_b_fail1", Sound.Effect);
                return;
            }
            _consumepower++;
            _playershipstat.PowerUsage++;
            Managers.Sound.Play("waves/ui/select_up1", Sound.Effect);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if ( _consumepower == 0)
                return;
            _consumepower--;
            _playershipstat.PowerUsage--;
            Managers.Sound.Play("waves/ui/select_down2", Sound.Effect);
        }
    }

    void PowerBarUpdate()
    {
        for (int i = 0; i < _systemlevel; i++)
        {
            transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
        }

        if (_consumepower == 0)
            return;

        for (int i = 0; i < _consumepower; i++)
        {
            transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void PowerButtonOnOff()
    {
        if(_consumepower > 0)
        {
            transform.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/icons/s_{BtnName}_green1");
            ss.highlightedSprite = Resources.Load<Sprite>($"Sprites/icons/s_{BtnName}_green2");
            transform.GetComponent<Button>().spriteState = ss;
        }
        else
        {
            transform.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/icons/s_{BtnName}_grey1");
            ss.highlightedSprite = Resources.Load<Sprite>($"Sprites/icons/s_{BtnName}_grey2");
            transform.GetComponent<Button>().spriteState = ss;
        }
    }
}
