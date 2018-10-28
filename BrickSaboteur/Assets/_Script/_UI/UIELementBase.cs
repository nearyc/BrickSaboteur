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
using DG.Tweening;
using NearyFrame.Base;
using UnityEngine;
namespace BrickSaboteur
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIELementBase<T> : ElementBaseSingle<T, IUITag> where T : UIELementBase<T>
    {
        protected CanvasGroup _group;
        public virtual void Show(float time=0.5f)
        {
            if (_group == null) _group = this.GetComponent<CanvasGroup>();
            this._group.interactable = true;
            this._group.blocksRaycasts=true;
            this._group.DOFade(1, time).SetUpdate(true);
        }
        public virtual void Hide(float time=0.5f)
        {
            if (_group == null) _group = this.GetComponent<CanvasGroup>();
            this._group.interactable = false;
             this._group.blocksRaycasts=false;
            this._group.DOFade(0, time).SetUpdate(true);
        }
    }
}
