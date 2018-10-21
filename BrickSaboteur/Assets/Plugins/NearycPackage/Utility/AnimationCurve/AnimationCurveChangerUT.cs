#region Author & Version
//==================================================== 
// Author：Nearyc 
// File name：......
// Version：V1.0.1
// Date : 
//*Function:曲线调解工具类
//===================================================
// Fix:
//===================================================

#endregion
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace Nearyc.Utility
{
    /// <summary>
    /// 用放大系数为scaler的newcurve，替换AnimationClip中为name的curve
    /// </summary>
    [System.Serializable]
    public class AnimationCurveChangerUT<T> : MonoBehaviour
    where T : Component
    {
        [SerializeField] private string[] names;
        [SerializeField] private Vector2 scaler;
        [SerializeField] private AnimationClip Clip;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private bool isBackOnDisable;
        private void Start()
        {
            // foreach (var in changeList) {
            AnimationCurveUT.ChangeCurveInClip<T>(Clip, curve, scaler, names);
            // }
        }
        private void OnDisable()
        {
            // foreach (var in changeList) {
            if (!isBackOnDisable) return;
            var scaler1 = new Vector2(1 / scaler.x, 1 / scaler.y);
            AnimationCurveUT.ChangeCurveInClip<T>(Clip, curve, scaler1, names);
            // }
        }
    }
}
