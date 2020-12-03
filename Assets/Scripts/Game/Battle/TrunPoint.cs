namespace Battle
{
    public enum TrunPoint
    {
        None            = 0,
        /// <summary>游戏开始(执行一次)</summary>
        GameStart       = 1,
        TrunStart       = 2,
        PickCard        = 3,
        Main_1st        = 4,
        Action          = 5,
        Main_2st        = 6,
        TurnEnd         = 7,
        /// <summary>游戏结束(只执行一次)</summary>
        GameEnd         = 9,
    }
}


