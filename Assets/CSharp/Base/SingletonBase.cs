using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingletonBase<T> :MonoBehaviour
    where T : MonoBehaviour
{
    private static T ins;
    public static T Ins
    {
        get
        {
            if (null == ins)
            {
                ins = new GameObject(typeof(T).ToString()).AddComponent<T>();
            }
            return ins;
        }
    }
}
