using Game.Common;
using LPTC;
using System.Net;
namespace LNet
{
    public class ClinetServerModel
    {
        private IPEndPoint[] m_playerIPs;
        /// <summary>
        /// 用位来表示 是否有人链接
        /// </summary>
        private int m_state;
        private ClientServer m_server;
        public ClinetServerModel(ClientServer server)
        {
            m_playerIPs = new IPEndPoint[2];
            m_state = 0;
            m_server = server;
            AddListeners();
        }

        private void AddListeners()
        {
            m_server.Handle.AddListener(Handle_C2S_LinkRoom);
            m_server.Handle.AddListener(Handle_C2CTalk);
        }

        private void Send<T>(IPEndPoint ip, T value) where T : IToBytes
        {
            UnityEngine.Debug.Log($"ClientServer 发送 {typeof(T).ToString()} 到 {ip}");
            m_server.Listener.S2C_Send(ip, value);
        } 
        /// <summary>
        /// 处理链接协议
        /// </summary>
        /// <param name="value"></param>
        private void Handle_C2S_LinkRoom(C2S_LinkRoom value)
        {
            int index = -1;
            var ip = m_server.ReceiveIP;
            bool isAdd = TryAddPlayerIP(ip, ref index);
            Send(ip, new S2C_Response_LinkRoom()
            {
                id = index,
            });
            if (isAdd)
            {
                Borad(new S2C_RoomState()
                {
                    state = m_state
                });
            }
        }

        
        /// <summary>
        /// 处理交谈协议
        /// </summary>
        /// <param name="value"></param>
        private void Handle_C2CTalk(C2C_Talk value)
        {
            Borad(value);
        }

        /// <summary>
        /// 向所有成员广播数据
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        private void Borad<T>(T value) where T : IToBytes
        {
            for (int i = 0; i < m_playerIPs.Length; i++)
            {
                if (m_state.GetBit(i) == 0)
                {
                    continue;
                }
                var playerip = m_playerIPs[i];
                Send(playerip, value);
            }
        }

        /// <summary>
        /// 添加一个成员
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool TryAddPlayerIP(IPEndPoint ip, ref int id)
        {
            bool isAdd = false;
            for (int i = 0; i < m_playerIPs.Length; i++)
            {
                if (m_state.GetBit(i) == 0)
                {
                    id = i;
                    m_state.SetBit(i, 1);
                    m_playerIPs[i] = ip;
                    isAdd = true;
                    UnityEngine.Debug.Log($"{ip} 链接成功");
                    break;
                }
            }
            return isAdd;
        }

    }

}
