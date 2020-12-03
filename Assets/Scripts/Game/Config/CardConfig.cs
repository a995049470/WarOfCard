using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    public class CardConfig : IConfig
    {
        public int ID;
        public int CardName;
        public CardType Type;
        public string Desc;
        
        public int Attack;
        public int Lifte;
        public int StartNum;
        //效果合集
        public List<int> Effects;
        
    }    
}


