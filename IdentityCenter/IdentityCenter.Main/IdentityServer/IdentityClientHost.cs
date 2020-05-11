using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using IdentityServer4;
using IdentityServer4.Configuration;
using Microsoft.IdentityModel.Tokens;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Serializer;
using System.Security.Cryptography.X509Certificates;

namespace IdentityCenter.Main.IdentityServer
{
    public class IdentityClientHost : EntityBase<IIdentityClientHostIMP>
    {
        private static IFactory<IIdentityClientHostIMP>? _identityClientHostIMPFactory;

        public static IFactory<IIdentityClientHostIMP> IdentityClientHostIMPFactory
        {
            set
            {
                _identityClientHostIMPFactory = value;
            }
        }
        public override IFactory<IIdentityClientHostIMP>? GetIMPFactory()
        {
            return _identityClientHostIMPFactory;
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
        /// 环境声明生成器名称
        /// </summary>
        public string EnvironmentClaimGeneratorName
        {
            get
            {
                return GetAttribute<string>(nameof(EnvironmentClaimGeneratorName));
            }
            set
            {
                SetAttribute<string>(nameof(EnvironmentClaimGeneratorName), value);
            }
        }

        /// <summary>
        /// 声明上下文生成器名称
        /// </summary>
        public string ClaimContextGeneratorName
        {
            get
            {
                return GetAttribute<string>(nameof(ClaimContextGeneratorName));
            }
            set
            {
                SetAttribute<string>(nameof(ClaimContextGeneratorName), value);
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

    }

    public interface IIdentityClientHostIMP
    {

    }

    [Injection(InterfaceType = typeof(IIdentityClientHostIMP), Scope = InjectionScope.Transient)]
    public class IdentityClientHostIMP: IIdentityClientHostIMP
    {

    }
}
