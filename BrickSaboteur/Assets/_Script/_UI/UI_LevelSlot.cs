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
using System;
using System.Collections;
using DG.Tweening;
using NearyFrame.Base;
using UniRx;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BrickSaboteur
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class UI_LevelSlot : ElementBase<IUITag>
    {
        public Image image;
        public bool isOn;
        public int level;
        public EDifficulty difficulty;
        private Button button;
        [SerializeField] private TMPro.TextMeshProUGUI _textMeshProText;
        protected override IEnumerator OnStart()
        {
            image = this.GetComponent<Image>();
            button = this.GetComponent<Button>();
            _textMeshProText = _textMeshProText.GetComponentFromDescendants(this, nameof(_textMeshProText));

            button.OnClickAsObservable().Subscribe(__ => OnChooseLevel()).AddTo(this);
            this.ObserveEveryValueChanged(x => x.level).Subscribe(x =>
            {
                if (isOn)
                    _textMeshProText.text = x.ToString();
                else
                    _textMeshProText.text = "X";
            }).AddTo(this);
            yield return null;
        }
        public void Init(bool isOn, int level, EDifficulty difficulty)
        {
            if (image == null)
                image = this.GetComponent<Image>();
            if (button == null)
                button = this.GetComponent<Button>();
            this.isOn = isOn;
            this.difficulty = difficulty;
            this.level = level;
            if (isOn)
            {
                button.interactable = true;
                this.image.color = new Color32(80, 140, 90, 255);
                image.DOColor(new Color32(0, 0, 0, 255), 1).From();
            }
            else
            {
                button.interactable = false;
                this.image.color = new Color32(90, 90, 90, 255);
                image.DOColor(new Color32(255, 255, 255, 255), 1).From();
            }
        }

        private void OnChooseLevel()
        {
            // MessageBroker.Default.Publish(new GameTag_GameStart(level, difficulty));
            BrickMgrM.LoaderManager.GameStart(level, difficulty);
        }
    }
}
