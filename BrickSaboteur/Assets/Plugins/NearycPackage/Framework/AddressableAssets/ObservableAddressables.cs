#region Author & Version
//==================================================== 
// Author：Nearyc 
// File name：......
// Version：V1.0.1
// Date : 
//*Function:
//===================================================
// Fix:
//===================================================

#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
#if USE_ADDRESSABLE
using UnityEngine.ResourceManagement;
#endif
namespace UniRx
{
    public static class ObservableAddressables
    {
        /// <summary>Instantiate</summary>
        public static System.IObservable<GameObject> ResourcesInstantiate<T>(object key, Action<AsyncOperation> onCompleted = null)
        where T : UnityEngine.Object
        {

            // Addressables.Instantiate<T>(key)
            return Observable.FromCoroutine<GameObject>((observer, token) =>
            {
                return ResourcesInstantiateCore<T>(key, observer, token, onCompleted);
            });
        }
        private static IEnumerator ResourcesInstantiateCore<T>(
            object key, IObserver<GameObject> observer, CancellationToken token, Action<AsyncOperation> onCompleted = null)
        where T : UnityEngine.Object
        {
            var aop = Resources.LoadAsync(key.ToString(), typeof(T));
            if (onCompleted != null)
            {
                aop.completed += onCompleted;
            }

            while (!aop.isDone && !token.IsCancellationRequested)
            {
                // Debug.Log(1);
                yield return null;
            }
            if (token.IsCancellationRequested) yield break;
            var obj = GameObject.Instantiate<GameObject>(aop.asset as GameObject);
            observer.OnNext(obj);
            observer.OnCompleted();
        }
#if USE_ADDRESSABLE
        /// <summary>ReleaseInstance</summary>
        /// 
        public static void ReleaseAsset<T>(T obj) where T : UnityEngine.Object
        {
            Addressables.ReleaseAsset<T>(obj);
        }
        /// <summary>ReleaseInstance</summary>
        public static void ReleaseInstance(UnityEngine.Object obj, float delay = 0)
        {
            Addressables.ReleaseInstance(obj, delay);
        }
        /// <summary>LoadAssets</summary>
        public static System.IObservable<IList<T>> LoadAssets<T>(object key, Action<IAsyncOperation<T>> onSingleCompleted = null, Action<IAsyncOperation<IList<T>>> onCompleted = null)
        where T : class
        {
            // Addressables.Instantiate<T>(key)
            return Observable.FromCoroutine<IList<T>>((observer, token) =>
            {
                return LoadAssetsCore(key, observer, token, onSingleCompleted, onCompleted);
            });
        }
        private static IEnumerator LoadAssetsCore<T>(
            object key, IObserver<IList<T>> observer, CancellationToken token, Action<IAsyncOperation<T>> onSingleCompleted = null, Action<IAsyncOperation<IList<T>>> onCompleted = null)
        where T : class
        {
            var aop = Addressables.LoadAssets<T>(key, onSingleCompleted);
            if (onCompleted != null)
            {
                aop.Completed += onCompleted;
            }
            while (!aop.IsDone && !token.IsCancellationRequested)
            {
                yield return null;
            }

            if (token.IsCancellationRequested) yield break;
            observer.OnNext(aop.Result);
            observer.OnCompleted();
        }
        /// <summary>LoadAsset</summary>
        public static System.IObservable<T> LoadAsset<T>(object key, Action<IAsyncOperation<T>> onCompleted = null)
        where T : class
        {
            // Addressables.Instantiate<T>(key)
            return Observable.FromCoroutine<T>((observer, token) =>
            {
                return LoadAssetCore(key, observer, token, onCompleted);
            });
        }
        private static IEnumerator LoadAssetCore<T>(
            object key, IObserver<T> observer, CancellationToken token, Action<IAsyncOperation<T>> onCompleted = null)
        where T : class
        {
            var aop = Addressables.LoadAsset<T>(key);
            if (onCompleted != null)
            {
                aop.Completed += onCompleted;
            }

            while (!aop.IsDone && !token.IsCancellationRequested)
            {
                yield return null;
            }

            if (token.IsCancellationRequested) yield break;
            observer.OnNext(aop.Result);
            observer.OnCompleted();
        }
        /// <summary>Instantiate</summary>
        public static System.IObservable<T> Instantiate<T>(object key, Transform parent = null, Action<IAsyncOperation<T>> onCompleted = null, bool isWorldSpace = false)
        where T : UnityEngine.Object
        {
            // Addressables.Instantiate<T>(key)
            return Observable.FromCoroutine<T>((observer, token) =>
            {
                return InstantiateCore(key, observer, token, parent, onCompleted, isWorldSpace);
            });
        }
        private static IEnumerator InstantiateCore<T>(
            object key, IObserver<T> observer, CancellationToken token, Transform parent = null, Action<IAsyncOperation<T>> onCompleted = null, bool isWorldSpace = false)
        where T : UnityEngine.Object
        {
            var aop = Addressables.Instantiate<T>(key, parent, isWorldSpace);
            if (onCompleted != null)
            {
                aop.Completed += onCompleted;
            }

            while (!aop.IsDone && !token.IsCancellationRequested)
            {
                // Debug.Log(1);
                yield return null;
            }

            if (token.IsCancellationRequested) yield break;
            observer.OnNext(aop.Result);
            observer.OnCompleted();
        }
        /// <summary>InstantiateAll</summary>
        public static System.IObservable<IList<T>> InstantiateAll<T>(
            object key, Transform parent = null, Action<IAsyncOperation<T>> onSingleCompleted = null, Action<IAsyncOperation<IList<T>>> onCompleted = null)
        where T : UnityEngine.Object
        {
            // Addressables.Instantiate<T>(key)
            return Observable.FromCoroutine<IList<T>>((observer, token) =>
            {
                return InstantiateAllCore<T>(key, observer, token, parent, onSingleCompleted, onCompleted);
            });
        }
        private static IEnumerator InstantiateAllCore<T>(
            object key, IObserver<IList<T>> observer, CancellationToken token, Transform parent = null, Action<IAsyncOperation<T>> onSingleCompleted = null, Action<IAsyncOperation<IList<T>>> onCompleted = null)
        where T : UnityEngine.Object
        {
            var aop = Addressables.InstantiateAll<T>(key, onSingleCompleted, parent);
            if (onCompleted != null)
            {
                aop.Completed += onCompleted;
            }
            while (!aop.IsDone && !token.IsCancellationRequested)
            {
                yield return null;
            }
            if (token.IsCancellationRequested) yield break;
            observer.OnNext(aop.Result);
            observer.OnCompleted();
        }
#endif
    }
}
