using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Context.ClaimContextGeneratorServices;
using MSLibrary.Context.DAL;

namespace MSLibrary.Context
{
    /// <summary>
    /// 基于声明的上下文生成器
    /// 负责根据声明初始化上下文
    /// </summary>
    public class ClaimContextGenerator : EntityBase<IClaimContextGeneratorIMP>
    {
        private static IFactory<IClaimContextGeneratorIMP> _claimContextGeneratorIMPFactory;

        public static IFactory<IClaimContextGeneratorIMP> ClaimContextGeneratorIMPFactory
        {
            set
            {
                _claimContextGeneratorIMPFactory = value;
            }
        }
        public override IFactory<IClaimContextGeneratorIMP> GetIMPFactory()
        {
            return _claimContextGeneratorIMPFactory;
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

        public void ContextInit(IEnumerable<Claim> claims)
        {
             _imp.ContextInit(this, claims);
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
    /// 基于声明的上下文生成器的实现接口
    /// </summary>
    public interface IClaimContextGeneratorIMP
    {
        void ContextInit(ClaimContextGenerator generator, IEnumerable<Claim> claims);

        Task Add(ClaimContextGenerator generator);
        Task Update(ClaimContextGenerator generator);
        Task Delete(ClaimContextGenerator generator);
    }

    [Injection(InterfaceType = typeof(IClaimContextGeneratorIMP), Scope = InjectionScope.Transient)]
    public class ClaimContextGeneratorIMP : IClaimContextGeneratorIMP
    {
        private static Dictionary<string, IFactory<IClaimContextGeneratorService>> _claimContextGeneratorServiceFactories = new Dictionary<string, IFactory<IClaimContextGeneratorService>>();

        public static Dictionary<string, IFactory<IClaimContextGeneratorService>> ClaimContextGeneratorServiceFactories
        {
            get
            {
                return _claimContextGeneratorServiceFactories;
            }
        }

        private IClaimContextGeneratorStore _claimContextGeneratorStore;

        public ClaimContextGeneratorIMP(IClaimContextGeneratorStore claimContextGeneratorStore)
        {
            _claimContextGeneratorStore = claimContextGeneratorStore;
        }



        public void ContextInit(ClaimContextGenerator generator, IEnumerable<Claim> claims)
        {
            if (!_claimContextGeneratorServiceFactories.TryGetValue(generator.Type, out IFactory<IClaimContextGeneratorService> generatorServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundClaimContextGeneratorServiceByType,
                    DefaultFormatting = "找不到类型为{0}的声明上下文生成服务，发生位置:{1}",
                    ReplaceParameters = new List<object>() { generator.Type, $"{this.GetType().FullName}.ClaimContextGeneratorServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundClaimContextGeneratorServiceByType, fragment);
            }

            generatorServiceFactory.Create().Do(claims);
        }

        public async Task Add(ClaimContextGenerator generator)
        {
            await _claimContextGeneratorStore.Add(generator);
        }

        public async Task Update(ClaimContextGenerator generator)
        {
            await _claimContextGeneratorStore.Update(generator);
        }

        public async Task Delete(ClaimContextGenerator generator)
        {
            await _claimContextGeneratorStore.Delete(generator.ID);
        }
    }
}
