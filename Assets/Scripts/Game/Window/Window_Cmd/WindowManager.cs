using System.Collections.Generic;
using UnityEngine;

namespace Game.Windows
{
    public class WindowManager : Single<WindowManager>
    {
        private Dictionary<string, Window> m_windowDic;

        public WindowManager()
        {
            m_windowDic = new Dictionary<string, Window>();
        }

        /// <summary>
        /// 给UI传递信息
        /// </summary>
        /// <param name="e"></param>
        /// <param name="o"></param>
        /// <typeparam name="T"></typeparam>
        public void SendMsg<T>(MsgData data) where T : Window
        {
            var key = nameof(T);
            m_windowDic[key].SendMsg(data);
        }

        public void CreateWindow<T>() where T : Window, new()
        {
            string key = nameof(T);
            m_windowDic[key] = new T();
        }
        
    }
}

