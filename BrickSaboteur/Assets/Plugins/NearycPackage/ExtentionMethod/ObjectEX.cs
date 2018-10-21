#region Author & Version
//==================================================== 
// Author：Nearyc 
// File name：......
// Version：V1.0.1
// Date : 
//*Function:Object扩展方法
//===================================================
// Fix:
//===================================================

#endregion
using UnityEngine;
namespace Nearyc.ExtesionMethod
{
    public static class ObjectEx
    {
        #region CEUO001 Instantiate
        public static T Instantiate<T>(this T selfObj) where T : Object
        {
            return Object.Instantiate(selfObj);
        }
        #endregion

        #region CEUO002 Instantiate
        public static T Name<T>(this T selfObj, string name) where T : Object
        {
            selfObj.name = name;
            return selfObj;
        }
        #endregion

        #region CEUO003 Destroy Self
        public static void DestroySelf<T>(this T selfObj) where T : Object
        {
            Object.Destroy(selfObj);
        }
        public static T DestroySelfGracefully<T>(this T selfObj) where T : Object
        {
            if (selfObj)
            {
                Object.Destroy(selfObj);
            }
            return selfObj;
        }
        #endregion

        #region CEUO004 Destroy Self AfterDelay 
        public static T DestroySelfAfterDelay<T>(this T selfObj, float afterDelay) where T : Object
        {
            Object.Destroy(selfObj, afterDelay);
            return selfObj;
        }
        public static T DestroySelfAfterDelayGracefully<T>(this T selfObj, float delay) where T : Object
        {
            if (selfObj)
            {
                Object.Destroy(selfObj, delay);
            }
            return selfObj;
        }
        #endregion
        #region CEUO005 DontDestroyOnLoad
        public static T DontDestroyOnLoad<T>(this T selfObj) where T : Object
        {
            Object.DontDestroyOnLoad(selfObj);
            return selfObj;
        }
        #endregion
        public static T As<T>(this Object selfObj) where T : Object
        {
            return selfObj as T;
        }
        // #region CEUO007 New
        // public static T New<T>(this T selfObj) where T : Object, new() {
        //     selfObj = new T();
        //     return selfObj;
        // }
        // #endregion
    }
}
