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
using NearyFrame.Base;
using UniRx;
using UnityEngine;
namespace BrickSaboteur
{
    /// <summary>
    /// 生命的Presenter
    /// </summary>
    /// <typeparam name="UI_Health"></typeparam>
    /// <typeparam name="IUITag"></typeparam>
    public class UI_Health : UIELementBase<UI_Health>
    {
        protected override void OnDestroy()
        {
            this.UnRegisterSelf(this);
        }
        protected override IEnumerator AfterStart()
        {
            this.RegisterSelf(this);
            yield return null;

            var hpStream = MessageBroker.Default.Receive<PropTag_ModifyHealth>();
            // ---------------------- 
            //非初始化
            hpStream.Where(x => x.isInit == false)
                .Subscribe(x =>
                {
                    ModifyHealth(x.value);
                }).AddTo(this);
            // ---------------------- 
            //初始化
            // hpStream.Where(x => x.isInit == true).Subscribe(x =>
            // {
            //     ModifyHealth(x.value - this.transform.childCount);
            // }).AddTo(this);
        }
        /// <summary>
        /// 增加或者减少生命的显示
        /// </summary>
        /// <param name="amount"></param>
        public void ModifyHealth(int amount)
        {
            if (amount > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    BrickMgrM.LoaderManager.InstantiatePrefabByPath<GameObject>(AssetPath.HealthIcon).Last().Subscribe(x =>
                    {
                        //会变大，需重置
                        x.transform.SetParent(this.transform);
                        x.transform.localScale = Vector3.one;
                        x.transform.localPosition = Vector3.zero;
                    });
                }
            }
            else if (amount < 0)
            {
                for (int i = 0; i < -amount; i++)
                {
                    if (this.transform.childCount > 0)
                    {
                        var temp = this.transform.GetChild(0);
                        if (temp != null)
                            BrickMgrM.LoaderManager.ReleaseObject(temp.gameObject);
                    }
                }
            }
            else
            {
                //TODO
            }
        }

    }
}
