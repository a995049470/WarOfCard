
using System;
/// <summary>
/// 记录句柄 和 对应的信息
/// </summary>
public struct NHandle
{
    private Type m_type;
    private int m_handle;

    public NHandle(Type t, int handle)
    {
        this.m_type = t;
        this.m_handle = handle;
    }
    public bool IsT<T>() where T : class
    {
        return typeof(T) == m_type;
    }
    
    public Type GetHandleType()
    {
        return m_type;
    }

    public T Get<T>() where T : class
    {
        return new Handle<T>(m_handle).Get();
    }
}
