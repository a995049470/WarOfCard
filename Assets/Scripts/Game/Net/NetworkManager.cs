using LPTC;
using System.Net;
namespace LNet
{
    public class NetworkManager : Single<NetworkManager>
    {
        public UdpAgent Agent;   
        public ClientServer Server;
        private bool m_isServer;
        private int m_serverPort;
        
        public NetworkManager()
        {
            Agent = new UdpAgent();
            var ip = Agent.User.GetLocalEndPoint();
            m_serverPort = new System.Random().Next(20000, 50000);
            var sip = new IPEndPoint(IPAddress.Any, m_serverPort);
            Server = new ClientServer(sip);
            m_isServer = false;
            AddListeners();
        }

        public void AddListeners()
        {
            Agent.Handle.AddListener(Handle_S2C_RoomIP);
        }

        
        public void Send_C2S_BuildRoom()
        {
            var sip = new IPEndPoint(Agent.User.GetLocalEndPoint().Address, m_serverPort);
            m_isServer = true;
            Server.Start();
            Send(new C2S_BuildRoom()
            {
                address = sip.Address.GetAddressBytes(),
                port = sip.Port,
            });
            Agent.ConenteToClinetServer(sip);
            Send(new C2S_LinkRoom());
        }

        public void Send_C2S_StartLinkRoom()
        {
            Send(new C2S_StartLinkRoom());
        }

        public void Handle_S2C_RoomIP(S2C_RoomIP value)
        {
            var sip = new IPEndPoint(new IPAddress(value.address), value.port);
            Agent.ConenteToClinetServer(sip);
            Send(new C2S_LinkRoom());
        }

        public void Send<T>(T value) where T : IToBytes
        {
            Agent.User.C2S_Send(value);
            UnityEngine.Debug.Log($"本地 :{Agent.User.GetLocalEndPoint()} 发送: {typeof(T).ToString()} 接收: {Agent.User.GetRemoteEndPoint()}");
        }

        public void Update()
        {
            Agent?.Update();
            Server?.Update();
        }
        
    }

}
