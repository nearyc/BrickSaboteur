#region Author & Version
//==================================================== 
// Author：Nearyc 
// File name：......
// Version：V1.0.1
// Date : 
//*Function:简易有限状态机工具类
//===================================================
// Fix:
//===================================================

#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nearyc.Utility
{
    public class Sub_FSM
    {
        public FiniteStateMachine.FSMState parentState;
        public FiniteStateMachine selfFSM;
        /// <summary>
        /// 父状态机中的状态
        /// </summary>
        /// <param name="parentFSM"></param>
        /// <param name="parentStateName"></param>
        public void ParentState(FiniteStateMachine parentFSM, string parentStateName)
        {
            parentState = parentFSM.stateDict[parentStateName];
        }
    }
    public class FiniteStateMachine
    {
        public delegate void Callfunc(params object[] param);
        public class FSMState
        {
            private string mName;

            public FSMState(string name)
            {
                mName = name;
            }
            public readonly Dictionary<string, FSMTranslation> TranslationDict = new Dictionary<string, FSMTranslation>();
        }
        public class FSMTranslation
        {
            public string fromState;
            public string name;
            public string toState;
            public Callfunc onTranslationCallback; // 回调函数

            public FSMTranslation(string fromState, string name, string toState, Callfunc onTranslationCallback)
            {
                this.fromState = fromState;
                this.toState = toState;
                this.name = name;
                this.onTranslationCallback = onTranslationCallback;
            }
        }
        public string State { get; private set; }
        public readonly Dictionary<string, FSMState> stateDict = new Dictionary<string, FSMState>();
        public void AddState(string name)
        {
            stateDict[name] = new FSMState(name);
        }
        public void AddTranslation(string fromState, string name, string toState, Callfunc callfunc)
        {
            stateDict[fromState].TranslationDict[name] = new FSMTranslation(fromState, name, toState, callfunc);
        }
        public void Start(string name)
        {
            State = name;
        }
        public void HandleEvent(string name, params object[] param)
        {
            if (State != null && stateDict[State].TranslationDict.ContainsKey(name))
            {
                FSMTranslation tempTranslation = stateDict[State].TranslationDict[name];
                tempTranslation.onTranslationCallback(param);
                State = tempTranslation.toState;
            }
        }
        public void Clear()
        {
            stateDict.Clear();
        }
    }
}
