using Battle;
using Game.Common;
using LNet;
using LPTC;
using UnityEngine;

namespace Game.Cmd
{
    [Cmd][Temp]
    public class LinkTest
    {
        /// <summary>
        /// 创建两个玩家链接服务器
        /// </summary>
        [Cmd]
        private static void TwoPlayerLinkServer()
        {
            NetworkManager.Instance.ServerStart();
            var ip = NetworkManager.Instance.Server.GetRemoteIP();
            var a0 = NetworkManager.Instance.Agent;
            var data = new LPTC.S2C_RoomIP()
            {
                address = ip.Address.GetAddressBytes(),
                port = ip.Port
            };
            a0.ConenteToClinetServer(ip);
            a0.Handle_S2C_RoomIP(data);
            UdpAgent a1 = new UdpAgent();
            a1.ConenteToClinetServer(ip);
            a1.Handle_S2C_RoomIP(data);
            
            BattleManager.Instance.GetPlayerModel(0);

            void TalkTest(int i, C2C_Talk talk)
            {
                Debug.Log($"{i}:{talk.talk}".AppendColor(Color.green));
            }

            BattleManager.Instance.GetPlayerModel(0).Handle.AddListener(x=>TalkTest(0, x));
            BattleManager.Instance.GetPlayerModel(1).Handle.AddListener(x=>TalkTest(1, x));
        }

        [Cmd]
        private static void SelfSendTalk(string value)
        {
            var data = new C2C_Talk();
            data.id = 0;
            data.talk = value;
            NetworkManager.Instance.Agent.Send(data);
        }

        [Cmd]
        private static void OpponentSendTalk(string value)
        {
            var data = new C2C_Talk();
            data.id = 1;
            data.talk = value;
            NetworkManager.Instance.Agent.Send(data);
        }


    }
}
