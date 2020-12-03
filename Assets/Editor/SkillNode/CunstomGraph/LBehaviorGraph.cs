using System.Collections.Generic;
using SkillEditor;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class LBehaviorGraph : NodeGraph
{
    public List<int> GetCode()
    {
        List<int> code = null;
        BehaviorStartNode node = null;
        for (int i = 0; i < nodes.Count; i++)
        {
            node = nodes[i] as BehaviorStartNode;
            if(node != null)
            {
                break;
            }
        }
        if(node != null)
        {
            code = node.GetCode(true);
        }
        else
        {
            code = new List<int>();
        }
        return code;
    }
}
