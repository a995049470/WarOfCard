using System;

namespace Battle
{
    //BUFF接收者
    public interface IBuffReciver 
    {
        /// <summary>
        /// 接收buff
        /// </summary>
        /// <param name="type">buff种类</param>
        /// <param name="buffHandle">buff的句柄</param>
        /// <param name="value">数值</param>
        void RecvieBuff(Enum type,int buffHandle, int value);
        /// <summary>
        /// 移除Buff
        /// </summary>
        /// <param name="type"></param>
        /// <param name="buffHandle"></param>
        void RemoveBuff(Enum type, int buffHandle);
    }
}


