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
    /// <summary> 改变生命 </summary>
    public struct Tag_ModifyHealth
    {
        public int value;
        public bool isInit;

        public Tag_ModifyHealth(int value, bool isInit = false)
        {
            this.value = value;
            this.isInit = isInit;
        }
    }
    /// <summary> 游戏开始 </summary>
    public struct Tag_GameStart
    {
        public int level;
        public EDifficulty difficulty;
        public Tag_GameStart(int level, EDifficulty difficulty)
        {
            this.level = level;
            this.difficulty = difficulty;
        }
    }
    /// <summary> 回到主菜单 </summary>
    public struct Tag_BackToMenu { }
    /// <summary> 游戏结束，胜利或者失败 </summary>
    public struct Tag_GameEnd
    {
        public bool isWinorNot;
        public Tag_GameEnd(bool isWin)
        {
            this.isWinorNot = isWin;
        }
    }
    /// <summary> 退出游戏 </summary>
    public struct Tag_QuitGame
    {

    }
}
