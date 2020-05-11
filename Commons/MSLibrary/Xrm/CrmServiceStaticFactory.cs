using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Collections;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm
{
    /// <summary>
    /// Crm服务静态工厂
    /// Crm服务获取的入口点
    /// </summary>
    public static class CrmServiceStaticFactory
    {
        public static int Timeout { get; set; } = 5 * 60;
        public static int Limit { get; set; } = 200;
             
        public static ICrmServiceFactoryRepository CrmServiceFactoryRepository { get; set; } = DIContainerContainer.Get<ICrmServiceFactoryRepository>();

        private static TimeLoad<string, CrmServiceFactory> _timeLoad=null;

        private static object _lockTimeLoad = new object();

        /// <summary>
        /// 获取指定工厂名称的工厂创建的Crm服务
        /// </summary>
        /// <param name="factoryName"></param>
        /// <returns></returns>
        public static async Task<ICrmService> Create(string factoryName)
        {
            initTimeLoad();
            var factory= await _timeLoad.GetDataAsync(factoryName);

            return await factory.Create();
        }

        private static void initTimeLoad()
        {
            if (_timeLoad==null)
            {
                lock(_lockTimeLoad)
                {
                    if (_timeLoad==null)
                    {
                        _timeLoad = new TimeLoad<string, CrmServiceFactory>
                            (
                             async(name)=>
                             {
                                 var factory= await CrmServiceFactoryRepository.QueryByName(name);

                                 if (factory == null)
                                 {
                                     var fragment = new TextFragment()
                                     {
                                         Code = TextCodes.NotFountCrmServiceFactorybyName,
                                         DefaultFormatting = "找不到名称为{0}的Crm服务工厂",
                                         ReplaceParameters = new List<object>() { name }
                                     };

                                     throw new UtilityException((int)Errors.NotFountCrmServiceFactorybyName, fragment);
                                 }
                                 return factory;
                             },
                             (name)=>
                             {
                                 throw new NotImplementedException();
                             },
                             Timeout,
                             Limit
                            );
                    }
                }
            }
        }
    }
}
