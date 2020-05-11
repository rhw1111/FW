using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary
{
    /// <summary>
    /// 选择器接口
    /// 根据名称选择
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISelector<T>
    {
        T Choose(string name);
    }
}
