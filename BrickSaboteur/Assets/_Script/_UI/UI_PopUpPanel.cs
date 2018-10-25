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
using DG.Tweening;
using UniRx;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace BrickSaboteur
{
    // public static class DoTweenEx
    // {
    //     public static Tweener DoRelativeMoveX(this Transform target, float endValue, float duration, bool snapping = false)
    //     {
    //         return target.DOMoveX(target.transform.localPosition.x + endValue, duration, snapping);
    //     }
    // }
    public class UI_PopUpPanel : UIELementBase<UI_PopUpPanel>
    {
        [SerializeField] Button _backToMenuButton;
        [SerializeField] Button _restartButton;
        [SerializeField] Button _nextLevelButton;
        [SerializeField] Image _blockBg;
        protected override void OnDestroy()
        {
            this.UnRegisterSelf(this);
        }
        protected override IEnumerator AfterStart()
        {
            this.RegisterSelf(this);
            _backToMenuButton = _backToMenuButton.GetComponentFromChildren(this, nameof(_backToMenuButton));
            _restartButton = _restartButton.GetComponentFromChildren(this, nameof(_restartButton));
            _nextLevelButton = _nextLevelButton.GetComponentFromChildren(this, nameof(_nextLevelButton));
            _blockBg = _blockBg.GetComponentFromChildren(this.transform.parent, nameof(_blockBg));
            yield return null;
            Hide();
            _nextLevelButton.OnClickAsObservable().Subscribe(__ => Hide()).AddTo(this);
        }
        public override void Show()
        {
            base.Show();
            this.transform.DOLocalMoveY(-100, 0.5f).SetEase(Ease.InOutBack);
            _blockBg.gameObject.SetActive(true);
        }
        public override void Hide()
        {
            base.Hide();
            this.transform.DOLocalMoveY(1000, 0.5f).SetEase(Ease.InOutBack);
            _blockBg.gameObject.SetActive(false);
        }
    }
}
