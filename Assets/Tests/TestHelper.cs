using System.Reflection;
using System;

namespace Tests
{
    public class TestHelper
    {
        private const BindingFlags c_flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        public static object GetInstanceField(object obj, string fname)
        {
            object res = null;
            var f = obj.GetType().GetField(fname, c_flags);
            res = f.GetValue(obj);
            return res;
        }

        public static void SetInstanceField(object obj, string fname, object value)
        {
            var f = obj.GetType().GetField(fname, c_flags);
            f.SetValue(obj, value);
        }   

        public static object GetStaticField(Type type, string fname)
        {
            object res = null;
            var f = type.GetField(fname, c_flags);
            res = f.GetValue(null);
            return res;
        }

        public static void SetStaticField(object obj, string fname, object value)
        {
            var f = obj.GetType().GetField(fname, c_flags);
            f.SetValue(null, value);
        }   

        public static MethodInfo GetInstanceMethod(object obj, string fname)
        {
            MethodInfo res = null;
            res = obj.GetType().GetMethod(fname, c_flags);
            return res;
        }

        public static MethodInfo GetStaticMethod(Type type, string fname)
        {
            MethodInfo res = null;
            res = type.GetMethod(fname, c_flags);
            return res;
        }
    }
}
