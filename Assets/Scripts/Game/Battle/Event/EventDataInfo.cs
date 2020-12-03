namespace Battle.Event
{
    
    //描述一个事件数据
    public class EventDataInfo
    {
        public int Handle;
        public LEventType EventType;

        public EventDataInfo()
        {
            
        }

        public EventDataInfo(LEventType type, int handle)
        {
            this.EventType = type;
            this.Handle = handle;
        }

        public int[] ToIntArray()
        {
            return new int[]{ Handle, EventType.GetHashCode() };
        }

        public static EventDataInfo FromIntArray(int[] array)
        {
        #if UNITY_EDITOR
           UnityEngine.Debug.Assert(array?.Length == 2);
        #endif
            return new EventDataInfo()
            {
                Handle = array[0],
                EventType = (LEventType)array[1]
            };
        }
    }
}


