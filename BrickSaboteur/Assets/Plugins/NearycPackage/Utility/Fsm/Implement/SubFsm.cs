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
	/// <summary>
	/// 子状态机，内含多个状态节点和子状态机节点
	/// </summary>
	public class SubFsm : FsmBase, IFsmNode
	{
		protected readonly MainFsm _mainFsm;
		protected readonly FsmBase _fsmBelongTo;
		public FsmBase FsmBelongTo => _fsmBelongTo;
		// ---------------------- 
		public readonly IntoState inState;
		public readonly OutState outState;
		// ---------------------- 
		public readonly List<StateTransition> transitionList;
		protected readonly string _name;
		public string Name => _name;
		// ---------------------- 
		/// <summary> 构造函数 </summary>
		public SubFsm(MainFsm mainFsm, string name, FsmBase fsmBelongTo = null)
		{
			this._mainFsm = mainFsm;
			this._fsmBelongTo = fsmBelongTo ?? mainFsm;
			transitionList = new List<StateTransition>();

			inState = new IntoState(this._mainFsm, this);
			outState = new OutState(this._mainFsm, this);
			_currentNode = inState;

			this._name = name ?? this.GetType().ToString();
			mainFsm.AddNode(this);
		}
		/// <summary>
		/// 进入子状态机，会在上一层中遍历，无需在此遍历enterState
		/// </summary>
		public void OnStateEnter()
		{
			_currentNode = inState;
		}
		/// <summary>
		/// 退出状态机
		/// </summary>
		public void OnStateExit()
		{
			_currentNode = inState;
		}
		/// <summary> 退出，供ExitState调用 </summary>
		public void Exit(OutState exit)
		{
			if (exit == this.outState)
			{
				transitionList.ForEach(x =>
				{
					if (_mainFsm.LoopThroughTrasitionConditions(this, x))
						return;
				});
			}
		}
		/// <summary>
		/// 改变当前状态为trasition.to，如果状态改变了，会清理mainFsm里储存的Trigger
		/// </summary>
		/// <param name="transition"></param>
		protected override void ChangeCurrentState(StateTransition transition)
		{
			//前者调用onExit，后者调用onEnter
			if (transition.from != null) transition.from.OnStateExit();
			_currentNode = transition.to;
			transition.to.OnStateEnter();

			//状态改变成功后立马遍历一次
			GetCurrentState().transitionList.ForEach(tra =>
			{
				if (LoopThroughTrasitionConditions(tra))
				{
					//在subfsm里，如果目标不是subfsm，每次变更状态就清理mainFsm里储存的Trigger
					if (_mainFsm.tempTriggerList.Count > 0 && transition.to is SubFsm == false)
						foreach (var item in _mainFsm.tempTriggerList)
						{
							_mainFsm.triggerSet.Remove(item);
							// _mainFsm.tempTriggerList.Remove(item);
						}
					_mainFsm.tempTriggerList.Clear();
					return;
				}
			});

			// mainFsm.tempTriggerList.Clear();
		}
		#region Transition
		public void AddTransition(StateTransition transition)
		{
			transitionList.Add(transition);
		}

		public void RemoveTrasition(StateTransition transition)
		{
			transitionList.Remove(transition);
		}
		#endregion
		#region Factory

		public StateTransition TransitionFactoty(IFsmNode from, IFsmNode to)
		{
			return new StateTransition(from, to, _mainFsm, this);
		}
		public FsmState FsmStateFactory(string name, Action onStateEnter = null, Action onStateExit = null, Action onStateUpdate = null)
		{
			return new FsmState(_mainFsm, this, name, onStateEnter, onStateExit, onStateUpdate);
		}
		#endregion
	}
}
