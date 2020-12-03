
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SkillEditor
{
    public class IsEngouhHpNode : SkillBaseNode
    {
		[Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		public byte Next = 0;
		[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		[Header("数值")]
		public byte Val_0 = 0;
		[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		public byte Enter = 0;

        protected override void Init()
        {
            base.Init();
            Desc = $"检查自己HP";
        }
        public override List<int> GetCode(bool isPositve)
        {
            List<int> list = new List<int>();

			if(!isPositve)
			{
				var c_enter = GetInputCode("Enter");
				list.AddRange(c_enter);
			}
			var c0 = GetInputCode("Val_0");
			list.AddRange(c0);
			list.Add(109);
			if(isPositve)
			{
				var c_next = GetOutputCode("Next");
				list.AddRange(c_next);
			}

            return list;
        }
    }
    
}