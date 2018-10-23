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
using NearyFrame;
using NearyFrame.Base;
using UniRx;
using Unity.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace BrickSaboteur
{
    public interface IUITag : IModuleTag<IUITag> { }

    //UI模块管理实现
    /// <summary>
    /// UI管理
    /// </summary>
    /// <typeparam name="IUITag"></typeparam>
    public class UIManager : ManagerBase<UIManager, IUITag>
    {
        public Canvas mainHudCanvas;
        public Canvas worldHudCanvas;
        [SerializeField] bool isGameHudInited = false;
        protected override void OnDestroy()
        {
            Mgr.Instance.UnRegisterModule(this);
            Debug.Log("UnRegister UIManager");
        }
        protected override System.Collections.IEnumerator OnInitializeRegisterSelf()
        {
            Mgr.Instance.RegisterModule(this);
            Debug.Log("Create UIManager");
            yield return BrickMgrM.WaitModule<ICameraTag>();

            mainHudCanvas = mainHudCanvas.GetComponentFromChildren(this, nameof(mainHudCanvas));
            mainHudCanvas.worldCamera = BrickMgrM.CameraManager.mainCam;
            worldHudCanvas = worldHudCanvas.GetComponentFromChildren(this, nameof(worldHudCanvas));
            worldHudCanvas.worldCamera = BrickMgrM.CameraManager.mainCam;
            var mainHudTran = mainHudCanvas.transform;
            var worldHudTran = worldHudCanvas.transform;
            //游戏开始，加载关卡
            MessageBroker.Default.Receive<Tag_GameStart>().Subscribe(x => LoadGameUI()).AddTo(this);;
            //游戏结束，弹出窗口
            MessageBroker.Default.Receive<Tag_GameEnd>().Subscribe(x => GameEnd(x.isWinorNot)).AddTo(this);;
            //回到主菜单,清理InGameUI
            MessageBroker.Default.Receive<Tag_BackToMenu>().Subscribe(__ => BackToMenu()).AddTo(this);;
        }
        private void LoadGameUI()
        {
            worldHudCanvas.gameObject.Children().ForEach(x => BrickMgrM.LoaderManager.ReleaseObject(x.gameObject));
            // BrickMgrM.LoaderManager.InstantiateAll<GameObject>("UIGameRoot", worldHudCanvas.transform, null).Subscribe();
            //  BrickMgrM.LoaderManager.InstantiatePrefabByPath<GameObject>("UI/GmaeUI/HealthRoot", worldHudCanvas.transform, null).Subscribe();
            // BrickMgrM.LoaderManager.InstantiatePrefabByPath<GameObject>("UI/GmaeUI/InputArea", worldHudCanvas.transform, null).Subscribe();
            // BrickMgrM.LoaderManager.InstantiatePrefabByPath<GameObject>("UI/GmaeUI/MultiplySkillRoot", worldHudCanvas.transform, null).Subscribe();
            // BrickMgrM.LoaderManager.InstantiatePrefabByPath<GameObject>("UI/GmaeUI/PlusSkillRoot", worldHudCanvas.transform, null).Subscribe();
        }
        private void GameEnd(bool isWInOrNot)
        {
            if (isWInOrNot == true)
            {
                //TODO
                Debug.Log("YOU WIN !");
            }
            else
            {
                //TODO
                Debug.Log("YOU Lose !");
            }
        }
        private void BackToMenu()
        {
            //TODO
            worldHudCanvas.gameObject.Children().ForEach(x => BrickMgrM.LoaderManager.ReleaseObject(x.gameObject));
        }
    }
}
