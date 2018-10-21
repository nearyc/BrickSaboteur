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
namespace BrickSaboteur
{
    public class LevelTileEntity : ElementBase<IEntityTag>
    {
        protected override IEnumerator OnStart()
        {
            yield return BrickMgrM.WaitModule<IEntityTag>();
        }
    }
}
