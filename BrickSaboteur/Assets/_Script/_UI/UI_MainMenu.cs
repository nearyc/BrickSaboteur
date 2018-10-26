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
    public abstract class UIELementBase<T> : ElementBaseSingle<T, IUITag> where T : UIELementBase<T>
    {
        protected CanvasGroup _group;
        public virtual void Show()
        {
            if (_group == null) _group = this.GetComponent<CanvasGroup>();
            this._group.interactable = true;
            this._group.DOFade(1, 0.5f);
        }
        public virtual void Hide()
        {
            if (_group == null) _group = this.GetComponent<CanvasGroup>();
            this._group.interactable = false;
            this._group.DOFade(0, 0.5f);
        }
    }
    public class UI_MainMenu : UIELementBase<UI_MainMenu>
    {
        [SerializeField] private Button _playButton; //开始游戏
        [SerializeField] private Button _eazyLevelButton; //简单难度
        [SerializeField] private Button _hardLevelButton; //困难难度
        // ---------------------- 
        [SerializeField] private Button _ladderButton; //排行榜
        [SerializeField] private Button _shopButton; //商城
        [SerializeField] private Toggle _musicToggle; //音乐开关
        [SerializeField] private Button _shareButton; //商城
        [SerializeField] private Image _offIcon; //音乐
        protected override void OnDestroy()
        {
            this.UnRegisterSelf(this);
        }
        protected override IEnumerator AfterStart()
        {

            _playButton = _playButton.GetComponentFromChildren(this, nameof(_playButton));
            _eazyLevelButton = _eazyLevelButton.GetComponentFromChildren(this, nameof(_eazyLevelButton));
            _hardLevelButton = _hardLevelButton.GetComponentFromChildren(this, nameof(_hardLevelButton));
            // ---------------------- 
            _ladderButton = _ladderButton.GetComponentFromDescendants(this, nameof(_ladderButton));
            _shopButton = _shopButton.GetComponentFromDescendants(this, nameof(_shopButton));
            _musicToggle = _musicToggle.GetComponentFromDescendants(this, nameof(_musicToggle));
            _shareButton = _shareButton.GetComponentFromDescendants(this, nameof(_shareButton));
            _offIcon = _offIcon.GetComponentFromDescendants(this, nameof(_offIcon));
            // ---------------------- 
            //按下按键，游戏开始
            _playButton.OnClickAsObservable().TakeUntilDestroy(this).Subscribe(__ => OnPlay());
            //按下按键，进入难度选择
            _eazyLevelButton.OnClickAsObservable().TakeUntilDestroy(this).Subscribe(__ => OnEzLevel());
            //按下按键，进入难度选择
            _hardLevelButton.OnClickAsObservable().TakeUntilDestroy(this).Subscribe(__ => OnHardLevel());
            //按下按键，进入排行榜
            _ladderButton.OnClickAsObservable().TakeUntilDestroy(this).Subscribe(__ => OnLadder());
            //按下按键，进入购物
            _shopButton.OnClickAsObservable().TakeUntilDestroy(this).Subscribe(__ => OnShop());
            //按下按键，关闭或者开启音乐
            _musicToggle.OnValueChangedAsObservable().TakeUntilDestroy(this).Subscribe(isOn => OnMusic(isOn));
            //按下按键，进入分享
            _shopButton.OnClickAsObservable().TakeUntilDestroy(this).Subscribe(__ => OnShare());

            this.RegisterSelf(this);
            yield return null;

        }
        public override void Show()
        {
            base.Show();
            this.transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.InExpo);
        }
        public override void Hide()
        {
            base.Hide();
            this.transform.DOLocalMoveX(-1000, 0.5f).SetEase(Ease.InExpo);
        }
        private void OnPlay()
        {
            // MessageBroker.Default.Publish(new GameTag_GameStart(1, EDifficulty.Eazy));
            BrickMgrM.LoaderManager.GameStart(1, EDifficulty.Eazy);
        }
        private void OnEzLevel()
        {
            this.Hide();
            BrickMgrM.UIModule.GetElement<UI_LevelPanel>().Show();
            BrickMgrM.UIModule.GetElement<UI_LevelPanel>().ShowLevelSlots(EDifficulty.Eazy);
        }
        private void OnHardLevel()
        {
            this.Hide();
            BrickMgrM.UIModule.GetElement<UI_LevelPanel>().Show();
            BrickMgrM.UIModule.GetElement<UI_LevelPanel>().ShowLevelSlots(EDifficulty.Hard);
        }
        private void OnLadder()
        {
            // BrickMgrM.UIModule.GetElement<UI_PopUpPanel>().Show();
        }
        private void OnShop()
        {
            // BrickMgrM.UIModule.GetElement<UI_PopUpPanel>().Hide();
        }
        private void OnMusic(bool isOn)
        {
            Debug.Log(isOn);
            if (isOn)
            {
                BrickMgrM.AudioManager.IsPlayAudio(true);
                _offIcon.gameObject.SetActive(false);
            }
            else
            {
                BrickMgrM.AudioManager.IsPlayAudio(false);
                _offIcon.gameObject.SetActive(true);
            }
        }
        private void OnShare()
        {

        }
    }
}
