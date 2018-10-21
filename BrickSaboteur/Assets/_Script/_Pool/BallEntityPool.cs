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
namespace BrickSaboteur
{
    public abstract class AsyncPoolHolder<T> : ElementBase<IPoolTag> where T : ElementBase
    {
        // [Sirenix.Serialization.OdinSerialize] 
        protected Nearyc.Utility.AsyncPool<T> pool;
        [SerializeField] protected string _ballPath => ballPath;
        protected abstract string ballPath { get; }
        protected override IEnumerator OnStart()
        {
            yield return BrickMgrM.WaitModule<IPoolTag>();
            pool = BrickMgrM.PoolModule.GetOrCreate<T>(_ballPath);
        }
    }
    public class BallEntityPool : AsyncPoolHolder<BallEntity>
    {
        protected override string ballPath => "Entity/Ball";

        protected override IEnumerator OnStart()
        {
            yield return base.OnStart();
            // Vector3 temp;
            pool.onBeforeRent += x =>
            {
                //TODO
                // temp = BrickMgrM.CameraManager.mainCam.ScreenToWorldPoint(Input.mousePosition);
                // temp.z = 0;
                // x.transform.position = temp;

            };
            pool.onBeforeReturn += x =>
            {
                //todo  
                if (x.rb == null)
                    x.rb = x.GetComponent<Rigidbody2D>();
                x.rb.velocity = Vector3.zero;
            };
            pool.MaxCount = 1000;
            pool.PreloadAsync(500, 5).Subscribe();
            // pool.StartShrinkTimer(10, 0.9f, 500);
        }
        private void Update()
        {
            // if (Input.anyKeyDown)
            // {
            //     BrickMgrM.PoolModule.GetPool<BallEntity>()
            //         .RentAsync()
            //         .Subscribe(x =>
            //         {
            //             // var z = Random.Range(0, 360);
            //             // x.Init(new Vector3(0, 0, z));
            //         });
            // }
        }
    }
}
