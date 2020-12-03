using System;

namespace Battle.Event
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EventAttribute : Attribute
    {
        public LEventType EventType;
        public EventAttribute(LEventType type)
        {
            this.EventType = type;
        }
    }
}


