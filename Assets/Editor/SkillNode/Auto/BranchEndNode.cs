
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SkillEditor
{
    public class BranchEndNode : SkillBaseNode
    {
		[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		public byte Enter = 0;

        protected override void Init()
        {
            base.Init();
            Desc = $"分支结束";
        }
        public override List<int> GetCode(bool isPositve)
        {
            List<int> list = new List<int>();

			if(!isPositve)
			{
				var c_enter = GetInputCode("Enter");
				list.AddRange(c_enter);
			}
			list.Add(129);

            return list;
        }
    }
    
}