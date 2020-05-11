using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using MSLibrary.LanguageTranslate;

namespace MSLibrary
{
    /// <summary>
    /// 类型扩展方法
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 获取指定类型的属性的类型
        /// propertyName格式为xxx.xxx.xxx
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Type GetPropertyType(this Type type, string propertyName)
        {
      
            var propertyNameList = propertyName.Split('.').ToList();
            foreach(var nameItem in propertyNameList)
            {
                PropertyInfo info = type.GetProperty(nameItem);
                if (info==null)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.NotFoundPropertyInTypeByName,
                        DefaultFormatting = "在类型{0}中，找不到名称为{1}的属性",
                        ReplaceParameters = new List<object>() { type.FullName, nameItem }
                    };

                    throw new UtilityException((int)Errors.NotFoundPropertyInTypeByName, fragment);
                }

                type = info.PropertyType;
            }

            return type;
        }
    }
}
