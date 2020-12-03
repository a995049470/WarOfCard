namespace Battle.Event
{
    [Event(LEventType.ActiveLaunch)]
    public class EventData_ActiveLaunch : EventData
    {
        public int PlayerID;
    }
}


