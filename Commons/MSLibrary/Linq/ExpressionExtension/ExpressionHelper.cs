using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace MSLibrary.Linq.ExpressionExtension
{
    /// <summary>
    /// 表达式树帮助器
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// 基于属性名称构建属性表达式
        /// 属性名称格式为XXX.XXX.XXX
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static MemberExpression BuilderMemberExpression(ParameterExpression parameter,string attributeName)
        {
            var attributeNameList = attributeName.Split('.').ToList();

            MemberExpression result = Expression.PropertyOrField(parameter, attributeNameList[0]);
            while(attributeNameList.Count>0)
            {           
                attributeNameList.RemoveAt(0);

                result = BuilderMemberExpression(result, attributeNameList);
            }

            return result;
        }


        private static MemberExpression BuilderMemberExpression(MemberExpression member,List<string> attributeNameList)
        {
            if (attributeNameList.Count==0)
            {
                return member;
            }

            return Expression.PropertyOrField(member, attributeNameList[0]);
        }



    }
}
