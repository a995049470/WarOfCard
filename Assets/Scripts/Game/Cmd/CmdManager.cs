using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Game.Cmd
{
    [Cmd]
    public class TempCmd
    {
        [Cmd]
        private static void TestString(string value)
        {
            Debug.Log("String : " + value);
        }
        [Cmd]
        private static void TestFloat(string value)
        {
            Debug.Log("Float :" + value);
        }
        [Cmd]
        private static void TestEmpty()
        {
            Debug.Log("Empty");
        }
    }

    public class CmdManager : Single<CmdManager>
    {
        //命令行方法集合 暂时只考虑静态方法
        private Dictionary<string, MethodInfo> m_methodDic;
        
        public CmdManager()
        {
            m_methodDic = new Dictionary<string, MethodInfo>();
            Assembly assembly = Assembly.Load("Game");
            var types = assembly.GetTypes();
            var cmdType = typeof(CmdAttribute);
            var flag = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (var type in types)
            {
                if(type.GetCustomAttribute(cmdType) == null)
                {
                    continue;
                }
                var methods = type.GetMethods(flag);
                foreach (var method in methods)
                {
                    if(method.GetCustomAttribute(cmdType) == null)
                    {
                        continue;
                    }
                    AddMethod(method);
                }
            }
        }
        /// <summary>
        /// 是否包含字段
        /// </summary>
        /// <param name="key"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        private bool IsContainStr(string key, string part)
        {
            if(string.IsNullOrEmpty(part))
            {
                return true;
            }
            bool isContain = false;
            int t = 0;
            for (int i = 0; i < key.Length; i++)
            {
                if(key[i] == part[t])
                {
                    t++;
                }
                if(t == part.Length)
                {
                    isContain = true;
                    break;
                }
            }
            return isContain;
        }

        /// <summary>
        /// 更具部分命令 获取可能的方法列表
        /// </summary>
        /// <param name="partCmd"></param>
        /// <returns></returns>
        public List<MethodInfo> GetMethodList(string partCmd)
        {
            List<MethodInfo> res = new List<MethodInfo>();
            if(string.IsNullOrEmpty(partCmd))
            {
                return res;
            }
            string part = partCmd.Split(' ')[0].ToLower();
            foreach (var kvp in m_methodDic)
            {
                if(IsContainStr(kvp.Key, part))
                {
                    res.Add(kvp.Value);
                }
            }
            return res;
        }

        /// <summary>
        /// 添加命令行方法
        /// </summary>
        /// <param name="info">静态方法</param>
        private void AddMethod(MethodInfo info)
        {
            if(!info.IsStatic)
            {
                Debug.LogError($"{info.Name} 不是静态方法");
                return;
            }
            string key = info.Name.ToLower();
            m_methodDic[key] = info;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cmd"></param>
        public void ExcuteMethod(string cmd)
        {
            var res = cmd.Split(' ');
            var key = res[0].ToLower();
            var args = new string[res.Length - 1];
            
            //参数赋值
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = res[i + 1];
            }   


            if(m_methodDic.ContainsKey(key))
            {
                var m = m_methodDic[key];
                if(m.GetParameters().Length != args.Length)
                {
                    Debug.Log($"参数错误");
                }
                m.Invoke(null, args);
            }
            else
            {
                Debug.Log($"方法名输入错误");
            }
        }
    }
}
