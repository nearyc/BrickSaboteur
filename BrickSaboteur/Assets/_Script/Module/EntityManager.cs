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
    public interface IEntityTag : IModuleTag<IEntityTag> { }
    /// <summary>
    /// 实体管理
    /// </summary>
    /// <typeparam name="EntityManager">Self</typeparam>
    /// <typeparam name="IEntityTag">Tag</typeparam>
    public class EntityManager : ManagerBase<EntityManager, IEntityTag>
    {
        public readonly List<BoardEntity> boards = new List<BoardEntity>();
        // [OdinSerialize]
        [Sirenix.OdinInspector.ShowInInspector] public readonly HashSet<BallEntity> balls = new HashSet<BallEntity>();
        public readonly List<SkillBonusEntity> bonusEntities = new List<SkillBonusEntity>();
        protected override void OnDestroy()
        {
            Mgr.Instance.UnRegisterModule(this);
            Debug.Log("UnRegister EntityManager");
        }

        protected override System.Collections.IEnumerator OnInitializeRegisterSelf()
        {
            Mgr.Instance.RegisterModule(this);

            yield return null;
            Debug.Log("Create EntityManager");
            //游戏开始，加载Board
            MessageBroker.Default.Receive<Tag_GameStart>()
                .Subscribe(x => BrickMgrM.LoaderManager.InstantiatePrefabByPath<GameObject>(AssetPath.Board, null).Subscribe()).AddTo(this);;
            //回到菜单
            MessageBroker.Default.Receive<Tag_BackToMenu>()
                .Subscribe(__ => ClearBalls(false)).AddTo(this);;
            //游戏结束
            MessageBroker.Default.Receive<Tag_GameEnd>()
                .Subscribe(x => ClearBalls(x.isWinorNot)).AddTo(this);;
            //如果在游戏状态且球变为0了，扣一点
            this.ObserveEveryValueChanged(x => x.balls.Count).Where(x => x == 0).Subscribe(__ => MessageBroker.Default.Publish(new Tag_ModifyHealth(-1)));
        }
        /// <summary>
        /// 清理小球
        /// </summary>
        /// <param name="isWin"></param>
        private void ClearBalls(bool isWin)
        {
            bonusEntities.ForEach(x => BrickMgrM.LoaderManager.ReleaseObject(x.gameObject));
            var tempList = ListPool<BallEntity>.Allocate();
            foreach (var item in balls)
            {
                tempList.Add(item);
            }
            foreach (var item in tempList)
            {
                BrickMgrM.LoaderManager.ReleaseObject(item.gameObject);
            }
            ListPool<BallEntity>.Free(tempList);
        }

        #region Registe
        public void RegisteBoard(BoardEntity board)
        {
            boards.Add(board);
        }
        public void UnRegisteBoard(BoardEntity board)
        {
            boards.Remove(board);
        }
        public void RegisteBall(BallEntity ball)
        {
            balls.Add(ball);
        }
        public void UnRegisteBall(BallEntity ball)
        {
            balls.Remove(ball);
        }
        public void RegisteBonus(SkillBonusEntity bonusEntity)
        {
            bonusEntities.Add(bonusEntity);
        }
        public void UnRegisteBonus(SkillBonusEntity bonusEntity)
        {
            bonusEntities.Remove(bonusEntity);
        }

        #endregion

    }

}
