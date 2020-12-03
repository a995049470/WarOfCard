using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using Cysharp.Threading.Tasks;
using System.Reflection;

public class T3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        var m_a = this.GetType().GetMethod("A", flags);
        var m_b = this.GetType().GetMethod("B", flags); 
        int value = 0;
        Debug.Log($"++value:{++value}  1");
        Debug.Log($"value++:{value++}  1");
        Debug.Log($"value:{value}   2");
    }
    
    async UniTask A()
    {
        await UniTask.Yield();
    }

    void B()
    {
        Debug.Log("D");
    }
}
