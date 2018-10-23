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
using System.Collections.Generic;
using Nearyc.Skill;
namespace Nearyc.Skill
{
    /// <summary>技能释放者</summary>
    public interface ICaster
    {
        Dictionary<System.Type, ISkill> SkillDict { get; }
        void UpdateSkill();
        bool AddSkill(ISkill skill);
        bool TryGetSKill<T>(out T skill) where T : class, ISkill;
    }
    /// <summary>技能的基础接口</summary>
    public interface ISkill
    {
        ESkillTag type { get; }
        PropertyFloat Cooldown { get; }
        bool Canuse { get; }
        void OnUpdate();
        void TryExecuteSKill(bool condition);
        System.Action onSkillStart { get; set; }
    }
    /// <summary>持续时间技能</summary>
    public interface IDurationSKill : ISkill
    {
        PropertyFloat Duration { get; }
        bool IsInUse { get; }
        void TryReleaseSkill();
        System.Action onSkillUpdate { get; set; }
        System.Action onSkillEnd { get; set; }
    }
    /// <summary>充能技能</summary>
    public interface IChargeSkill : ISkill
    {
        PropertyFloat Count { get; }
    }
    /// <summary>引导技能</summary>
    public interface IChannelingSkill : ISkill
    {
        PropertyFloat Count { get; }
        System.Action onChannelUpdate { get; set; }
        System.Action onChannelEnd { get; set; }
    }

    [System.Flags]
    public enum ESkillTag
    {
        Node = 1 << 0,
        Spell = 1 << 1,
        Attack = 1 << 2,
    }
}
