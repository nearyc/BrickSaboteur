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
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace BrickSaboteur
{
    public interface IInputTag : IModuleTag<IInputTag>
    {

    }
    /// <summary>
    /// 输入管理
    /// </summary>
    /// <typeparam name="IUITag">Tag</typeparam>
    public class InputManager : ManagerBase<InputManager, IInputTag>
    {
        // public InputArea
        protected override System.Collections.IEnumerator OnInitializeRegisterSelf()
        {
            Mgr.Instance.RegisterModule(this);
            yield return null;
            Debug.Log("Create InputManager");
        }
        protected override void OnDestroy()
        {
            Mgr.Instance.UnRegisterModule(this);
            Debug.Log("UnRegister InputManager");
        }
        private void Update()
        {
            UpdateDirectionInputs();
        }
        private void UpdateDirectionInputs()
        {
            // InputOrigin.SetHorizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        }
    }

}
