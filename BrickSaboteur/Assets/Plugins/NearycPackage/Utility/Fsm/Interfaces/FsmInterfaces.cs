using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearyc.Utility
{
	public interface IFsm
	{
		FsmStateNode CurrentState { get; }
	}
	public interface IMainFsm : IFsm
	{
		void SetTrigger(ETriggerCondition eTrigger);
		void SetBool(EBoolCondition eBool, bool value);
		void SetFloat(EFloatCondition eFloat, float value);
	}
	public interface IFsmNode
	{
		string Name { get; }
		FsmBase FsmBelongTo { get; }
		void OnStateEnter();
		void OnStateExit();
		void AddTransition(StateTransition transition);
		void RemoveTrasition(StateTransition transition);
	}
}
