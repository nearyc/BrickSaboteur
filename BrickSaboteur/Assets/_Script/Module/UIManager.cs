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
using NearyFrame;
using NearyFrame.Base;
using Unity.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace BrickSaboteur
{
    public interface IUITag : IModuleTag<IUITag> { }

    //UI模块管理实现
    /// <summary>
    /// UI管理
    /// </summary>
    /// <typeparam name="IUITag"></typeparam>
    public class UIManager : ManagerBase<UIManager, IUITag>
    {
        public Canvas mainHudCanvas;
        public Canvas worldHudCanvas;
        protected override void OnDestroy()
        {
            Mgr.Instance.UnRegisterModule(this);
            Debug.Log("UnRegister UIManager");
        }
        protected override System.Collections.IEnumerator OnInitializeRegisterSelf()
        {
            Mgr.Instance.RegisterModule(this);
            Debug.Log("Create UIManager");
            yield return BrickMgrM.WaitModule<ICameraTag>();

            mainHudCanvas = mainHudCanvas.GetComponentFromChildren(this, nameof(mainHudCanvas));
            mainHudCanvas.worldCamera = BrickMgrM.CameraManager.mainCam;
            worldHudCanvas = worldHudCanvas.GetComponentFromChildren(this, nameof(worldHudCanvas));
            worldHudCanvas.worldCamera = BrickMgrM.CameraManager.mainCam;
            var mainHudTran = mainHudCanvas.transform;
            var worldHudTran = worldHudCanvas.transform;
            // BrickMgrM.LoaderManager.InstantiateAll<GameObject>("UIMainRoot", mainHudTran).Subscribe(null);
            // yield return BrickMgrM.LoaderManager.InstantiateAll<GameObject>("UIWorldRoot", worldHudTran);
        }
    }
}
