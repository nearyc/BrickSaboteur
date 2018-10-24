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
using System.Collections;
using System.Reflection;
using NearyFrame.Base;
using UnityEngine;
namespace NearyFrame
{
    public interface IModuleTag
    {

    }
    public interface IModuleTag<E> : IModuleTag where E : IModuleTag<E>
    {
        // bool InitializeCondition { get; }
        bool RegisterSingleton(ElementBase<E> ele);
        void UnRegisterSingleton(ElementBase<E> ele);
        T GetElement<T>() where T : ElementBase<E>,
        new();
    }

}
