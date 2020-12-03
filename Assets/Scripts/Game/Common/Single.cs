using System;

// public interface IToIntArray
// {
//     void ToIntArray();
// }

public abstract class Single<T> where T : class, new()
{
    
    private static T s_instance;
    public static T Instance
    {
        get
        {
            if(s_instance == null)
            {
                s_instance = new T();
            }
            return s_instance;
        }
    }
    
    public void Dispose()
    {
        s_instance = null;
    }
}