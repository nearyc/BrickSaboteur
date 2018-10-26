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
using System.Linq;
using DG.Tweening;
using NearyFrame;
using NearyFrame.Base;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
#if USE_ADDRESSABLE
using UnityEngine.ResourceManagement;
#endif
using UnityEngine.SceneManagement;
namespace BrickSaboteur
{
#if USE_ADDRESSABLE
    [System.Serializable]
    public class Refer : AssetReference
    {

    }
#endif
    public enum EDifficulty
    {
        Eazy = 1,
        Hard = 2,
    }

    public interface ILoaderTag : IModuleTag<ILoaderTag> { }
    /// <summary>
    /// 游戏管理
    /// </summary>
    /// <typeparam name="LoaderManager">Self</typeparam>
    /// <typeparam name="ILoaderTag">Tag</typeparam>
    public class LoaderManager : ManagerBase<LoaderManager, ILoaderTag>, ILoaderTag
    {
        //TODO Game State
        [Sirenix.OdinInspector.ShowInInspector] private string _prefabPrefix => AddressablePathEx.PREFAB_PREFIX;
        [Sirenix.OdinInspector.ShowInInspector] private string _prefabSuffix => AddressablePathEx.PREFAB_SUFFIX;
        [SerializeField] private int sceneIndex = 0;
        public int currentLevelIndex = 0;
        public EDifficulty currentDifficulty;
        protected override void OnDestroy()
        {
            Mgr.Instance.UnRegisterModule(this);
            Debug.Log("UnRegister LoaderManager");
        }
        protected override System.Collections.IEnumerator OnInitializeRegisterSelf()
        {
            // Screen.SetResolution(1366, 768, true);
            isLoaded = false;
            Mgr.Instance.RegisterModule(this);
            Debug.Log("Create LoaderManager");
            yield return BrickMgrM.WaitModule<IPropertyTag>();
            //TEST
            BackToMainMenu();
            yield return new WaitForSeconds(1);
            isLoaded = true;
        }
        //游戏开始,1和2都对应游戏scene,通过切换销毁物体
        public void GameStart(int level, EDifficulty difficulty)
        {
            currentLevelIndex = level;

            if (sceneIndex != 1)
                LoadSceneByNum(1).Last().Subscribe(__ =>
                {
                    MessageBroker.Default.Publish(new GameTag_GameStart(level, difficulty));
                    sceneIndex = 1;
                    this.currentDifficulty = difficulty;
                    // levelIndex++;
                });
            else
            {
                LoadSceneByNum(2).Last().Subscribe(__ =>
                {
                    MessageBroker.Default.Publish(new GameTag_GameStart(level, difficulty));
                    sceneIndex = 2;
                    this.currentDifficulty = difficulty;
                    // levelIndex++;
                });
            }
        }
        //游戏结束，胜利或失败
        public void GameEnd(bool isWin)
        {
            Debug.Log("GameEnd");
            //TODO
            MessageBroker.Default.Publish(new GameTag_GameEnd(isWin));
            //test
            GameStart(++currentLevelIndex, currentDifficulty);
        }
        //回到主菜单，3对应主菜单scene
        public void BackToMainMenu()
        {
            LoadSceneByNum(3).Last().Subscribe(__ =>
            {
                MessageBroker.Default.Publish(new GameTag_BackToMenu());
            });
        }
        private void SceneTransition()
        {
            this.InstantiatePrefabByPath<GameObject>(AssetPath.Black)
                .Do(x => x.transform.parent = BrickMgrM.UIModule.constHudCanvas.transform)
                .Select(x => x.GetComponent<UnityEngine.UI.Image>())
                .Do(x => x.DOFade(0, 1f))
                .Delay(System.TimeSpan.FromMilliseconds(1000))
                .Subscribe(x =>
                {
                    this.ReleaseObject(x.gameObject);
                });
        }
        #region Addressable
        // ---------------------------------
        // ---------------------------------
        public IObservable<AsyncOperation> LoadScene(string key, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            // var op = Addressables.LoadScene($"Assets/_Scenes/{key}.unity", loadMode);
            SceneTransition();

            var stream = SceneManager.LoadSceneAsync(key, loadMode).AsAsyncOperationObservable();
            return stream;
        }
        public IObservable<AsyncOperation> LoadSceneByNum(int num, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            SceneTransition();
            var stream = SceneManager.LoadSceneAsync("Scene_" + num, loadMode).AsAsyncOperationObservable();
            return stream;
        }
        public void ReleaseObject(UnityEngine.Object obj, float delay = 0)
        {
#if USE_ADDRESSABLE
            Addressables.ReleaseInstance(obj, delay);
#else
            UnityEngine.Object.Destroy(obj, delay);
#endif

        }

