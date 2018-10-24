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
namespace BrickSaboteur
{
    #region Game

    /// <summary> 游戏开始 </summary>
    public struct GameTag_GameStart
    {
        public int level;
        public EDifficulty difficulty;
        public GameTag_GameStart(int level, EDifficulty difficulty)
        {
            this.level = level;
            this.difficulty = difficulty;
        }
    }
    /// <summary> 回到主菜单 </summary>
    public struct GameTag_BackToMenu { }
    /// <summary> 游戏结束，胜利或者失败 </summary>
    public struct GameTag_GameEnd
    {
        public bool isWinorNot;
        public GameTag_GameEnd(bool isWin)
        {
            this.isWinorNot = isWin;
        }
    }
    /// <summary> 退出游戏 </summary>
    public struct GameTag_QuitGame { }
    /// <summary> 扣一点生命，重新上弹 </summary>
    public struct GameTag_Reload { }
    #endregion
    #region Property

    /// <summary> 改变生命 </summary>
    public struct PropTag_ModifyHealth
    {
        public int value;
        public bool isInit;

        public PropTag_ModifyHealth(int value, bool isInit = false)
        {
            this.value = value;
            this.isInit = isInit;
        }
    }
    /// <summary> 改变生命 </summary>
    public struct PropTag_ModifySliderSize
    {
        public int value;
        public bool isInit;

        public PropTag_ModifySliderSize(int value, bool isInit = false)
        {
            this.value = value;
            this.isInit = isInit;
        }
    }
    /// <summary> 释放Multiply </summary>
    ///     /// <summary> 释放plus </summary>
    public struct PropTag_ModifyPlusCount { public int value; }
    public struct PropTag_ModifyMultiplyCount { public int value; }
    #endregion
    #region Skill
    public struct SkillTag_TryPlusSkill { }
    public struct SkillTag_TryModifySkill { }
    #endregion
}
