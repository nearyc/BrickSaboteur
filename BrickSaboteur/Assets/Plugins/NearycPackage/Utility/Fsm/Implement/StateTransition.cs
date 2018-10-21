#region Author & Version
//==================================================== 
// Author：Nearyc 
// File name：......
// Version：V1.0.1
// Date : 2018.10.20
//*Function:
//===================================================
// Fix:
//===================================================

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearyc.Utility
{
	public sealed class StateTransition
	{
		private readonly MainFsm _mainFsm;
		public readonly FsmBase fsmBelongTo;
		public IFsmNode from;
		public readonly IFsmNode to;
		private readonly HashSet<Func<bool>> _conditionSet;
		// ---------------------- 
		/// <summary> 构造函数 供anystate调用 </summary>
		public StateTransition(AnyState anyState, IFsmNode to, MainFsm mainFsm, FsmBase fsmBelongTo = null)
		{
			if (anyState == null) return;
			if (from == to)
			{
				//Todo Error
				return;
			}
			this._mainFsm = mainFsm;
			this.fsmBelongTo = fsmBelongTo ?? mainFsm;
			this.from = null;
			this.to = to;
			_conditionSet = new HashSet<Func<bool>>();

			anyState.AddTransition(this);
		}
		/// <summary> 构造函数 </summary>
		public StateTransition(IFsmNode from, IFsmNode to, MainFsm mainFsm, FsmBase fsmBelongTo = null)
		{
			if (from == to)
			{
				//Todo Error
				return;
			}
			this._mainFsm = mainFsm;
			this.fsmBelongTo = fsmBelongTo ?? mainFsm;
			this.from = from;
			this.to = to;
			_conditionSet = new HashSet<Func<bool>>();

			this.from.AddTransition(this);
		}
		// ---------------------------------
		#region Condition
		/// <summary> 添加条件 </summary>
		public StateTransition AddCondition(Func<bool> condition)
		{
			if (_conditionSet.Add(condition))
			{
				//Todo
			}
			return this;
		}
		/// <summary> 移除条件 </summary>
		public StateTransition RemoveCondition(Func<bool> condition)
		{
			if (_conditionSet.Remove(condition))
			{
				//Todo
			}
			return this;
		}
		/// <summary>
		/// 得到hashSet中所有的条件
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Func<bool>> GetConditions()
		{
			return _conditionSet;
		}
		#endregion
		// private void Test()
		// {
		//     this.AddCondition(() =>
		//     {
		//         var bo = _mainFsm.triggerSet.Contains(ETriggerCondition.DefaultT) && _mainFsm.boolDict[EBoolCondition.DefaultB] == false && _mainFsm.floatDict[EFloatCondition.DefaultF] > 1f;
		//         return bo;
		//     });
		// }
	}
}
