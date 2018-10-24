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
    /// <summary> 挡板，考虑改名Slider </summary>

    public class SliderEntity : ElementBase<IEntityTag>
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
    }
}
