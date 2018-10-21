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
    public class BallEntity : ElementBase<IEntityTag>
    {
        [SerializeField][Range(5, 10)] public float initSpeed = 5;
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
        public void Init(Vector3 pos, Vector3 rotation)
        {
            transform.position = pos;
            transform.localRotation = Quaternion.Euler(rotation);
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            rb.velocity = transform.up * initSpeed;
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
        private void OnCollisionEnter2D(Collision2D other)
        {
            var level = other.transform.GetComponent<LevelTileEntity>();
            if (level != null)
            {
                foreach (var contact in other.contacts)
                {
                    var point = (Vector2) contact.point;
                    point += this.rb.velocity.normalized * -0.05f;
                    BrickMgrM.GridManager.ReleaseTileWorldPos(point);
                }
            }
        }
    }
}
