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
using UniRx;
using Unity.Linq;
using UnityEngine.UI;
using UnityEngine;
namespace BrickSaboteur
{
    public class UI_Pause : UIELementBase<UI_MultiplySkill>
    {
        [Sirenix.OdinInspector.ShowInInspector]
        Button _pauseButton;
        protected override IEnumerator AfterStart()
        {
            this.RegisterSelf(this);
            yield return null;
            _pauseButton = _pauseButton.GetComponentFromDescendants(this, nameof(_pauseButton));
            _pauseButton.OnClickAsObservable().Subscribe(__ =>
            {
                BrickMgrM.UIModule.GetElement<UI_PopUpPanel>().Show();
                Time.timeScale=0f;
            }).AddTo(this);
        }

        protected override void OnDestroy()
        {
            this.UnRegisterSelf(this);
        }
    }
}
