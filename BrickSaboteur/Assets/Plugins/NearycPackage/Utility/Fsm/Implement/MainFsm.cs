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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nearyc.Utility
{
	public class MainFsm : FsmBase
	{
		public readonly List<Enum> tempTriggerList;
		public readonly HashSet<Enum> triggerSet;
		public readonly Dictionary<Enum, bool> boolDict;
		public readonly Dictionary<Enum, float> floatDict;
		public readonly AnyState anyState;
		public readonly IntoState intoState;
		/// <summary> 构造函数 </summary>
		public MainFsm(Type typeBool,Type tyFloat)
		{
			triggerSet = new HashSet<Enum>();
			boolDict = new Dictionary<Enum, bool>();
			floatDict = new Dictionary<Enum, float>();
			tempTriggerList = new List<Enum>();
			anyState = new AnyState(this);
			intoState = new IntoState(this, this);

			_currentNode = intoState;

			// ---------------------------------
			// 通过反射初始化FLoat和Bool对应的字典
			var boolFields = typeBool.GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (var item in boolFields)
			{
				boolDict.Add((EBoolCondition)item.GetValue(null), false);
			}
			var floatFields = tyFloat.GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (var item in floatFields)
			{
				floatDict.Add((EFloatCondition)item.GetValue(null), 0);
			}
		}
		private void UpdateCurrent()
		{
			var cur = GetCurrentState();
			cur.FsmBelongTo.SetCurrentNode(cur);
			var belongFsm = cur.FsmBelongTo as SubFsm;
			SubFsm higher;
			//从下往上设置每个子节点currentnode
			while (belongFsm != null)
			{
				higher = belongFsm.FsmBelongTo as SubFsm;
				if (higher != null)
				{
					//更高节点是SubFsm
					higher.SetCurrentNode(belongFsm);
				}
				else if (belongFsm.FsmBelongTo is MainFsm)
				{
					//更高节点是mainfsm
					(belongFsm.FsmBelongTo as MainFsm).SetCurrentNode(belongFsm);
					break;
				}
				belongFsm = higher;
			}
		}
		#region External

		/// <summary>
		/// 启动（重启）状态机，外部调用
		/// </summary>
		public void StartOrRestart()
		{
			_currentNode = intoState;
			GetCurrentState().transitionList.ForEach(x =>
			{
				if (LoopThroughTrasitionConditions(x))
					return;
			});
		}
		/// <summary>
		/// 表现出当前状态的行为，外部调用
		/// </summary>
		public void Update()
		{
			GetCurrentState().OnStateUpdate();
		}
		/// <summary>
		/// SetTrigger,外部调用
		/// </summary>
		/// <param name="eTrigger"></param>
		public void SetTrigger(ETriggerCondition eTrigger)
		{
			triggerSet.Add(eTrigger);
			//优先anyState
			anyState.transitionList.ForEach(tra =>
			{
				// 目标状态不为当前状态
				if (tra.to != GetCurrentState())
					if (LoopThroughTrasitionConditions(tra, () =>
					{
						//如果目标是subfsm，需将trigger加入temp，不移除trigger，待后续继续使用
						if (tra.to is SubFsm)
						{
							tempTriggerList.Add(eTrigger);
						}
					}))
					{
						//成功变更状态
						//从AnyStte更新状态后，需从下往上更新current
						UpdateCurrent();
						// 已经成功找到下一个状态，停止遍历
						return;
					}
			});
			//再Current
			GetCurrentState().transitionList.ForEach(tra =>
			{
				if (LoopThroughTrasitionConditions(tra, () =>
				{
					//如果目标是subfsm，需将trigger加入temp，不移除trigger，待后续继续使用
					if (tra.to is SubFsm)
					{
						tempTriggerList.Add(eTrigger);
					}
				}))
				{
					// 已经成功找到下一个状态，停止遍历
					return;
				}
			});
			//即使没有改变状态，也将这次的trigger移除
			triggerSet.Remove(eTrigger);
		}
		/// <summary>
		/// SetFloat,外部调用
		/// </summary>
		/// <param name="eFloat"></param>
		/// <param name="value"></param>
		public void SetFloat(EFloatCondition eFloat, float value)
		{
			//if (floatDict.ContainsKey(eFloat))
			//else
			//floatDict.Add(eFloat, value);
			floatDict[eFloat] = value;
			//优先anyState
			anyState.transitionList.ForEach(tra =>
			{
				// 目标状态不为当前状态
				if (tra.to != GetCurrentState())
					if (LoopThroughTrasitionConditions(tra))
					{
						//成功变更状态,从AnyStte更新状态后，需从下往上更新current
						UpdateCurrent();
						// 已经成功找到下一个状态，停止遍历
						return;
					}
			});
			//再Current
			GetCurrentState().transitionList.ForEach(tra =>
			{
				if (LoopThroughTrasitionConditions(tra))
					//成功变更状态，直接更新状态后，只有本fsm的current变化
					// 已经成功找到下一个状态，停止遍历
					return;
			});
		}
		/// <summary>
		/// SetBool,外部调用
		/// </summary>
		/// <param name="eBool"></param>
		/// <param name="value"></param>
		public void SetBool(EBoolCondition eBool, bool value)
		{
			//if (boolDict.ContainsKey(eBool))
			//else
			//	boolDict.Add(eBool, value);
			boolDict[eBool] = value;
			//优先anyState
			anyState.transitionList.ForEach(tra =>
			{
				// 目标状态不为当前状态
				if (tra.to != GetCurrentState())
					if (LoopThroughTrasitionConditions(tra))
					{
						//成功变更状态
						//从AnyStte更新状态后，需从下往上更新current
						UpdateCurrent();
						// 已经成功找到下一个状态，停止遍历
						return;
					}
			});
			//再Current
			GetCurrentState().transitionList.ForEach(tra =>
			{
				if (LoopThroughTrasitionConditions(tra))
					//成功变更状态，直接更新状态后，只有本fsm的current变化
					// 已经成功找到下一个状态，停止遍历
					return;
			});
		}
		#endregion
		#region Factory

		public FsmState FsmStateFactory(string name, Action onStateEnter = null, Action onStateExit = null, Action onStateUpdate = null)
		{
			return new FsmState(this, this, name, onStateEnter, onStateExit, onStateUpdate);
		}
		public StateTransition TransitionFactoty(IFsmNode from, IFsmNode to)
		{
			return new StateTransition(from, to, this, this);
		}
		#endregion
	}
}
