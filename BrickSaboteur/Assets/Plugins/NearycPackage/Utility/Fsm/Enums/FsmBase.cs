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
	/// 状态机基类
	/// </summary>
	public abstract class FsmBase : IFsm
	{
		/// <summary> 当前状态，可以是子状态机中的当前状态 </summary>
		public FsmStateNode CurrentState => GetCurrentState();
		/// <summary> 当前节点，可以是状态或子状态机 </summary>
		protected IFsmNode _currentNode;
		/// <summary> 此状态机下所有节点，TODO </summary>
		readonly Dictionary<string, IFsmNode> _nodeDict = new Dictionary<string, IFsmNode>();
		/// <summary>
		/// 得到当前状态
		/// </summary>
		/// <returns></returns>
		protected FsmStateNode GetCurrentState()
		{
			//curreNode可以为subFsm或者stateNode
			var subFsm = _currentNode as SubFsm;
			if (subFsm != null)
			{
				return subFsm.CurrentState;
			}
			else
			{
				var state = _currentNode as FsmStateNode;
				if (state != null) return state;
				else throw new Exception("Current cannot be null !");
			}
		}

		#region Node
		/// <summary>
		/// 添加节点
		/// </summary>
		/// <param name="name"></param>
		/// <param name="newNode"></param>
		/// <returns></returns>
		public bool AddNode(IFsmNode newNode, string name = null)
		{
            name=name??newNode.Name;
			// if (_nodeDict.ContainsKey(name)){
                _nodeDict[name]=newNode;
                return true;
			// return false;
            // }
            // else{
                // _nodeDict.Add(name,newNode);
            //  return true;
            // }

			//todo report same key
		}
		/// <summary>
		/// 移除节点
		/// </summary>
		/// <param name="name"></param>
		public void RemoveNode(string name)
		{
			_nodeDict.Remove(name);
		}
		/// <summary>
		/// 尝试得到节点
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public bool TryGetNode<T>(string name, out T node) where T : class, IFsmNode
		{
			IFsmNode temp;
			if (_nodeDict.TryGetValue(name, out temp))
			{
				node = temp as T;
				return true;
			}
			node = null;
			return false;
		}
		#endregion

		/// <summary>
		/// 改变当前状态为trasition.to
		/// </summary>
		/// <param name="transition"></param>
		protected virtual void ChangeCurrentState(StateTransition transition)
		{
			//前者调用onExit，后者调用onEnter
			if (transition.from != null) transition.from.OnStateExit();
			_currentNode = transition.to;
			transition.to.OnStateEnter();

			//状态改变成功后立马遍历一次改变后状态
			GetCurrentState().transitionList.ForEach(tra =>
			{
				if (LoopThroughTrasitionConditions(tra))
				{
					return;
				}
			});

			// OnChangeCurrentState(transition);
		}
		// protected abstract void OnChangeCurrentState(StateTransition transition);
		/// <summary>遍历Trasition里所有的condition </summary>
		/// <param name="tra"></param>
		/// <param name="funcBeforeChangeState"></param>
		/// <returns> return :是否改变了状态，改变了就停止遍历</returns>
		protected bool LoopThroughTrasitionConditions(StateTransition trasition, Action funcBeforeChangeState = null)
		{
			foreach (var func in trasition.GetConditions())
			{
				if (func.Invoke())
				{
					//成功找到下一个状态，停止遍历
					if (funcBeforeChangeState != null) funcBeforeChangeState.Invoke();
					ChangeCurrentState(trasition);
					return true;
				}
			}
			return false;
		}
		/// <summary> 供当前fsm的子fsm调用 </summary>
		/// <summary>遍历Trasition里所有的condition </summary>
		/// <param name="tra"></param>
		/// <param name="funcWhenChangeState"></param>
		/// <returns>是否改变了状态，改变了就停止遍历</returns>
		public bool LoopThroughTrasitionConditions(SubFsm subFsm, StateTransition tra, Action funcWhenChangeState = null)
		{
			//防止误调用
			if (!_nodeDict.ContainsKey(subFsm.Name)) return false;

			return LoopThroughTrasitionConditions(tra, funcWhenChangeState);
		}
		/// <summary>
		/// 设置节点，节点必须属于本状态机的
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool SetCurrentNode(IFsmNode node)
		{
			if (_nodeDict.ContainsKey(node.Name))
			{
				_currentNode = node;
				return true;
			}
			return false;
		}

	}
}
