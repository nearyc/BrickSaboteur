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
using NearyFrame;
using NearyFrame.Base;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.SceneManagement;
namespace BrickSaboteur
{

    [System.Serializable]
    public class Refer : AssetReference
    {

    }
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
    public class LoaderManager : ManagerBase<LoaderManager, ILoaderTag>
    {
        public EDifficulty lastDifficulty; //上次游戏的难度
        public int lastLevel; //上次游戏的关卡数
        //TODO Game State
        public bool isInGameState = false; //游戏状态
        [Sirenix.OdinInspector.ShowInInspector] private string _prefabPrefix => AddressablePathEx.PREFAB_PREFIX;
        [Sirenix.OdinInspector.ShowInInspector] private string _prefabSuffix => AddressablePathEx.PREFAB_SUFFIX;
        [SerializeField] private int sceneIndex = 0;
        [SerializeField] private int levelIndex = 0;
        protected override void OnDestroy()
        {
            Mgr.Instance.UnRegisterModule(this);
            Debug.Log("UnRegister LoaderManager");
        }
        protected override System.Collections.IEnumerator OnInitializeRegisterSelf()
        {
            isLoaded = false;
            Mgr.Instance.RegisterModule(this);
            Debug.Log("Create LoaderManager");
            gameObject.AddComponent<PropertyManager>();
            gameObject.AddComponent<SkillHolder>();

            lastDifficulty = (EDifficulty) PlayerPrefs.GetInt("EDifficulty", 1);
            lastLevel = PlayerPrefs.GetInt("Level", 1);

            GameStart(1, EDifficulty.Eazy);
            //游戏结束，设置状态为false
            MessageBroker.Default.Receive<Tag_GameEnd>().Subscribe(__ => isInGameState = false).AddTo(this);
            //回到主菜单，设置状态为false
            MessageBroker.Default.Receive<Tag_BackToMenu>().Subscribe(__ => isInGameState = false).AddTo(this);

            yield return new WaitForSeconds(1);
            isLoaded = true;
        }
        //游戏开始,1和2都对应游戏scene
        public void GameStart(int level, EDifficulty difficulty)
        {
            if (sceneIndex != 1)
                LoadScene(1).Completed += __ =>
                {
                    MessageBroker.Default.Publish(new Tag_GameStart(level, difficulty));
                    sceneIndex = 1;
                    levelIndex++;
                };
            else
            {
                LoadScene(2).Completed += __ =>
                {
                    MessageBroker.Default.Publish(new Tag_GameStart(level, difficulty));
                    sceneIndex = 2;
                    levelIndex++;
                };
            }
        }
        //游戏结束
        public void GameEnd(bool isWin)
        {
            //TODO
            MessageBroker.Default.Publish(new Tag_GameEnd(isWin));
            //test
            GameStart(levelIndex, EDifficulty.Eazy);
        }
        //回到主菜单，3对应主菜单scene
        public void BackToMainMenu()
        {
            LoadScene(3).Completed += x =>
            {
                MessageBroker.Default.Publish(new Tag_BackToMenu());
            };
        }
        #region Addressable
        // ---------------------------------
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
        public System.IObservable<T> InstantiatePrefabByPath<T>(string path, Transform parentTran, Action<IAsyncOperation<T>> onCompleted = null, bool isWorldSpace = false)
        where T : UnityEngine.Object
        {
            var fullPath = $"{_prefabPrefix}{path}{_prefabSuffix}";

            return ObservableAddressables.Instantiate<T>(fullPath, parentTran, onCompleted, isWorldSpace);
        }
        // ---------------------------------
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
        // ---------------------------------
        public IAsyncOperation<Scene> LoadScene(string key, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            var op = Addressables.LoadScene($"Assets/_Scenes/{key}.unity", loadMode);
            return op;
        }
        public IAsyncOperation<Scene> LoadScene(int num, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            var op = Addressables.LoadScene($"Assets/_Scenes/Scene_{num}.unity", loadMode);
            return op;
        }
        public void ReleaseObject(UnityEngine.Object obj, float delay = 0)
        {
            Addressables.ReleaseInstance(obj, delay);
        }
        public Scene CurrentScene => SceneManager.GetActiveScene();
        #endregion
    }
}
