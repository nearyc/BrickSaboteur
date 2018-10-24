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
using UnityEngine.AddressableAssets;

namespace BrickSaboteur
{
    /// <summary>
    /// addressable path
    /// </summary>
    public static class AssetPath
    {
        public static string PrefabPrefix => AddressablePathEx.PREFAB_PREFIX;
        public static string PrefabSuffix => AddressablePathEx.PREFAB_SUFFIX;
        #region Entity
        public const string Slider = "Entity/Slider_";
        public const string Ball = "Entity/Ball";
        public const string Bonus = "Entity/Bonus_";
        // public const string Slider = "Entity/Slider";
        public const string LevelBackGround = "Entity/Level_BackGround";
        #endregion
        #region Level
        public const string EasyLevel = "Level/Level_1_";
        public const string HardLevel = "Level/Level_2_";
        #endregion
        #region UI
        public const string HealthIcon = "UI/HealthIcon";
        public const string LevelSlotButton = "UI/LevelSlotButton";
        #endregion
    }
    /// <summary>
    /// playerpref key
    /// </summary>
    public static class PlayerPrefKey
    {
        public const string PlusCount = "PlusCount";
        public const string MultiplyCount = "MultiplyCount";
        public const string Level = "Level";
        public const string Difiicuilty = "Difficuilty";

    }
}
