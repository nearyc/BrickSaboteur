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
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace BrickSaboteur
{
    /// <summary>
    /// 小球的PoolHolder
    /// </summary>
    /// <typeparam name="BallEntity"></typeparam>
    public class BallEntityPool : AsyncPoolHolder<BallEntity>
    {
        protected override string Path => AssetPath.Ball;
        [SerializeField] Transform parent;

        protected override IEnumerator OnStart()
        {
            yield return base.OnStart();
            pool.parent = this.transform;
            pool.onBeforeRent += x =>
            {

            };
            pool.onBeforeReturn += x =>
            {
                if (x.rb == null)
                    x.rb = x.GetComponent<Rigidbody2D>();
                x.rb.velocity = Vector3.zero;
            };
            pool.MaxCount = 1000;
            pool.PreloadAsync(300, 10).Subscribe();
        }
    }
}
