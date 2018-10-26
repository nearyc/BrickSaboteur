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
    // [System.Flags]
    public enum EBonusTag
    {
        Plus = 1 << 0,
        Multiply = 1 << 1,
        Life = 1 << 2,
        SliderSize = 1 << 3,
    }
    /// <summary>
    /// 属性的Bonus
    /// </summary>
    /// <typeparam name="IEntityTag"></typeparam>
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class BonusEntity : ElementBase<IEntityTag>
    {
        [SerializeField] EBonusTag type;
        // [SerializeField] SkillHolder skillHolder;
        [SerializeField][Range(2, 5)] float _speed = 4;
        Rigidbody2D _rb;
        protected override IEnumerator OnStart()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
            yield return BrickMgrM.WaitModule<IEntityTag>();
            yield return BrickMgrM.WaitModule<IPropertyTag>();
            BrickMgrM.EntityModule.RegisteBonus(this);
            // //依赖skillHolder
            // skillHolder = BrickMgrM.PropertyModule.GetElement<SkillHolder>();
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
            var board = other.gameObject.GetComponent<SliderEntity>();
            if (board != null)
            {
                Debug.Log(other.gameObject.name);
                switch (type)
                {
                    case EBonusTag.Plus:
                        //释放plus
                        BrickMgrM.PropertyModule.plusCount.ModifyCurrent(1);
                        MessageBroker.Default.Publish(new SkillTag_TryPlusSkill());
                        MessageBroker.Default.Publish(new PropTag_ModifyPlusCount { value = -1 });
                        break;
                    case EBonusTag.Multiply:
                        //释放Multiply
                        BrickMgrM.PropertyModule.multiplyCount.ModifyCurrent(1);
                        MessageBroker.Default.Publish(new SkillTag_TryModifySkill());
                        MessageBroker.Default.Publish(new PropTag_ModifyMultiplyCount { value = -1 });
                        break;
                    case EBonusTag.Life:
                        //增加生命
                        MessageBroker.Default.Publish(new PropTag_ModifyHealth(1));
                        break;
                    case EBonusTag.SliderSize:
                        MessageBroker.Default.Publish(new PropTag_ModifySliderSize(1, false));
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
