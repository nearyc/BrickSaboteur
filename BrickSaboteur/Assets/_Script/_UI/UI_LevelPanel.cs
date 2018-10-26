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
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NearyFrame.Base;
using UniRx;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace BrickSaboteur
{
    public class UI_LevelPanel : UIELementBase<UI_LevelPanel>
    {
        [SerializeField] private Button _backButton;
        // ---------------------- 
        [SerializeField] Sprite _lockIcon;
        [SerializeField] Sprite _unLockIcon;
        [SerializeField] private Transform _content;
        public EDifficulty difficulty;
        public List<UI_LevelSlot> slotsList;
        [Sirenix.OdinInspector.ShowInInspector] private Nearyc.Utility.AsyncPool<UI_LevelSlot> pool;
        protected override void OnDestroy()
        {
            this.UnRegisterSelf(this);
        }
        public override void Show()
        {
            base.Show();
            this.transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.InExpo);
        }
        public override void Hide()
        {
            base.Hide();
            this.transform.DOLocalMoveX(1000, 0.5f).SetEase(Ease.InExpo);
        }
        protected override IEnumerator AfterStart()
        {
            this.RegisterSelf(this);
            yield return BrickMgrM.WaitModule<IPoolTag>();
            slotsList = new List<UI_LevelSlot>();

            _content = gameObject.Descendants().Where(x => x.name == "Content").First().transform;
            _backButton = _backButton.GetComponentFromChildren(this, nameof(_backButton));
            _backButton.OnClickAsObservable().TakeUntilDestroy(this).Subscribe(__ =>
            {
                this.Hide();
                BrickMgrM.UIModule.GetElement<UI_MainMenu>().Show();
            });
            // ---------------------- 
            string path = AssetPath.PrefabPrefix + AssetPath.LevelSlotButton;
            var aop = Resources.LoadAsync(path);
            Debug.Log(path);
            yield return aop;
            // ---------------------- 
            //创建物品池
            pool = BrickMgrM.PoolModule.GetOrCreate<UI_LevelSlot>(aop.asset as GameObject);
            // pool.setInActiveWhenReturn = false;
            pool.parent = _content;
            pool.onBeforeRent += x =>
            {
                x.transform.parent = _content;
                x.transform.localScale = Vector3.one;
                x.transform.localPosition = Vector3.zero;
            };
            pool.onBeforeReturn += x =>
            {
                x.transform.parent = _content;
            };
            pool.PreloadAsync(100, 10).Subscribe();
            PreloadLevelSlot(100, 1, EDifficulty.Eazy);
            // ---------------------- 
        }
        public void ShowLevelSlots(EDifficulty difficulty)
        {
            if (difficulty == EDifficulty.Eazy)
                ShowLevelSlotsCore(100, 11, difficulty);
            else
                ShowLevelSlotsCore(100, 5, difficulty);
        }
        /// <summary>
        /// 加载level slot
        /// </summary>
        /// <param name="count">图标，解锁或未解锁</param>
        /// <param name="level"></param>
        /// <param name="difficulty"></param>
        private void PreloadLevelSlot(int count, int level, EDifficulty difficulty)
        {
            // ClearLevelSlot();
            if (showLevelStream != null) showLevelStream.Dispose();
            showLevelStream = Observable.Interval(System.TimeSpan.FromMilliseconds(10))
                .Take(count)
                .Delay(System.TimeSpan.FromMilliseconds(250))
                .Subscribe(levelCount =>
                {
                    if (levelCount < level)
                    {
                        pool.RentAsync().Subscribe(x =>
                        {
                            x.Init(true, (int) levelCount, difficulty);
                            slotsList.Add(x);
                        });
                    }
                    else
                    {
                        pool.RentAsync().Subscribe(x =>
                        {
                            x.Init(false, (int) levelCount, difficulty);
                            slotsList.Add(x);
                        });
                    }
                }).AddTo(this);
        }
        private void ShowLevelSlotsCore(int count, int level, EDifficulty difficulty)
        {
            for (int i = 1; i <= 100; i++)
            {
                if (i < level)
                {
                    slotsList[i - 1].Init(true, (int) i, difficulty);
                }
                else
                {
                    slotsList[i - 1].Init(false, (int) i, difficulty);
                }
            }
        }
        private System.IDisposable showLevelStream;

        private void ClearLevelSlot()
        {
            foreach (var item in slotsList)
            {
                pool.Return(item);
            }
            slotsList.Clear();
        }
    }
}
