using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.LanguageTranslate;

namespace MSLibrary
{
    /// <summary>
    /// IEnumerator扩展方法
    /// </summary>
    public static class IEnumerableExtension
    {
        public static async Task<string> ToDisplayString<T>(this IEnumerable<T> obj,Func<T,Task<string>> getDisplayAction,Func<Task<string>> getIntervalAction)
        {
            StringBuilder strDisplay = new StringBuilder();
            var strInterval = await getIntervalAction();
            foreach(var item in obj)
            {
                strDisplay.Append(await getDisplayAction(item));
                strDisplay.Append(strInterval);
            }

            if (strDisplay.Length>0)
            {
                strDisplay.Remove(strDisplay.Length - strInterval.Length, strInterval.Length);
            }

            return strDisplay.ToString();
        }

        public static string ToDisplayString<T>(this IEnumerable<T> obj, Func<T, string> getDisplayAction, Func<string> getIntervalAction)
        {
            StringBuilder strDisplay = new StringBuilder();
            var strInterval =  getIntervalAction();
            foreach (var item in obj)
            {
                strDisplay.Append( getDisplayAction(item));
                strDisplay.Append(strInterval);
            }

            if (strDisplay.Length > 0)
            {
                strDisplay.Remove(strDisplay.Length - strInterval.Length, strInterval.Length);
            }

            return strDisplay.ToString();
        }


    }

}
