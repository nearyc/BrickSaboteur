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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nearyc.Skill;
using Nearyc.Utility;
using NearyFrame;
using NearyFrame.Base;
using UniRx;
using Unity.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;
namespace BrickSaboteur
{
    /// <summary> 挡板 </summary>

    public class BoardEntity : ElementBase<IEntityTag>
    {
        EdgeCollider2D _collider;

        protected override System.Collections.IEnumerator OnStart()
        {
            _collider = GetComponent<EdgeCollider2D>();
            yield return BrickMgrM.WaitModule<IEntityTag>();
            BrickMgrM.EntityModule.RegisteBoard(this);

        }
        private void OnDestroy()
        {
            BrickMgrM.EntityModule.UnRegisteBoard(this);
        }
        float _moveX;
        private void Update()
        {
            // transform.position += InputOrigin.delta;
            // transform.Rotate(new Vector3(0, 0, 20));
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            // var ball = other.gameObject.GetComponent<BallEntity>();
            // if (ball != null)
            // {
            //     var half = _collider.size.x / 2;
            //     var offset = -(transform.position - other.transform.position).x;
            //     var offsetAngle = Mathf.Rad2Deg * Mathf.PI * offset / half / 2;
            //     var angle = 180 - offsetAngle;
            //     ball.transform.Rotate(0, 0, angle);
            //     ball.rb.velocity = ball.transform.up * ball.initSpeed;
            // }
        }
    }
}
