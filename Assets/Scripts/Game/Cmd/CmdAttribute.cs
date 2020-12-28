using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Cmd
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class CmdAttribute : Attribute
    {
        
    }

}
