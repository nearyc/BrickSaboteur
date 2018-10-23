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
using NearyFrame.Base;
using UnityEngine;
namespace BrickSaboteur
{
    /// <summary>
    /// 小球和Bonus的死亡区域
    /// </summary>
    /// <typeparam name="IEntityTag"></typeparam>
    public class DieAreaEntity : ElementBase<IEntityTag>
    {

        protected override IEnumerator OnStart()
        {
            yield return BrickMgrM.WaitModule<IEntityTag>();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            var ball = other.GetComponent<BallEntity>();
            if (ball != null)
            {
                BrickMgrM.PoolModule.GetPool<BallEntity>().Return(ball);
            }
        }
        /// <summary>
        /// 防止enter没成功
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerStay2D(Collider2D other)
        {
            var ball = other.GetComponent<BallEntity>();
            if (ball != null)
            {
                BrickMgrM.PoolModule.GetPool<BallEntity>().Return(ball);
            }
        }
    }

}
