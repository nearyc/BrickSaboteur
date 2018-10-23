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
    public interface IPropertyTag : IModuleTag<IPropertyTag>
    {

    }
    /// <summary>
    /// 输入管理
    /// </summary>
    /// <typeparam name="IUITag">Tag</typeparam>
    public class PropertyManager : ManagerBase<PropertyManager, IPropertyTag>
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
    }

}
