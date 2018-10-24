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
using NearyFrame;
using NearyFrame.Base;
using UnityEngine;
namespace BrickSaboteur
{
    public interface IAudioTag : IModuleTag<IAudioTag> { }
    public class AudioManager : ManagerBase<AudioManager, IAudioTag>, IAudioTag
    {
        protected override void OnDestroy()
        {
            Mgr.Instance.UnRegisterModule(this);
            Debug.Log("UnRegister AudioManager");
        }

        protected override IEnumerator OnInitializeRegisterSelf()
        {
            Mgr.Instance.RegisterModule(this);
            Debug.Log("Create AudioManager");
            yield return null;
        }
    }
}
