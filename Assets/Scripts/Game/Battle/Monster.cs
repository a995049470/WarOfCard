using System;

namespace Battle
{
    //卡片拥有的怪物对象
    public class Monster : IBuffReciver
    {
       
        private Handle<Card> m_cardHandle;
        private PropData m_data;


        public Monster(int cardHadle, int attack, int life)
        {
            m_cardHandle = new Handle<Card>(cardHadle);
            m_data = new PropData();
            m_data.AddData(PropType.Attack, attack);
            m_data.AddData(PropType.MaxLife, life);
            m_data.AddData(PropType.CurLife, life);
        }

        /// <summary>
        /// 获取某条属性的值
        /// </summary>
        /// <param name="type">属性枚举</param>
        /// <returns></returns>
        public int GetPropVlaue(PropType type)
        {
            return m_data.GetData(type).GetFinalValue();
        }

        // public int GetAttack()
        // {
        //     return m_data.GetData(PropType.Attack).GetFinalValue();
        // }

        // public int GetMaxLife()
        // {
        //     return m_data.GetData(PropType.MaxLife).GetFinalValue();
        // }

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
    }
}


    