        public Scene CurrentScene => SceneManager.GetActiveScene();
#if USE_ADDRESSABLE
        public System.IObservable<IList<T>> InstantiateAll<T>(object key, Transform parentTran,
            Action<UnityEngine.ResourceManagement.IAsyncOperation<T>> onSingleCompleted = null,
            Action<IAsyncOperation<IList<T>>> onCompleted = null)
        where T : UnityEngine.Object
        {
            return ObservableAddressables.InstantiateAll<T>(key, parentTran, onSingleCompleted, onCompleted);
        }

        public System.IObservable<T> Instantiate<T>(object key, Transform parentTran, Action<IAsyncOperation<T>> onCompleted = null, bool isWorldSpace = false)
        where T : UnityEngine.Object
        {
            return ObservableAddressables.Instantiate<T>(key, parentTran, onCompleted, isWorldSpace);;
        }
        public System.IObservable<T> InstantiatePrefabByPath<T>(string path, Transform parentTran,
            Action<IAsyncOperation<T>> onCompleted = null,
            bool isWorldSpace = false)
        where T : UnityEngine.Object
        {
            var fullPath = $"{_prefabPrefix}{path}";

            return ObservableAddressables.Instantiate<T>(fullPath, parentTran, onCompleted, isWorldSpace);
        }
#else
        public System.IObservable<GameObject> InstantiatePrefabByPath<T>(string path,
            Action<AsyncOperation> onCompleted = null,
            // Action<IAsyncOperation<T>> onCompleted = null,
            bool isWorldSpace = false)
        where T : UnityEngine.Object
        {
            var fullPath = $"{_prefabPrefix}{path}";

            // return ObservableAddressables.Instantiate<T>(fullPath, parentTran, onCompleted, isWorldSpace);
            return ObservableAddressables.ResourcesInstantiate<T>(fullPath, onCompleted);
        }
#endif
        // ---------------------------------
#if USE_ADDRESSABLE
        public System.IObservable<IList<T>> LoadAssets<T>(object key, Transform parentTran,
            Action<UnityEngine.ResourceManagement.IAsyncOperation<T>> onSingleCompleted = null,
            Action<IAsyncOperation<IList<T>>> onCompleted = null)
        where T : UnityEngine.Object
        {
            return ObservableAddressables.LoadAssets<T>(key, onSingleCompleted, onCompleted);
        }
        public System.IObservable<T> LoadAsset<T>(object key, Action<IAsyncOperation<T>> onCompleted = null)
        where T : UnityEngine.Object
        {
            return ObservableAddressables.LoadAsset<T>(key, onCompleted);
        }

        public System.IObservable<T> LoadAssetByPath<T>(string path, Action<IAsyncOperation<T>> onCompleted = null)
        where T : UnityEngine.Object
        {
            var fullPath = $"{_prefabPrefix}{path}{_prefabSuffix}";

            return ObservableAddressables.LoadAsset<T>(fullPath, onCompleted);
        }
        // ---------------------------------
        public IAsyncOperation<IList<IResourceLocation>> LoadResourceLocation(object key, System.Action<IAsyncOperation<IResourceLocation>> callback)
        {
            var op = Addressables.LoadAssets<IResourceLocation>(key, callback);
            return op;
        }
        public IAsyncOperation<IList<IResourceLocation>> LoadResourceLocation(object key, List<IResourceLocation> list)
        {

            var op = Addressables.LoadAssets<IResourceLocation>(key, x => list.Add(x.Result));
            return op;
        }
#endif

        #endregion
    }
}
