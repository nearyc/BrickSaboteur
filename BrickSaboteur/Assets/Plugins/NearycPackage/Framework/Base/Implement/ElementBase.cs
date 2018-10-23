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
using Sirenix.OdinInspector;
using UnityEngine;
namespace NearyFrame.Base
{
    public abstract class ElementBase : MonoBehaviour
    {

    }
    /// <summary>
    /// 游戏元素抽象,可重复
    /// </summary>
    /// <typeparam name="Tag"></typeparam>
    public abstract class ElementBase<Tag> : ElementBase where Tag : IModuleTag<Tag>
    {
		[SerializeField]
		protected bool _isInited => Mgr.Instance.IsInited;
        protected virtual System.Collections.IEnumerator Start()
        {
            yield return OnStart();
            int time = 0;
            while (Mgr.Instance.GetModule<Tag>() == null)
            {
                yield return Const.InitializeWaitForSecond;
                time++;
                //
                if (time > 100)
                {
                    Destroy(this);
                    throw new TagWrongException(typeof(Tag).Name + " --Tag错误");
                }
            }
        }
        protected abstract System.Collections.IEnumerator OnStart();
    }

    [System.Serializable]
    public class TagWrongException : System.Exception
    {
        public TagWrongException() { }
        public TagWrongException(string message) : base(message) { }
        public TagWrongException(string message, System.Exception inner) : base(message, inner) { }
        protected TagWrongException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
