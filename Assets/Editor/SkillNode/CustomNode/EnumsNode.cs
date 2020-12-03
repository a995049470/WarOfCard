using System;
using System.Collections.Generic;

namespace SkillEditor
{

    public abstract class EnumsNode<T> : SkillBaseNode where T : Enum
    {
        [Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
        public byte Next = 0;
        public List<T> ValueList = new List<T>();
        protected override void Init()
        {
            base.Init();
            Desc = $"{typeof(T).Name}集合 节点";
        }
        public override List<int> GetCode(bool isPositive)
        {
            var list = new List<int>();
            list.Add(LSkill.LBehaviorVM.PushValuesCode);
            int cnt = ValueList.Count;
            list.Add(cnt);
            for (int i = 0; i < cnt; i++)
            {
                list.Add(ValueList[i].GetHashCode());
            }
            if(isPositive)
            {
                var c_next = GetOutputCode("Next");
                list.AddRange(c_next);
            }
            return list;
        }
    }

}
