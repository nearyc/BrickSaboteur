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
// using UnityEngine.ResourceManagement;
// using static UnityEngine.AddressableAssets.Addressables;

namespace UnityEngine.AddressableAssets
{
    public static class AddressablePathEx
    {
        public const string RESOURCES_PREFIX = "Assets/Resources/";
        public const string PREFAB_PREFIX = "_Prefabs/";
        public const string PREFAB_SUFFIX = ".prefab";

        // public static IAsyncOperation<T> InstantiateP<T>(string path, Transform parent = null, bool isWorldPos = false) where T : UnityEngine.Object
        // {
        //     return Addressables.Instantiate<T>($"{PREFAB_PREFIX}{path}{PREFAB_SUFFIX}", parent, isWorldPos);

        // }
        // public static IAsyncOperation<T> LoadAssetP<T>(string path) where T : UnityEngine.Object
        // {
        //     return Addressables.LoadAsset<T>($"{PREFAB_PREFIX}{path}{PREFAB_SUFFIX}");
        // }
        // public static IAsyncOperation<IList<T>> InstantiateAllP<T>(string path, Action<IAsyncOperation<T>> onComp, Transform parent = null, bool isWorldPos = false) where T : UnityEngine.Object
        // {
        //     return Addressables.InstantiateAll<T>($"{PREFAB_PREFIX}{path}{PREFAB_SUFFIX}", onComp, parent, isWorldPos);
        // }

        // public static IAsyncOperation<T> InstantiateNoP<T>(string path, Transform parent = null, bool isWorldPos = false) where T : UnityEngine.Object
        // {
        //     return Addressables.Instantiate<T>($"{PREFAB_PREFIX}{path}", parent, isWorldPos);
        // }
        // public static IAsyncOperation<T> LoadAssetNoP<T>(string path) where T : UnityEngine.Object
        // {
        //     return Addressables.LoadAsset<T>($"{PREFAB_PREFIX}{path}");
        // }
        // public static IAsyncOperation<IList<T>> InstantiateAllNoP<T>(string path, Action<IAsyncOperation<T>> onComp, Transform parent = null, bool isWorldPos = false) where T : UnityEngine.Object
        // {
        //     return Addressables.InstantiateAll<T>($"{PREFAB_PREFIX}{path}", onComp, parent, isWorldPos);
        // }
    }
}
