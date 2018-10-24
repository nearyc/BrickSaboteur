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
using NearyFrame.Base;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BrickSaboteur
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class UI_LevelSlot : ElementBase<IUITag>
    {
        public Image image;
        public int level;
        public EDifficulty difficulty;
        private Button button;
        protected override IEnumerator OnStart()
        {
            image = this.GetComponent<Image>();
            button = this.GetComponent<Button>();
            button.OnClickAsObservable().TakeUntilDestroy(button).Subscribe(__ => OnChooseLevel());
            yield return null;
        }
        public void Init(Sprite sprite, int level, EDifficulty difficulty)
        {
            if (image == null)
                image = this.GetComponent<Image>();
            this.image.sprite = sprite;
            this.level = level;
            this.difficulty = difficulty;
        }
        private void OnChooseLevel()
        {
            // MessageBroker.Default.Publish(new GameTag_GameStart(level, difficulty));
            BrickMgrM.LoaderManager.GameStart(level, difficulty);
        }
    }
}
