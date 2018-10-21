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
	/// 主状态机（mainFsm） 子状态机（subFsm） 状态节点（stateNode） 状态变化（stateTrasition）
	/// 主状态机含有几个集合，对应不同的Enum （Trigger，Bool，Float等） 通过设置集合的值来实现状态改变
	/// 主状态机和子状态机中都可以有若干Node，其中子状态机也可以为Node
	/// 
	/// 主状态机和子状态机 都有入口（IntoState）根据stateTrasition切换初始状态
	/// 主状态机 有anyState，可以根据stateTrasition切到任何状态
	/// 子状态机 还有出口（outState） 
	/// 
	/// Node间转换需通过stateTrasition来变化
	/// 除了anyState节点，一般只访问当前层状态机中的节点
	/// 节点不关心如何进入，只关心所有的transition
	/// 
	/// 根据当前节点的不同，表现出不同的行为（GetCurrentState().OnStateUpdate();）
	/// 
	/// 再由一层monobehavior类包装MainFsm
	/// 再由其他类来调用包装类里的GetCurrentStateName，SetBool，SetTrigger，SetFloat...
	/// 
	/// Example:
	/// idle <---> run
	/// anystate --->flySubFsm
	/// flySubFsmIn --->Drop
	/// Drop ---> flySubFsmOut ---> flySubFsm --->idle
	/// </summary>
	public class TestCreatureFsm : FsmMono
	{
		protected override void InitFsm()
		{
            _fsm = new MainFsm(typeof(EBoolCondition),typeof(EFloatCondition));
			//创建默认
			var idle = new IdleState(_fsm, _fsm, "Idle");
			//设置Idle为默认
			_fsm.TransitionFactoty(_fsm.intoState, idle);
			//or By HelperMethod
			var idle2 = _fsm.FsmStateFactory("Idle2");
			//创建跑
			var run = new FsmState(_fsm, _fsm, "Run", () => Debug("I am going to run"), () => Debug("I dont run anymore"), () => Debug("I am runing"));
			//设置条件，如果速度大于0.1 且 在地面上，就run
			_fsm.TransitionFactoty(idle, run)
				.AddCondition(() => GetFloat(EFloatCondition.SpeedMagnitute) > 0.1f && GetBool(EBoolCondition.IsGrounded));
			//反之,速度<=0.1
			_fsm.TransitionFactoty(run, idle)
				.AddCondition(() => GetFloat(EFloatCondition.SpeedMagnitute) <= 0.1f && GetBool(EBoolCondition.IsGrounded));
			//创建飞，trigger为飞
			var flySubFsm = new SubFsm(_fsm, "FlyFsm");
			var tran = new StateTransition(_fsm.anyState, flySubFsm, _fsm);
			//Anystate,Triggger为Fly，就进入Fly子状态机
			tran.AddCondition(() => GetTrigger(ETriggerCondition.Fly));
			//到地面上，就回到idle
			_fsm.TransitionFactoty(flySubFsm, idle)
				.AddCondition(() => GetBool(EBoolCondition.IsGrounded));
			//坠落，速度大于0.1
			var drop = new FsmState(_fsm, flySubFsm, "Drop", () => Debug("I am going to drop !"));
			flySubFsm.TransitionFactoty(flySubFsm.inState, drop)
				.AddCondition(() => GetFloat(EFloatCondition.SpeedMagnitute) > 0.1f);
			//回到地上，变为idle
			flySubFsm.TransitionFactoty(drop, flySubFsm.outState)
				.AddCondition(() => GetBool(EBoolCondition.IsGrounded));
			//状态机启动
			_fsm.StartOrRestart();
		}
		protected void Update()
		{
			// 调用当前状态的onStateUpdate,展现不同行为
			_fsm.Update();
		}
		//Test
		private void TestOutterInstanceCalled()
		{
			//Some event happeneded
			_fsm.SetBool(EBoolCondition.IsGrounded, true);
			//Some event happeneded
			_fsm.SetFloat(EFloatCondition.SpeedMagnitute, 10);
			//Some event happeneded
			_fsm.SetTrigger(ETriggerCondition.Fly);
		}
	}
	//Test
	public abstract class MonoBehaviourTest { }
	/// <summary>
	/// fsm Monobahaviour Whrapper
	/// </summary>
	public abstract class FsmMono : MonoBehaviourTest
	{
		protected MainFsm _fsm;
		#region 供外部调用的方法

		public FsmStateNode CurrentState => _fsm.CurrentState;
		public string CurrentStateName => _fsm.CurrentState.Name;
		public void SetTrigger(ETriggerCondition eTrigger)
		{
			_fsm.SetTrigger(eTrigger);
		}
		public void SetBool(EBoolCondition eBool, bool value)
		{
			_fsm.SetBool(eBool, value);
		}
		public void SetFloat(EFloatCondition eFloat, float value)
		{
			_fsm.SetFloat(eFloat, value);
		}
		#endregion

		#region Virtual
		protected virtual void Start()
		{

			InitFsm();
		}
		protected abstract void InitFsm();
		//Test
		protected virtual void Debug(object str)
		{
			Console.WriteLine(str);
		}
		#endregion

		#region HelperMethod
		protected float GetFloat(EFloatCondition eFloat)
		{
			return _fsm.floatDict[eFloat];
		}
		protected bool GetBool(EBoolCondition eBool)
		{
			return _fsm.boolDict[eBool];
		}
		protected bool GetTrigger(ETriggerCondition eTrigger)
		{
			return _fsm.triggerSet.Contains(eTrigger);
		}
		#endregion
	}
}
