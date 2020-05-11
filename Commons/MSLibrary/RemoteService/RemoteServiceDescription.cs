using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Security;
using MSLibrary.RemoteService.AuthInfoGeneratorServices;
using MSLibrary.RemoteService.DAL;

namespace MSLibrary.RemoteService
{
    /// <summary>
    /// 远程服务描述
    /// </summary>
    public class RemoteServiceDescription : EntityBase<IRemoteServiceDescriptionIMP>
    {
        private static IFactory<IRemoteServiceDescriptionIMP> _remoteServiceDescriptionIMPFactory;

        public static IFactory<IRemoteServiceDescriptionIMP> RemoteServiceDescriptionIMPFactory
        {
            set
            {
                _remoteServiceDescriptionIMPFactory = value;
            }
        }

        public override IFactory<IRemoteServiceDescriptionIMP> GetIMPFactory()
        {
            return _remoteServiceDescriptionIMPFactory;
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
        /// 服务名称
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
        /// 服务地址
        /// </summary>
        public string Address
        {
            get
            {
                return GetAttribute<string>("Address");
            }
            set
            {
                SetAttribute<string>("Address", value);
            }
        }

        /// <summary>
        /// 服务身份验证时传递的信息的类型
        /// </summary>
        public string AuthInfoType
        {
            get
            {
                return GetAttribute<string>("AuthInfoType");
            }
            set
            {
                SetAttribute<string>("AuthInfoType", value);
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

        public async Task<Dictionary<string, string>> GenerateAuthInfo()
        {
            return await _imp.GenerateAuthInfo(this);
        }
    }

    public interface IRemoteServiceDescriptionIMP
    {
        Task Add(RemoteServiceDescription description);
        Task Update(RemoteServiceDescription description);
        Task Delete(RemoteServiceDescription description);
        Task<Dictionary<string, string>> GenerateAuthInfo(RemoteServiceDescription description);
    }

    [Injection(InterfaceType = typeof(IRemoteServiceDescriptionIMP), Scope = InjectionScope.Transient)]
    public class RemoteServiceDescriptionIMP : IRemoteServiceDescriptionIMP
    {
        private static Dictionary<string, IFactory<IAuthInfoGeneratorService>> _authInfoGeneratorServiceFactories =new Dictionary<string, IFactory<IAuthInfoGeneratorService>>();

        public static Dictionary<string, IFactory<IAuthInfoGeneratorService>> AuthInfoGeneratorServiceFactories
        {
            get
            {
                return _authInfoGeneratorServiceFactories;
            }
        }


        private IRemoteServiceDescriptionStore _remoteServiceDescriptionStore;

        public RemoteServiceDescriptionIMP(IRemoteServiceDescriptionStore remoteServiceDescriptionStore)
        {
            _remoteServiceDescriptionStore = remoteServiceDescriptionStore;
        }
        public async Task Add(RemoteServiceDescription description)
        {
            await _remoteServiceDescriptionStore.Add(description);
        }

        public async Task Delete(RemoteServiceDescription description)
        {
            await _remoteServiceDescriptionStore.Delete(description.ID);
        }

        public async Task<Dictionary<string, string>> GenerateAuthInfo(RemoteServiceDescription description)
        {
            if (!_authInfoGeneratorServiceFactories.TryGetValue(description.AuthInfoType, out IFactory<IAuthInfoGeneratorService> serviceFactory))
            {

                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRemoteServiceAuthInfoGeneratorServiceByType,
                    DefaultFormatting = "找不到类型为{0}的远程服务验证信息生成服务，发生位置：{1}",
                    ReplaceParameters = new List<object>() { description.AuthInfoType, $"{this.GetType().FullName}.AuthInfoGeneratorServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundRemoteServiceAuthInfoGeneratorServiceByType, fragment);

            }

            return await serviceFactory.Create().Generate();
        }

        public async Task Update(RemoteServiceDescription description)
        {
            await _remoteServiceDescriptionStore.Update(description);
        }
    }
}
