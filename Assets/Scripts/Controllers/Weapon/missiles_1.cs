using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class missiles_1 : WeaponController
{
    protected override void UpdateReloadAnimation(float ratio)
    {
        SpriteAtlas _sa = Managers.Resource.Load<SpriteAtlas>("Sprites/weapons/missiles_1_strip3");
        if (ratio > 1 && ratio <= 1.1f)
        {
            GetComponent<SpriteRenderer>().sprite = _sa.GetSprite("missiles_1_strip3_1");
        }
        else if (ratio >= 0 && ratio < 1)
        {
            GetComponent<SpriteRenderer>().sprite = _sa.GetSprite("missiles_1_strip3_0");
        }
    }
}
