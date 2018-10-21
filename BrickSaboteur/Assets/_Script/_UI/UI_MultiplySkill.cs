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
using UnityEngine;
using UnityEngine.UI;
namespace BrickSaboteur
{
    public class UI_MultiplySkill : ElementBaseSingle<UI_MultiplySkill, IUITag>
    {
        [SerializeField] Text countText;
        [SerializeField] Button skillButton;
        [SerializeField] SkillHolder skillHolder;
        protected override void OnDestroy()
        {
            this.UnRegisterSelf(this);
        }
        protected override IEnumerator AfterStart()
        {
            yield return null;
            this.RegisterSelf(this);

            countText = countText.GetComponentFromDescendants(this, nameof(countText));
            skillButton = skillButton.GetComponentFromChildren(this, nameof(skillButton));
            yield return BrickMgrM.WaitModule<IInputTag>();
            yield return new WaitForSeconds(0.1f);
            skillHolder = BrickMgrM.InputModule.GetElement<SkillHolder>();

            skillButton.OnClickAsObservable()
                .Subscribe(__ => skillHolder.TryExecuteMultiply()).AddTo(this);
        }
        private void Update()
        {
            if (!_isInited) return;
            countText.text = skillHolder.multiplyCount.Current.ToString();
        }

    }
}
