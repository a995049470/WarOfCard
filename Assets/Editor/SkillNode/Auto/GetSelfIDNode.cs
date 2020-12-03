
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SkillEditor
{
    public class GetSelfIDNode : SkillBaseNode
    {
		[Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
		public byte Next = 0;

        protected override void Init()
        {
            base.Init();
            Desc = $"获取自己ID";
        }
        public override List<int> GetCode(bool isPositve)
        {
            List<int> list = new List<int>();

			list.Add(103);
			if(isPositve)
			{
				var c_next = GetOutputCode("Next");
				list.AddRange(c_next);
			}

            return list;
        }
    }
    
}