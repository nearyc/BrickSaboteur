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
    public interface ILoaderTag : IModuleTag<ILoaderTag> { }
    /// <summary>
    /// 加载游戏管理
    /// </summary>
    /// <typeparam name="LoaderManager">Self</typeparam>
    /// <typeparam name="ILoaderTag">Tag</typeparam>
    public class LoaderManager : ManagerBase<LoaderManager, ILoaderTag>
    {
        [Sirenix.OdinInspector.ShowInInspector] 
        private string _prefix => AddressablePathEx.PREFIX;
        [Sirenix.OdinInspector.ShowInInspector] 
        private string _prefabSuffix => AddressablePathEx.PREFAB_SUFFIX;
        private List<UnityEngine.ResourceManagement.IResourceLocation> some;
        protected override void OnDestroy()
        {

            Mgr.Instance.UnRegisterModule(this);
            Debug.Log("UnRegister LoaderManager");
        }
		// private void Update()
		// {
		// 	if (Input.GetMouseButtonDown(0))
		// 	{
		// 		Debug.Log(123);

		// 		BrickMgrM.LoaderManager.InstantiateByPath<GameObject>("Entity/Bonus_1", this.transform, x =>
		// 		{
		// 			var temp = BrickMgrM.CameraManager.mainCam.ScreenToWorldPoint(Input.mousePosition);
		// 			temp.z = 0;
		// 			x.Result.transform.position = temp;
		// 		}).Subscribe();
		// 	}	
		// }
		protected override System.Collections.IEnumerator OnInitializeRegisterSelf()
        {

            isLoaded = false;
            Mgr.Instance.RegisterModule(this);
            Debug.Log("Create LoaderManager");
            gameObject.AddComponent<InputManager>();
            //  yield return Addressables.InstantiateAll<GameObject>("Managers", null, this.transform);
            yield return null;
            isLoaded = true;
            //Addressables.LoadAssets<Texture2D>("default",x=>{
            //    Debug.Log(x.Result.name);
            //});
        }
        public void ReloadGame()
        {
            SceneManager.LoadScene(0);
            Destroy(this.gameObject);
        }
        bool isLoadingScene;
        IEnumerator LoadGame()
        {
            isLoadingScene = true;
            yield return SceneManager.LoadSceneAsync(0);
            Destroy(this.gameObject);
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
        public System.IObservable<T> InstantiateByPath<T>(string path, Transform parentTran, Action<IAsyncOperation<T>> onCompleted = null, bool isWorldSpace = false)
        where T : UnityEngine.Object
        {
            var fullPath = $"{_prefix}{path}{_prefabSuffix}";

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
            var fullPath = $"{_prefix}{path}{_prefabSuffix}";

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
