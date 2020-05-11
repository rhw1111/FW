using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MSLibrary.DI;

namespace MSLibrary.Oauth
{
    /// <summary>
    /// Oauth终结点
    /// </summary>
    public class OauthEndpoint:EntityBase<IOauthEndpointIMP>
    {
        private static IFactory<IOauthEndpointIMP> _oauthEndpointIMPFactory;

        public static IFactory<IOauthEndpointIMP> OauthEndpointIMPFactory
        {
            set
            {
                _oauthEndpointIMPFactory = value;
            }
        }

        public override IFactory<IOauthEndpointIMP> GetIMPFactory()
        {
            return _oauthEndpointIMPFactory;
        }

        public OauthEndpoint():base()
        {
            
        }

        /// <summary>
        /// 编号
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
        /// 系统名称
        /// </summary>
        public string ClientId
        {
            get
            {
                return GetAttribute<string>("ClientId");
            }
            set
            {
                SetAttribute<string>("ClientId", value);
            }
        }

        /// <summary>
        /// 系统基地址
        /// </summary>
        public string ClientBaseUrl
        {
            get
            {
                return GetAttribute<string>("ClientBaseUrl");
            }
            set
            {
                SetAttribute<string>("ClientBaseUrl", value);
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

        public async Task<bool> ValidateUrl(string url)
        {
            return await _imp.ValidateUrl(this, url);
        }
    }


    public interface IOauthEndpointIMP
    {
        Task Add(OauthEndpoint endpoint);
        Task Update(OauthEndpoint endpoint);
        Task Delete(OauthEndpoint endpoint);

        Task<bool> ValidateUrl(OauthEndpoint endpoint,string url);
    }

    public class OauthEndpointIMP : IOauthEndpointIMP
    {
        public Task Add(OauthEndpoint endpoint)
        {
            throw new NotImplementedException();
        }

        public Task Delete(OauthEndpoint endpoint)
        {
            throw new NotImplementedException();
        }

        public Task Update(OauthEndpoint endpoint)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateUrl(OauthEndpoint endpoint, string url)
        {
            Uri uri = new Uri(url);
            Uri baseUri = new Uri(endpoint.ClientBaseUrl);
            if (baseUri.IsBaseOf(uri))
            {
                return await Task.FromResult(true);
            }
            else
            {
                return await Task.FromResult(false);
            }
        }
    }


    public class OauthEndpointIMPFactory : IFactory<IOauthEndpointIMP>
    {
        public IOauthEndpointIMP Create()
        {
            throw new NotImplementedException();
        }
    }
}
