using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeSystemBar : MonoBehaviour
{
    [SerializeField]
    string _systembarName;

    public string SystemBarName { get { return _systembarName; } set { _systembarName = value; } }
}
