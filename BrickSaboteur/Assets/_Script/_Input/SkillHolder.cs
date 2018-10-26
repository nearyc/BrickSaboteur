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
        [SerializeField] NormalSKill multiply;

        protected override void OnDestroy()
        {
            this.UnRegisterSelf(this);
        }
        protected override IEnumerator AfterStart()
        {
            this.RegisterSelf(this);
            yield return null;
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

            //回到菜单释放协程
            MessageBroker.Default.Receive<GameTag_BackToMenu>().Subscribe(x =>
            {
                if (_tempStream != null) _tempStream.Dispose();
            }).AddTo(this);;
            //游戏结束释放协程
            MessageBroker.Default.Receive<GameTag_GameEnd>().Subscribe(x =>
            {
                if (_tempStream != null) _tempStream.Dispose();
            }).AddTo(this);;
            //改变生命
            MessageBroker.Default.Receive<SkillTag_TryModifySkill>().Subscribe(x => TryExecuteMultiply()).AddTo(this);
            MessageBroker.Default.Receive<SkillTag_TryPlusSkill>().Subscribe(x => TryExecutePlus()).AddTo(this);

        }
        //TEST

        private void TryExecuteMultiply()
        {
            multiply.TryExecuteSKill(BrickMgrM.PropertyModule.multiplyCount.CurrentValue > 0);
        }
        private void TryExecutePlus()
        {
            plus.TryExecuteSKill(BrickMgrM.PropertyModule.plusCount.CurrentValue > 0);
        }
        private void PlusSKillExecute()
        {
            for (int i = 0; i < 3; i++)
            {
                if (BrickMgrM.EntityModule.BallsSet.Count > 500)
                {
                    return;
                }
                BrickMgrM.PoolModule.GetPool<BallEntity>()
                    .RentAsync()
                    .Subscribe(x =>
                    {
                        var zFloat = 45 * (i - 1) + Random.Range(-30, 30);
                        x.Init(BrickMgrM.EntityModule.SlidersList[0].transform.position + new Vector3(0, 0.5f, 0), new Vector3(0, 0, zFloat));
                    });
            }
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
            var tempList = ListPool<BallEntity>.Allocate();
            var counter = 0;
            foreach (var item in BrickMgrM.EntityModule.BallsSet)
            {
                tempList.Add(item);
            }
            yield return null;
            foreach (var ball in tempList)
            {
                if (BrickMgrM.EntityModule.BallsSet.Count > 500)
                {
                    yield break;
                }
                if (ball.gameObject.activeInHierarchy == true)
                {
                    var zFloat = Random.Range(-80, 45);
                    ball.Init(ball.transform.position, new Vector3(0, 0, zFloat));

                    BrickMgrM.PoolModule.GetPool<BallEntity>()
                        .RentAsync()
                        .Subscribe(x =>
                        {
                            zFloat = Random.Range(-45, 80);
                            x.Init(ball.transform.position, new Vector3(0, 0, zFloat));
                        });
                    BrickMgrM.PoolModule.GetPool<BallEntity>()
                        .RentAsync()
                        .Subscribe(x =>
                        {
                            zFloat = Random.Range(110, 340);
                            x.Init(ball.transform.position, new Vector3(0, 0, zFloat));
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
