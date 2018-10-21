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

    public class SkillBonusEntity : ElementBase<IEntityTag>
    {
        [SerializeField] EBonusTag type;
        [SerializeField] SkillHolder skillHolder;
        [SerializeField][Range(1, 5)] float _speed;

        protected override IEnumerator OnStart()
        {
            yield return BrickMgrM.WaitModule<IEntityTag>();
            yield return BrickMgrM.WaitModule<IInputTag>();
            skillHolder = BrickMgrM.InputModule.GetElement<SkillHolder>();

        }
        private void Update()
        {
            if (_isInited)
                this.transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            var board = other.gameObject.GetComponent<BoardEntity>();
            if (board != null)
            {
                switch (type)
                {
                    case EBonusTag.Plus:
                        skillHolder.plusCount.ModifyCurrent(1);
                        skillHolder.TryExecutePlus();
                        break;
                    case EBonusTag.Multiply:
                        skillHolder.multiplyCount.ModifyCurrent(1);
                        skillHolder.TryExecuteMultiply();
                        break;
                    case EBonusTag.Life:
                        skillHolder.lifeCount.ModifyCurrent(1);
                        break;
                    default:

                        break;
                }
                BrickMgrM.LoaderManager.ReleaseObject(this);
            }
            var diad = other.gameObject.GetComponent<DieAreaEntity>();
            if (diad != null)
            {
                BrickMgrM.LoaderManager.ReleaseObject(this);
            }
        }
    }
}
