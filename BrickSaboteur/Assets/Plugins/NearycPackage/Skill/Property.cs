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
    public struct Property
    {
        int basee;
        [SerializeField] int max;
        public int min;
        // public int current;
        [Sirenix.OdinInspector.ShowInInspector]
        public int CurrentValue => current.Value;
        // [Sirenix.OdinInspector.ShowInInspector] 
        public IntReactiveProperty current { get; private set; }
        public FloatReactiveProperty percent => new FloatReactiveProperty((float) current.Value / max);
        List<PropertyNode<int>> adds;
        List<PropertyNode<int>> mores;
        public System.Action onCurrentZero;
        public int Max => max;
        //  public int Current => current;
        public void Init(int @base = -1, int? current = null, int? min = null)
        {
            this.basee = @base == -1 ? this.basee : @base;
            adds = new List<PropertyNode<int>>();
            mores = new List<PropertyNode<int>>();
            this.current = new IntReactiveProperty();
            this.min = min.HasValue?min.Value : 0;

            OnValueChanged();
            this.current.Value = current.HasValue?current.Value : max;
        }
        public void ModifyCurrent(int amount)
        {
            current.Value += amount;
            if (current.Value > max)
            {
                current.Value = max;
            }
            if (current.Value < min)
            {
                current.Value = min;
                if (onCurrentZero != null)
                    onCurrentZero();
            }
        }
        private void OnValueChanged()
        {
            if (max == 0) max = basee == 0 ? 1 : basee;

            float percent = (float) current.Value / (float) max;
            float moreSum = 0;
            mores.ForEach(x => moreSum += x.value);
            if (moreSum <= -100)
            {
                current.Value = max = 0;
                return;
            }
            float addSum = 0;
            adds.ForEach(x => addSum += x.value);
            max = System.Convert.ToInt32((basee + addSum) * (1 + moreSum / 100));
            current.Value = (int) ((float) max * percent);
        }
        public void Add(PropertyNode<int> change)
        {
            adds.Add(change);
            OnValueChanged();
        }
        public void Add(int change)
        {
            adds.Add(new PropertyNode<int>(change, null));
            OnValueChanged();
        }
        public void RemoveAdd(PropertyNode<int> toRemove)
        {
            adds.Remove(toRemove);
            OnValueChanged();
        }
        public void RemoveAdd(int change)
        {
            adds.Remove(new PropertyNode<int>(change, null));
            OnValueChanged();
        }
        public void More(PropertyNode<int> change)
        {
            mores.Add(change);
            OnValueChanged();
        }
        public void More(int change)
        {
            mores.Add(new PropertyNode<int>(change, null));
            OnValueChanged();
        }
        public void RemoveMore(PropertyNode<int> toRemove)
        {
            mores.Remove(toRemove);
            OnValueChanged();
        }
        public void RemoveMore(int change)
        {
            mores.Remove(new PropertyNode<int>(change, null));
            OnValueChanged();
        }

    }
}
