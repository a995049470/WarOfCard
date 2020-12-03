using System.Collections.Generic;
using Battle.Event;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle
{
    
    ///<summary>用于描述一个行为</summary>
    public class LBehavior
    {
        public enum DataType
        {
            PlayerID,
            CardHandle,
            CardEffect,
            EventDataInfo,
            BuffHandle,
        }

        ///<summary>行为字节码</summary>
        private int[] m_codes;
        //private CodeType m_codeType;
        //字节码指针
        private int m_ptr;
        //对象集合
        private Stack<int> m_valueStack;
        //用以描述对象集合的连续性
        private Stack<int> m_continuityStack;
        //是否成功运行
        //如果这是一个条件判定行为 只要该字段为false 就结束判定行为
        private bool m_isSuccess;
        //外界传输的数据字典 key:类型 value: int值 或者句柄
        private Dictionary<DataType, int> m_dataDic;

        
        public LBehavior(int playerID, int[] codes) 
        {
            m_ptr = 0;
            m_codes = codes ?? new int[0];
            m_valueStack = new Stack<int>();
            m_continuityStack = new Stack<int>();
            m_dataDic = new Dictionary<DataType, int>();
            SetPlayerID(playerID);
        }

        public LBehavior()
        {
            m_ptr = 0;
            m_codes = new int[0];
            m_valueStack = new Stack<int>();
            m_continuityStack = new Stack<int>();
            m_dataDic = new Dictionary<DataType, int>();
            SetPlayerID(0);
        }

       

        
        /// <summary>
        /// 向外源数据字典中增加值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="handle"></param>
        public void SetData(DataType type, int handle)
        {
            m_dataDic[type] = handle;
        }        

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetData(DataType type)
        {
            if(!m_dataDic.TryGetValue(type,out int value))
            {
                value = 0;
            }
            return value;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T GetData<T>(DataType type) where T : class
        {
            if(!m_dataDic.TryGetValue(type,out int value))
            {
                value = 0;
            }
            return HandleManager<T>.Instance.Get(value);
        }
        
        /// <summary>
        /// 虚拟机开始执行之前的初始化
        /// </summary>
        public void Init()
        {
            m_ptr = 0;
            m_isSuccess = true;
            m_valueStack.Clear();
            m_continuityStack.Clear();
        }

        public async UniTask AsynceExecute()
        {
           await BattleManager.Instance.VM.AsyncExecuteBehaviour(this);
        }
        
        public void SyncExecute()
        {
            BattleManager.Instance.VM.SyncExecuteBehaviour(this);
        }

        /// <summary>
        /// 获取当前执行结果 是否成功
        /// </summary>
        public bool GetExcuteResult()
        {
            return m_isSuccess;
        }

        /// <summary>
        /// 执行失败
        /// </summary>
        public void SetExecuteFail()
        {
            m_isSuccess = false;
        }

        /// <summary>
        /// 将指针移到最后一位
        /// </summary>
        public void MovePtrToEnd()
        {
            m_ptr = m_codes.Length;
        }
        
        /// <summary>
        /// 改变行为对应的字节码
        /// </summary>
        public void SetCodes(int[] codes)
        {
            m_codes = codes ?? new int[0];
        }

        /// <summary>
        /// 设置发动玩家
        /// </summary>
        public void SetPlayerID(int playerID)
        {
            SetData(DataType.PlayerID, playerID);
        }
        
        /// <summary>
        /// 设置卡片句柄
        /// </summary>
        public void SetCardHandle(int cardHandle)
        {
            SetData(DataType.CardHandle, cardHandle);
        }
        
        
        ///<summary> 获取玩家ID</summary>
        public int GetPlayerID()
        {
            return m_dataDic[DataType.PlayerID];
        }

        ///<summary> 获取字节码</summary>
        public int[] GetCodes()
        {
            return m_codes;
        }

        /// <summary>
        /// 获取卡片句柄
        /// </summary>
        public int GetCardHandle()
        {
            return m_dataDic[DataType.CardHandle];
        }
        
        ///<summary> 获取一个code, ptr += 1</summary>
        public int PopCode()
        {
            return m_codes[m_ptr++];
        }
        ///<summary> 获取一段code, ptr += count</summary>
        public int[] PopCodes(int count)
        {
            var vlaues = new int[count];
            for (int i = 0; i < count; i++)
            {
                vlaues[i] = m_codes[m_ptr++];
            }
            return vlaues;
        }

        public bool IsEnd()
        {
            return m_ptr >= m_codes.Length || !m_isSuccess;
        }

         ///<summary>向值栈顶放入一个数字</summary>
        public void PushValue(int value)
        {
            m_valueStack.Push(value);
            m_continuityStack.Push(1);
        }
        ///<summary>向值栈顶放入一段数字</summary>
        public void PushValues(int[] values)
        {
            int cnt = values.Length;
            for (int i = cnt - 1; i >= 0; i--)
            {
                m_valueStack.Push(values[i]);
            }
            m_continuityStack.Push(cnt);
        }
        ///<summary>向值栈顶放入一段数字</summary>
        public void PushValues(List<int> values)
        {
            int cnt = values.Count;
            for (int i = cnt - 1; i >= 0; i--)
            {
                m_valueStack.Push(values[i]);
            }
            m_continuityStack.Push(cnt);
        }

        ///<summary>从值栈顶端取出一个值</summary>
        public int PopValue()
        {

            int cnt = m_continuityStack.Pop();
#if UNITY_EDITOR
            if(cnt != 1)
            {
                Debug.LogError("栈顶连续的数字数量不为1");
            }
#endif
            return m_valueStack.Pop();
        }

        ///<summary>从值栈顶端取出一段值 (倒着取)</summary>
        public int[] PopValues()
        {
            int cnt = m_continuityStack.Pop();
            int[] res = new int[cnt];
            for (int i = 0; i < cnt; i++)
            {
                res[i] = m_valueStack.Pop();
            }
            return res;
        }
        
        /// <summary>
        /// 获取当前指针的位置
        /// </summary>
        /// <returns></returns>
        public int GetPtr()
        {
            return m_ptr;
        }

        /// <summary>
        /// 设置当前指针的位置
        /// </summary>
        public void SetPtr(int ptr)
        {
            m_ptr = ptr;
        }
    }
}


