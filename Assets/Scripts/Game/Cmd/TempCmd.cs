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
}
