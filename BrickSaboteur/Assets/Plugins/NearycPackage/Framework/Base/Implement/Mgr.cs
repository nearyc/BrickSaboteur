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
using UnityEngine;
namespace NearyFrame.Base
{
    /// <summary>
    /// 全局游戏管理,游戏入口
    /// </summary>
    public class Mgr : MonoBehaviour, ISingleton
    {
		[Sirenix.OdinInspector.ShowInInspector]
		public bool IsInited { get; private set; } = false;
        #region Singleton
        private Mgr() { }
        public static Mgr Instance { get; private set; }
        public virtual void Awake()
        {
            Singleton();
            _moduleDict = new Dictionary<Type, IModule>();
			Invoke("Inited", 1);
        }
        public void Singleton()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
		private void Inited()
		{
			IsInited = true;
		}
        #endregion
        [Sirenix.OdinInspector.ShowInInspector] 
        private Dictionary<Type, IModule> _moduleDict;
        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="module">模块</param>
        /// <typeparam name="Tag">Type</typeparam>
        /// <returns>是否注册成功</returns>
        public bool RegisterModule<Tag>(IModule<Tag> module) where Tag : IModuleTag<Tag>
        {
            if (_moduleDict.ContainsKey(typeof(Tag)))
            {
                return false;
            }

            _moduleDict.Add(typeof(Tag), module);
            return true;
        }
        /// <summary>
        /// 取消注册模块
        /// </summary>
        /// <param name="module">模块</param>
        /// <typeparam name="Tag">Tag</typeparam>
        public void UnRegisterModule<Tag>(IModule<Tag> module) where Tag : IModuleTag<Tag>
        {
            _moduleDict.Remove(typeof(Tag));
        }
        /// <summary>
        /// 取得模块
        /// </summary>
        /// <typeparam name="Tag">Tag</typeparam>
        /// <returns></returns>
        public IModule<Tag> GetModule<Tag>() where Tag : IModuleTag<Tag>
        {
            IModule imodule;
            if (_moduleDict.TryGetValue(typeof(Tag), out imodule))
            {
                return imodule as IModule<Tag>;
            }
            return null;
        }
    }
}
