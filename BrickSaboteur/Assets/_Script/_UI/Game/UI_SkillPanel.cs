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
using TMPro;
using UniRx;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace BrickSaboteur
{
    public class UI_SkillPanel : UIELementBase<UI_MultiplySkill>
    {
        // [SerializeField] Text countText;
        [SerializeField] TextMeshProUGUI _multTextMeshProText;
        [SerializeField] Button _multSkillButton;
        [SerializeField] TMPro.TextMeshProUGUI _plusTextMeshProText;
        [SerializeField] Button _plusSkillButton;
        protected override void OnDestroy()
        {
            this.UnRegisterSelf(this);
        }
        protected override IEnumerator AfterStart()
        {
            this.RegisterSelf(this);

            _multTextMeshProText = _multTextMeshProText.GetComponentFromDescendants(this, nameof(_multTextMeshProText));
            _multSkillButton = _multSkillButton.GetComponentFromDescendants(this, nameof(_multSkillButton));
            _plusTextMeshProText = _plusTextMeshProText.GetComponentFromDescendants(this, nameof(_plusTextMeshProText));
            _plusSkillButton = _plusSkillButton.GetComponentFromDescendants(this, nameof(_plusSkillButton));
            yield return BrickMgrM.WaitModule<IPropertyTag>();
            yield return null;
            //按下按键尝试释放
            _multSkillButton.OnClickAsObservable().Subscribe(__ =>
            {
                MessageBroker.Default.Publish(new SkillTag_TryModifySkill());
                MessageBroker.Default.Publish(new PropTag_ModifyMultiplyCount { value = -1 });
            }).AddTo(this);
            _plusSkillButton.OnClickAsObservable().Subscribe(__ =>
            {
                MessageBroker.Default.Publish(new SkillTag_TryPlusSkill());
                MessageBroker.Default.Publish(new PropTag_ModifyPlusCount { value = -1 });
            }).AddTo(this);
            // ---------------------- 
            //更新plus
            BrickMgrM.PropertyModule.plusCount.current.SubscribeWithState(_plusTextMeshProText, (x, t) => t.text = x.ToString()).AddTo(this);
            //更新mult
            BrickMgrM.PropertyModule.multiplyCount.current.SubscribeWithState(_multTextMeshProText, (x, t) => t.text = x.ToString()).AddTo(this);
        }
    }
}
