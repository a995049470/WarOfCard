using System.Collections;
using System.Collections.Generic;
using LPTC;
using LNet;
using LSkill;
using Battle.Event;

namespace Battle
{

    public class BattleManager : Single<BattleManager>
    {
        private const int c_defaultHp = 8000;

        ///<summary>本机操作的玩家ID</summay>
        public int SelfIndex;
        private PlayerModel[] m_playerModels;
        public GroundModel BattleGroundModel;
        public TimePointModel BattleTimePointModel;
        public LBehaviorVM VM;

        //TODO:暂时只有一个消息中心
        public EventStation EStation;
        

        public BattleManager()
        {
            m_playerModels = new PlayerModel[2]
            {
                new PlayerModel(0, c_defaultHp),
                new PlayerModel(1, c_defaultHp),
            };
        
            BattleGroundModel = new GroundModel();
            BattleTimePointModel = new TimePointModel();
            VM = new LBehaviorVM();
            EStation = new EventStation();
            SelfIndex = -1;
            AddListeners(); 
            
        }
        
        public void AddListeners()
        {
            NetworkManager.Instance.Agent.Handle.AddListener(Handle_S2C_Response_LinkRoom);
            NetworkManager.Instance.Agent.Handle.AddListener(Handle_S2C_RoomState);
            NetworkManager.Instance.Agent.Handle.AddListener(Handle_C2C_Talk);
        }

        public PlayerModel GetPlayerModel(int index)
        {
            return m_playerModels[index];
        }

        public void Handle_S2C_Response_LinkRoom(S2C_Response_LinkRoom value)
        {
            SelfIndex = value.id;
        }

        public void Handle_S2C_RoomState(S2C_RoomState value)
        {
            for (int i = 0; i < m_playerModels.Length; i++)
            {
                m_playerModels[i].SetActive(value.state.GetBit(i) == 1);
            }
        }

        public void Handle_C2C_Talk(C2C_Talk value)
        {
        #if UNITY_EDITOR
            if(value.id < 0 || value.id >= m_playerModels.Length)
            {
                UnityEngine.Debug.LogError($"{value.GetType()} id : {value.id} 错误");
                return;
            }
        #endif
            m_playerModels[value.id].Handle.Handle(value);
        }

        public void Update()
        {

        }  

        
    }    
}


