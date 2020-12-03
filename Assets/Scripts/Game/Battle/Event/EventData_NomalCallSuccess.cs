using UnityEngine;

namespace Battle.Event
{
    [Event(LEventType.NomalCallSuccess)]
    public class EventData_NomalCallSuccess : EventData
    {
        public int CardHandle;
        public Vector2Int Location;
    }
}


