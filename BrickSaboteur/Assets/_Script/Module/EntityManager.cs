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
using Nearyc.Roslyn;
using NearyFrame;
using NearyFrame.Base;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace BrickSaboteur
{
    public interface IEntityTag : IModuleTag<IEntityTag>
    {
        void RegisteBoard(SliderEntity slider);
        void UnRegisteBoard(SliderEntity slider);
        void RegisteBall(BallEntity ball);
        void UnRegisteBall(BallEntity ball);
        void RegisteBonus(BonusEntity bonusEntity);
        void UnRegisteBonus(BonusEntity bonusEntity);
        List<SliderEntity> SlidersList { get; }
        HashSet<BallEntity> BallsSet { get; }
        List<BonusEntity> BonusList { get; }
        BallEntity startBall { get; set; }
    }
    /// <summary>
    /// 实体管理
    /// </summary>
    /// <typeparam name="EntityManager">Self</typeparam>
    /// <typeparam name="IEntityTag">Tag</typeparam>
    public class EntityManager : ManagerBase<EntityManager, IEntityTag>, IEntityTag
    {
        private int _sliderSize;
        [Sirenix.OdinInspector.ShowInInspector]
        public List<SliderEntity> SlidersList { get; private set; } = new List<SliderEntity>();
        [Sirenix.OdinInspector.ShowInInspector]
        public HashSet<BallEntity> BallsSet { get; private set; } = new HashSet<BallEntity>();
        [Sirenix.OdinInspector.ShowInInspector]
        public List<BonusEntity> BonusList { get; private set; } = new List<BonusEntity>();
        [Sirenix.OdinInspector.ShowInInspector]
        public BallEntity startBall { get; set; }

        protected override void OnDestroy()
        {
            Mgr.Instance.UnRegisterModule(this);
            Debug.Log("UnRegister EntityManager");
        }

        protected override System.Collections.IEnumerator OnInitializeRegisterSelf()
        {
            Mgr.Instance.RegisterModule(this);
            Debug.Log("Create EntityManager");
            yield return BrickMgrM.WaitModule<IPropertyTag>();
            //加载slider
            MessageBroker.Default.Receive<PropTag_ModifySliderSize>().Subscribe(x => GenerateSlider()).AddTo(this);
            //回到菜单
            MessageBroker.Default.Receive<GameTag_BackToMenu>()
                .Subscribe(__ => ClearBalls(false)).AddTo(this);;
            //开始
            MessageBroker.Default.Receive<GameTag_GameStart>()
                .Subscribe(__ => MessageBroker.Default.Publish(new GameTag_Reload())).AddTo(this);
            //游戏结束
            MessageBroker.Default.Receive<GameTag_GameEnd>()
                .Subscribe(x => ClearBalls(x.isWinorNot)).AddTo(this);;
            // //Load
            MessageBroker.Default.Receive<GameTag_Reload>().Subscribe(__ => GenerateStartBall()).AddTo(this);
            //如果在游戏状态且球变为0了，扣一点
            this.ObserveEveryValueChanged(x => x.BallsSet.Count)
                .Where(x => x == 0 && LoaderManager.isLoaded)
                .Subscribe(__ =>
                {
                    MessageBroker.Default.Publish(new PropTag_ModifyHealth(-1));
                    MessageBroker.Default.Publish(new GameTag_Reload());
                });
        }
        /// <summary>
        /// 生成slider
        /// </summary>
        private void GenerateSlider()
        {
            //等待一帧
            Observable.NextFrame().Subscribe(__ =>
            {
                var path = AssetPath.Slider + BrickMgrM.PropertyModule.SliderSize.CurrentValue;
                Debug.Log(path);

                var pos = new Vector3(0, -16, 0);
                if (SlidersList.Count > 0)
                {
                    pos = SlidersList[0].transform.localPosition;
                    BrickMgrM.LoaderManager.ReleaseObject(SlidersList[0].gameObject);
                }

                BrickMgrM.LoaderManager.InstantiatePrefabByPath<GameObject>(path).Subscribe(x => x.transform.localPosition = pos);
            });
        }
        /// <summary>
        /// 生成开始的小球
        /// </summary>
        private void GenerateStartBall()
        {
            if (startBall == null)
            {
                BrickMgrM.PoolModule.GetPool<BallEntity>().RentAsync()
                    .Do(x =>
                    {
                        x.gameObject.SetActive(false);
                        startBall = x;
                    })
                    .Delay(System.TimeSpan.FromMilliseconds(250))
                    .Subscribe(x =>
                    {
                        x.gameObject.SetActive(true);
                        if (SlidersList.Count > 0)
                        {
                            x.Init(SlidersList[0].transform.position + Vector3.up * 0.5f, Vector3.zero, 0);
                        }
                        else
                        {
                            x.Init(new Vector3(0, -16, 0) + Vector3.up * 0.5f, Vector3.zero, 0);
                        }
                    });
            }
        }
        /// <summary>
        /// 清理小球
        /// </summary>
        /// <param name="isWin"></param>
        private void ClearBalls(bool isWin)
        {
            BonusList.ForEach(x => BrickMgrM.LoaderManager.ReleaseObject(x.gameObject));
            var tempList = ListPool<BallEntity>.Allocate();
            foreach (var item in BallsSet)
            {
                tempList.Add(item);
            }
            foreach (var item in tempList)
            {
                BrickMgrM.LoaderManager.ReleaseObject(item.gameObject);
                // BrickMgrM.PoolModule.GetPool<BallEntity>().Return(item);
            }
            ListPool<BallEntity>.Free(tempList);
        }

        #region Registe
        public void RegisteBoard(SliderEntity slider)
        {
            SlidersList.Add(slider);
        }
        public void UnRegisteBoard(SliderEntity slider)
        {
            SlidersList.Remove(slider);
        }
        public void RegisteBall(BallEntity ball)
        {
            BallsSet.Add(ball);
        }
        public void UnRegisteBall(BallEntity ball)
        {
            BallsSet.Remove(ball);
        }
        public void RegisteBonus(BonusEntity bonusEntity)
        {
            BonusList.Add(bonusEntity);
        }
        public void UnRegisteBonus(BonusEntity bonusEntity)
        {
            BonusList.Remove(bonusEntity);
        }

        #endregion

    }

}
