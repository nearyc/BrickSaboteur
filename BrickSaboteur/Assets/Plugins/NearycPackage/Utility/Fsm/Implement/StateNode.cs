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
	/// 状态基类
	/// </summary>
	public abstract class FsmStateBase
	{
		public readonly List<StateTransition> transitionList;
		/// <summary> 构造函数
		public FsmStateBase()
		{
			transitionList = new List<StateTransition>();
		}
		public virtual void AddTransition(StateTransition transition)
		{
			transitionList.Add(transition);
			//如果是anyState，移除from
			if (this is AnyState)
			{
				transition.from = null;
			}
		}

		public virtual void RemoveTrasition(StateTransition transition)
		{
			transitionList.Remove(transition);
		}
	}
	/// <summary>
	/// 状态节点基类，与AnyState区分
	/// </summary>
	public abstract class FsmStateNode : FsmStateBase, IFsmNode
	{
		protected readonly MainFsm _mainFsm;
		protected readonly FsmBase _fsm;
		protected readonly string _name;
		public string Name => _name;

		public FsmBase FsmBelongTo => _fsm;
		/// <summary> 构造函数

		public FsmStateNode(MainFsm finiteStateMachine, FsmBase fsmBase, string name = null) : base()
		{
			_mainFsm = finiteStateMachine;
			this._fsm = fsmBase;
			this._name = name ?? this.GetType().ToString();
			fsmBase.AddNode(this);
		}
		public override void AddTransition(StateTransition transition)
		{
			//不能是自己,避免循环
			if (this == transition.to) return;
			transition.from = this;
			base.AddTransition(transition);
		}
		public virtual void OnStateEnter() { }
		public virtual void OnStateExit() { }
		public virtual void OnStateUpdate() { }

	}
	/// <summary>
	/// 一般的fsmState
	/// </summary>
	public class FsmState : FsmStateNode
	{
		readonly Action onStateEnter;
		readonly Action onStateExit;
		readonly Action onStateUpdate;
		/// <summary> 构造函数
		public FsmState(MainFsm finiteStateMachine, FsmBase fsmBase, string name, Action onStateEnter = null, Action onStateExit = null, Action onStateUpdate = null) : base(finiteStateMachine, fsmBase, name)
		{
			this.onStateEnter = onStateEnter;
			this.onStateExit = onStateExit;
			this.onStateUpdate = onStateUpdate;
		}
		public override void OnStateEnter()
		{
			if (onStateEnter != null)
				onStateEnter();
		}
		public override void OnStateExit()
		{
			if (onStateExit != null)
				onStateExit();
		}
		public override void OnStateUpdate()
		{
			if (onStateUpdate != null)
				onStateUpdate();
		}
	}

	/// <summary>
	/// 用于进入任何不是当前状态的，状态或子状态机
	/// </summary>
	public sealed class AnyState : FsmStateBase
	{
		readonly FsmBase fsmBase;

		public AnyState(FsmBase fsmBase)
		{
			this.fsmBase = fsmBase;
		}
	}
	/// <summary> 无trasitionList,只能调用fsmBase的trasitionList </summary>
	public sealed class OutState : FsmStateNode
	{
		public OutState(MainFsm mainFsm, FsmBase fsmBase) : base(mainFsm, fsmBase, "exit") { }

		public override void OnStateEnter()
		{
			(_fsm as SubFsm).Exit(this);
		}
	}
	/// <summary> 不能被trasition to，用于进入状态机时调用trasitionList </summary>
	public sealed class IntoState : FsmStateNode
	{
		public IntoState(MainFsm mainFsm, FsmBase fsmBase) : base(mainFsm, fsmBase, "enter") { }

	}
	//Test
	public class IdleState : FsmStateNode
	{
		public IdleState(MainFsm mainFsm, FsmBase fsmBase, string name = null) : base(mainFsm, fsmBase, name)
		{

		}
		public override void OnStateEnter()
		{

		}

		public override void OnStateExit()
		{

		}

	}
	//Test
	public class MoveState : FsmStateNode
	{
		public MoveState(MainFsm mainFsm, FsmBase fsmBase, string name = null) : base(mainFsm, fsmBase, name)
		{

		}
		public override void OnStateEnter()
		{

		}

		public override void OnStateExit()
		{

		}

	}
}
