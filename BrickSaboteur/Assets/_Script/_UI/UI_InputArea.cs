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
using Sirenix.Serialization;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;
using UnityEngine;
namespace BrickSaboteur
{
    public class UI_InputArea : ElementBaseSingle<UI_InputArea, IUITag>
    {
        [OdinSerialize] private float deltaVertical;

        [OdinSerialize] Camera _camera;
        [OdinSerialize][Range(1, 3)] float _modifier;
        // [OdinSerialize] Vector2 _center;
        protected override IEnumerator AfterStart()
        {
            yield return null;
            this.RegisterSelf(this);

            _camera = Camera.main;

            // var beginDragTrigger = this.gameObject.AddComponent<ObservableBeginDragTrigger>();
            var dragTrigger = this.gameObject.AddComponent<ObservableDragTrigger>();
            var endDragTrigger = this.gameObject.AddComponent<ObservableEndDragTrigger>();
            // ---------------------------------
            float? last = null;
            float worldX;
            Vector3 temp = new Vector3();
            // ---------------------------------
            dragTrigger.OnDragAsObservable()
                .RepeatUntilDestroy(this)
                .Subscribe(data =>
                {
                    worldX = _camera.ScreenToWorldPoint(data.position).x * _modifier;
                    deltaVertical = last == null?0: (worldX - last.Value);
                    temp.x = deltaVertical;
                    BrickMgrM.EntityModule.boards.ForEach(x => x.transform.position += temp);
                    last = worldX;
                });

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
