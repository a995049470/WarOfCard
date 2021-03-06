﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using LSkill;
using System.IO;
using UnityEditor;

namespace SkillEditor
{
    public class NodeBuilder 
    {
        
        private static string m_savePath = $"{Application.dataPath}/Editor/SkillNode/Auto";

        public static string DefalutContent()
        {
            string content = @"
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SkillEditor
{
    public class {node_name} : SkillBaseNode
    {
{var_fileds}
        protected override void Init()
        {
            base.Init();
            Desc = $""{desc}"";
        }
        public override List<int> GetCode(bool isPositve)
        {
            List<int> list = new List<int>();

{var_codes}
            return list;
        }
    }
    
}";
            return content;
        }
        [MenuItem("Tool/生成技能节点")]
        public static void BulidNode()
        {
            ClearNode();
            var type = typeof(LBehaviorVM);
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            for (int i = 0; i < methods.Length; i++)
            {
                BuilderNode(methods[i]);
            }
            AssetDatabase.Refresh();
        }

        [MenuItem("Tool/清除技能节点")]
        public static void ClearNode()
        {
            var fs = Directory.GetFiles(m_savePath);
            for (int i = 0; i < fs.Length; i++)
            {
                File.Delete(fs[i]);
            }
            AssetDatabase.Refresh();
        }

        public static void BuilderNode(MethodInfo method)
        {   
            var skillInfo = method.GetCustomAttribute<SkillNodeInfoAttribute>();
            bool isCreate = skillInfo?.IsAuto ?? false;
            if(!isCreate)
            {
                return;
            }
            string var_fileds = "";
            string var_codes = "";
            string method_name = method.Name;
            string desc = skillInfo.Desc;
            string node_name = $"{method.Name}Node";
            string content = DefalutContent();
            if(skillInfo.IsHasNext)
            {
                var_fileds += "\t\t[Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]\n" +
                              "\t\tpublic byte Next = 0;\n";
            }

            if(skillInfo.Ports.Length > 0)
            {
                int cnt = skillInfo.Ports.Length;
                for (int i = 0; i < cnt; i++)
                {
                    var port = skillInfo.Ports[i];
                    var_fileds += "\t\t[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]\n" +
                                  $"\t\t[Header(\"{port}\")]\n" + $"\t\tpublic byte Val_{i} = 0;\n";
                    var_codes = $"\t\t\tvar c{i} = GetInputCode(\"Val_{i}\");\n" + $"\t\t\tlist.AddRange(c{i});\n{var_codes}"; 
                }
            }

            if(skillInfo.IsHasEnter)
            {
                var_fileds += "\t\t[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]\n" +
                              "\t\tpublic byte Enter = 0;\n";
            }   
            var_codes = $"{var_codes}\t\t\tlist.Add({skillInfo.Id});\n"; 
            if(skillInfo.IsHasEnter)
            {
                string str = $"\t\t\t\tvar c_enter = GetInputCode(\"Enter\");\n" + $"\t\t\t\tlist.AddRange(c_enter);\n"; 
               str = $"\t\t\tif(!isPositve)\n\t\t\t{{\n{str}\t\t\t}}\n";
                var_codes = $"{str}{var_codes}";
            }
            if(skillInfo.IsHasNext)
            {
                string str = $"\t\t\t\tvar c_next = GetOutputCode(\"Next\");\n" + $"\t\t\t\tlist.AddRange(c_next);\n"; 
                str = $"\t\t\tif(isPositve)\n\t\t\t{{\n{str}\t\t\t}}\n";
                var_codes = $"{var_codes}{str}";
            }
                                
            content = content.Replace("{var_fileds}", var_fileds).Replace("{method_name}", method_name)
                             .Replace("{desc}", desc).Replace("{node_name}", node_name)
                             .Replace("{var_codes}", var_codes);
            string path = $"{m_savePath}/{node_name}.cs";
            File.WriteAllText(path, content);    
        }
    }
}
