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
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace NearyFrame.Base
{
    public abstract class ManagerBase : MonoBehaviour { }
    /// <summary>
    /// UI模块抽象,单例
    /// </summary>
    /// <typeparam name="IModuleElement">Self</typeparam>
    /// <typeparam name="Tag">Tag</typeparam>
    public abstract class ManagerBase<T, Tag> : ManagerBase, IModule<Tag>, ISingleton where T : ManagerBase<T, Tag>, IModuleTag<Tag>
        where Tag : IModuleTag<Tag>
        {
            protected static bool isLoaded = true;
            private static T _instance;
            #region Singleton
            private System.Collections.IEnumerator Start()
            {
                while (Mgr.Instance == null)
                {
                    yield return Const.InitializeWaitForSecond;
                }
                Singleton();
                yield return Const.InitializeWaitForSecond;
                _moduleElementDict = new Dictionary<Type, ElementBase<Tag>>();
                while (isLoaded == false)
                {
                    yield return null;
                }
                yield return OnInitializeRegisterSelf();
            }
            public void Singleton()
            {
                if (_instance != null)
                {
                    Destroy(this);
                }
                else
                {
                    _instance = this as T;
                }
            }

            #endregion
            [Sirenix.OdinInspector.ShowInInspector]
            protected Dictionary<Type, ElementBase<Tag>> _moduleElementDict;
            /// <summary>
            /// 创建时候，注册自己
            /// </summary>
            protected abstract System.Collections.IEnumerator OnInitializeRegisterSelf();
            /// <summary>
            /// 销毁时，取消注册
            /// </summary>
            protected abstract void OnDestroy();
            public virtual bool RegisterSingleton(ElementBase<Tag> ele)
            {
                if (_moduleElementDict.ContainsKey(ele.GetType()))
                {
                    return false;
                }
                _moduleElementDict.Add(ele.GetType(), ele);
                return true;
            }
            public virtual void UnRegisterSingleton(ElementBase<Tag> ele)
            {
                _moduleElementDict.Remove(ele.GetType());
            }

            public U GetElement<U>() where U : ElementBase<Tag>,
            new()
            {
                ElementBase<Tag> temp;
                if (_moduleElementDict.TryGetValue(typeof(U), out temp))
                {
                    return temp as U;
                }
                return default(U);
            }
        }
}
