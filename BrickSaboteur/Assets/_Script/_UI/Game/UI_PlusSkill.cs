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
    /// <summary>
    /// PLUS技能的Presenter
    /// </summary>
    /// <typeparam name="UI_PlusSkill"></typeparam>
    /// <typeparam name="IUITag"></typeparam>
    public class UI_PlusSkill : UIELementBase<UI_PlusSkill>
    {
        [SerializeField] TMPro.TextMeshProUGUI _textMeshProText;
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

            _textMeshProText = _textMeshProText.GetComponentFromDescendants(this, nameof(_textMeshProText));
            skillButton = skillButton.GetComponentFromChildren(this, nameof(skillButton));
            yield return BrickMgrM.WaitModule<IPropertyTag>();
            yield return new WaitForSeconds(0.1f);
            skillHolder = BrickMgrM.PropertyModule.GetElement<SkillHolder>();
            // ---------------------- 
            //按下按键尝试释放
            skillButton.OnClickAsObservable().Subscribe(__ =>
            {
                MessageBroker.Default.Publish(new SkillTag_TryPlusSkill());
                MessageBroker.Default.Publish(new PropTag_ModifyPlusCount { value = -1 });
            }).AddTo(this);
            // ---------------------- 
            //更新数字
            BrickMgrM.PropertyModule.plusCount.current.SubscribeWithState(_textMeshProText, (x, t) => t.text = x.ToString()).AddTo(this);
        }
    }
}
