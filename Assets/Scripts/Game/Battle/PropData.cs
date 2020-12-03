using System;
using System.Collections.Generic;

namespace Battle
{
    /// <summary>
    /// 属性数据
    /// </summary>
    public class PropData
    {
        private Dictionary<Enum, SingleProp> m_propDic;
        public PropData()
        {
            m_propDic = new Dictionary<Enum, SingleProp>();
        }

        public void AddData(Enum id, int data)
        {
            m_propDic[id] = new SingleProp(data);
        }
        
        /// <summary>
        /// 获取单一属性 若不存在 则新建
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SingleProp GetData(Enum id)
        {
            if(!m_propDic.ContainsKey(id))
            {
                AddData(id, 0);
            }
            return m_propDic[id];
        }
    }
}


    