using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_TextBox : UI_Popup
{
    enum Buttons
    {
        ContinueButton,
    }

    [SerializeField]
    GameObject _button;
    [SerializeField]
    Text _eventText;

    public Text EventText { get { return _eventText; } set { _eventText = value; } }

    public override void Init()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;

        _button.gameObject.BindEvent(OnMouseEnter, UIEvent.PointerEnter);
        _button.gameObject.BindEvent(OnMouseExit, UIEvent.PointerExit);
        _button.gameObject.BindEvent(OnClickButton);
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

    private void OnClickButton(PointerEventData obj)
    {
        Managers.UI.ClosePopupUI();
    }
}
