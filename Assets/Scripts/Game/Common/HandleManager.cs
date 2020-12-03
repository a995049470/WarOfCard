using System;
using System.Collections.Generic;


public class HandleManager<T> : Single<HandleManager<T>> where T : class
{
    private List<T> m_dataList;
    private List<UInt16> m_magicList;
    private Stack<UInt16> m_freeSoltStack;
    
    public HandleManager()
    {
        m_dataList = new List<T>();
        m_magicList = new List<UInt16>();
        m_freeSoltStack = new Stack<UInt16>();
    }

    public T Get(Handle<T> handle)
    {
        T value = null;
        var index = handle.GetIndex();
        if(index < m_dataList.Count && 
           m_magicList[index] == handle.GetMagic())
        {
            value = m_dataList[index];
        }
        return value;
    }
    
    public bool IsInvalidHandle(int ptr)
    {
        bool isInvalid = Get(ptr) == null;
        return isInvalid;
    }

    public T Get(int ptr)
    {
        Handle<T> handle = new Handle<T>(ptr);
        T value = Get(handle);
        return value;
    }

    public Handle<T> Put(T value)
    {
        Handle<T> handle = default;
        if(m_freeSoltStack.Count > 0)
        {
            UInt16 index = m_freeSoltStack.Pop();
            handle = new Handle<T>(index);
            m_magicList[index] = handle.GetMagic();
            m_dataList[index] = value;
        }
        else
        {
            UInt16 index = (UInt16)m_dataList.Count;
            handle = new Handle<T>(index);
            m_magicList.Add(handle.GetMagic());
            m_dataList.Add(value);
        }
        return handle;
    }

    public Int32 Put_I32(T value)
    {
        return Put(value).GetHandle_I32();
    }

    public bool TryRePut(Handle<T> handle, out Handle<T> reHandle)
    {
        bool isRePut = false;
        T value = Get(handle);
        reHandle = new Handle<T>(0);
        if(value != null)
        {
            isRePut = true;
            Free(handle);
            reHandle = Put(value);
        }
        return isRePut;
    }

    public bool TryRePut(int ptr, out int rePtr)
    {
        bool isRePut = false;
        rePtr = 0;
        T value = Get(ptr);
        if(value != null)
        {
            isRePut = true;
            Free(ptr);
            rePtr = Put_I32(value);
        }
        return isRePut;
    }
    
    /// <summary>
    /// 释放变量的唯一引用(临时变量一定要在域内的时候free)
    /// </summary>
    /// <param name="handle"></param>
    public void Free(Handle<T> handle)
    {
        var index = handle.GetIndex();
        if(index < m_dataList.Count &&
           m_magicList[index] == handle.GetMagic())
        {
            m_magicList[index] = 0;
            m_dataList[index] = null;
            m_freeSoltStack.Push(index);
        }
    }

     /// <summary>
    /// 释放变量的唯一引用(临时变量一定要在域内的时候free)
    /// </summary>
    /// <param name="handle"></param>
    public void Free(int ptr)
    {
        Free(new Handle<T>(ptr));
    }
    

}
