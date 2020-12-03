
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SkillEditor
{
    public class SaveDataNode : SkillBaseNode
    {
		[Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		public byte Next = 0;
		[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		[Header("数据类型")]
		public byte Val_0 = 0;
		[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		[Header("数据(只能是int)")]
		public byte Val_1 = 0;

        protected override void Init()
        {
            base.Init();
            Desc = $"拷贝数据并保存";
        }
        public override List<int> GetCode(bool isPositve)
        {
            List<int> list = new List<int>();

			var c1 = GetInputCode("Val_1");
			list.AddRange(c1);
			var c0 = GetInputCode("Val_0");
			list.AddRange(c0);
			list.Add(126);
			if(isPositve)
			{
				var c_next = GetOutputCode("Next");
				list.AddRange(c_next);
			}

            return list;
        }
    }
    
}