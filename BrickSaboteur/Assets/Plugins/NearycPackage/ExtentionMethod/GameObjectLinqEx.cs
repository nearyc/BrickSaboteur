#region Author & Version
//==================================================== 
// Author：Nearyc 
// File name：......
// Version：V1.0.1
// Date : 
//*Function:GameObject扩展方法
//===================================================
// Fix:
//===================================================

#endregion
using System.Linq;
using System.Text.RegularExpressions;
using Unity.Linq;
using UnityEngine;
namespace Unity.Linq
{
    public static class GameObjectLinqEx
    {
        /// <summary>
        /// 去除_，将首字母变为大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string RegexInitialWithUpper(string str)
        {
            // if (Regex.IsMatch(str, @"^[m]{1}\S*$")) //去掉m_
            //     str = str.Substring(2, str.Length - 1);
            if (Regex.IsMatch(str, @"^[_]{1}\S*$")) //去掉_
                str = str.Substring(1, str.Length - 1);
            if (Regex.IsMatch(str, @"^[a-z]{1}\S*$")) //大写开头
                str = char.ToUpper(str[0]) + str.Substring(1, str.Length - 1);
            return str;
        }
        public static T GetComponentFromChildren<T, U>(this T origin, U parent, string name)where T : Component
        where U : Component
        {
            try
            {

                name = RegexInitialWithUpper(name);
                var temp = parent.gameObject
                    .Children()
                    .OfComponent<T>()
                    .Where(x => x.name.Contains(name))
                    .FirstOrDefault()
                    .GetComponent<T>();
                if (temp == null)
                {
                    return default(T);
                }
                else
                {
                    return temp;
                }
            }
            catch
            {
                Debug.LogError("Didnt find " + name);
            }
            return default(T);
        }
        public static T GetComponentFromDescendants<T, U>(this T origin, U parent, string name)where T : Component
        where U : Component
        {
            name = RegexInitialWithUpper(name);
            var temp = parent.gameObject
                .Descendants()
                .OfComponent<T>()
                .Where(x => x.name.Contains(name))
                .FirstOrDefault()
                .GetComponent<T>();
            if (temp == null)
            {
                return default(T);
            }
            else
            {
                return temp;
            }
        }
        public static GameObject FindRootFromAncesterDescendants<U>(this GameObject origin, U parent, string name)
        where U : Component
        {
            name = RegexInitialWithUpper(name);
            var temp = parent.gameObject
                .Ancestors()
                .Descendants()
                .Where(x => x.name.Contains(name))
                .FirstOrDefault();
            if (temp == null)
            {
                return default(GameObject);
            }
            else
            {
                origin = temp;
                return temp;
            }
        }
    }
}
