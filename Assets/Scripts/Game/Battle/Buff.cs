using System.Collections.Generic;
namespace Battle
{

    /// <summary>
    /// buff的影响
    /// </summary>
    public class Buff
    { 
        //发起者
        private Handle<CardEffect> m_launcher;
        //buff接收者
        private Handle<IBuffReciver> m_reciver;
        private LBehavior m_behavior;
        // //添加buff时执行
        // private int[] m_codes_enter;
        //刷新buff时执行(包括第一次执行buff效果)
        private int[] m_codes_refresh;
        //解除buff时执行
        private int[] m_codes_exit;

        /// <summary>
        /// 构建buff
        /// </summary>
        /// <param name="launcher"></param>
        /// <param name="reciver"></param>
        /// <param name="codes_refresh"></param>
        /// <param name="codes_exit"></param>
        public Buff(int launcher, int reciver, int[] codes_refresh, int[] codes_exit)
        {
            m_launcher = new Handle<CardEffect>(launcher);   
            m_reciver = new Handle<IBuffReciver>(reciver);
            m_codes_refresh = codes_refresh ?? new int[0];
            m_codes_exit = codes_exit ?? new int[0];
            var playerID = m_launcher.Get().GetCard().GetPlayerID();
            m_behavior = new LBehavior();
            m_behavior.SetData(LBehavior.DataType.PlayerID, playerID);
        }

        /// <summary>
        /// 将自身句柄存入behavior中
        /// </summary>
        public void SaveSelfHandleData(int handle)
        {
            m_behavior.SetData(LBehavior.DataType.BuffHandle, handle);
        }

        /// <summary>
        /// 刷新buff的行为
        /// </summary>
        /// <param name="buffHandle">当前这个buff的句柄</param>
        public void ExecuteCodes_Refresh()
        {  
            m_behavior.SetCodes(m_codes_refresh);
            m_behavior.SyncExecute();
        }


        /// <summary>
        /// 解除buff的行为
        /// </summary>
        /// <param name="buffHandle">当前这个buff的句柄</param>
        public void ExecuteCodes_Exit()
        {
        
            
        }

        
    }
}


