using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.Security.WhitelistPolicy.DAL;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using MSLibrary.EntityMetadata;

namespace MSLibrary.Security.WhitelistPolicy
{
    /// <summary>
    /// 系统操作
    /// 用于标识一个系统的操作动作
    /// </summary>
    public class SystemOperation : EntityBase<ISystemOperationIMP>
    {
        private static IFactory<ISystemOperationIMP> _systemOperationIMPFactory;

        public static IFactory<ISystemOperationIMP> SystemOperationIMPFactory
        {
            set
            {
                _systemOperationIMPFactory = value;
            }
        }
        public override IFactory<ISystemOperationIMP> GetIMPFactory()
        {
            return _systemOperationIMPFactory;
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
        /// 操作名称
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
        /// 新增系统动作
        /// </summary>
        /// <returns></returns>
        public async Task Add()
        {
            await _imp.Add(this);
        }

        /// <summary>
        /// 修改系统动作
        /// </summary>
        /// <returns></returns>
        public async Task Update()
        {
            await _imp.Update(this);
        }
        /// <summary>
        /// 删除系统动作
        /// </summary>
        /// <returns></returns>
        public async Task Delete()
        {
            await _imp.Delete(this);
        }
        /// <summary>
        /// 新增白名单关联关系
        /// </summary>
        /// <param name="whitelistId">白名单编号</param>
        /// <returns></returns>
        public async Task AddWhitelistRelation(Guid whitelistId)
        {
            await _imp.AddWhitelistRelation(this, whitelistId);
        }
        /// <summary>
        /// 移除白名单关联关系
        /// </summary>
        /// <param name="whitelistId">白名单编号</param>
        /// <returns></returns>
        public async Task RemoveWhitelistRelation(Guid whitelistId)
        {
            await _imp.RemoveWhitelistRelation(this,whitelistId);
        }

        /// <summary>
        /// 根据白名单Id获取关联的白名单
        /// </summary>
        /// <param name="whitelistId">白名单Id</param>
        /// <returns></returns>
        public async Task<Whitelist> GetWhitelist(Guid whitelistId)
        {
            return await _imp.GetWhitelist(this,whitelistId);
        }

        /// <summary>
        /// 根据白名单的系统名称获取关联的白名单
        /// </summary>
        /// <param name="systemName">白名单的系统名称</param>
        /// <returns></returns>
        public async Task<Whitelist> GetWhitelist(string systemName)
        {
            return await _imp.GetWhitelist(this, systemName);
        }

        /// <summary>
        /// 检测是否匹配白名单设置
        /// </summary>
        /// <param name="systemName">系统名称</param>
        /// <param name="signature">签名</param>
        /// <param name="ip">请求的IP地址</param>
        /// <returns></returns>
        public async Task<ValidateResult> Validate(string systenName,string signature,string ip)
        {
            return await _imp.Validate(this,systenName,signature,ip);
        }
    }



    public interface ISystemOperationIMP
    {
        Task Add(SystemOperation operation);
        Task Update(SystemOperation operation);
        Task Delete(SystemOperation operation);
        Task AddWhitelistRelation(SystemOperation operation,Guid whitelistId);
        Task RemoveWhitelistRelation(SystemOperation operation,Guid whitelistId);
        Task<Whitelist> GetWhitelist(SystemOperation operation,Guid whitelistId);
        Task<Whitelist> GetWhitelist(SystemOperation operation, string systemName);
        Task<Whitelist> GetWhitelist(SystemOperation operation, string systemName,int status);
        Task<ValidateResult> Validate(SystemOperation operation,string systemName, string signature, string ip);
    }

    [Injection(InterfaceType = typeof(ISystemOperationIMP), Scope = InjectionScope.Transient)]
    public class SystemOperationIMP : ISystemOperationIMP
    {
        private ISystemOperationStore _systemOperationStore;
        private IWhitelistStore _whitelistStore;
        private ISystemOperationWhitelistRelationStore _systemOperationWhitelistRelationStore;
        private ISecurityService _securityService;
        private IOptionSetValueMetadataRepository _optionSetValueMetadataRepository;

        public SystemOperationIMP(ISystemOperationStore systemOperationStore, IWhitelistStore whitelistStore, ISystemOperationWhitelistRelationStore systemOperationWhitelistRelationStore, ISecurityService securityService, IOptionSetValueMetadataRepository optionSetValueMetadataRepository)
        {
            _systemOperationStore = systemOperationStore;
            _whitelistStore = whitelistStore;
            _systemOperationWhitelistRelationStore = systemOperationWhitelistRelationStore;
            _securityService = securityService;
            _optionSetValueMetadataRepository = optionSetValueMetadataRepository;
        }
        public async Task Add(SystemOperation operation)
        {
            await _systemOperationStore.Add(operation);
        }

