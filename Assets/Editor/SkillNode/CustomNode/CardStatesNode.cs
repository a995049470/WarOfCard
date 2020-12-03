using System.Collections.Generic;
using Battle;
using UnityEngine;

namespace SkillEditor
{
    public class CardStatesNode : SkillBaseNode
    {
        [Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
        public byte Next = 0;
        [Header("时点集合")]
        public List<CardState> CardStateList;
        // [Header("是否取反")]
        // public bool IsNegate;
        protected override void Init()
        {
            base.Init();
            Desc = $"卡片状态节点";
        }
        public override List<int> GetCode(bool isPositive)
        {
            List<int> list = new List<int>();
            list.Add(0);//方法ID
            int t = 0;
            var cnt = CardStateList?.Count ?? 0;
            for (int i = 0; i < cnt; i++)
            {
                t |= 1 << CardStateList[i].GetHashCode();
            }
            list.Add(t);
            if(isPositive)
            {
                var c_next = GetOutputCode("Next");
                list.AddRange(c_next);
            }
            return list;
        }
    }

}
