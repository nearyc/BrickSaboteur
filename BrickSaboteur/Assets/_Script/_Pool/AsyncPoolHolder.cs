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
using UnityEngine;
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
        protected string _path => Path;
        protected abstract string Path { get; }
        protected override IEnumerator OnStart()
        {
            yield return BrickMgrM.WaitModule<IPoolTag>();
            pool = BrickMgrM.PoolModule.GetOrCreate<T>(_path);
        }
    }
}
