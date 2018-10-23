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
    [System.Flags]
    public enum EBonusTag
    {
        Plus = 1 << 0,
        Multiply = 1 << 1,
        Life = 1 << 2,
    }
    /// <summary>
    /// 属性的Bonus
    /// </summary>
    /// <typeparam name="IEntityTag"></typeparam>
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class SkillBonusEntity : ElementBase<IEntityTag>
    {
        [SerializeField] EBonusTag type;
        [SerializeField] SkillHolder skillHolder;
        [SerializeField][Range(2, 5)] float _speed = 2;
        Rigidbody2D _rb;
        protected override IEnumerator OnStart()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
            yield return BrickMgrM.WaitModule<IEntityTag>();
            yield return BrickMgrM.WaitModule<IPropertyTag>();
            BrickMgrM.EntityModule.RegisteBonus(this);
            //依赖skillHolder
            skillHolder = BrickMgrM.PeropertyModule.GetElement<SkillHolder>();
            //下降
            Observable.EveryUpdate()
                .Subscribe(__ =>
                {
                    this.transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
                }).AddTo(this);

        }
        private void OnDestroy()
        {
            BrickMgrM.EntityModule.UnRegisteBonus(this);
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            var board = other.gameObject.GetComponent<BoardEntity>();
            if (board != null)
            {
                Debug.Log(other.gameObject.name);
                switch (type)
                {
                    case EBonusTag.Plus:
                        //释放plus
                        skillHolder.plusCount.ModifyCurrent(1);
                        skillHolder.TryExecutePlus();
                        break;
                    case EBonusTag.Multiply:
                        //释放Multiply
                        skillHolder.multiplyCount.ModifyCurrent(1);
                        skillHolder.TryExecuteMultiply();
                        break;
                    case EBonusTag.Life:
                        //增加生命
                        MessageBroker.Default.Publish(new Tag_ModifyHealth(1));
                        break;
                    default:

                        break;
                }
                BrickMgrM.LoaderManager.ReleaseObject(this.gameObject);
            }
            var diad = other.gameObject.GetComponent<DieAreaEntity>();
            if (diad != null)
            {
                BrickMgrM.LoaderManager.ReleaseObject(this.gameObject);
            }
        }
    }
}
