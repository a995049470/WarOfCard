
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SkillEditor
{
    public class GetDataNode : SkillBaseNode
    {
		[Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		public byte Next = 0;
		[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		[Header("数据类型")]
		public byte Val_0 = 0;

        protected override void Init()
        {
            base.Init();
            Desc = $"获取数据";
        }
        public override List<int> GetCode(bool isPositve)
        {
            List<int> list = new List<int>();

			var c0 = GetInputCode("Val_0");
			list.AddRange(c0);
			list.Add(125);
			if(isPositve)
			{
				var c_next = GetOutputCode("Next");
				list.AddRange(c_next);
			}

            return list;
        }
    }
    
}