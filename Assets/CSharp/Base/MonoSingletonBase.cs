using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBase<T> where T : new()
{
    private static T ins;
    public static T Ins
    {
        get
        {
            if (null == ins)
            {
                ins = new T();
            }
            return ins;
        }
    }
}
