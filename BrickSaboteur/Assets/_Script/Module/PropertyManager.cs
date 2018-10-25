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
using Nearyc.Skill;
using NearyFrame;
using NearyFrame.Base;
using UniRx;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace BrickSaboteur
{
    public interface IPropertyTag : IModuleTag<IPropertyTag>
    {

    }
    /// <summary>
    /// 管理数据
    /// </summary>
    /// <typeparam name="IUITag">Tag</typeparam>
    public class PropertyManager : ManagerBase<PropertyManager, IPropertyTag>, IPropertyTag
    {
        // public InputArea
        [SerializeField] public Property plusCount;
        [SerializeField] public Property multiplyCount;
        [SerializeField] public Property HealthCount;
        [SerializeField] public Property SliderSize;
        //Game
        public EDifficulty Difficulty; //上次游戏的难度

        public int Level; //上次游戏的关卡数
        //State
        public bool isInGameState = false;
        protected override void OnDestroy()
        {
            Mgr.Instance.UnRegisterModule(this);
            Debug.Log("UnRegister PropertyManager");
        }
        protected override System.Collections.IEnumerator OnInitializeRegisterSelf()
        {
            Mgr.Instance.RegisterModule(this);
            Debug.Log("Create PropertyManager");
            yield return BrickMgrM.WaitModule<ILoaderTag>();

            //初始化数据
            plusCount.Init(100, PlayerPrefs.GetInt(PlayerPrefKey.PlusCount, 999));
            multiplyCount.Init(100, PlayerPrefs.GetInt(PlayerPrefKey.MultiplyCount, 999));
            // HealthCount.Init(3);
            //
            Difficulty = (EDifficulty) PlayerPrefs.GetInt(PlayerPrefKey.Difiicuilty, 1);
            Level = PlayerPrefs.GetInt(PlayerPrefKey.Level, 1);

            //改变生命
            MessageBroker.Default.Receive<PropTag_ModifyHealth>().Where(x => x.isInit == false).Subscribe(x => OnModifyHealth(x.value)).AddTo(this);
            //改变plus
            MessageBroker.Default.Receive<PropTag_ModifyPlusCount>().Subscribe(x => OnModifyPlus(x.value)).AddTo(this);
            //改变multiply
            MessageBroker.Default.Receive<PropTag_ModifyMultiplyCount>().Subscribe(x => OnModufyMultiply(x.value)).AddTo(this);
            //改变slider size
            MessageBroker.Default.Receive<PropTag_ModifySliderSize>().Where(x => x.isInit == false).Subscribe(x => OnModifySliderSize(x.value)).AddTo(this);
            //游戏开始
            MessageBroker.Default.Receive<GameTag_GameStart>().Subscribe(__ => OnGameStart()).AddTo(this);
            //退出游戏
            MessageBroker.Default.Receive<GameTag_QuitGame>().Subscribe(__ => OnGameEnd()).AddTo(this);
            //游戏结束
            MessageBroker.Default.Receive<GameTag_GameEnd>().Subscribe(__ => OnGameEnd()).AddTo(this);
            //回到菜单
            MessageBroker.Default.Receive<GameTag_BackToMenu>().Subscribe(__ => OnGameEnd()).AddTo(this);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                OnModifyPlus(5);
                OnModufyMultiply(5);
            }
        }
        //每次游戏开始，生命为3
        System.IDisposable healthStram;
        private void OnGameStart()
        {
            SliderSize.Init(5, 1);
            HealthCount.Init(6, 3, -1);
            //延迟推送生命
            Observable.Timer(System.TimeSpan.FromMilliseconds(250))
                .Subscribe(__ =>
                {
                    // MessageBroker.Default.Publish(new PropTag_ModifyHealth(HealthCount.CurrentValue, true));
                    BrickMgrM.UIModule.GetElement<UI_Health>().ModifyHealth(HealthCount.CurrentValue);
                    MessageBroker.Default.Publish(new PropTag_ModifySliderSize(SliderSize.CurrentValue, true));
                    isInGameState = true;
                    Debug.Log("Init health");
                });
            //监视生命
            if (healthStram != null) healthStram.Dispose();
            healthStram = this.ObserveEveryValueChanged(x => x.HealthCount.CurrentValue)
                .Do(x => Debug.Log(x))
                .Where(x => x < 0 && isInGameState == true)
                .Subscribe(__ => BrickMgrM.LoaderManager.GameEnd(false)).AddTo(this);
        }
        private void OnGameEnd()
        {
            PlayerPrefs.SetInt(PlayerPrefKey.Level, Level);
            PlayerPrefs.SetInt(PlayerPrefKey.Difiicuilty, (int) Difficulty);
            isInGameState = false;
        }
        private void OnModifyHealth(int amoumt)
        {
            HealthCount.ModifyCurrent(amoumt);
        }
        private void OnModifySliderSize(int amoumt)
        {
            SliderSize.ModifyCurrent(amoumt);
        }
        private void OnModifyPlus(int amount)
        {
            plusCount.ModifyCurrent(amount);
            PlayerPrefs.SetInt(PlayerPrefKey.PlusCount, plusCount.CurrentValue);
        }
        private void OnModufyMultiply(int amount)
        {
            multiplyCount.ModifyCurrent(amount);
            PlayerPrefs.SetInt(PlayerPrefKey.MultiplyCount, multiplyCount.CurrentValue);
        }

    }
}
