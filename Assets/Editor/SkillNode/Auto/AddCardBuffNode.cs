
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SkillEditor
{
    public class AddCardBuffNode : SkillBaseNode
    {
		[Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		public byte Next = 0;
		[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		[Header("buff句柄")]
		public byte Val_0 = 0;
		[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		[Header("目标")]
		public byte Val_1 = 0;
		[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		[Header("属性枚举")]
		public byte Val_2 = 0;
		[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		[Header("值")]
		public byte Val_3 = 0;
		[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		public byte Enter = 0;

        protected override void Init()
        {
            base.Init();
            Desc = $"buff影响卡片一条属性";
        }
        public override List<int> GetCode(bool isPositve)
        {
            List<int> list = new List<int>();

			if(!isPositve)
			{
				var c_enter = GetInputCode("Enter");
				list.AddRange(c_enter);
			}
			var c3 = GetInputCode("Val_3");
			list.AddRange(c3);
			var c2 = GetInputCode("Val_2");
			list.AddRange(c2);
			var c1 = GetInputCode("Val_1");
			list.AddRange(c1);
			var c0 = GetInputCode("Val_0");
			list.AddRange(c0);
			list.Add(119);
			if(isPositve)
			{
				var c_next = GetOutputCode("Next");
				list.AddRange(c_next);
			}

            return list;
        }
    }
    
}