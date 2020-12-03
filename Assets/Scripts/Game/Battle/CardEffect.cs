using System;
using System.Collections.Generic;
using Config;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Battle
{
    public class CardEffect
    {
        //条件代码
        private int[] m_codes_condition;
        // 支付cost 并 选择对象
        private int[] m_codes_cost_chose;
        // 效果主题字节码
        private int[] m_codes_body;
        // 处理事件监听字节码
        private int[] m_codes_event;

        private LBehavior m_behavior;
        private Handle<Card> m_cardHanlde;

        public CardEffect(int cardHandle, int[] codes_condition, int[] codes_cost_chose, int[] codes_body)
        {
            m_cardHanlde = new Handle<Card>(cardHandle);
            m_codes_condition = codes_condition ?? new int[0];
            m_codes_cost_chose = codes_cost_chose ?? new int[0];
            m_codes_body = codes_body ?? new int[0];
            m_behavior = new LBehavior();
        }

        /// <summary>   
        /// 获取卡片句柄
        /// </summary>
        public int GetCardHandle()
        {
            return m_cardHanlde.GetHandle_I32();
        }

        /// <summary>
        /// 获取卡片
        /// </summary>
        public Card GetCard()
        {
            return HandleManager<Card>.Instance.Get(m_cardHanlde);
        }
        

        /// <summary>
        /// 判断是否满足发动条件
        /// </summary>
        /// <param name="cardHandle">卡片句柄</param>
        /// <param name="playerID">玩家ID</param>
        public bool IsFillCondition(int playerID, int cardHandle)
        {
            m_behavior.SetPlayerID(playerID);
            m_behavior.SetCodes(m_codes_condition);
            BattleManager.Instance.VM.SyncExecuteBehaviour(m_behavior);
            bool isFill = m_behavior.GetExcuteResult();
            return isFill;
        }


        /// <summary>
        /// 执行支付cost && 选择效果对象的字节码
        /// </summary>
        /// <param name="cardHandle">卡片句柄</param>
        /// <param name="playerID">玩家ID</param>
        public async UniTask ExecuteCodes_Cost_Chose(int playerID, int cardHandle)
        {
            m_behavior.SetPlayerID(playerID);
            m_behavior.SetCodes(m_codes_cost_chose);
            await BattleManager.Instance.VM.AsyncExecuteBehaviour(m_behavior);
        }

        /// <summary>
        /// 执行效果主体代码
        /// </summary>
        /// <param name="cardHandle">卡片句柄</param>
        /// <param name="playerID">玩家ID</param>
        public async UniTask ExecuteCodes_Body(int playerID, int cardHandle)
        {
            m_behavior.SetPlayerID(playerID);
            m_behavior.SetCodes(m_codes_body);
            await BattleManager.Instance.VM.AsyncExecuteBehaviour(m_behavior);
        }

        

    }
}


