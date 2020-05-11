using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Template;
using MSLibrary.LanguageTranslate;
using MSLibrary.SerialNumber.DAL;

namespace MSLibrary.SerialNumber
{
    /// <summary>
    /// 序列号生成配置
    /// 用户控制各种不同的序列号生成规则
    /// </summary>
    public class SerialNumberGeneratorConfiguration : EntityBase<ISerialNumberGeneratorConfigurationIMP>
    {
        private static IFactory<ISerialNumberGeneratorConfigurationIMP> _serialNumberGeneratorConfigurationIMPFactory;

        public static IFactory<ISerialNumberGeneratorConfigurationIMP> SerialNumberGeneratorConfigurationIMPFactory
        {
            set
            {
                _serialNumberGeneratorConfigurationIMPFactory = value;
            }
        }

        public override IFactory<ISerialNumberGeneratorConfigurationIMP> GetIMPFactory()
        {
            return _serialNumberGeneratorConfigurationIMPFactory;
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
        /// 前缀模板
        /// </summary>
        public string PrefixTemplate
        {
            get
            {
                return GetAttribute<string>("PrefixTemplate");
            }
            set
            {
                SetAttribute<string>("PrefixTemplate", value);
            }
        }

        /// <summary>
        /// 流水号长度
        /// </summary>
        public int SerialLength
        {
            get
            {
                return GetAttribute<int>("SerialLength");
            }
            set
            {
                SetAttribute<int>("SerialLength", value);
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

        /// <summary>
        /// 生成序列号
        /// </summary>
        /// <param name="parameters">生成过程中需要使用到的参数列表</param>
        /// <returns></returns>
        public async Task<string> GenerateSerialNumber(Dictionary<string, object> parameters)
        {
            return await _imp.GenerateSerialNumber(this, parameters);
        }


    }


    public interface ISerialNumberGeneratorConfigurationIMP
    {
        Task Add(SerialNumberGeneratorConfiguration configuration);
        Task Update(SerialNumberGeneratorConfiguration configuration);
        Task Delete(SerialNumberGeneratorConfiguration configuration);

        Task<string> GenerateSerialNumber(SerialNumberGeneratorConfiguration configuration, Dictionary<string, object> parameters);
    }


    [Injection(InterfaceType = typeof(ISerialNumberGeneratorConfigurationIMP), Scope = InjectionScope.Transient)]
    public class SerialNumberGeneratorConfigurationIMP : ISerialNumberGeneratorConfigurationIMP
    {
        private ITemplateService _templateService;
        private ISerialNumberRecordRepository _serialNumberRecordRepository;
        private ISerialNumberGeneratorConfigurationStore _serialNumberGeneratorConfigurationStore;

        public SerialNumberGeneratorConfigurationIMP(ITemplateService templateService, ISerialNumberRecordRepository serialNumberRecordRepository, ISerialNumberGeneratorConfigurationStore serialNumberGeneratorConfigurationStore)
        {
            _templateService = templateService;
            _serialNumberRecordRepository = serialNumberRecordRepository;
            _serialNumberGeneratorConfigurationStore = serialNumberGeneratorConfigurationStore;
        }
        public async Task Add(SerialNumberGeneratorConfiguration configuration)
        {
            await _serialNumberGeneratorConfigurationStore.Add(configuration);
        }

        public async Task Delete(SerialNumberGeneratorConfiguration configuration)
        {
            await _serialNumberGeneratorConfigurationStore.Delete(configuration.ID);
        }

        public async Task<string> GenerateSerialNumber(SerialNumberGeneratorConfiguration configuration, Dictionary<string, object> parameters)
        {
            var lcid = ContextContainer.GetValue<int>(ContextTypes.CurrentUserLcid);
            //使用模板服务获取前缀
            var strPrefix = await _templateService.Convert(configuration.PrefixTemplate, new TemplateContext(lcid, parameters));
            //查询序列号记录
            var record = await _serialNumberRecordRepository.QueryByPrefix(strPrefix);
            //如果不存在，则尝试新增，如果新增失败，并且错误Code为重复，则重新查询
            //如果发现仍不存在，则抛出错误
            long recordValue = 0;
            if (record == null)
            {
                record = new SerialNumberRecord()
                {
                    Prefix = strPrefix
                };

                bool needQueryAgain = false;
                try
                {
                    await record.Add();
                }
                catch (UtilityException ex)
                {
                    if (ex.Code == 314720331)
                    {
                        needQueryAgain = true;
                    }
                    else
                    {
                        throw;
                    }
                }

                if (needQueryAgain)
                {
                    record = await _serialNumberRecordRepository.QueryByPrefix(strPrefix);
                    if (record == null)
                    {
                        TextFragment fragment = new TextFragment()
                        {
                             Code= TextCodes.NotFoundSerialNumberRecordByPrefix,
                              DefaultFormatting= "前缀为{0}的序列号记录不存在",
                               ReplaceParameters=new List<object>() { strPrefix }
                        };
                        throw new UtilityException((int)Errors.NotFoundSerialNumberRecordByPrefix, fragment);
                    }
                    await record.RefreashValue();

                }
            }
            else
            {
                await record.RefreashValue();
            }

            recordValue = record.Value;
            return $"{strPrefix}{recordValue.GetPrefixComplete(configuration.SerialLength)}";
        }

        public async Task Update(SerialNumberGeneratorConfiguration configuration)
        {
            await _serialNumberGeneratorConfigurationStore.Update(configuration);
        }
    }
}
