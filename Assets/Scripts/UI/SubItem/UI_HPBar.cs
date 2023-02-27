using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        HPBar
    }

    [SerializeField]
    float ratio;

    ShipStat _shipstat;

    public override void Init()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;

        Bind<GameObject>(typeof(GameObjects));
        _shipstat = transform.parent.GetComponent<ShipStat>();
    }

    private void Update()
    {
        ratio = _shipstat.Hp / (float)_shipstat.MaxHp;
        SetHpRatio(ratio);
    }

    public void SetHpRatio(float ratio)
    {
        var hpBar = GetObject((int)GameObjects.HPBar);
        hpBar.GetComponent<Image>().fillAmount = ratio;

        if (ratio > 0.66f && ratio <= 1.0f)
        {
            hpBar.GetComponent<Image>().color = new Color(4/255f, 255/255f, 0);
        }
        else if (ratio > 0.33f && ratio <= 0.66f)
        {
            hpBar.GetComponent<Image>().color = new Color(255/255f, 255/255f, 0);
        }
        else if (ratio >= 0 && ratio <= 0.33f)
        {
            hpBar.GetComponent<Image>().color = new Color(255/255f, 11/255f, 0);
        }
        else
        {
            hpBar.GetComponent<Image>().color = new Color(4/255f, 255/255f, 0);
        }
    }
}