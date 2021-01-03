using System;
using UnityEngine;

namespace Game.Windows
{
    public abstract class Window
    {
        protected GameObject m_window;

        public Window()
        {
            
        }


        /// <summary>
        /// 初始化组件
        /// </summary>
        protected virtual void Init()
        {

        }

        public virtual void Open()
        {
            m_window.SetActive(true);
        }

        public virtual void Close()
        {
            m_window.SetActive(false);
        }

        public virtual void SendMsg(MsgData data)
        {

        }
    }
}

