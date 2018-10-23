#region Author & Version
//==================================================== 
// Author：Nearyc 
// File name：......
// Version：V1.0.1
// Date : 2018.10.1
//*Function: Mgr快捷方法
//===================================================
// Fix:
//===================================================

#endregion
using BrickSaboteur;
using UnityEngine;

namespace NearyFrame.Base
{
    public class BrickMgrM
    {
        public static UIManager UIModule => Mgr.Instance.GetModule<IUITag>() as UIManager;
        public static PropertyManager PeropertyModule => Mgr.Instance.GetModule<IPropertyTag>() as PropertyManager;
        public static EntityManager EntityModule => Mgr.Instance.GetModule<IEntityTag>() as EntityManager;
        public static PoolManager PoolModule => Mgr.Instance.GetModule<IPoolTag>() as PoolManager;
        public static LoaderManager LoaderManager => Mgr.Instance.GetModule<ILoaderTag>() as LoaderManager;
        public static CameraManager CameraManager => Mgr.Instance.GetModule<ICameraTag>() as CameraManager;
        public static GridManager GridManager => Mgr.Instance.GetModule<IGridTag>() as GridManager;
        public static System.Collections.IEnumerator WaitBoardInit()
        {
            while (EntityModule == null || EntityModule.boards.Count < 1)
            {
                yield return null;
            }
        }
        public static System.Collections.IEnumerator WaitModule<Tag>() where Tag : IModuleTag<Tag>
        {
            while (Mgr.Instance.GetModule<Tag>() == null)
            {
                yield return null;
            }
        }

    }
}
