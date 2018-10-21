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

namespace BrickSaboteur
{
    public interface ICameraTag : IModuleTag<ICameraTag> { }
    public class CameraManager : ManagerBase<CameraManager, ICameraTag>
    {
        public Camera mainCam;
        public Camera UICam;
        // public CinemachineVirtualCamera virtualCame1;
        protected override void OnDestroy()
        {
            Mgr.Instance.UnRegisterModule(this);
            Debug.Log("UnRegister CameraManager");
        }

        protected override System.Collections.IEnumerator OnInitializeRegisterSelf()
        {
            Mgr.Instance.RegisterModule(this);
            yield return null;
            Debug.Log("Create CameraManager");

            // virtualCame1 = virtualCame1.GetComponentFromChildren(this, nameof(virtualCame1));
            mainCam = mainCam.GetComponentFromChildren(this, nameof(mainCam));
            UICam = UICam.GetComponentFromChildren(this, nameof(UICam));
            yield return BrickMgrM.WaitBoardInit();
            // virtualCame1.Follow = MgrM.EntityModule.player.transform;
        }
    }
}
