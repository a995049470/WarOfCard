using UnityEngine;

namespace Battle.Event
{
    [Event(LEventType.SelectLocation)]
    public class EventData_SelectLocation : EventData
    {
        public int PlayerID;
        public Vector2Int Location;
    }
}


