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
using System.Collections;
using NearyFrame.Base;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BrickSaboteur
{
    /// <summary>
    /// AsyncPoolHolder基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AsyncPoolHolder<T> : ElementBase<IPoolTag> where T : ElementBase
    {
        // [Sirenix.Serialization.OdinSerialize] 
        protected Nearyc.Utility.AsyncPool<T> pool;
        [SerializeField]
        protected string _path => _prefabPrefix + Path;
        protected abstract string Path { get; }

        [Sirenix.OdinInspector.ShowInInspector] private string _prefabPrefix => AddressablePathEx.PREFAB_PREFIX;
        [Sirenix.OdinInspector.ShowInInspector] private string _prefabSuffix => AddressablePathEx.PREFAB_SUFFIX;
        protected override IEnumerator OnStart()
        {
            yield return BrickMgrM.WaitModule<IPoolTag>();
            var aop = Resources.LoadAsync(_path);
            yield return aop;

            pool = BrickMgrM.PoolModule.GetOrCreate<T>(aop.asset as GameObject);
        }
    }
}
