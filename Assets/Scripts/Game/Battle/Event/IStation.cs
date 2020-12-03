namespace Battle.Event
{
    /// <summary>
    /// 有个接口的都有单独的事件分发站
    /// </summary>
    public interface IStation
    {
        EventStation GetEventStation();
    }
}


