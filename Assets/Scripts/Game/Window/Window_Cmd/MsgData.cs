using System;

namespace Game.Windows
{
    public class MsgData
    {
        private Enum m_enum;
        private object m_value;
        public MsgData(Enum e, object o = null)
        {
            m_enum = e;
            m_value = o;
        }

        public T GetEnum<T>() where T : Enum
        {
            return (T)m_enum;
        }

        public T GetValue<T>()
        {
            return (T)m_value;
        }
    }
}

