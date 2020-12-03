
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SkillEditor
{
    public class IsSuitEventNode : SkillBaseNode
    {
		[Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		public byte Next = 0;
		[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		[Header("事件类型")]
		public byte Val_0 = 0;
		[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		[Header("目标类型集合")]
		public byte Val_1 = 0;
		[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		public byte Enter = 0;

        protected override void Init()
        {
            base.Init();
            Desc = $"判断事件类型是否正确";
        }
        public override List<int> GetCode(bool isPositve)
        {
            List<int> list = new List<int>();

			if(!isPositve)
			{
				var c_enter = GetInputCode("Enter");
				list.AddRange(c_enter);
			}
			var c1 = GetInputCode("Val_1");
			list.AddRange(c1);
			var c0 = GetInputCode("Val_0");
			list.AddRange(c0);
			list.Add(122);
			if(isPositve)
			{
				var c_next = GetOutputCode("Next");
				list.AddRange(c_next);
			}

            return list;
        }
    }
    
}