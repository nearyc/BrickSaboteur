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
        public static LoaderManager LoaderManager => Mgr.Instance.GetModule<ILoaderTag>() as LoaderManager;
        public static PropertyManager PropertyModule => Mgr.Instance.GetModule<IPropertyTag>() as PropertyManager;
        public static IUITag UIModule => Mgr.Instance.GetModule<IUITag>() as IUITag;
        public static IEntityTag EntityModule => Mgr.Instance.GetModule<IEntityTag>() as IEntityTag;
        public static IPoolTag PoolModule => Mgr.Instance.GetModule<IPoolTag>() as IPoolTag;
        public static ICameraTag CameraManager => Mgr.Instance.GetModule<ICameraTag>() as ICameraTag;
        public static IGridTag GridManager => Mgr.Instance.GetModule<IGridTag>() as IGridTag;
        public static IAudioTag AudioManager => Mgr.Instance.GetModule<IAudioTag>() as IAudioTag;

        // public static System.Collections.IEnumerator WaitBoardInit()
        // {
        //     while (EntityModule == null || EntityModule.SlidersList.Count < 1)
        //     {
        //         yield return null;
        //     }
        // }
        public static System.Collections.IEnumerator WaitModule<Tag>() where Tag : IModuleTag<Tag>
        {
            while (Mgr.Instance.GetModule<Tag>() == null)
            {
                yield return null;
            }
        }
    }
}
