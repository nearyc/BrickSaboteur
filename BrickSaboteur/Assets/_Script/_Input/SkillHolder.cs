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
using Nearyc.Roslyn;
using Nearyc.Skill;
using NearyFrame.Base;
using UniRx;
using UnityEngine;
namespace BrickSaboteur
{
    /// <summary>
    /// 管理技能和属性，后期考虑拆分
    /// </summary>
    /// <typeparam name="SkillHolder"></typeparam>
    /// <typeparam name="IInputTag"></typeparam>
    public class SkillHolder : ElementBaseSingle<SkillHolder, IPropertyTag>
    {
        [SerializeField] NormalSKill plus;
        [SerializeField] public Property plusCount;
        [SerializeField] NormalSKill multiply;
        [SerializeField] public Property multiplyCount;
        [SerializeField] public Property HealthCount;

        protected override void OnDestroy()
        {
            this.UnRegisterSelf(this);
        }
        protected override IEnumerator AfterStart()
        {
            this.RegisterSelf(this);
            yield return null;
            //初始化数据
            plusCount.Init(100, PlayerPrefs.GetInt(PlayerPrefKey.PlusCount, 5));
            multiplyCount.Init(100, PlayerPrefs.GetInt(PlayerPrefKey.MultiplyCount, 5));
            //创建plus技能
            plus = new NormalSKill(this, ESkillTag.Node, 0.5f);
            plus.onSkillStart += PlusSKillExecute;
            //创建multiply技能
            multiply = new NormalSKill(this, ESkillTag.Node, 0.5f);
            multiply.onSkillStart += MultiplySkillExecute;
            Observable.EveryUpdate().Subscribe(__ =>
            {
                plus.OnUpdate();
                multiply.OnUpdate();
            });
            //改变生命
            MessageBroker.Default.Receive<Tag_ModifyHealth>().Subscribe(x => ModifyHealth(x.value)).AddTo(this);
            //延迟推送生命
            MessageBroker.Default.Receive<Tag_GameStart>().Subscribe(x =>
            {
                HealthCount.Init(3);
                Observable.Timer(System.TimeSpan.FromMilliseconds(600))
                    .Subscribe(__ => MessageBroker.Default.Publish(new Tag_ModifyHealth(HealthCount.CurrentValue, true)));
            }).AddTo(this);;
            //释放协程
            MessageBroker.Default.Receive<Tag_BackToMenu>().Subscribe(x =>
            {
                if (_tempStream != null) _tempStream.Dispose();
            }).AddTo(this);;
            //释放协程
            MessageBroker.Default.Receive<Tag_GameEnd>().Subscribe(x =>
            {
                if (_tempStream != null) _tempStream.Dispose();
            }).AddTo(this);;
            //游戏结束判断，如果生命值小于0
            this.ObserveEveryValueChanged(x => x.HealthCount.CurrentValue).Where(x => x < 1)
                .Subscribe(__ => BrickMgrM.LoaderManager.GameEnd(false)).AddTo(this);
        }
        //TEST
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                ModifyPlus(5);
                ModufyMultiply(5);
            }
        }
        public void TryExecuteMultiply()
        {
            multiply.TryExecuteSKill(multiplyCount.CurrentValue > 0);
        }
        public void TryExecutePlus()
        {
            plus.TryExecuteSKill(plusCount.CurrentValue > 0);
        }
        private void PlusSKillExecute()
        {
            ModifyPlus(-1);
            for (int i = 0; i < 3; i++)
            {
                BrickMgrM.PoolModule.GetPool<BallEntity>()
                    .RentAsync()
                    .Subscribe(x =>
                    {
                        var zFloat = 45 * (i - 1) + Random.Range(-30, 30);
                        x.Init(BrickMgrM.EntityModule.boards[0].transform.position + new Vector3(0, 0.5f, 0), new Vector3(0, 0, zFloat));
                    });
            }
        }
        private void ModifyHealth(int amoumt)
        {
            HealthCount.ModifyCurrent(amoumt);
        }
        private void ModifyPlus(int amount)
        {
            plusCount.ModifyCurrent(amount);
            PlayerPrefs.SetInt(PlayerPrefKey.PlusCount, plusCount.CurrentValue);
        }
        private void ModufyMultiply(int amount)
        {
            multiplyCount.ModifyCurrent(amount);
            PlayerPrefs.SetInt(PlayerPrefKey.MultiplyCount, multiplyCount.CurrentValue);
        }
        /// <summary>
        /// 限定最多只有一个协程
        /// </summary>
        System.IDisposable _tempStream;
        private void MultiplySkillExecute()
        {
            if (_tempStream != null)
                _tempStream.Dispose();
            _tempStream = Observable.FromCoroutine(MultiplyAsync).Subscribe().AddTo(this);
        }
        /// <summary>
        /// 每帧最多生成50个小球
        /// </summary>
        /// <returns></returns>
        private IEnumerator MultiplyAsync()
        {
            ModufyMultiply(-1);
            var tempList = ListPool<BallEntity>.Allocate();
            var counter = 0;
            foreach (var item in BrickMgrM.EntityModule.balls)
            {
                tempList.Add(item);
            }
            yield return null;
            foreach (var item in tempList)
            {
                if (item.gameObject.activeInHierarchy == true)
                {
                    var zFloat = Random.Range(-80, 45);
                    item.Init(item.transform.position, new Vector3(0, 0, zFloat));

                    BrickMgrM.PoolModule.GetPool<BallEntity>()
                        .RentAsync()
                        .Subscribe(x =>
                        {
                            zFloat = Random.Range(-45, 80);
                            x.Init(item.transform.position, new Vector3(0, 0, zFloat));
                        });
                    BrickMgrM.PoolModule.GetPool<BallEntity>()
                        .RentAsync()
                        .Subscribe(x =>
                        {
                            zFloat = Random.Range(110, 340);
                            x.Init(item.transform.position, new Vector3(0, 0, zFloat));
                        });
                    counter++;
                }
                if (counter > 50)
                {
                    counter = 0;
                    yield return null;
                }
            }
            ListPool<BallEntity>.Free(tempList);
        }
    }
}
