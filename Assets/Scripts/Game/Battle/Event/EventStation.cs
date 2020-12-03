using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Battle.Event
{
    //用于处理的事件 
    public class EventStation
    {
        //下发数据类的异步方法
        private List<Func<int, UniTask>>[] m_asyncListeners;
        //下发数据类的同步方法
        private List<Action<int>>[] m_syncListeners;
        private static int s_eventTypeNum = -1 ;
        
        /// <summary>
        /// 初始化一个事件处理类
        /// </summary>
        /// <param name="handle">被监听对象的句柄</param>
        public EventStation()
        {
            if(s_eventTypeNum < 0)
            {
                s_eventTypeNum = Enum.GetValues(typeof(LEventType)).Length;
            }
            m_asyncListeners = new List<Func<int, UniTask>>[s_eventTypeNum];
            m_syncListeners = new List<Action<int>>[s_eventTypeNum];
            for (int i = 0; i < m_asyncListeners.Length; i++)
            {
                m_asyncListeners[i] = new List<Func<int, UniTask>>();
                m_syncListeners[i] = new List<Action<int>>();
            }
        }

        public void AddListener(LEventType type, Func<int, UniTask> func)
        {
            m_asyncListeners[type.GetHashCode()].Add(func); 
        }

        public void AddListener(LEventType type, Action<int> action)
        {
            m_syncListeners[type.GetHashCode()].Add(action); 
        }

        public void RemoveListener(LEventType type, Func<int, UniTask> func)
        {
            m_asyncListeners[type.GetHashCode()].Remove(func); 
        }

        public void RemoveListener(LEventType type, Action<int> action)
        {
            m_syncListeners[type.GetHashCode()].Remove(action); 
        }
        

        /// <summary>
        /// 异步发送广播
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataHandle"></param>
        /// <returns></returns>
        public async UniTask AsyncBroadcast(LEventType type, int dataHandle)
        {
            var funcList = m_asyncListeners[type.GetHashCode()];
            for (int i = 0; i < funcList.Count; i++)
            {
                await funcList[i].Invoke(dataHandle);
            }
        }
        
        /// <summary>
        /// 同步广播
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataHandle"></param>
        public void SyncBroadcast(LEventType type, int dataHandle)
        {
            var actionList = m_syncListeners[type.GetHashCode()];
            for (int i = 0; i < actionList.Count; i++)
            {
               actionList[i].Invoke(dataHandle);
            }
        }

        /// <summary>
        /// 先同步 后异步
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataHandle"></param>
        /// <returns></returns>
        public async UniTask Broadcast(LEventType type, int dataHandle)
        {
            SyncBroadcast(type, dataHandle);
            await AsyncBroadcast(type, dataHandle);
        }

        
    }
}


