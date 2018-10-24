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
    public interface ICameraTag : IModuleTag<ICameraTag>
    {
        Camera MainCam { get; }
        Camera UICame { get; }
    }
    public class CameraManager : ManagerBase<CameraManager, ICameraTag>, ICameraTag
    {
        private Camera _mainCam;
        public Camera MainCam => _mainCam;
        private Camera _UICam;
        public Camera UICame => _UICam;
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

            _mainCam = _mainCam.GetComponentFromChildren(this, nameof(_mainCam));
            _UICam = _UICam.GetComponentFromChildren(this, nameof(_UICam));
            // yield return BrickMgrM.WaitBoardInit();
        }
    }
}
