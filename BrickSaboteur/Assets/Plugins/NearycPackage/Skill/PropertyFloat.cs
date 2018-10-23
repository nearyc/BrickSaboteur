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
using UniRx;
using UnityEngine;
namespace Nearyc.Skill
{
    [System.Serializable]
    public struct PropertyFloat
    {
        float basee;
        [SerializeField] float max;
        float min;
       [Sirenix.OdinInspector.ShowInInspector] public float Current => current.Value;
        private FloatReactiveProperty current;
        public FloatReactiveProperty percent => new FloatReactiveProperty((float) current.Value / max);
        List<PropertyNode<float>> adds;
        List<PropertyNode<float>> mores;
        public float Max => max;
        public System.Action onCurrentEnterZero;
        public void Init(float @base = -1, float? current = null, float? min = null)
        {
            this.basee = @base == -1 ? this.basee : @base;
            adds = new List<PropertyNode<float>>();
            mores = new List<PropertyNode<float>>();
            this.current = new FloatReactiveProperty();
            this.min = min.HasValue?min.Value : 0;

            OnValueChanged();
            this.current.Value = current.HasValue? current.Value : max;

        }
        public void ModifyCurrent(float amount)
        {
            current.Value += amount;
            if (current.Value > max)
            {
                current.Value = max;
            }
            if (current.Value < min)
            {
                current.Value = min;
                if (onCurrentEnterZero != null)
                    onCurrentEnterZero();
            }
        }
        private void OnValueChanged()
        {
            if (max == 0) max = basee == 0 ? 1 : basee;

            float percent = current.Value / max;

            float moreSum = 0;
            mores.ForEach(x => moreSum += x.value);
            if (moreSum <= -100)
            {
                current.Value = max = 0;
                return;
            }
            float addSum = 0;
            adds.ForEach(x => addSum += x.value);
            max = (basee + addSum) * (1 + moreSum / 100);
            current.Value = (max * percent);
        }
        public void Add(PropertyNode<float> change)
        {
            adds.Add(change);
            OnValueChanged();
        }
        public void Add(float change)
        {
            adds.Add(new PropertyNode<float>(change, null));
            OnValueChanged();
        }
        public void RemoveAdd(PropertyNode<float> toRemove)
        {
            adds.Remove(toRemove);
            OnValueChanged();
        }
        public void RemoveAdd(float change)
        {
            adds.Remove(new PropertyNode<float>(change, null));
            OnValueChanged();
        }
        public void More(PropertyNode<float> change)
        {
            mores.Add(change);
            OnValueChanged();
        }
        public void More(float change)
        {
            mores.Add(new PropertyNode<float>(change, null));
            OnValueChanged();
        }
        public void RemoveMore(PropertyNode<float> toRemove)
        {
            mores.Remove(toRemove);
            OnValueChanged();
        }
        public void RemoveMore(float change)
        {
            mores.Remove(new PropertyNode<float>(change, null));
            OnValueChanged();
        }

    }
}
