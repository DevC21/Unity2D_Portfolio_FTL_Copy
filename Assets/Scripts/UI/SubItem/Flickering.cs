using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flickering : MonoBehaviour
{
    Image image;
    [SerializeField]
    Text text;

    bool updown = true;
    float a = 100;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if(updown)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, a / 255f);
            if (text != null)
                text.color = new Color(text.color.r, text.color.g, text.color.b, a / 255f);
            a += Time.deltaTime * 60;
        }
        else if(!updown)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, a / 255f);
            if (text != null)
                text.color = new Color(text.color.r, text.color.g, text.color.b, a / 255f);
            a -= Time.deltaTime * 60;
        }
        SetUpDown();
    }

    void SetUpDown()
    {
        if (a / 255f <= 0.6)
            updown = true;
        if (a / 255f >= 1)
            updown = false;
    }


}
