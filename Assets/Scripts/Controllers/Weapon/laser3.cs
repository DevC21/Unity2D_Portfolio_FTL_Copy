using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class laser3 : WeaponController
{


    SpriteAtlas _sa;

    protected override void Init()
    {
        base.Init();
        _sa = Managers.Resource.Load<SpriteAtlas>("Sprites/weapons/laser3_strip12");
    }

    protected override void UpdateReloadAnimation(float ratio)
    {
        if (ratio > 0.8f && ratio <= 1.0f)
        {
            GetComponent<SpriteRenderer>().sprite = _sa.GetSprite("laser3_strip12_5");
        }
        else if (ratio > 0.6f && ratio <= 0.8f)
        {
            GetComponent<SpriteRenderer>().sprite = _sa.GetSprite("laser3_strip12_4");
        }
        else if (ratio > 0.4f && ratio <= 0.6f)
        {
            GetComponent<SpriteRenderer>().sprite = _sa.GetSprite("laser3_strip12_2");
        }
        else if (ratio > 0.2f && ratio <= 0.4f)
        {
            GetComponent<SpriteRenderer>().sprite = _sa.GetSprite("laser3_strip12_1");
        }
        else if (ratio >= 0 && ratio <= 0.2f)
        {
            GetComponent<SpriteRenderer>().sprite = _sa.GetSprite("laser3_strip12_0");
        }
    }

    protected override void FireAnimation(float _animTime)
    {
        if (_animTime < 0.4f && _animTime >= 0.258f)
        {
            GetComponent<SpriteRenderer>().sprite = _sa.GetSprite("laser3_strip12_11");
        }
        else if (_animTime < 0.258f && _animTime >= 0.215f)
        {
            GetComponent<SpriteRenderer>().sprite = _sa.GetSprite("laser3_strip12_10");
        }
        else if (_animTime < 0.215f && _animTime >= 0.172f)
        {
            GetComponent<SpriteRenderer>().sprite = _sa.GetSprite("laser3_strip12_9");
        }
        else if (_animTime < 0.172f && _animTime >= 0.129f)
        {
            GetComponent<SpriteRenderer>().sprite = _sa.GetSprite("laser3_strip12_8");
        }
        else if (_animTime < 0.129f && _animTime >= 0.086f)
        {
            GetComponent<SpriteRenderer>().sprite = _sa.GetSprite("laser3_strip12_7");
        }
        else if (_animTime < 0.086f && _animTime >= 0.043f)
        {
            GetComponent<SpriteRenderer>().sprite = _sa.GetSprite("laser3_strip12_6");
        }
        else if(_animTime < 0.043f && _animTime >= 0)
        {
            GetComponent<SpriteRenderer>().sprite = _sa.GetSprite("laser3_strip12_5");
        }
    }
}
