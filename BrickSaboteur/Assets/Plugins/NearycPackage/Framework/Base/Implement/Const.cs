#region Author & Version
//==================================================== 
// Author：Nearyc 
// File name：......
// Version：V1.0.1
// Date : 
//*Function: 提供全局Const
//===================================================
// Fix:
//===================================================

#endregion
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
namespace NearyFrame
{
    public static class Const
    {
        private const float InitializeWaitTime = 0.1f;
        public readonly static WaitForSeconds InitializeWaitForSecond = new WaitForSeconds(InitializeWaitTime);
        // public static System.IObservable<T> ToObservable<T>(this UnityEngine.ResourceManagement.IAsyncOperation asyncOperation)
        // where T : class
        // {
        //     if (asyncOperation == null)throw new System.ArgumentNullException("asyncOperation");

        //     return Observable.FromCoroutine<T>((observer, cancellationToken) => Test(asyncOperation, observer, cancellationToken));
        // }
        // static System.Collections.IEnumerator Test<T>(
        //     UnityEngine.ResourceManagement.IAsyncOperation op,
        //     System.IObserver<T> ob,
        //     System.Threading.CancellationToken tk
        // )   where T : class
        // {
        //     while (!op.IsDone && !tk.IsCancellationRequested)
        //     {
        //         ob.OnNext(op.Current as T);
        //         yield return null;
        //     }
        //     if (!tk.IsCancellationRequested)
        //     {
        //         ob.OnNext(op.Result as T);
        //         ob.OnCompleted();
        //     }
        // }
        // public static System.IObservable<float> ToObservable1(this UnityEngine.ResourceManagement.IAsyncOperation asyncOperation)
        // {
        //     if (asyncOperation == null)throw new System.ArgumentNullException("asyncOperation");

        //     return Observable.FromCoroutine<float>((observer, cancellationToken) => Test1(asyncOperation, observer, cancellationToken));
        // }
        // static System.Collections.IEnumerator Test1(
        //     UnityEngine.ResourceManagement.IAsyncOperation op,
        //     System.IObserver<float> ob,
        //     System.Threading.CancellationToken tk
        // )
        // {
        //     while (!op.IsDone && !tk.IsCancellationRequested)
        //     {
        //         ob.OnNext(op.PercentComplete );
        //         yield return null;
        //     }
        //     if (!tk.IsCancellationRequested)
        //     {
        //         ob.OnNext(op.PercentComplete );
        //         ob.OnCompleted();
        //     }
    }
}
// }
