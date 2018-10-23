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
using UnityEngine;
namespace BrickSaboteur
{
    public class CameraAutoAdapt : ElementBase<ICameraTag>
    {

        float devHeight = 9.6f;
        float devWidth = 6.4f;

        protected override IEnumerator OnStart()
        {
            yield return null;
            // float screenHeight = Screen.height;

            // Debug.Log("screenHeight = " + screenHeight);

            // //this.GetComponent<Camera>().orthographicSize = screenHeight / 200.0f;

            // float orthographicSize = this.GetComponent<Camera>().orthographicSize;

            // float aspectRatio = Screen.width * 1.0f / Screen.height;

            // float cameraWidth = orthographicSize * 2 * aspectRatio;
            // Debug.Log(Screen.width);
            // Debug.Log("cameraWidth = " + cameraWidth);

            // if (cameraWidth < devWidth)
            // {
            //     orthographicSize = devWidth / (2 * aspectRatio);
            //     Debug.Log("new orthographicSize = " + orthographicSize);
            //     this.GetComponent<Camera>().orthographicSize = orthographicSize;
            // }
            this.GetComponent<Camera>().aspect = 9 / 16;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
