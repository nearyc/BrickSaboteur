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
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;
namespace BrickSaboteur
{
    /// <summary>
    /// 小球的PoolHolder
    /// </summary>
    /// <typeparam name="BallEntity"></typeparam>
    public class BallEntityPool : AsyncPoolHolder<BallEntity>
    {
        // [SerializeField] public Tilemap levelTile;
        protected override string Path => AssetPath.Ball;
        [SerializeField] Transform parent;

        protected override IEnumerator OnStart()
        {
            yield return base.OnStart();
            pool.parent = this.transform;
            pool.onBeforeRent += x =>
            {
                // var temp = BrickMgrM.GridManager.LevelTile;
                // x.transform.localScale = levelTile != null?levelTile.transform.localScale : Vector3.one;
            };
            pool.onBeforeReturn += x =>
            {
                if (x.rb == null)
                    x.rb = x.GetComponent<Rigidbody2D>();
                x.rb.velocity = Vector3.zero;
            };
            pool.MaxCount = 500;
            pool.PreloadAsync(100, 10).Subscribe();
            // pool.
        }
    }
}
