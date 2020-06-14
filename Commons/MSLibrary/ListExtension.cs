using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary
{
    /// <summary>
    /// 针对List的扩展方法
    /// </summary>
    public static class ListExtension
    {
        /// <summary>
        /// 将一个列表按size分组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static List<List<T>> SplitGroup<T>(this List<T> list,int size)
        {
            List<List<T>> result = new List<List<T>>();
            int currentIndex = 0;
            List<T> currentList=null;
            while(true)
            {
                if (currentIndex%size==0 || currentIndex==0)
                {
                    currentList = new List<T>();
                    result.Add(currentList);
                }

                currentList.Add(list[currentIndex]);

                currentIndex++;
                if (currentIndex>= list.Count)
                {
                    break;
                }
            }

            return result;
        }
    }
}
