using System.Collections.Generic;
using Battle;
using LSkill;
using UnityEngine;

namespace SkillEditor
{
    public class SingleBuffNode : SkillBaseNode
    {
        [System.Serializable]
        public class PropInfo
        {
            public PropType type;
            public int value;
            public int[] ToIntArrary()
            {
                return new int[2]{ type.GetHashCode(), value };
            }
        }
        [Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
        public byte Next = 0;
        [Header("属性集合")]
        public List<PropInfo> Val_0;
        protected override void Init()
        {
            base.Init();
            Desc = $"buff影响的属性";
        }
         public override List<int> GetCode(bool isPositive)
         {
            var list = new List<int>();
            list.Add(LBehaviorVM.PushValuesCode);
            list.Add(list.Count * 2);
            Val_0.ForEach(x=>
            {
                list.AddRange(x.ToIntArrary());
            });
            if(isPositive)
            {
                var c_next = GetOutputCode("Next");
                list.AddRange(c_next);
            }
            return list;
         }
    }

}
