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
            m_serverPort = new System.Random().Next(20000, 50000);
            var sip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), m_serverPort);
            Server = new ClientServer(sip);
            m_isServer = false;
        }

        public void Update()
        {
            Agent?.Update();
            Server?.Update();
        }

        /// <summary>
        /// 开启服务器
        /// </summary>
        public void ServerStart()
        {
            m_isServer = true;
            Server.Start();
        }
        
    }

}
