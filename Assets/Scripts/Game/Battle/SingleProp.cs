using System.Collections.Generic;
namespace Battle
{
    /// <summary>
    /// 单条属性的描述
    /// </summary>
    public class SingleProp
    {
        private int m_initialValue;
        //key buff句柄 value 增加的值
        private Dictionary<int, int> m_buffDic;
        private int m_finalValue;

        public SingleProp(int initialValue)
        {
            m_initialValue = initialValue;
            m_finalValue = initialValue;
            m_buffDic = new Dictionary<int, int>();
        }

        /// <summary>
        /// 设置某个buff对改属性的影响值
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="value"></param>
        public void SetBuffValue(int handle, int value)
        {
            int oldValue = GetVlaue(handle);
            m_buffDic[handle] = value;
            ResetFinalValue(handle, oldValue);
        }
        
        /// <summary>
        /// 解除buff影响
        /// </summary>
        /// <param name="handle"></param>
        public void RemoveBuffValue(int handle)
        {
            int oldValue = GetVlaue(handle);
            m_buffDic.Remove(handle);
            ResetFinalValue(handle, oldValue);
        }

        /// <summary>
        /// 重新设置最终值
        /// </summary>
        private void ResetFinalValue(int buffHandle, int oldValue)
        {
            m_finalValue = m_finalValue - oldValue + GetVlaue(buffHandle);
        }

        /// <summary>
        /// 获取某个buff对应的数值
        /// </summary>
        /// <param name="buffHandle"></param>
        /// <returns></returns>
        public int GetVlaue(int buffHandle)
        {
            if(!m_buffDic.TryGetValue(buffHandle, out int value))
            {
                value = 0;
            }
            return value;
        }

        /// <summary>
        /// 获取该条属性最终值
        /// </summary>
        /// <returns></returns>
        public int GetFinalValue()
        {
            return m_finalValue;
        }
    }
}


    