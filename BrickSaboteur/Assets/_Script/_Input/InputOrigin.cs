#region Author & Version
//==================================================== 
// Author：Nearyc 
// File name：......
// Version：V1.0.1
// Date : 
//*Function:原始输入静态，从这里调用输入数据
//===================================================
// Fix:
//===================================================

#endregion
using NearyFrame.Base;
using UniRx;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace BrickSaboteur
{
    /// <summary>
    /// 原始输入静态，从这里调用输入数据
    /// </summary>
    public static class InputOrigin
    {
        public static Vector3 delta = new Vector3();
        // public static bool isUseRawDirectionInput = false;
        // //
        // public static Vector2 Direction { private get; set; }
        // public static Vector2 GetDirection => Direction;
        // //
        // public static Vector2 DirectionRaw { private get; set; }
        // public static Vector2 GetDirectionRaw => DirectionRaw;
        // //
        // public static float SetHorizontal { private get; set; }
        // public static float Horizontal => SetHorizontal;
        // //
        // public static float SetVertical { private get; set; }
        // public static float Vertical => SetVertical;
        // //
        // public static Vector2 MouseWorldPos => BrickMgrM.CameraManager.mainCam.ScreenToWorldPoint(Input.mousePosition);
        // //
        // public static bool IsJump => CrossPlatformInputManager.GetButton("Jump") || CrossPlatformInputManager.GetButton("Fire2");
        // public static bool IsRun => CrossPlatformInputManager.GetButton("Run");
        // public static bool IsFire1 => CrossPlatformInputManager.GetButton("Fire1");
        // public static bool IsFire2 => CrossPlatformInputManager.GetButton("Fire2");
        // public static bool IsCrouch => SetVertical < 0.1f;
    }
}
