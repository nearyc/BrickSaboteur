#region Author & Version
//==================================================== 
// Author：Nearyc 
// File name：......
// Version：V1.0.1
// Date : 2018.10.20
//*Function:
//===================================================
// Fix:
//===================================================

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearyc.Utility
{
	public enum ETriggerCondition
	{
		DefaultT = 0,
		Fly,
	}

	public enum EBoolCondition
	{
		DefaultB = 100,
		IsGrounded,
	}

	public enum EFloatCondition
	{
		DefaultF = 200,
		SpeedMagnitute,
	}
}
