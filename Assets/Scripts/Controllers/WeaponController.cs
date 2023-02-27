using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static Define;

public class WeaponController : MonoBehaviour
{
    float _animTime;

    Coroutine _coShot;

    ShipStat _shipstat;

    [SerializeField]
    Vector3 OnPos;
    [SerializeField]
    Vector3 OffPos;

    float _speed = 0.5f;

    [SerializeField]
    int Shots = 1;

    [SerializeField]
    float maxShotDelay = 0.3f;

    [SerializeField]
    float curShotDelay;

    [SerializeField]
    string _weaponName;

    [SerializeField]
    int _powerRequired;

    [SerializeField]
    bool isOn = false;

    [SerializeField]
    bool _selected = false;

    [SerializeField]
    bool _targetMode = false;

    [SerializeField]
    bool _singleShot = true;

    [SerializeField]
    bool _locktargetMode = false;

    [SerializeField]
    Vector3 _target;

    [SerializeField]
    int _targetHp;

    public GameObject bulletObjA;
    public AudioClip FireSound;
    public GameObject Crosshair_Placed;
    public Sprite crossHair;
    public Sprite crossHair_locked;

    float _ratio;

    [SerializeField]
    protected WeaponState _state = WeaponState.Reload;

    public virtual WeaponState State
    {
        get { return _state; }
        set
        {
            if (_state == value)
                return;

            _state = value;
        }
    }

    public string WeaponName { get { return _weaponName; } set { _weaponName = value; } }
    public int PowerRequired { get { return _powerRequired; } set { _powerRequired = value; } }
    public bool OnOff { get { return isOn; } set { isOn = value; } }
    public bool Selected { get { return _selected; } set { _selected = value; } }
    public bool TargetMode { get { return _targetMode; } set { _targetMode = value; } }
    public bool SingleShot { get { return _singleShot; } set { _singleShot = value; } }
    public bool LockTargetMode { get { return _locktargetMode; } set { _locktargetMode = value; } }
    public Vector3 TARGET { get { return _target; } set { _target = value; } }
    public int TargetHp { get { return _targetHp; } set { _targetHp = value; } }
    public float Ratio { get { return _ratio; } set { _ratio = value; } }


    protected virtual void Init()
    {
        _singleShot = true;
        _shipstat = transform.parent.GetComponent<ShipStat>();
    }

    void Start()
    {
        Init();
    }

    protected virtual void Update()
    {
        _ratio = curShotDelay / maxShotDelay;
        PointTarget();
        fire();
        reload();
        if (Input.GetMouseButtonDown(1) && _selected)
        {
            if (_targetMode)
            {
                _targetMode = false;
                _selected = false;
            }
            if (_locktargetMode)
            {
                Crosshair_Placed.GetComponent<SpriteRenderer>().enabled = false;
                _selected = false;
            }
        }

        if (_targetHp <= 0)
        {
            _targetMode = false;
            _locktargetMode = false;
            SingleShot = true;
            if (Crosshair_Placed != null)
                Crosshair_Placed.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (_state == WeaponState.Reload)
        {
            UpdateReloadAnimation(_ratio);
        }
        else if (_state == WeaponState.Fire)
        {
            _animTime += Time.deltaTime;
            FireAnimation(_animTime);
        }
    }

    void PointTarget()
    {
        if(_selected && CursorController.instance.State == CursorState.TargetMode && _targetMode)
        {
            if (Input.GetMouseButton(0))
            {
                _target = Input.mousePosition;
                Crosshair_Placed.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0 , 0, 10f);
                Crosshair_Placed.GetComponent<SpriteRenderer>().enabled = true;
                _singleShot = false;
                _targetMode = false;
                _selected = false;
                CursorController.instance.State = CursorState.Idle;
                CursorController.instance.BasicCursor();
            }
        }
        if (_selected && CursorController.instance.State == CursorState.TargetMode && _locktargetMode)
        {
            if (Input.GetMouseButton(0))
            {
                _target = Input.mousePosition;
                Crosshair_Placed.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10f);
                Crosshair_Placed.GetComponent<SpriteRenderer>().enabled = true;
                SingleShot = false;
                _selected = false;
                CursorController.instance.State = CursorState.Idle;
                CursorController.instance.BasicCursor();
            }
        }
    }

    void fire()
    {
        if (_singleShot)
        {
            return;
        }
        if (!isOn)
            return;
        if (curShotDelay < maxShotDelay)
        { //재장전 시간이 충분치 않다면
            return;
        }
        _singleShot = true;
        if (!_locktargetMode)
            Crosshair_Placed.GetComponent<SpriteRenderer>().enabled = false;
        if (_locktargetMode)
        {
            _singleShot = false;
        }

        _coShot = StartCoroutine("CoStartShot");
        curShotDelay = 0;
    }

    void reload()
    {
        if (isOn)
        {
            if (curShotDelay >= maxShotDelay)
                return;
            Vector3 moveDir = OnPos - transform.position;
            float dist = moveDir.magnitude;
            if (dist < _speed * Time.deltaTime)
            {
                transform.position = OnPos;
            }
            else
            {
                transform.position += moveDir.normalized * _speed * Time.deltaTime;
            }
            curShotDelay += Time.deltaTime; //시간 값 더하기
        }
        else
        {
            if (curShotDelay <= 0)
                return;
            Vector3 moveDir = OffPos - transform.position;
            float dist = moveDir.magnitude;
            if (dist < _speed * Time.deltaTime)
            {
                transform.position = OffPos;
            }
            else
            {
                transform.position += moveDir.normalized * _speed * Time.deltaTime;
            }
            curShotDelay -= Time.deltaTime;
        }
    }

    protected virtual void UpdateAnimation()
    {

    }

    protected virtual void UpdateReloadAnimation(float ratio)
    {

    }

    protected virtual void FireAnimation(float _animTIme)
    {

    }

    IEnumerator CoStartShot()
    {
        for (int i = 1; i <= Shots; i++)
        {
            GameObject bullet = Instantiate(bulletObjA, transform.position + new Vector3(0.7f, 0, 0), Quaternion.identity, transform);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
            Managers.Sound.Play(FireSound, Sound.Effect);
            _state = WeaponState.Fire;
            _animTime = 0;
            yield return new WaitForSeconds(0.3f);
        }
        _state = WeaponState.Reload;
    }


}
