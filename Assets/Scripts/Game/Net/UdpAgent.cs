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
            //ConenteToCloud();
            AddListeners();
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

        public void AddListeners()
        {
            Handle.AddListener(Handle_S2C_RoomIP);
        }


        public void ClearClinet()
        {
            m_receivedList.Clear();
            User?.Dispose();
            User = null;
        }

        public void Send_C2S_BuildRoom()
        {
            var sip = NetworkManager.Instance.Server.GetRemoteIP();
            Send(new C2S_BuildRoom()
            {
                address = sip.Address.GetAddressBytes(),
                port = sip.Port,
            });
            ConenteToClinetServer(sip);
            Send(new C2S_LinkRoom());
        }

        public void Send_C2S_StartLinkRoom()
        {
            Send(new C2S_StartLinkRoom());
        }

        public void Handle_S2C_RoomIP(S2C_RoomIP value)
        {
            var sip = new IPEndPoint(new IPAddress(value.address), value.port);
            ConenteToClinetServer(sip);
            Send(new C2S_LinkRoom());
        }

        public void Send<T>(T value) where T : IToBytes
        {
            User.C2S_Send(value);
            UnityEngine.Debug.Log($"本地 :{User.GetLocalEndPoint()} 发送: {typeof(T).ToString()} 接收: {User.GetRemoteEndPoint()}");
        }
    }

}
