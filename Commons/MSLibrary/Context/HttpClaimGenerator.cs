using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Context.HttpClaimGeneratorServices;
using MSLibrary.Context.DAL;

namespace MSLibrary.Context
{
    /// <summary>
    /// Http声明生成器
    /// 负责根据Http上下文生成声明
    /// </summary>
    public class HttpClaimGenerator:EntityBase<IHttpClaimGeneratorIMP>
    {
        private static IFactory<IHttpClaimGeneratorIMP> _httpClaimGeneratorIMPFactory;

        public static IFactory<IHttpClaimGeneratorIMP> HttpClaimGeneratorIMPFactory
        {
            set
            {
                _httpClaimGeneratorIMPFactory = value;
            }
        }
        public override IFactory<IHttpClaimGeneratorIMP> GetIMPFactory()
        {
            return _httpClaimGeneratorIMPFactory;
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>("ID");
            }
            set
            {
                SetAttribute<Guid>("ID", value);
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return GetAttribute<string>("Name");
            }
            set
            {
                SetAttribute<string>("Name", value);
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type
        {
            get
            {
                return GetAttribute<string>("Type");
            }
            set
            {
                SetAttribute<string>("Type", value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }

        /// <summary>
        /// 根据http上下文生成声明
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> Generate(HttpContext httpContext)
        {
            return await _imp.Generate(this, httpContext);
        }

        public async Task Add()
        {
            await _imp.Add(this);
        }
        public async Task Update()
        {
            await _imp.Update(this);
        }
        public async Task Delete()
        {
            await _imp.Delete(this);

        }

    }

    /// <summary>
    /// HttpClaimGenerator的实现接口
    /// </summary>
    public interface IHttpClaimGeneratorIMP
    {
        Task<ClaimsIdentity> Generate(HttpClaimGenerator generator, HttpContext httpContext);

        Task Add(HttpClaimGenerator generator);
        Task Update(HttpClaimGenerator generator);
        Task Delete(HttpClaimGenerator generator);
    }

    [Injection(InterfaceType = typeof(IHttpClaimGeneratorIMP), Scope = InjectionScope.Transient)]
    public class HttpClaimGeneratorIMP : IHttpClaimGeneratorIMP
    {
        private static Dictionary<string, IFactory<IHttpClaimGeneratorService>> _httpClaimGeneratorServiceFactories = new Dictionary<string, IFactory<IHttpClaimGeneratorService>>();

        public static Dictionary<string, IFactory<IHttpClaimGeneratorService>> HttpClaimGeneratorServiceFactories
        {
            get
            {
                return _httpClaimGeneratorServiceFactories;
            }
        }

        private IHttpClaimGeneratorStore _httpClaimGeneratorStore;

        public HttpClaimGeneratorIMP(IHttpClaimGeneratorStore httpClaimGeneratorStore)
        {
            _httpClaimGeneratorStore = httpClaimGeneratorStore;
        }

        public async Task Add(HttpClaimGenerator generator)
        {
            await _httpClaimGeneratorStore.Add(generator);
        }

        public async Task Delete(HttpClaimGenerator generator)
        {
            await _httpClaimGeneratorStore.Delete(generator.ID);
        }

        /// <summary>
        /// 根据Http上下文生成声明
        /// 具体生成服务通过静态键值对注册，键为生成器名称
        /// 该方法将调用对应名称的生成服务来完成
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> Generate(HttpClaimGenerator generator, HttpContext httpContext)
        {
            if (!_httpClaimGeneratorServiceFactories.TryGetValue(generator.Type,out IFactory<IHttpClaimGeneratorService> generatorServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundHttpClaimGeneratorServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Http声明生成服务，发生位置:{1}",
                    ReplaceParameters = new List<object>() { generator.Type, $"{this.GetType().FullName}.HttpClaimGeneratorServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundHttpClaimGeneratorServiceByType, fragment);

            }

            var result= await generatorServiceFactory.Create().Do(httpContext);

            return result;
        }

        public async Task Update(HttpClaimGenerator generator)
        {
            await _httpClaimGeneratorStore.Update(generator);
        }
    }
}
