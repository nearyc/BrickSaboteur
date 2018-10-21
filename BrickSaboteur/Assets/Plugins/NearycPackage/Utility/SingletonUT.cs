#region Author & Version
//==================================================== 
// Author：Nearyc 
// File name：......
// Version：V1.0.1
// Date : 
//*Function: 单例工具类
//===================================================
// Fix:
//===================================================

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nearyc.Utility
{
    /// <summary>
    /// 普通单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NSingleton<T> where T : NSingleton<T>, new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new T();
                return _instance;
            }
        }
    }
    /// <summary>
    /// 调用到FindOfType的Mono单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NMonoSingleton2<T> : MonoBehaviour
    where T : NMonoSingleton2<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] objs = FindObjectsOfType(typeof(T)) as T[];
                    if (objs.Length > 0)
                        _instance = objs[0];
                    if (objs.Length > 1)
                    {
                        Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                    }
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        //obj.hideFlags = HideFlags.HideAndDontSave;
                        obj.name = typeof(T).ToString() + "(Clone)";
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
    }
    /// <summary>
    /// 一般的Mono单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NMonoSingleton<T> : MonoBehaviour
    where T : NMonoSingleton<T>
    {
        public static T Instance { get; private set; }
        public virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Destroy(this);
            }
            OnInitialize();
        }
        protected abstract void OnInitialize();
    }
    /// <summary>
    /// 不会随场景销毁的单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NMonoSingletonPersist<T> : MonoBehaviour
    where T : Component
    {
        public static T Instance { get; private set; }
        public virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
            OnInitialize();
        }
        protected abstract void OnInitialize();
    }

}
