using System;
using System.Collections.Generic;
using Battle.Event;
using LPTC;
using UnityEngine;

namespace Battle
{

    public class PlayerModel : IStation
    {       
        public LPTCHandle Handle;
        //玩家 所有 卡组 手牌 场上 墓地 异次元 的卡片句柄
        private int m_id;
        private bool m_isActive;
        private List<int>[] m_siteCardLists;
        private int m_hp;
        private Handle<EventStation> m_stationHandle;
    
        public PlayerModel(int index, int hp)
        {
            m_id = index;
            m_hp = hp;
            Handle = new LPTCHandle();
            int count = Enum.GetValues(typeof(SiteType)).Length;
            m_siteCardLists = new List<int>[count];
            for (int i = 0; i < count; i++)
            {
                m_siteCardLists[i] = new List<int>();
            }
            m_stationHandle = new EventStation().GetHandle();
        }

        ///<summary>初始化卡组的卡片</summary>
        public void InitDeck(int[] cards)
        {
            var list = GetCardList(SiteType.Deck);
            for (int i = 0; i < cards.Length; i++)
            {
                var card = new Card(m_id, cards[i]);
                var handle = HandleManager<Card>.Instance.Put_I32(card);
                list.Add(handle);
            }
        }

        /// <summary>
        /// 获取玩家生命值
        /// </summary>
        public int GetHp()
        {
            return m_hp;
        }

        //通常召唤
        public void NormalCall(int cardHandle, Vector2Int location)
        {
            ChangeCardSite(cardHandle, SiteType.Ground);
            HandleManager<Card>.Instance.Get(cardHandle).CallMonster(cardHandle, location);
        }
        
        //TODO:区分不同情况的hp减少
        public void CutHp(int value, CutHpType type)
        {
            m_hp -= value;
        }


        //TODO:处理卡组中卡片数量不足的情况
        ///<summary>从卡组普通抽取n张卡</summary>
        public void NomalDrawCard(int count)
        {
           var deckList = GetCardList(SiteType.Deck);
           //改变卡片位置时 会改变卡片列表
           //获取所有需要抽的卡片
           int[] cards = new int[count];
           for (int i = 0; i < count; i++)
           {
               cards[i] = deckList[i];
           }
           for (int i = 0; i < count; i++)
           {
               ChangeCardSite(cards[i], SiteType.Hand);
           }
        }

        ///<summary>获取玩家某一个位置的所有卡片</summary>
        public List<int> GetCardList(SiteType type)
        {
            return m_siteCardLists[type.GetHashCode()];
        }

        // private void ChangeCardLocation(int cardHandle, Vector2Int location)
        // {
        //     HandleManager<Card>.Instance.Get(cardHandle).SetLocation(location);
        // }

        //改变卡片的位置
        public void ChangeCardSite(int ptr, SiteType targetType)
        {
            var card = HandleManager<Card>.Instance.Get(ptr);
            var otype = card.GetSiteType();
            //重新设置卡片的位置类别
            card.SetSiteType(targetType);
            ChangeCardSiteOnList(ptr, ptr, otype, targetType);
        }   

        ///<summary>改变卡片再列表中的位置</summary>
        private void ChangeCardSiteOnList(int optr, int ntpr, SiteType otype, SiteType ntype)
        {
           var olist = GetCardList(otype);
           var nlist = GetCardList(ntype);
           olist.Remove(optr);
           nlist.Add(ntpr);
        }
        
        
        public void SetActive(bool isActive)
        {
            if(m_isActive == isActive)
            {
                return;
            }
            m_isActive = isActive;   
        }

        public EventStation GetEventStation()
        {
            return m_stationHandle.Get();
        }
    }
}


