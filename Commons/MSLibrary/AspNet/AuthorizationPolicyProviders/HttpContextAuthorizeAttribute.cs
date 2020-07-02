using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.AspNet.AuthorizationPolicyProviders
{
    /// <summary>
    /// 基于Http上下文的授权特性
    /// 使用该特性，表明Policy需要从HttpcContext中获取
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class HttpContextAuthorizeAttribute : AuthorizeAttribute
    {
        public const string _prefix = "#HttpContext_";


        public Type Type
        {
            get
            {
                var type = Type.GetType(Policy.Substring(_prefix.Length));
                return type;
            }
            set
            {
                if (value == null)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.ValueNotAllowNull,
                        DefaultFormatting = "值不能为null，发生位置为{0}",
                        ReplaceParameters = new List<object>() { $"{this.GetType().FullName}.Type" }
                    };

                    throw new UtilityException((int)Errors.ValueNotAllowNull, fragment, 1, 0);
                }


                if (!value.IsAssignableFrom(typeof(IHttpContextPolicyResolveService)))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.TypeNotImplimentInterface,
                        DefaultFormatting = "类型{0}没有实现接口{1}",
                        ReplaceParameters = new List<object>() { value.FullName, typeof(IHttpContextPolicyResolveService).FullName }
                    };

                    throw new UtilityException((int)Errors.TypeNotImplimentInterface, fragment, 1, 0);
                }
                Policy = $"{_prefix}{value.AssemblyQualifiedName}";
            }
        }
    }

    public interface IHttpContextPolicyResolveService
    {
        Task<AuthorizationPolicy> Execute(HttpContext context);
    }
}
