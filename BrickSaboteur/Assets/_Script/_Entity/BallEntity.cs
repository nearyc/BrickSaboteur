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
using NearyFrame.Base;
using UnityEngine;
namespace BrickSaboteur
{
    /// <summary>
    /// 小球
    /// </summary>
    /// <typeparam name="IEntityTag"></typeparam>
    public class BallEntity : ElementBase<IEntityTag>
    {
        [Range(5, 10)] public float initSpeed = 10;
        CircleCollider2D _collider;
        public Rigidbody2D rb;
        protected override System.Collections.IEnumerator OnStart()
        {
            _collider = GetComponent<CircleCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            yield return BrickMgrM.WaitModule<IEntityTag>();

            BrickMgrM.EntityModule.RegisteBall(this);
            rb.gravityScale = 0;
            //Init(Vector3.zero, new Vector3(0, 0, -180));
        }
        /// <summary>
        /// 初始化角度和位置
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="rotation"></param>
        public void Init(Vector3 pos, Vector3 rotation, float? speed = null)
        {
            transform.position = pos;
            transform.localRotation = Quaternion.Euler(rotation);
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            var modifier = speed.HasValue?speed.Value : initSpeed;
            rb.velocity = transform.up * modifier;
        }
        private void OnEnable()
        {
            if (_isInited)
                BrickMgrM.EntityModule.RegisteBall(this);
        }
        private void OnDisable()
        {
            BrickMgrM.EntityModule.UnRegisteBall(this);
        }
        /// <summary>
        /// 碰到LevelTile就删除对应位置的Tile
        /// </summary>
        /// <param name="other"></param>
        private void OnCollisionEnter2D(Collision2D other)
        {
            var level = other.transform.GetComponent<LevelTileEntity>();
            if (level != null)
            {
                foreach (var contact in other.contacts)
                {
                    var point = (Vector2) contact.point;
                    point += this.rb.velocity.normalized * -0.02f;
                    BrickMgrM.GridManager.ReleaseTileWorldPos(point);
                }
            }
        }
    }
}
