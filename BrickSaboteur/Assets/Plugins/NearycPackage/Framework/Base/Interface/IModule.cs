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
using System;
using System.Collections.Generic;
using NearyFrame.Base;

namespace NearyFrame
{
    public interface IModule
    {

    }
    public interface IModule<E> : IModule where E : IModuleTag<E>
    {
        // Dictionary<Type, ElementBase<E>> ModuleElementDict { get; }
        bool RegisterSingleton(ElementBase<E> ele);
        void UnRegisterSingleton(ElementBase<E> ele);
        T GetElement<T>() where T : ElementBase<E>,
        new();
    }
}