        public async Task AddWhitelistRelation(SystemOperation operation, Guid whitelistId)
        {
            await _systemOperationWhitelistRelationStore.Add(operation.ID, whitelistId);
        }

        public async Task Delete(SystemOperation operation)
        {
            await _systemOperationStore.Delete(operation.ID);
        }

        public async Task<Whitelist> GetWhitelist(SystemOperation operation, Guid whitelistId)
        {
            return await _whitelistStore.QueryBySystemOperationRelation(operation.ID, whitelistId);
        }

        public async Task<Whitelist> GetWhitelist(SystemOperation operation, string systemName)
        {
            return await _whitelistStore.QueryBySystemOperationRelation(operation.ID, systemName);
        }

        public async Task<Whitelist> GetWhitelist(SystemOperation operation, string systemName, int status)
        {
            return await _whitelistStore.QueryBySystemOperationRelation(operation.ID, systemName,status);
        }

        public async Task RemoveWhitelistRelation(SystemOperation operation, Guid whitelistId)
        {
            await _systemOperationWhitelistRelationStore.Delete(operation.ID, whitelistId);
        }

        public async Task Update(SystemOperation operation)
        {
            await _systemOperationStore.Update(operation);
        }

        /// <summary>
        /// 检测请求是否合法
        /// 如果signature等于对应白名单中的密钥，则直接返回true，
        /// 否则signature的格式必须为JWT格式，其中playload的格式为
        /// {
        ///     "iat":颁发时间,
        ///     "exp":过期时间,
        ///     "systemname":系统名称
        /// }
        /// 签名密钥为对应白名单中的密钥
        /// 将判断是否过期、签名中的systemname是否与传入的systemname一致
        /// 如果检测IP已打开，则还需要检查IP是否在可信IP中
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="systemName"></param>
        /// <param name="signature"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public async Task<ValidateResult> Validate(SystemOperation operation,string systemName, string signature, string ip)
        {
            ValidateResult result = new ValidateResult()
            {
                Result = true
            };
            //获取关联的白名单
            var whitelist=await GetWhitelist(operation, systemName,1);
            if (whitelist==null)
            {
                result.Result = false;
                result.Description = string.Format(StringLanguageTranslate.Translate(TextCodes.NotFoundWhitelistInSystemOperationWithNameStatus, "在系统操作{0}中找不到系统名称为{1}、状态为{2}的白名单"),operation.Name,systemName,OptionSetMetadataValueHelper.GetLable(_optionSetValueMetadataRepository,$"{typeof(Whitelist).FullName}.Status",1));
                return result;
            }

            //判断签名是否等于密钥
            if (signature==whitelist.SystemSecret)
            {
                return result;
            }

            //判断JWT是否正确

            var jwtValidateResult=_securityService.ValidateJWT(whitelist.SystemSecret, signature);
            if (!jwtValidateResult.ValidateResult.Result)
            {
                return jwtValidateResult.ValidateResult;
            }



            //检查系统名称是否正确
            if (!jwtValidateResult.Playload.TryGetValue("systemname",out string strSystemName))
            {            
                result.Result = false;
                result.Description= string.Format(StringLanguageTranslate.Translate(TextCodes.NotFoundKeyNameInSystemOperation, "在系统操作{0}的验证方法中，JWT的Playload中找不到键为{1}的键值对"), operation.Name,"systemname");
                return result;
            }
   
            if (string.IsNullOrEmpty(strSystemName) || strSystemName!= systemName)
            {
                result.Result = false;
                result.Description = string.Format(StringLanguageTranslate.Translate(TextCodes.SystemNameNotEqualInSystemOperationValidation, "在系统操作{0}的验证方法中，签名中的系统名称为{1}，传入的系统名称为{2}，两者不相等"), operation.Name, strSystemName,systemName);
                return result;
            }
            

            //如果启用了IP检测，则还需要检测IP
            if (whitelist.EnableIPValidation)
            {
                if (!whitelist.TrustIPs.Contains(ip))
                {
                    result.Result = false;
                    result.Description = string.Format(StringLanguageTranslate.Translate(TextCodes.IPFailInSystemOperationValidation, "在系统操作{0}的验证方法中，白名单系统名称为{1}的合法IP为{2}，访问IP为{3}，两者不匹配"),operation.Name,systemName,whitelist.TrustIPs,ip);
                    return result;
                }
            }

            return result;
        }
    }
}
