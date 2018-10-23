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
    /// Multiply技能对应的Prensenter
    /// </summary>
    /// <typeparam name="UI_MultiplySkill"></typeparam>
    /// <typeparam name="IUITag"></typeparam>
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
            yield return BrickMgrM.WaitModule<IPropertyTag>();
            yield return new WaitForSeconds(0.1f);
            skillHolder = BrickMgrM.PeropertyModule.GetElement<SkillHolder>();
            //按下按键尝试释放
            skillButton.OnClickAsObservable().Subscribe(__ => skillHolder.TryExecuteMultiply()).AddTo(this);
            //更新数字
            skillHolder.multiplyCount.current.SubscribeToText(countText).AddTo(this);
        }
    }
}
