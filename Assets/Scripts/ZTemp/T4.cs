using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Reflection;

public class TestAsync
{

    private CancellationTokenSource m_cts;

    public TestAsync()
    {
        m_cts = new CancellationTokenSource();
    }

    public async UniTask A()
    {
        var task = UniTask.Delay(3000, false, PlayerLoopTiming.Update, m_cts.Token);

        await task;
        Debug.Log("A");
        
    }

    public async UniTask B()
    {
        await UniTask.Delay(1000);
        Debug.Log("B");
        
    }

    public async UniTask C()
    {
        await UniTask.Delay(1000);
        Debug.Log("C");
       
    }

    public async UniTask<int> D()
    {
        await UniTask.Delay(0);
        return 10;
    }

    

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_cts.Cancel();
            Debug.Log("取消令牌");
        }
    }
}

public class T4 : MonoBehaviour
{
    
   

    // Start is called before the first frame update
    async UniTask Start()
    {
        var async = new TestAsync();
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        var a = async.GetType().GetMethod("A", flags);
        var atask = (UniTask)a.Invoke(async, null);
        await atask;
        var b = async.GetType().GetMethod("B", flags);
        var btask = (UniTask)b.Invoke(async, null);
        await btask;
    }

    

    

   
}
