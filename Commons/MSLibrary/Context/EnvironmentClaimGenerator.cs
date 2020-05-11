using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Context.EnvironmentClaimGeneratorServices;
using MSLibrary.Context.DAL;

namespace MSLibrary.Context
{
    /// <summary>
    /// 环境声明生成器
    /// 负责从当前环境中生成上下文
    /// </summary>
    public class EnvironmentClaimGenerator : EntityBase<IEnvironmentClaimGeneratorIMP>
    {
        private static IFactory<IEnvironmentClaimGeneratorIMP> _environmentClaimGeneratorIMPFactory;

        public static IFactory<IEnvironmentClaimGeneratorIMP> EnvironmentClaimGeneratorIMPFactory
        {
            set
            {
                _environmentClaimGeneratorIMPFactory = value;
            }
        }

        public override IFactory<IEnvironmentClaimGeneratorIMP> GetIMPFactory()
        {
            return _environmentClaimGeneratorIMPFactory;
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
        /// 生成声明
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> Generate()
        {
            return await _imp.Generate(this);
        }

        public async Task Add()
        {
            await _imp.Add(this);
        }

        public async Task Update()
        {
            await _imp.Add(this);
        }
        public async Task Delete()
        {
            await _imp.Delete(this);
        }

    }

    public interface IEnvironmentClaimGeneratorIMP
    {
        Task<ClaimsIdentity> Generate(EnvironmentClaimGenerator generator);

        Task Add(EnvironmentClaimGenerator generator);
        Task Update(EnvironmentClaimGenerator generator);
        Task Delete(EnvironmentClaimGenerator generator);
    }

    [Injection(InterfaceType = typeof(IEnvironmentClaimGeneratorIMP), Scope = InjectionScope.Transient)]
    public class EnvironmentClaimGeneratorIMP : IEnvironmentClaimGeneratorIMP
    {
        private static Dictionary<string, IFactory<IEnvironmentClaimGeneratorService>> _environmentClaimGeneratorServiceFactories = new Dictionary<string, IFactory<IEnvironmentClaimGeneratorService>>();

        public static Dictionary<string, IFactory<IEnvironmentClaimGeneratorService>> EnvironmentClaimGeneratorServiceFactories
        {
            get
            {
                return _environmentClaimGeneratorServiceFactories;
            }
        }

        private IEnvironmentClaimGeneratorStore _environmentClaimGeneratorStore;

        public EnvironmentClaimGeneratorIMP(IEnvironmentClaimGeneratorStore environmentClaimGeneratorStore)
        {
            _environmentClaimGeneratorStore = environmentClaimGeneratorStore;
        }

        public async Task Add(EnvironmentClaimGenerator generator)
        {
            await _environmentClaimGeneratorStore.Add(generator);
        }

        public async Task Delete(EnvironmentClaimGenerator generator)
        {
            await _environmentClaimGeneratorStore.Delete(generator.ID);
        }

        /// 根据环境生成声明
        /// 具体生成服务通过静态键值对注册，键为生成器名称
        /// 该方法将调用对应名称的生成服务来完成
        public async Task<ClaimsIdentity> Generate(EnvironmentClaimGenerator generator)
        {
            if (!_environmentClaimGeneratorServiceFactories.TryGetValue(generator.Type, out IFactory<IEnvironmentClaimGeneratorService> generatorServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEnvironmentClaimGeneratorServiceByType,
                    DefaultFormatting = "找不到类型为{0}的环境声明生成服务，发生位置:{1}",
                    ReplaceParameters = new List<object>() { generator.Type, $"{this.GetType().FullName}.EnvironmentClaimGeneratorServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundEnvironmentClaimGeneratorServiceByType, fragment);
            }

            return await generatorServiceFactory.Create().Do();
        }

        public async Task Update(EnvironmentClaimGenerator generator)
        {
            await _environmentClaimGeneratorStore.Update(generator);
        }
    }
}
