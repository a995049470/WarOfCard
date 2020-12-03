using System;
using System.Collections.Generic;
using Battle.Event;
using Config;
using UnityEngine;

namespace Battle
{

    //所有卡片 和 场地上的可交互单位全部抽象为Card
    //Card 管理关于卡片的一切
    public class Card : IStation, IBuffReciver
    {
        public enum Prop 
        {
            
        }
        ///<summarry>卡片配置ID</summary>
        private int m_cardConfigID;
        private CardConfig m_config;
        //卡片位置
        private SiteType m_siteType;
        //当卡片再场上时具备的位置属性
        private Vector2Int m_location;
        //卡片所有效果的句柄集合
        private Handle<CardEffect>[] m_cardEffects;
        private int m_playerID;
        private Handle<EventStation> m_StationHandle;
        //怪兽句柄
        private Handle<Monster> m_monsterHandle;
        private PropData m_data;
        

        public Card(int playerID, int configID)
        {
            m_playerID = playerID;
            m_cardConfigID = configID;
            m_siteType = SiteType.Deck;
            m_StationHandle = HandleManager<EventStation>.Instance.Put(new EventStation());
            m_data = new PropData();
        }

        /// <summary>
        /// 获取持有者ID
        /// </summary>
        public int GetPlayerID()
        {
            return m_playerID;
        }

        /// <summary>
        /// 获取持有者的管理类
        /// </summary>
        public PlayerModel GetPlayerModel()
        {
            return BattleManager.Instance.GetPlayerModel(m_playerID);
        }

        ///<summary>获取卡片的位置</summary>
        public SiteType GetSiteType()
        {
            return m_siteType;
        }

    

        ///<summary>设置卡片的位置</summary>
        public void SetSiteType(SiteType type)
        {
            m_siteType = type;
        }    

        //设置卡片位置
        public void SetLocation(Vector2Int location)
        {
            m_location = location;
        }

        //增加怪兽对象
        public void CallMonster(int cardHandle, Vector2Int location)
        {
            var monster = new Monster(cardHandle, m_config.Attack, m_config.Lifte);
            m_monsterHandle = HandleManager<Monster>.Instance.Put(monster);
            m_location = location;
        }

        public EventStation GetEventStation()
        {
            return m_StationHandle.Get();
        }

        
        public void RecvieBuff(Enum type, int buffHandle, int value)
        {
            var prop = m_data.GetData(type);
            prop.SetBuffValue(buffHandle, value);
        }

        
        public void RemoveBuff(Enum type, int buffHandle)
        {
            var prop = m_data.GetData(type);
            prop.RemoveBuffValue(buffHandle);
        }

        public int GetMonsterHandle()
        {
            return m_monsterHandle.GetHandle_I32();
        }
    }
}


