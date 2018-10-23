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
using UniRx.Triggers;
using Unity.Linq;
using UnityEngine;
namespace BrickSaboteur
{
    /// <summary>
    /// Board的输入区域，控制其左右移动
    /// </summary>
    /// <typeparam name="UI_InputArea"></typeparam>
    /// <typeparam name="IUITag"></typeparam>
    public class UI_InputArea : ElementBaseSingle<UI_InputArea, IUITag>
    {
        [SerializeField] private float deltaVertical;
        [SerializeField] Camera _camera;
        [SerializeField][Range(1, 3)] float _modifier;
        protected override IEnumerator AfterStart()
        {
            yield return null;
            this.RegisterSelf(this);

            _camera = Camera.main;

            var dragTrigger = this.gameObject.AddComponent<ObservableDragTrigger>();
            var endDragTrigger = this.gameObject.AddComponent<ObservableEndDragTrigger>();
            // ---------------------------------
            float? last = null;
            float worldX;
            Vector3 temp = new Vector3();
            // ---------------------------------
            // 拖动时候Board变化modifier倍数的相对距离
            dragTrigger.OnDragAsObservable()
                .RepeatUntilDestroy(this)
                .Subscribe(data =>
                {
                    if (BrickMgrM.LoaderManager.isInGameState == false) BrickMgrM.LoaderManager.isInGameState = true;
                    worldX = _camera.ScreenToWorldPoint(data.position).x * _modifier;
                    deltaVertical = last == null?0: (worldX - last.Value);
                    temp.x = deltaVertical;
                    BrickMgrM.EntityModule.boards.ForEach(x => x.transform.position += temp);
                    last = worldX;
                });
            //结束拖动
            endDragTrigger.OnEndDragAsObservable()
                .RepeatUntilDestroy(this)
                .Subscribe(data =>
                {
                    last = null;
                    deltaVertical = 0;
                });
        }

        protected override void OnDestroy()
        {
            this.UnRegisterSelf(this);
        }
    }
}
