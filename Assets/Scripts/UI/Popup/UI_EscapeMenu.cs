using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_EscapeMenu : UI_Popup
{
    enum Buttons
    {
        ContinueButton,
        MainMenuButton,
        HangarButton,
        ResetButton,
        OptionsButton,
        ControlsButton,
        SaveButton,
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));

        foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
        {
            GetButton((int)button).gameObject.BindEvent(OnMouseEnter, UIEvent.PointerEnter);
            GetButton((int)button).gameObject.BindEvent(OnMouseExit, UIEvent.PointerExit);
        }

        GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton);
        GetButton((int)Buttons.MainMenuButton).gameObject.BindEvent(OnClickStartButton);



        Managers.Sound.Clear();
        Managers.Sound.Play("music/bp_MUS_TitleScreen", Sound.Bgm);
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


    private void OnClickContinueButton(PointerEventData obj)
    {

    }

    private void OnClickStartButton(PointerEventData obj)
    {

    }
}

