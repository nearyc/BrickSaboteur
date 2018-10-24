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
using NearyFrame.Base;
using UniRx;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace BrickSaboteur
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIELement<T> : ElementBaseSingle<T, IUITag> where T : UIELement<T>
    {
        CanvasGroup _group;
        public void Show()
        {
            this._group.interactable = true;
            this._group.alpha = 1;
        }
        public void Hide()
        {
            this._group.alpha = 0;
            this._group.interactable = false;
        }
    }
    public class UI_MainMenu : UIELement<UI_MainMenu>
    {
        [SerializeField] private Button _playButton; //开始游戏
        [SerializeField] private Button _eazyLevelButton; //简单难度
        [SerializeField] private Button _hardLevelButton; //困难难度
        [SerializeField] private Button _shopButton; //商城
        [SerializeField] private Button _ladderButton; //排行榜
        [SerializeField] private Toggle _musicToggle; //音乐开关
        protected override void OnDestroy()
        {
            this.UnRegisterSelf(this);
        }
        protected override IEnumerator AfterStart()
        {

            _playButton = _playButton.GetComponentFromChildren(this, nameof(_playButton));
            _eazyLevelButton = _eazyLevelButton.GetComponentFromChildren(this, nameof(_eazyLevelButton));
            _hardLevelButton = _hardLevelButton.GetComponentFromChildren(this, nameof(_hardLevelButton));
            _shopButton = _shopButton.GetComponentFromChildren(this, nameof(_shopButton));
            _ladderButton = _ladderButton.GetComponentFromChildren(this, nameof(_ladderButton));
            _musicToggle = _musicToggle.GetComponentFromChildren(this, nameof(_musicToggle));
            // ---------------------- 
            //按下按键，游戏开始
            _playButton.OnClickAsObservable().Subscribe(__ => OnPlay());
            //按下按键，进入难度选择
            _eazyLevelButton.OnClickAsObservable().Subscribe(__ => OnEzLevel());
            //按下按键，进入难度选择
            _hardLevelButton.OnClickAsObservable().Subscribe(__ => OnHardLevel());
            //按下按键，进入购物
            _shopButton.OnClickAsObservable().Subscribe(__ => OnShop());
            //按下按键，进入排行榜
            _ladderButton.OnClickAsObservable().Subscribe(__ => OnLadder());
            //按下按键，关闭或者开启音乐
            _musicToggle.OnValueChangedAsObservable().Subscribe(__ => OnMusic());

            this.RegisterSelf(this);
            yield return null;

        }
        private void OnPlay()
        {
            // MessageBroker.Default.Publish(new GameTag_GameStart(1, EDifficulty.Eazy));
            BrickMgrM.LoaderManager.GameStart(1, EDifficulty.Eazy);
        }
        private void OnEzLevel()
        {

        }
        private void OnHardLevel()
        {

        }
        private void OnShop()
        {

        }
        private void OnLadder()
        {

        }
        private void OnMusic()
        {

        }
    }
}
