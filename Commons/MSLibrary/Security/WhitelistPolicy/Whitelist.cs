using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Security.WhitelistPolicy.DAL;

namespace MSLibrary.Security.WhitelistPolicy
{
    public class Whitelist : EntityBase<IWhitelistIMP>
    {
        private static IFactory<IWhitelistIMP> _whitelistIMPFactory;

        public static IFactory<IWhitelistIMP> WhitelistIMPFactory
        {
            set
            {
                _whitelistIMPFactory = value;
            }
        }

        public override IFactory<IWhitelistIMP> GetIMPFactory()
        {
            return _whitelistIMPFactory;
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
        /// 系统名称
        /// </summary>
        public string SystemName
        {
            get
            {
                return GetAttribute<string>("SystemName");
            }
            set
            {
                SetAttribute<string>("SystemName", value);
            }
        }

        /// <summary>
        /// 系统密钥
        /// </summary>
        public string SystemSecret
        {
            get
            {
                return GetAttribute<string>("SystemSecret");
            }
            set
            {
                SetAttribute<string>("SystemSecret", value);
            }
        }
        /// <summary>
        /// 可信IP地址,多个用,隔开
        /// </summary>
        public string TrustIPs
        {
            get
            {
                return GetAttribute<string>("TrustIPs");
            }
            set
            {
                SetAttribute<string>("TrustIPs", value);
            }
        }


        /// <summary>
        /// 是否启用IP检测
        /// </summary>
        public bool EnableIPValidation
        {
            get
            {
                return GetAttribute<bool>("EnableIPValidation");
            }
            set
            {
                SetAttribute<bool>("EnableIPValidation", value);
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
        /// 状态
        /// 0：不可用，1：可用
        /// </summary>
        public int Status
        {
            get
            {
                return GetAttribute<int>("Status");
            }
            set
            {
                SetAttribute<int>("Status", value);
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public async Task Add()
        {
            await _imp.Add(this);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public async Task Update()
        {
            await _imp.Update(this);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task Delete()
        {
            await _imp.Delete(this);
        }
    }


    public interface IWhitelistIMP
    {
        Task Add(Whitelist data);
        Task Update(Whitelist data);
        Task Delete(Whitelist data);
    }

    [Injection(InterfaceType = typeof(IWhitelistIMP), Scope = InjectionScope.Transient)]
    public class WhitelistIMP : IWhitelistIMP
    {
        private IWhitelistStore _whitelistStore;

        public WhitelistIMP(IWhitelistStore whitelistStore)
        {
            _whitelistStore = whitelistStore;
        }
        public async Task Add(Whitelist data)
        {
            await _whitelistStore.Add(data);
        }

        public async Task Delete(Whitelist data)
        {
            await _whitelistStore.Delete(data.ID);
        }

        public async Task Update(Whitelist data)
        {
            await _whitelistStore.Update(data);
        }
    }
}
