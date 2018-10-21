#region Author & Version
//==================================================== 
// Author：Nearyc 
// File name：......
// Version：V1.0.1
// Date : 
//*Function:技能封装
//===================================================
// Fix:
//===================================================

#endregion
using System;
using UniRx;
using UnityEngine;

namespace Nearyc.Utility
{

    /// <summary>普通技能</summary>
    public struct NDurationSkill<T>
    {
        public float cooldown; //冷却时间
        public float cooldownCounter; //计时器
        public float duration;
        public float durationCounter;
        public bool inUse;
        public bool CanUse => !inUse && isTriggered & (cooldownCounter >= cooldown); //能否使用
        private bool isTriggered; //是否触发了，由外部设置
        public void UpdateCounter(float t)
        {
            if (cooldownCounter <= cooldown)
            {
                cooldownCounter += t;
            }
            if (inUse)
            {
                durationCounter += t;
                onTriggerUpdate();
                if (durationCounter >= duration)
                {
                    inUse = false;
                    onTriggerExit();
                }
            }
        }
        /// <summary>提前结束技能</summary>
        public void TryReleaseSkill()
        {
            durationCounter = duration;
            inUse = false;
            onTriggerExit();
        }
        /// <summary>尝试触发技能</summary>
        public void TryTriggerSKill(bool condition)
        {
            isTriggered = condition;
            if (CanUse)
            {
                isTriggered = false;
                cooldownCounter = 0;
                durationCounter = 0;
                inUse = true;
                onTriggedEnter();
            }
        }
        public System.Func<T> onTriggedEnter; //触发技能
        public System.Func<T> onTriggerUpdate; //触发技能
        public System.Func<T> onTriggerExit; //触发技能
    }
    /// <summary>普通技能</summary>
    public struct NSkill<T>
    {
        public float cooldown; //冷却时间
        public bool CanUse => _isTriggered & (_cooldownCounter >= cooldown); //能否使用
        private float _cooldownCounter; //计时器
        private bool _isTriggered; //是否触发了，由外部设置
        public void UpdateCounter(float t)
        {
            if (_cooldownCounter <= cooldown)
            {
                _cooldownCounter += t;
            }
        }

        /// <summary>尝试触发技能</summary>
        public void TryTriggerSKill(bool condition)
        {
            _isTriggered = condition;
            if (CanUse)
            {
                _isTriggered = false;
                _cooldownCounter = 0;
                triggedAction();
            }
        }
        public System.Func<T> triggedAction; //触发技能
    }
    /// <summary>
    /// 技能冷却时间
    /// </summary>
    public struct NCoolDown
    {
        public float coolDown;
        public float Counter; //计时器，需要Manager统一计时
        public bool CanUse => Counter > coolDown; //技能计时器大于冷却时间时候可以触发
        public void Execute()
        {
            if (!CanUse) return;
            triggedAction();
        }
        public System.Action triggedAction;
    }
    /// <summary>
    /// 技能触发条件，触发时，调用triggedAction
    /// </summary>
    public struct NTrigger
    {
        private bool _isTriggered;
        public bool IsTriggered
        {
            set
            {
                _isTriggered = value;
                if (_isTriggered)
                {
                    _isTriggered = false;
                    triggedAction();
                }
            }
        }
        public System.Action triggedAction;
    }
}
