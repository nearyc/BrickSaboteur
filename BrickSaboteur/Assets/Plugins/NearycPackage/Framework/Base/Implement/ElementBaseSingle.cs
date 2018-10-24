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
using UnityEngine;
namespace NearyFrame.Base
{
    /// <summary>
    /// 单例游戏元素抽象,不可重复
    /// </summary>
    /// <typeparam name="T">Self</typeparam>
    /// <typeparam name="Tag">ITag</typeparam>
    public abstract class ElementBaseSingle<T, Tag> : ElementBase<Tag>, ISingleton
    where T : ElementBaseSingle<T, Tag>, ISingleton
    where Tag : IModuleTag<Tag>
    {
        #region Singleton
        private static T _instance;
        protected override System.Collections.IEnumerator Start()
        {
            yield return base.Start();
            Singleton();
            while (Mgr.Instance == null || InitializeCondition)
            {
                yield return Const.InitializeWaitForSecond;
            }
            yield return AfterStart();
        }
        protected override System.Collections.IEnumerator OnStart()
        {
            //Do nothing
            yield return null;
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
        public bool InitializeCondition => Mgr.Instance.GetModule<Tag>() == null;

        #endregion
        /// <summary>
        /// 创建时，注册自己
        /// </summary>
        protected abstract System.Collections.IEnumerator AfterStart();
        /// <summary>
        /// 销毁时，取消注册
        /// </summary>
        protected abstract void OnDestroy();
        protected virtual void RegisterSelf(ElementBase<Tag> b)
        {
            Mgr.Instance.GetModule<Tag>().RegisterSingleton(b);
        }
        protected virtual void UnRegisterSelf(ElementBase<Tag> b)
        {
            Mgr.Instance.GetModule<Tag>().UnRegisterSingleton(b);
        }
    }
}
