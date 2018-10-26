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
    /// <summary>
    /// Multiply技能对应的Prensenter
    /// </summary>
    /// <typeparam name="UI_MultiplySkill"></typeparam>
    /// <typeparam name="IUITag"></typeparam>
    public class UI_MultiplySkill : UIELementBase<UI_MultiplySkill>
    {
        // [SerializeField] Text countText;
        [SerializeField] TextMeshProUGUI _textMeshProText;
        [SerializeField] Button _skillButton;
        [SerializeField] SkillHolder _skillHolder;
        protected override void OnDestroy()
        {
            this.UnRegisterSelf(this);
        }
        protected override IEnumerator AfterStart()
        {
            this.RegisterSelf(this);

            _textMeshProText = _textMeshProText.GetComponentFromDescendants(this, nameof(_textMeshProText));
            _skillButton = _skillButton.GetComponentFromChildren(this, nameof(_skillButton));
            yield return BrickMgrM.WaitModule<IPropertyTag>();
            yield return null;
            _skillHolder = BrickMgrM.PropertyModule.GetElement<SkillHolder>();
            //按下按键尝试释放
            _skillButton.OnClickAsObservable().Subscribe(__ =>
            {
                MessageBroker.Default.Publish(new SkillTag_TryModifySkill());
                MessageBroker.Default.Publish(new PropTag_ModifyMultiplyCount { value = -1 });
            }).AddTo(this);
            //更新数字
            BrickMgrM.PropertyModule.multiplyCount.current.SubscribeWithState(_textMeshProText, (x, t) => t.text = x.ToString()).AddTo(this);
        }
    }
}
