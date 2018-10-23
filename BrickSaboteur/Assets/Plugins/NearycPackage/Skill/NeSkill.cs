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
using Nearyc.Skill;
using UniRx;
using UnityEngine;
namespace Nearyc.Skill
{
    /// <summary>
    /// 一般技能
    /// </summary>
    public class NormalSKill : NeSkill
    {
        public NormalSKill(Component addToDispose, ESkillTag type, float cooldown = 0) : base(addToDispose, type, cooldown)
        {

        }
        public override void OnUpdate()
        {
            _cooldown.ModifyCurrent(Time.deltaTime);
        }
    }
    /// <summary>
    /// 持续时间技能
    /// </summary>
    public class DurationSKill : NeSkill, IDurationSKill
    {

        public DurationSKill(Component addToDispose, ESkillTag type, float cooldown = 0, float dutation = 0) : base(addToDispose, type, cooldown)
        {

        }

        [SerializeField] protected PropertyFloat _duration;
        public PropertyFloat Duration => _duration;

        [SerializeField] protected bool _isInUse;

        public bool IsInUse => _isInUse;

        public Action onSkillUpdate { get; set; }
        public Action onSkillEnd { get; set; }
        public override void TryExecuteSKill(bool condition)
        {
            //Todo
            throw new Exception();
        }
        public override void OnUpdate()
        {
            _duration.ModifyCurrent(Time.deltaTime);
            _cooldown.ModifyCurrent(Time.deltaTime);
        }

        public virtual void TryReleaseSkill()
        {
            _duration.ModifyCurrent(_duration.Max);
            _isInUse = false;
            onSkillEnd();
        }
    }
    /// <summary>
    /// 技能基类
    /// </summary>
    [System.Serializable]
    public abstract class NeSkill : ISkill
    {
        [SerializeField] protected PropertyFloat _cooldown;
        public PropertyFloat Cooldown => _cooldown;
        [SerializeField] protected ESkillTag _type;
        public ESkillTag type => _type;
        protected bool _isTryExecute;
        protected bool _canUse = true;
        public bool Canuse => _canUse;
        public System.Action onSkillStart { get; set; }
        public NeSkill(Component addToDispose, ESkillTag type, float cooldown = 0)
        {
            _cooldown.Init(cooldown);
            _type = type;
        }
        public virtual void TryExecuteSKill(bool condition)
        {
            _isTryExecute = condition;
            if (_canUse && _isTryExecute)
            {
                _isTryExecute = false;
                _cooldown.ModifyCurrent(-_cooldown.Max);
                onSkillStart();
            }
        }
        public abstract void OnUpdate();
    }
}
// {
// _cooldown.ModifyCurrent(Time.deltaTime);
// if (_isInUse)
// {
//     _duration.ModifyCurrent(Time.deltaTime);
//     onSkillUpdate();
//     if (_duration.Current >= _duration.Max)
//     {
//         _isInUse = false;
//         onSkillEnd();
//     }
// }
// }

// public virtual void TryReleaseSkill()
// {
//     // _duration.ModifyCurrent(_duration.Max);
//     // _isInUse = false;
//     onSkillEnd();
// }

// public class NeSkill<T> : NeSkill<T, T>
// {
//     public NeSkill(
//         Component addToDispose, ESkillType type, float cooldown = 0, float duration = 0) : base(addToDispose, type, cooldown, duration)
//     {

//     }
// }
