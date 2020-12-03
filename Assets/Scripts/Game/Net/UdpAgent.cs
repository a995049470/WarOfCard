using System.Collections.Generic;
using LUDP;
using System.Threading.Tasks;
using LPTC;
using System.Net;
using System;
using System.Threading;

namespace LNet
{
    enum ConnectType
    {
        None, 
        Cloud,
        ClinetServer,
    }
    public class UdpAgent
    {
        public UdpUser User;
        public int ID;
        public LPTCHandle Handle;
        private Task m_cloudTask;
        private Task m_serverTask;
        private ConnectType m_connectType;
        private List<Received> m_receivedList;
        private bool m_isReceive;
        private CancellationTokenSource m_cts;

        public UdpAgent()
        {
            m_connectType = ConnectType.None;
            m_receivedList = new List<Received>();
            m_isReceive = true;
            m_cts = new CancellationTokenSource();
            Handle = new LPTCHandle();
            ConenteToCloud();
        }

        public void Update()
        {
            if(m_receivedList.Count == 0)
            {
                return;
            }
            for (int i = 0; i < m_receivedList.Count; i++)
            {
                var r = m_receivedList[i];
                int start = 0;
                var id = Helper.To_ushort(r.msg, ref start);
                UnityEngine.Debug.Log($"接收到了: {(LPTCType)id}");
                Handle.Handle(r.msg);

            }
            m_receivedList.Clear();
            
        }

        public void ConenteToCloud()
        {
            if(m_connectType == ConnectType.Cloud)
            {
                return;
            }
            ClearClinet();
            m_isReceive = true;
            m_connectType = ConnectType.Cloud;
            User = UdpUser.ConnectToServer();

            m_cloudTask = m_cloudTask ?? Task.Run(async ()=>
            {
                while (true)
                {
                    var received = await User?.Receive();
                    m_receivedList.Add(received);
                }
            });
           
        }

        public void ConenteToClinetServer(IPEndPoint ip)
        {
            if(m_connectType == ConnectType.ClinetServer)
            {
                return;
            }
            ClearClinet();
            m_isReceive = true;
            m_connectType = ConnectType.ClinetServer;
            User = UdpUser.ConnectTo(ip);
            
            m_serverTask = m_serverTask ?? Task.Run(async ()=>
            {
                while (true)
                {
                    var received = await User?.Receive();
                    m_receivedList.Add(received);
                }
            });
        }

        public void ClearClinet()
        {
            //m_isReceive = false;
            // if(m_task != null)
            // {
            //     //m_ctoken.ThrowIfCancellationRequested();
            //     m_cts.Cancel();
            //     //m_task?.Wait();
            //     //m_task?.Dispose();
            //     m_task = null;
            // }
            m_receivedList.Clear();
            User?.Dispose();
            User = null;
        }
    }

}
