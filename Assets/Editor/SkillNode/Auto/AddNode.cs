
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SkillEditor
{
    public class AddNode : SkillBaseNode
    {
		[Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		public byte Next = 0;
		[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		[Header("左")]
		public byte Val_0 = 0;
		[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		[Header("右")]
		public byte Val_1 = 0;

        protected override void Init()
        {
            base.Init();
            Desc = $"加法";
        }
        public override List<int> GetCode(bool isPositve)
        {
            List<int> list = new List<int>();

			var c1 = GetInputCode("Val_1");
			list.AddRange(c1);
			var c0 = GetInputCode("Val_0");
			list.AddRange(c0);
			list.Add(100);
			if(isPositve)
			{
				var c_next = GetOutputCode("Next");
				list.AddRange(c_next);
			}

            return list;
        }
    }
    
}