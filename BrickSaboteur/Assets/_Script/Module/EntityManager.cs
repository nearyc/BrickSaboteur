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
using System.Collections.Generic;
using System.Linq;
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
         public readonly HashSet<BallEntity> balls = new HashSet<BallEntity>();
        protected override void OnDestroy()
        {
            Mgr.Instance.UnRegisterModule(this);
            Debug.Log("UnRegister EntityManager");
        }

        protected override System.Collections.IEnumerator OnInitializeRegisterSelf()
        {
            // boards = new List<BoardEntity>();
            // _balls = new HashSet<BallEntity>();
            Mgr.Instance.RegisterModule(this);

            yield return null;
            Debug.Log("Create EntityManager");

        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {

            }
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
        #endregion

    }

}
