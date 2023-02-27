using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotateAround : MonoBehaviour
{
    GameObject _anchor;

    public GameObject Anchor { get { return _anchor; } set { _anchor = value; } }

    void Start()
    {
        transform.position = _anchor.transform.position + new Vector3(0.2f, 0f, 0f);
    }

    void FixedUpdate()
    {
        transform.RotateAround(_anchor.transform.position, Vector3.forward, Time.deltaTime * 30);
    }
}
