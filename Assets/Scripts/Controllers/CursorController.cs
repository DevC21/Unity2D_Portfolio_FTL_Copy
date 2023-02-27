using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class CursorController : MonoBehaviour
{
    //int _mask = (1 << (int)Define.Layer.UI) | (1 << (int)Define.Layer.Monster);

    Texture2D _basicValidIcon;
    Texture2D _basicIcon;
    Texture2D _crosshairs_1;
    Texture2D _crosshairs_2;
    Texture2D _crosshairs_3;
    Texture2D _crosshairs_4;
    Texture2D _lockcrosshairs_1;
    Texture2D _lockcrosshairs_2;
    Texture2D _lockcrosshairs_3;
    Texture2D _lockcrosshairs_4;

    public static CursorController instance;

    CursorState _state = CursorState.Idle;
    //enum CursorType
    //{
    //    None,
    //    BasicValid,
    //    Basic,
    //}

    //CursorType _cursorType = CursorType.None;

    public CursorState State { get { return _state; } set { _state = value; } }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        _basicValidIcon = Managers.Resource.Load<Texture2D>("Sprites/Cursor/pointerValid");
        _basicIcon = Managers.Resource.Load<Texture2D>("Sprites/Cursor/pointerInvalid");
        _crosshairs_1 = Managers.Resource.Load<Texture2D>("Sprites/Cursor/mouse_crosshairs2_1");
        _crosshairs_2 = Managers.Resource.Load<Texture2D>("Sprites/Cursor/mouse_crosshairs2_2");
        _crosshairs_3 = Managers.Resource.Load<Texture2D>("Sprites/Cursor/mouse_crosshairs2_3");
        _crosshairs_4 = Managers.Resource.Load<Texture2D>("Sprites/Cursor/mouse_crosshairs2_4");
        _lockcrosshairs_1 = Managers.Resource.Load<Texture2D>("Sprites/Cursor/mouse_crosshairs3_1");
        _lockcrosshairs_2 = Managers.Resource.Load<Texture2D>("Sprites/Cursor/mouse_crosshairs3_2");
        _lockcrosshairs_3 = Managers.Resource.Load<Texture2D>("Sprites/Cursor/mouse_crosshairs3_3");
        _lockcrosshairs_4 = Managers.Resource.Load<Texture2D>("Sprites/Cursor/mouse_crosshairs3_4");
        BasicCursor();
    }

    private void Update()
    {
        switch (State)
        {
            case CursorState.TargetMode:
                if (Input.GetMouseButtonDown(1))
                {
                    State = CursorState.Idle;
                    BasicCursor();
                }
                break;
            case CursorState.Idle:
                if (Input.GetMouseButtonDown(1))
                {
 
                }
                break;
        }
    }



    public void BasicCursor()
    {
        Cursor.SetCursor(_basicIcon, Vector2.zero, CursorMode.Auto);
    }

    public void BasicValidCursor()
    {
        Cursor.SetCursor(_basicValidIcon, Vector2.zero, CursorMode.Auto);
    }

    public void Crosshairs(int i)
    {
        Vector2 hotSpot = new Vector2 { x = _crosshairs_1.width / 2, y = _crosshairs_1.height / 2 };
        switch (i)
        {
            case 0:
                Cursor.SetCursor(_crosshairs_1, hotSpot, CursorMode.Auto);
                break;
            case 1:
                Cursor.SetCursor(_crosshairs_2, hotSpot, CursorMode.Auto);
                break;
            case 2:
                Cursor.SetCursor(_crosshairs_3, hotSpot, CursorMode.Auto);
                break;
            case 3:
                Cursor.SetCursor(_crosshairs_4, hotSpot, CursorMode.Auto);
                break;
        }
    }

    public void LockCrosshairs(int i)
    {
        Vector2 hotSpot = new Vector2 { x = _lockcrosshairs_1.width / 2, y = _lockcrosshairs_1.height / 2 };
        switch (i)
        {
            case 0:
                Cursor.SetCursor(_lockcrosshairs_1, hotSpot, CursorMode.Auto);
                break;
            case 1:
                Cursor.SetCursor(_lockcrosshairs_2, hotSpot, CursorMode.Auto);
                break;
            case 2:
                Cursor.SetCursor(_lockcrosshairs_3, hotSpot, CursorMode.Auto);
                break;
            case 3:
                Cursor.SetCursor(_lockcrosshairs_4, hotSpot, CursorMode.Auto);
                break;
        }
    }
}
