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
#if USE_ADDRESSABLE
using UnityEngine.ResourceManagement;
#endif
using UnityEngine.SceneManagement;
namespace BrickSaboteur
{

    /// <summary>
    /// 加载游戏管理
    /// </summary>
    /// <typeparam name="LoaderManager">Self</typeparam>
    /// <typeparam name="ILoaderTag">Tag</typeparam>
    public class AssetBundleTest : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.UI.Text te1;
        [SerializeField]
        UnityEngine.UI.Text te2;
        [SerializeField]
        UnityEngine.UI.Text te3;
        [SerializeField]
        UnityEngine.UI.Text te4;
        [SerializeField]
        string path = "E:/UnityGame/BrickSaboteur/BrickSaboteur/AssetBundles/StandaloneWindows/entity.folder";
        [SerializeField]
        string ballName = "assets/_prefabs/entity/ball.prefab";
        private void Start()
        {
            //var bundle = AssetBundle.LoadFromFile(path);
            //foreach(var asset in bundle.GetAllAssetNames())
            //{
            //	Debug.Log(asset);
            //}
            //var prefab = bundle.LoadAsset("ball");
            //Instantiate(prefab);
            te1.text = Application.dataPath;
            te2.text = Application.streamingAssetsPath;
#if USE_ADDRESSABLE
            te3.text = Addressables.BuildPath;
            te4.text = Addressables.RuntimePath;

            Debug.Log(Application.dataPath);
            Debug.Log(Application.streamingAssetsPath);
            Debug.Log(Addressables.BuildPath);
            Debug.Log(Addressables.RuntimePath);
#endif

        }
        private void Update()
        {

        }
    }
}
