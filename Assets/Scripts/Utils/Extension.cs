using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
	public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
	{
		return Util.GetOrAddComponent<T>(go);
	}

	public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
	{
		UI_Base.BindEvent(go, action, type);
	}

	public static bool IsValid(this GameObject go)
	{
		return go != null && go.activeSelf;
	}

    public static void InitializeArray<T>(this T[] array) where T : class, new()
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = new T();
        }
    }

    public static T[] InitializeArray<T>(int length) where T : class, new()
    {
        T[] array = new T[length];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = new T();
        }

        return array;
    }
}
