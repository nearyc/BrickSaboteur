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
using Unity.Linq;
using UnityEngine;
namespace BrickSaboteur
{
    public class GridEntity : ElementBase<IEntityTag>
    {
        BoxCollider2D _midTrigger;
        // public Transform leftPos;
        public Transform right;
        //  Transform _mid;
        public int difficulty;
        protected override IEnumerator OnStart()
        {
            yield return null;
            right = right.GetComponentFromChildren(this, nameof(right));

            var temp = this.name.Substring(0, this.name.Length - 7);
            var name = temp.Split(',');
            foreach (var item in name)
            {
                // Debug.Log(item);
            }
            difficulty = System.Convert.ToInt32(name[name.Length - 1]);

            _midTrigger = this.gameObject.AddComponent<BoxCollider2D>();
            _midTrigger.offset = right.localPosition / 2;
            _midTrigger.size = new Vector2(1, 100);
            _midTrigger.isTrigger = true;
            //Todo Generate ----
        }
        bool triggered;
        //人物到中间时，生成下一个网格
        private void OnTriggerEnter2D(Collider2D other)
        {
            // var player = other.GetComponent<BoardEntity>();
            // if (player != null && !triggered)
            // {

            //     triggered = true;
            //     MgrM.GridManager.GenerateNext();
            // }
        }
    }
}
