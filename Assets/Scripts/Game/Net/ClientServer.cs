using System.Collections.Generic;
using LUDP;
using System.Threading.Tasks;
using LPTC;
using System.Net;
using System;

namespace LNet
{
    enum ClientServerState
    {
        None,
        Run,
    }


    public class ClientServer
    {
        public UdpListener Listener;
        public LPTCHandle Handle;
        private Task m_task;
        private ClientServerState m_state;
        private List<Received> m_receivedList;
        private ClinetServerModel m_model;
        public IPEndPoint ReceiveIP;
        /// <summary>
        /// 服务器IP
        /// </summary>
        private IPEndPoint m_serverIp;
        
        
        public ClientServer(IPEndPoint ip)
        {
            m_state = ClientServerState.None;
            m_receivedList = new List<Received>();
            Listener = new UdpListener(ip);
            Handle = new LPTCHandle();
            m_model = new ClinetServerModel(this);
            m_serverIp = ip;
        }

        /// <summary>
        /// 获取远程IP
        /// </summary>
        public IPEndPoint GetRemoteIP()
        {
            return m_serverIp;
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
                ReceiveIP = r.sender;
                ushort id = (ushort)(r.msg[0] | r.msg[1] << 8);
                Handle.Handle(r.msg);
            }
            m_receivedList.Clear();
        }

        public void Stop()
        {
            m_receivedList.Clear();
            m_task?.Dispose();
            m_state = ClientServerState.None;
        }

        public void Start()
        {
            if(m_state == ClientServerState.Run)
            {
                return;
            }
            m_state = ClientServerState.Run;
            m_task = Task.Run(async ()=>
            {
                while (m_state == ClientServerState.Run)
                {
                    var received = await Listener.Receive();
                    m_receivedList.Add(received);
                }
            });
        }

        


    }

}
