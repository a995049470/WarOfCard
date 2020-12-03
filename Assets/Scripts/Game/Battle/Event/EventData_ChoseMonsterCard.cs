namespace Battle.Event
{
    [Event(LEventType.ChoseMonsterCard)]
    public class EventData_ChoseMonsterCard : EventData
    {
        public int PlayerID;
        public int CardHanle;
    }
}


