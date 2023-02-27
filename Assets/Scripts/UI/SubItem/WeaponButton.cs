using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class WeaponButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    WeaponController[] weapons;

    WeaponsPowerButton WeaponPwBtn;

    [SerializeField]
    WeaponBox weaponBox;


    [SerializeField]
    GameObject Overlay;
    [SerializeField]
    Image WeaponChargebar;
    [SerializeField]
    Text Index;
    [SerializeField]
    Text WeaponName;

    [SerializeField]
    int i;
    Color off = new Color(123 / 255f, 123 / 255f, 120 / 255f);
    Color on = new Color(243 / 255f, 255 / 255f, 230 / 255f);
    Color charged = new Color(120 / 255f, 255 / 255f, 120 / 255f);
    Color Locked = new Color(255 / 255f, 255 / 255f, 0 / 255f);
    Color Selected = new Color(255 / 255f, 100 / 255f, 100 / 255f);

    void Start()
    {
        WeaponPwBtn = weaponBox.WeaponPwBtn;
        i = int.Parse(transform.name.Split('_')[1]) - 1;
        weapons = weaponBox.WEAPONS;
        if (weapons.Length <= i)
            return;
        GameObject go = Managers.Resource.Instantiate("Etc/Crosshairs_Placed", weapons[i].transform);
        go.GetComponent<SpriteRenderer>().enabled = false;
        weapons[i].Crosshair_Placed = go;
        weapons[i].crossHair = Managers.Resource.Load<Sprite>($"Sprites/misc/crosshairs_placed{i + 1}");
        weapons[i].crossHair_locked = Managers.Resource.Load<Sprite>($"Sprites/misc/crosshairs_placed{i + 1}_yellow");
    }

    void Update()
    {
        UpdateWeaponChargebar();
        UpdateWeaponName();
        GetDirInput();
    }

    void GetDirInput()
    {
        if ( Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown((KeyCode)(49 + i)))
        {
            SelectLockWeapon();
            return;
        }

        if (Input.GetKeyDown((KeyCode)(49 + i)))
        {
            SelectWeapon();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftControl))
        {
            SelectLockWeapon();
            return;
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SelectWeapon();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (WeaponPwBtn.ConsumePower == 0)
                return;
            if (weapons.Length <= i)
                return;
            if (!weapons[i].OnOff)
            {
                return;
            }
            WeaponPwBtn.ConsumePower -= weapons[i].PowerRequired;
            WeaponPwBtn.Playershipstat.PowerUsage -= weapons[i].PowerRequired;

            weaponBox.PowerOccupancy -= weapons[i].PowerRequired;
            weapons[i].OnOff = false;
            weapons[i].LockTargetMode = false;
            weapons[i].TargetMode = false;
            weapons[i].Crosshair_Placed.GetComponent<SpriteRenderer>().enabled = false;
            ColorChangeOff();
            Managers.Sound.Play("waves/ui/select_down2", Sound.Effect);
        }
    }


    public void OnPointerEnter(PointerEventData obj)
    {
        if (weapons.Length <= i)
            return;
        Overlay.SetActive(true);
    }

    public void OnPointerExit(PointerEventData obj)
    {
        if (weapons.Length <= i)
            return;
        Overlay.SetActive(false);
    }

    void SelectWeapon()
    {
        if (weapons.Length <= i)
            return;
        if (weapons[i].OnOff)
        {
            CursorController.instance.Crosshairs(i);
            CursorController.instance.State = CursorState.TargetMode;
            weapons[i].Crosshair_Placed.GetComponent<SpriteRenderer>().sprite = weapons[i].crossHair;
            weapons[i].LockTargetMode = false;
            weapons[i].TargetMode = true;
            weapons[i].Selected = true;
            return;
        }
        if (WeaponPwBtn.SystemLevel <= WeaponPwBtn.ConsumePower)
        {
            Managers.Sound.Play("waves/ui/select_b_fail1", Sound.Effect);
            return;
        }
        if (WeaponPwBtn.Playershipstat.PowerUsage + weapons[i].PowerRequired > WeaponPwBtn.Playershipstat.Reactor)
        {
            Managers.Sound.Play("waves/ui/select_b_fail1", Sound.Effect);
            return;
        }
        WeaponPwBtn.ConsumePower += weapons[i].PowerRequired;
        WeaponPwBtn.Playershipstat.PowerUsage += weapons[i].PowerRequired;
        weaponBox.PowerOccupancy += weapons[i].PowerRequired;

        weapons[i].OnOff = true;
        ColorChangeOn();
        Managers.Sound.Play("waves/ui/select_up1", Sound.Effect);
    }

    void SelectLockWeapon()
    {
        if (weapons[i].OnOff)
        {
            weapons[i].Crosshair_Placed.GetComponent<SpriteRenderer>().sprite = weapons[i].crossHair_locked;
            CursorController.instance.State = CursorState.TargetMode;
            CursorController.instance.LockCrosshairs(i);
            weapons[i].LockTargetMode = true;
            weapons[i].Selected = true;
        }
    }

    void UpdateWeaponName()
    {
        if (weapons.Length <= i)
            return;
        WeaponName.text = weapons[i].WeaponName;
    }

    void UpdateWeaponChargebar()
    {
        if (weapons.Length <= i)
            return;
        float ratio = weapons[i].Ratio;
        WeaponChargebar.fillAmount = ratio;
        if (weapons[i].LockTargetMode)
        {
            ColorChangeLocked();
            return;
        }
        if (weapons[i].Selected)
        {
            ColorChangeSelected();
            return;
        }

        if (ratio >= 1)
            ColorChangeCharged();
        else if (weapons[i].OnOff && ratio < 1)
            ColorChangeOn();
        else if (!weapons[i].OnOff && ratio < 1)
            ColorChangeOff();
    }

    void ColorChangeOn()
    {
        GetComponent<Image>().color = on;
        WeaponChargebar.color = on;
        Index.color = on;
        WeaponName.color = on;
    }

    void ColorChangeOff()
    {
        GetComponent<Image>().color = off;
        WeaponChargebar.color = off;
        Index.color = off;
        WeaponName.color = off;
    }

    void ColorChangeCharged()
    {
        gameObject.GetComponent<Image>().color = charged;
        WeaponChargebar.color = charged;
        Index.color = charged;
        WeaponName.color = charged;
    }

    void ColorChangeLocked()
    {
        GetComponent<Image>().color = Locked;
        WeaponChargebar.color = Locked;
        Index.color = Locked;
        WeaponName.color = Locked;
    }

    void ColorChangeSelected()
    {
        GetComponent<Image>().color = Selected;
        WeaponChargebar.color = Selected;
        Index.color = Selected;
        WeaponName.color = Selected;
    }
}
