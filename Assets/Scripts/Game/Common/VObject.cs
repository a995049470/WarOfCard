using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 将值包装成引用类型
/// </summary>
/// <typeparam name="T"></typeparam>
public class VObject<T> where T : struct
{
    private T Value;
    public VObject(T value)
    {
        this.Value = value;
    }
}
