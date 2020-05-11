using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace IdentityCenter.Main.IdentityServer
{
    /// <summary>
    /// 认证客户端绑定
    /// </summary>
    public class IdentityClientBinding : EntityBase<IIdentityClientBindingIMP>
    {
        public override IFactory<IIdentityClientBindingIMP>? GetIMPFactory()
        {
            return null;
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {

                return GetAttribute<Guid>(nameof(ID));
            }
            set
            {
                SetAttribute<Guid>(nameof(ID), value);
            }
        }


        /// <summary>
        /// 允许的重定向基地址
        /// </summary>
        public List<string> AllowReturnBaseUrls
        {
            get
            {

                return GetAttribute<List<string>>(nameof(AllowReturnBaseUrls));
            }
            set
            {
                SetAttribute<List<string>>(nameof(AllowReturnBaseUrls), value);
            }
        }

        /// <summary>
        /// 允许的跨域源列表
        /// </summary>
        public List<string> AllowedCorsOrigins
        {
            get
            {
                return GetAttribute<List<string>>(nameof(AllowedCorsOrigins));
            }
            set
            {
                SetAttribute<List<string>>(nameof(AllowedCorsOrigins), value);
            }
        }

        /// <summary>
        /// 认证服务地址
        /// </summary>
        public string IdentityServerUrl
        {
            get
            {

                return GetAttribute<string>(nameof(IdentityServerUrl));
            }
            set
            {
                SetAttribute<string>(nameof(IdentityServerUrl), value);
            }
        }

        /// <summary>
        /// 认证服务地址（内部）
        /// </summary>
        public string IdentityServerInnerUrl
        {
            get
            {

                return GetAttribute<string>(nameof(IdentityServerInnerUrl));
            }
            set
            {
                SetAttribute<string>(nameof(IdentityServerInnerUrl), value);
            }
        }


        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {

                return GetAttribute<string>(nameof(Name));
            }
            set
            {
                SetAttribute<string>(nameof(Name), value);
            }
        }

        /// <summary>
        /// 顺序号
        /// </summary>
        public long Sequence
        {
            get
            {

                return GetAttribute<long>(nameof(Sequence));
            }
            set
            {
                SetAttribute<long>(nameof(Sequence), value);
            }
        }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(CreateTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(CreateTime), value);
            }
        }



        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(ModifyTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(ModifyTime), value);
            }
        }

        public virtual Task<IIdentityClientBindingOptionsInit> InitOptions()
        {
            throw new NotImplementedException();
        }
    }


    public interface IIdentityClientBindingIMP
    {
        Task<IIdentityClientBindingOptionsInit> InitOptions(IdentityClientBinding binding);
    }

    public interface IIdentityClientBindingOptionsInit
    {
        void Init<T>(T options);
    }


    [Injection(InterfaceType = typeof(IIdentityClientBindingIMP), Scope = InjectionScope.Transient)]
    public class IdentityClientBindingIMP : IIdentityClientBindingIMP
    {
        public virtual Task<IIdentityClientBindingOptionsInit> InitOptions(IdentityClientBinding binding)
        {
            throw new NotImplementedException();
        }
    }
}
