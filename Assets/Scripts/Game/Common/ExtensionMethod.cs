using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Common
{
    public static class ExtensionMethod
    {
        public static int[] ToIntArray(this Vector2Int v2)
        {
            var res = new int[2];
            res[0] = v2.x;
            res[1] = v2.y;
            return res;
        }

        public static Vector2Int ToVector2Int(this int[] nums)
        {
#if UNITY_EDITOR
            if (nums.Length != 2)
            {
                Debug.Log($"Array<int> 无法转成 Vector2Int");
                return Vector2Int.zero;
            }
#endif
            var res = new Vector2Int(nums[0], nums[1]);
            return res;
        }

        public static Vector3 GetColorRGB(this Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }

        public static void Foreach<T>(this T[] array, Action<T> action)
        {
            var len = array.Length;
            for (int i = 0; i < len; i++)
            {
                action?.Invoke(array[i]);
            }
        }

        public static void SetBit(ref this int n, int i, int v)
        {
            int t = i;
            if (v == 1)
            {
                n = n | (1 << t);
            }
            else if (v == 0)
            {
                n = n & ~(1 << t);
            }
        }

        public static int GetBit(ref this int n, int i)
        {
            int t = i;
            var res = (n & (1 << t)) >> t;
            return res;
        }

        public static Handle<T> GetHandle<T>(this T value) where T : class
        {
            return HandleManager<T>.Instance.Put(value);
        }

        public static int GetHandle_I32<T>(this T value) where T : class
        {
            return HandleManager<T>.Instance.Put(value).GetHandle_I32();
        }

        public static T Get<T>(this int value) where T : class
        {
            return HandleManager<T>.Instance.Get(value);
        }

        public static void Free<T>(this int value) where T : class
        {
            HandleManager<T>.Instance.Free(value);
        }

        /// <summary>
        /// 给字符串添加颜色
        /// </summary>
        /// <param name="value"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string AppendColor(this string value, Color color)
        {
            return $"<color=#{color.ToHeaxColor()}>{value}</color>";
        }
        public static string AppendColor(this string value, string color)
        {
            return $"<color=#{color}>{value}</color>";
        }

        public static string ToHeaxColor(this Color color)
        {
            int cnt = 3;
            string[] parts = new string[cnt];
            for (int i = 0; i < cnt; i++)
            {
                var str = Convert.ToString(Mathf.RoundToInt(color[i] * 255), 16);
                if(str.Length == 1) str = $"0{str}";
                parts[i] = str;
            }
            return $"{parts[0]}{parts[1]}{parts[2]}";
        }

        public static int SameCharCount(this string str, char c)
        {
            int cnt = 0;
            int len = str?.Length ?? 0;
            for (int i = 0; i < len; i++)
            {
                if(str[i] == c)
                {
                    cnt ++;
                }
            }
            return cnt;
        }
    }

}

