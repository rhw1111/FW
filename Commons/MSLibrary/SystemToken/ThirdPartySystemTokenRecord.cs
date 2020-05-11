using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.DI;
using MSLibrary.SystemToken.DAL;
using MSLibrary.LanguageTranslate;


namespace MSLibrary.SystemToken
{
    /// <summary>
    /// 第三方系统令牌记录
    /// 唯一键：
    /// SystemLoginEndpointID+AuthorizationEndpointID+UserKey
    /// </summary>
    public class ThirdPartySystemTokenRecord : EntityBase<IThirdPartySystemTokenRecordIMP>
    {
        private static IFactory<IThirdPartySystemTokenRecordIMP> _thirdPartySystemTokenRecordIMPFactory;

        public static IFactory<IThirdPartySystemTokenRecordIMP> ThirdPartySystemTokenRecordIMPFactory
        {
            set
            {
                _thirdPartySystemTokenRecordIMPFactory = value;
            }
        }

        public override IFactory<IThirdPartySystemTokenRecordIMP> GetIMPFactory()
        {
            return _thirdPartySystemTokenRecordIMPFactory;
        }

        /// <summary>
        /// ID
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
        /// 所属系统登录终结点ID
        /// </summary>
        public Guid SystemLoginEndpointID
        {
            get
            {
                return GetAttribute<Guid>("SystemLoginEndpointID");
            }
            set
            {
                SetAttribute<Guid>("SystemLoginEndpointID", value);
            }
        }

        /// <summary>
        /// 所属系统登录终结点
        /// </summary>
        public SystemLoginEndpoint SystemLoginEndpoint
        {
            get
            {
                return GetAttribute<SystemLoginEndpoint>("SystemLoginEndpoint");
            }
            set
            {
                SetAttribute<SystemLoginEndpoint>("SystemLoginEndpoint", value);
            }
        }


        /// <summary>
        /// 所属验证终结点ID
        /// </summary>
        public Guid AuthorizationEndpointID
        {
            get
            {
                return GetAttribute<Guid>("AuthorizationEndpointID");
            }
            set
            {
                SetAttribute<Guid>("AuthorizationEndpointID", value);
            }
        }

        /// <summary>
        /// 所属验证终结点
        /// </summary>
        public AuthorizationEndpoint AuthorizationEndpoint
        {
            get
            {
                return GetAttribute<AuthorizationEndpoint>("AuthorizationEndpoint");
            }
            set
            {
                SetAttribute<AuthorizationEndpoint>("AuthorizationEndpoint", value);
            }
        }

        /// <summary>
        /// 用户关键字
        /// </summary>
        public string UserKey
        {
            get
            {
                return GetAttribute<string>("UserKey");
            }
            set
            {
                SetAttribute<string>("UserKey", value);
            }
        }

        /// <summary>
        /// 第三方令牌
        /// </summary>
        public string Token
        {
            get
            {
                return GetAttribute<string>("Token");
            }
            set
            {
                SetAttribute<string>("Token", value);
            }
        }

        /// <summary>
        /// 令牌过期时间
        /// </summary>
        public int Timeout
        {
            get
            {
                return GetAttribute<int>("Timeout");
            }
            set
            {
                SetAttribute<int>("Timeout", value);
            }
        }

        /// <summary>
        /// 最后刷新时间
        /// </summary>
        public DateTime LastRefeshTime
        {
            get
            {
                return GetAttribute<DateTime>("LastRefeshTime");
            }
            set
            {
                SetAttribute<DateTime>("LastRefeshTime", value);
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

    }

    public interface IThirdPartySystemTokenRecordIMP
    {
        Task Add(ThirdPartySystemTokenRecord record);
        Task Update(ThirdPartySystemTokenRecord record);
        Task Delete(ThirdPartySystemTokenRecord record);

        Task Refresh(ThirdPartySystemTokenRecord record);
    }

    [Injection(InterfaceType = typeof(IThirdPartySystemTokenRecordIMP), Scope = InjectionScope.Transient)]
    public class ThirdPartySystemTokenRecordIMP : IThirdPartySystemTokenRecordIMP
    {
        private IThirdPartySystemTokenRecordStore _thirdPartySystemTokenRecordStore;

        public ThirdPartySystemTokenRecordIMP(IThirdPartySystemTokenRecordStore thirdPartySystemTokenRecordStore)
        {
            _thirdPartySystemTokenRecordStore = thirdPartySystemTokenRecordStore;
        }
        public async Task Add(ThirdPartySystemTokenRecord record)
        {
            await _thirdPartySystemTokenRecordStore.Add(record);
        }

        public async Task Delete(ThirdPartySystemTokenRecord record)
        {
            await _thirdPartySystemTokenRecordStore.Delete(record.UserKey, record.ID);
        }

        public async Task Refresh(ThirdPartySystemTokenRecord record)
        {
            var newToken=await record.AuthorizationEndpoint.RefreshToken(record.SystemLoginEndpoint,record.UserKey, record.Token);
        }

        public async Task Update(ThirdPartySystemTokenRecord record)
        {
            await _thirdPartySystemTokenRecordStore.Update(record);
        }
    }
}
