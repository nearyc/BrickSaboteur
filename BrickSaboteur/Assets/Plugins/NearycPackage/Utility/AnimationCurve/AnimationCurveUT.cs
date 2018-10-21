#region Author & Version
//==================================================== 
// Author：Nearyc 
// File name：......
// Version：V1.0.1
// Date : 
//*Function:曲线工具类
//===================================================
// Fix:
//===================================================

#endregion
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Nearyc.Utility
{
    public static class AnimationCurveUT
    {
        /// <summary>
        /// 用放大系数为scaler的newcurve，替换AnimationClip中为name的curve
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="newCurve"></param>
        /// <param name="scaler"></param>
        /// <param name="names"></param>
        /// <typeparam name="T">name的curve的component类型</typeparam>
        public static void ChangeCurveInClip<T>(AnimationClip clip, AnimationCurve newCurve, Vector2 scaler, params string[] names)
        where T : Component
        {

            ScaleCurve(newCurve, scaler);
            foreach (var name in names)
            {
                clip.SetCurve("", typeof(T), name, newCurve);
                Debug.Log(typeof(T));
            }

            // if (clip != null) {
            //     foreach (var binding in AnimationUtility.GetCurveBindings(clip)) {
            //         foreach (var name in names) {
            //             if (binding.propertyName == name) {
            //                 AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
            //                 // _curve1 = curve;
            //                 clip.SetCurve("", typeof(T), name, newCurve);
            //                 Debug.Log(typeof(T));
            //             }
            //         }
            //     }
            // }
        }
        /// <summary>
        /// 用scaler系数放大curve
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="scaler"></param>
        public static void ScaleCurve(AnimationCurve curve, Vector2 scaler)
        {
            List<Keyframe> keys = new List<Keyframe>();
            foreach (var key in curve.keys)
            {
                keys.Add(key);
            }
            //
            System.Func<Keyframe, Vector2, Keyframe> scaleKey = (k, s) =>
            {
                var a = k.time * s.x;
                var b = k.value * s.y;
                Keyframe newKey = new Keyframe(a, b);
                return newKey;
            };
            //
            for (int i = 0; i < curve.keys.Length; i++)
            {
                curve.MoveKey(i, scaleKey(keys[i], scaler));
            }
        }
    }
}
