using System.Collections.Generic;

namespace SkillEditor
{   
    

    public class ValueNode : SkillBaseNode
    {
        [Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
        public byte Next = 0;
        public int Value;

        protected override void Init()
        {
            base.Init();
            Desc = $"值节点";
        }

        public override List<int> GetCode(bool isPositive)
        {
            List<int> list = new List<int>();
            list.AddRange(new int[]{ LSkill.LBehaviorVM.PushValueCode, Value});
            if(isPositive)
            {
                var c_next = GetOutputCode("Next");
                list.AddRange(c_next);
            }
            return list;
        }
    }

}
