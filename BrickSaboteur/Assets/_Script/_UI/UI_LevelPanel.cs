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
using NearyFrame.Base;
using UniRx;
using Unity.Linq;
using UnityEngine;
namespace BrickSaboteur
{

    public class UI_LevelPanel : UIELement<UI_LevelPanel>
    {
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
        protected override IEnumerator AfterStart()
        {
            this.RegisterSelf(this);
            yield return BrickMgrM.WaitModule<IPoolTag>();
            slotsList = new List<UI_LevelSlot>();
            _content = gameObject.Descendants().Where(x => x.name == "Content").First().transform;

            string path = AssetPath.PrefabPrefix + AssetPath.LevelSlotButton;
            var aop = Resources.LoadAsync(path);
            Debug.Log(path);
            yield return aop;
            // ---------------------- 
            //创建物品池
            pool = BrickMgrM.PoolModule.GetOrCreate<UI_LevelSlot>(aop.asset as GameObject);
            pool.setInActiveWhenReturn = false;
            pool.parent = _content;
            pool.onBeforeRent += x =>
            {
                x.transform.parent = _content;
                x.transform.localScale = Vector3.one;
                x.transform.localPosition = Vector3.zero;
            };
            pool.onBeforeReturn += x =>
            {
                x.transform.parent = _content = this.transform;
            };
            pool.PreloadAsync(100, 10);
            yield return new WaitForSeconds(3);
            //TEST
            ShowLevelSlots(100, 10, EDifficulty.Eazy);
            yield return new WaitForSeconds(3);
            ClearLevelSlot();
        }
        /// <summary>
        /// 加载level slot
        /// </summary>
        /// <param name="count">图标，解锁或未解锁</param>
        /// <param name="level"></param>
        /// <param name="difficulty"></param>
        private void ShowLevelSlots(int count, int level, EDifficulty difficulty)
        {
            for (int i = 1; i <= count; i++)
            {
                if (i < level)
                {
                    pool.RentAsync().Subscribe(x =>
                    {
                        x.Init(_unLockIcon, i, difficulty);
                        slotsList.Add(x);
                    });
                }
                else
                {
                    pool.RentAsync().Subscribe(x =>
                    {
                        x.Init(_lockIcon, i, difficulty);
                        slotsList.Add(x);
                    });
                }
            }
        }
        private void ClearLevelSlot()
        {
            // for (int i = slotsList.Count; i > 0; i++)
            // {
            //     pool.Return(slotsList[i - 1]);
            // }
            foreach (var item in slotsList)
            {
                pool.Return(item);
            }
        }
    }
}
