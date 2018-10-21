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
    public class DieAreaEntity : ElementBase<IEntityTag>
    {

        protected override IEnumerator OnStart()
        {
            yield return BrickMgrM.WaitModule<IEntityTag>();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            var ball = other.GetComponent<BallEntity>();
            if (other != null)
            {
                BrickMgrM.PoolModule.GetPool<BallEntity>().Return(ball);
            }
        }
        private void OnTriggerStay2D(Collider2D other)
        {
            var ball = other.GetComponent<BallEntity>();
            if (other != null)
            {
                BrickMgrM.PoolModule.GetPool<BallEntity>().Return(ball);
            }
        }
    }

}
