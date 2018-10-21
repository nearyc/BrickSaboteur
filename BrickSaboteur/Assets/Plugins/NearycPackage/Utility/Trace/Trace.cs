#region Author & Version
//==================================================== 
// Author：Nearyc 
// File name：......
// Version：V1.0.1
// Date : 2018.10.1
//*Function:
//===================================================
// Fix:
//===================================================

#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using Nearyc.Roslyn;
using UnityEngine;
namespace UnityEngine
{
    public sealed class Trace
    {
        private Trace() { }
        // private static Trace _trace;
        // private static ObjectPool<Trace> _pool = new ObjectPool<Trace>(() => _trace??new Trace());
        public static void Debug(object str)
        {
            UnityEngine.Debug.Log(str.ToString());
            // if (_trace == null)
            // {
            //     _trace = _pool.Allocate();
            // }
            // return _trace;
        }
        public static void Error(object str)
        {
            UnityEngine.Debug.LogError(str.ToString());
        }
    }
    public static class TraceExtentionMethod
    {
        public static T TraceDebug<T>(this T obj, object str) where T : UnityEngine.Object
        {
            Trace.Debug(str);
            return obj;
        }

        public static object TraceDebug<T>(this Object obj, object str)
        {
            Trace.Debug(str);
            return obj;
        }
        // ---------------------------------
        public static T TraceError<T>(this T obj, object str) where T : UnityEngine.Object
        {
            Trace.Error(str);
            return obj;
        }

        public static object TraceError(this object obj, object str)
        {
            Trace.Error(str);
            return obj;
        }
    }
}
