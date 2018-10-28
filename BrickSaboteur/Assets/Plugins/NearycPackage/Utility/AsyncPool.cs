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
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
#if USE_ADDRESSABLE
using UnityEngine.ResourceManagement;
#endif
namespace Nearyc.Utility
{

    public abstract class AsyncPool { }
    /// <summary>
    /// Async Component Pool
    /// </summary>
    /// <typeparam name="T"></typeparam>

    [System.Serializable]
    public class AsyncPool<T> : AsyncPool, IDisposable
    where T : UnityEngine.Component
    {
        public AsyncPool(Func<IObservable<T>> createInstanceAsync, int maxCount = 1000)
        {
            this.createInstanceAsync = createInstanceAsync;
            MaxCount = maxCount;
        }
        public AsyncPool(string referPath, Func<IObservable<T>> createInstanceAsync = null, int maxCount = 1000)
        {
            // this.prefab = prefab;
            this.referPath = referPath;
            this.createInstanceAsync = createInstanceAsync??DefaultCreateInstanceAsync;
            MaxCount = maxCount;
        }
#if USE_ADDRESSABLE
        /// <summary>用AssetReference方式生成</summary>
        public AsyncPool(AssetReference refer, Func<IObservable<T>> createInstanceAsync = null, int maxCount = 1000)
        {
            // this.prefab = prefab;
            this.refer = refer;
            this.createInstanceAsync = createInstanceAsync??DefaultCreateInstanceAsync;
            MaxCount = maxCount;
        }
#endif
        /// <summary>用Gameobject方式生成</summary>
        public AsyncPool(GameObject prefab, Func<IObservable<T>> createInstanceAsync = null, int maxCount = 1000)
        {
            this.prefab = prefab;
            // this.refer = refer;
            this.createInstanceAsync = createInstanceAsync??DefaultCreateInstanceAsync;
            MaxCount = maxCount;
        }
        public Transform parent;
        public bool setInActiveWhenReturn { get; set; } = true;
        bool isDisposed = false;
        [Sirenix.OdinInspector.ShowInInspector]
        Queue<T> q;
        public int MaxCount { get; set; }
        protected GameObject prefab;
#if USE_ADDRESSABLE
        protected AssetReference refer;
#endif
        protected string referPath;
        protected Func<IObservable<T>> createInstanceAsync;

        /// <summary>默认的生成方法</summary>
        private IObservable<T> DefaultCreateInstanceAsync()
        {
#if USE_ADDRESSABLE
            //  优先使用refer
            if (refer != null)
            {
                return ObservableAddressables.Instantiate<GameObject>(refer, parent).Select(x => x.GetComponent<T>());
            }
            //  次先使用referPath
            else if (referPath != null)
            {
                return ObservableAddressables.Instantiate<GameObject>(referPath, parent).Select(x => x.GetComponent<T>());
            }
            // 后选用Gameobject
#endif
            if (prefab != null)
            {
                var t = GameObject.Instantiate(prefab).GetComponent<T>();
                t.transform.SetParent(parent);
                return Observable.Return(t);
            }
            else
            {
                throw new Exception("Pool has no object to instantiate!");
            }
        }
        /// <summary>
        /// Create instance when needed.
        /// </summary>
        protected virtual IObservable<T> CreateInstanceAsync()
        {
            //TODO 
            return createInstanceAsync();
        }
        /// <summary>
        /// Called before return to pool, useful for set active object(it is default behavior).
        /// </summary>
        protected virtual void OnBeforeRent(T instance)
        {
            onBeforeRent(instance);
            instance.gameObject.SetActive(true);
        }
        public Action<T> onBeforeRent;
        /// <summary>
        /// Called before return to pool, useful for set inactive object(it is default behavior).
        /// </summary>
        protected virtual void OnBeforeReturn(T instance)
        {
            if (setInActiveWhenReturn)
            {
                instance.gameObject.SetActive(false);
            }
            else
            {
                instance.gameObject.transform.position = new Vector3(-9999, -9999, -9999);
            }
            onBeforeReturn(instance);
        }
        public Action<T> onBeforeReturn;
        /// <summary>
        /// Called when clear or disposed, useful for destroy instance or other finalize method.
        /// </summary>
        protected virtual void OnClear(T instance)
        {
            if (instance == null) return;

            var go = instance.gameObject;
            if (go == null) return;
#if USE_ADDRESSABLE
            Addressables.ReleaseInstance(instance);
#else
            UnityEngine.Object.Destroy(go);
#endif
        }

        /// <summary>
        /// Current pooled object count.
        /// </summary>
        public int Count
        {
            get
            {
                if (q == null) return 0;
                return q.Count;
            }
        }

        /// <summary>
        /// Get instance from pool.
        /// </summary>
        public IObservable<T> RentAsync()
        {
            if (isDisposed) throw new ObjectDisposedException("ObjectPool was already disposed.");
            if (q == null) q = new Queue<T>();

            if (q.Count > 0)
            {
                var instance = q.Dequeue();
                OnBeforeRent(instance);
                return Observable.Return(instance);
            }
            else
            {
                var instance = CreateInstanceAsync();
                return instance.Do(x => OnBeforeRent(x));
            }
        }
        /// <summary>
        /// Return instance to pool.
        /// </summary>
        public void Return(T instance)
        {
            if (isDisposed) throw new ObjectDisposedException("ObjectPool was already disposed.");
            if (instance == null) throw new ArgumentNullException("instance");

            if (q == null) q = new Queue<T>();

            // if ((q.Count + 1) == MaxCount)
            // {
            //     // throw new InvalidOperationException("Reached Max PoolSize");
            //     Debug.Log("Reached Max PoolSize");
            //     return;
            // }

            OnBeforeReturn(instance);
            q.Enqueue(instance);
        }

        /// <summary>
        /// Trim pool instances. 
        /// </summary>
        /// <param name="instanceCountRatio">0.0f = clear all ~ 1.0f = live all.</param>
        /// <param name="minSize">Min pool count.</param>
        /// <param name="callOnBeforeRent">If true, call OnBeforeRent before OnClear.</param>
        public void Shrink(float instanceCountRatio, int minSize, bool callOnBeforeRent = false)
        {
            if (q == null) return;

            if (instanceCountRatio <= 0) instanceCountRatio = 0;
            if (instanceCountRatio >= 1.0f) instanceCountRatio = 1.0f;

            var size = (int) (q.Count * instanceCountRatio);
            size = Math.Max(minSize, size);

            while (q.Count > size)
            {
                var instance = q.Dequeue();
                if (callOnBeforeRent)
                {
                    OnBeforeRent(instance);
                }
                OnClear(instance);
            }
        }
        public IDisposable shrinkDisposable;
        /// <summary>
        /// If needs shrink pool frequently, start check timer.
        /// </summary>
        /// <param name="checkInterval">Interval of call Shrink.</param>
        /// <param name="instanceCountRatio">0.0f = clearAll ~ 1.0f = live all.</param>
        /// <param name="minSize">Min pool count.</param>
        /// <param name="callOnBeforeRent">If true, call OnBeforeRent before OnClear.</param>
        public IDisposable StartShrinkTimer(int spanFromSeconds = 10, float instanceCountRatio = 0.8f, int minSize = 3, bool callOnBeforeRent = false)
        {
            shrinkDisposable = Observable.Interval(System.TimeSpan.FromSeconds(spanFromSeconds))
                .TakeWhile(_ => !isDisposed)
                .Subscribe(_ =>
                {
                    Shrink(instanceCountRatio, minSize, callOnBeforeRent);
                });
            return shrinkDisposable;
        }

        /// <summary>
        /// Clear pool.
        /// </summary>
        public void Clear(bool callOnBeforeRent = false)
        {
            if (q == null) return;
            while (q.Count != 0)
            {
                var instance = q.Dequeue();
                if (callOnBeforeRent)
                {
                    OnBeforeRent(instance);
                }
                OnClear(instance);
            }
        }

        /// <summary>
        /// Fill pool before rent operation.
        /// </summary>
        /// <param name="preloadCount">Pool instance count.</param>
        /// <param name="threshold">Create count per frame.</param>
        public IObservable<Unit> PreloadAsync(int preloadCount, int threshold)
        {
            if (q == null) q = new Queue<T>(preloadCount);

            return Observable.FromMicroCoroutine<Unit>((observer, cancel) => PreloadCore(preloadCount, threshold, observer, cancel));
        }

        IEnumerator PreloadCore(int preloadCount, int threshold, IObserver<Unit> observer, CancellationToken cancellationToken)
        {
            while (Count < preloadCount && !cancellationToken.IsCancellationRequested)
            {
                var requireCount = preloadCount - Count;
                if (requireCount <= 0) break;

                var createCount = Math.Min(requireCount, threshold);

                var loaders = new IObservable<Unit>[createCount];
                for (int i = 0; i < createCount; i++)
                {
                    var instanceFuture = CreateInstanceAsync();
                    loaders[i] = instanceFuture.ForEachAsync(x => Return(x));
                }
                var awaiter = Observable.WhenAll(loaders).ToYieldInstruction(false, cancellationToken);
                while (!(awaiter.HasResult || awaiter.IsCanceled || awaiter.HasError))
                {
                    yield return null;
                }

                if (awaiter.HasError)
                {
                    observer.OnError(awaiter.Error);
                    yield break;
                }
                else if (awaiter.IsCanceled)
                {
                    yield break; // end.
                }
            }

            observer.OnNext(Unit.Default);
            observer.OnCompleted();
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    Clear(false);
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }

}
