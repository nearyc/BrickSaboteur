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
using Nearyc.Utility;
using NearyFrame;
using NearyFrame.Base;
using UniRx;
using UniRx.Toolkit;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace BrickSaboteur
{
    public interface IPoolTag : IModuleTag<IPoolTag> { }
    /// <summary>
    /// 物品池管理
    /// </summary>
    /// <typeparam name="PoolManager">Self</typeparam>
    /// <typeparam name="IPoolTag">Tag</typeparam>
    public class PoolManager : ManagerBase<PoolManager, IPoolTag>
    {
        #region Base
        protected override void OnDestroy()
        {
            Mgr.Instance.UnRegisterModule(this);
            Debug.Log("Unregister PoolManager");
        }

        protected override System.Collections.IEnumerator OnInitializeRegisterSelf()
        {
            Mgr.Instance.RegisterModule(this);
            Debug.Log("Create PoolManager");
            yield return null;

            _moduleElementDict = null; //   用不到
        }
        #endregion
        [Sirenix.OdinInspector.ShowInInspector] Dictionary<string, AsyncPool> _pools;
        /// <summary>
        /// 创建或得到异步池
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        public AsyncPool<U> GetOrCreate<U>(GameObject prefab, string key = null, Func<IObservable<U>> createFunc = null, int maxCount = 100) where U : Component
        {
            if (_pools == null)
                _pools = new Dictionary<string, AsyncPool>();
            if (key == null) key = typeof(U).ToString();
            if (_pools.ContainsKey(key))
            {
                return _pools[key] as AsyncPool<U>;
            }
            else
            {
                var pool = new AsyncPool<U>(prefab, createFunc, maxCount);
                pool.parent = this.transform;
                _pools.Add(key, pool);
                return pool;
            }
        }
        public AsyncPool<U> GetOrCreate<U>(AssetReference refer, string key = null, Func<IObservable<U>> createFunc = null, int maxCount = 100) where U : Component
        {
            if (_pools == null)
                _pools = new Dictionary<string, AsyncPool>();
            if (key == null) key = typeof(U).ToString();
            if (_pools.ContainsKey(key))
            {
                return _pools[key] as AsyncPool<U>;
            }
            else
            {
                var pool = new AsyncPool<U>(refer, createFunc, maxCount);
                pool.parent = this.transform;
                _pools.Add(key, pool);
                return pool;
            }
        }

        [Sirenix.OdinInspector.ShowInInspector] private string _prefabPrefix => AddressablePathEx.PREFAB_PREFIX;
        [Sirenix.OdinInspector.ShowInInspector] private string _prefabSuffix => AddressablePathEx.PREFAB_SUFFIX;
        public AsyncPool<U> GetOrCreate<U>(string referPath, string key = null, Func<IObservable<U>> createFunc = null, int maxCount = 100) where U : Component
        {
            if (_pools == null)
                _pools = new Dictionary<string, AsyncPool>();
            if (key == null) key = typeof(U).ToString();
            if (_pools.ContainsKey(key))
            {
                return _pools[key] as AsyncPool<U>;
            }
            else
            {
                var pool = new AsyncPool<U>($"{_prefabPrefix}{referPath}{_prefabSuffix}", createFunc, maxCount);
                Debug.Log(pool);
                pool.parent = this.transform;
                _pools.Add(key, pool);
                return pool;
            }
        }
        public AsyncPool<U> GetOrCreateFunc<U>(Func<IObservable<U>> createInstanceAsync, string key = null, int maxCount = 100) where U : Component
        {
            if (_pools == null)
                _pools = new Dictionary<string, AsyncPool>();
            if (key == null) key = typeof(U).ToString();
            if (_pools.ContainsKey(key))
            {
                return _pools[key] as AsyncPool<U>;
            }
            else
            {
                var pool = new AsyncPool<U>(createInstanceAsync, maxCount);
                pool.parent = this.transform;
                _pools.Add(key, pool);
                return pool;
            }
        }
        public AsyncPool<U> GetPool<U>(string key = null) where U : Component
        {
            if (_pools == null)
                _pools = new Dictionary<string, AsyncPool>();
            if (key == null) key = typeof(U).ToString();
            if (_pools.ContainsKey(key))
            {
                // Debug.Log(key);
                return _pools[key] as AsyncPool<U>;
            }
            else
            {
                return null;
            }
        }

        public void ReleasePool<U>(string key = null) where U : Component
        {
            if (key == null) key = typeof(U).ToString();
            if (_pools.ContainsKey(key))
            {
                (_pools[key] as AsyncPool<U>).Clear();
            }
            _pools.Remove(key);
        }

    }

}
