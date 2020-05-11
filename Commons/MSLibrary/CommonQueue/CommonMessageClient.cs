using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.CommonQueue
{
    public class CommonMessageClient : EntityBase<ICommonMessageClientIMP>
    {
        private static IFactory<ICommonMessageClientIMP> _commonMessageClientIMPFactory;

        public static IFactory<ICommonMessageClientIMP> CommonMessageClientIMPFactory
        {
            set
            {
                _commonMessageClientIMPFactory = value;
            }
        }

        public override IFactory<ICommonMessageClientIMP> GetIMPFactory()
        {
            return _commonMessageClientIMPFactory;
        }


        /// <summary>
        /// id
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
        /// 消息关键字
        /// 用于保证相同关键字的消息属于同一队列
        /// </summary>
        public string Key
        {
            get
            {
                return GetAttribute<string>("Key");
            }
            set
            {
                SetAttribute<string>("Key", value);
            }
        }

        /// <summary>
        /// 消息类型
        /// 同一类型的消息属于同一组
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
        /// 消息内容
        /// </summary>
        public string Data
        {
            get
            {
                return GetAttribute<string>("Data");
            }
            set
            {
                SetAttribute<string>("Data", value);
            }
        }

        /// <summary>
        /// 期望执行时间
        /// </summary>
        public DateTime? ExpectationExecuteTime
        {
            get
            {
                return GetAttribute<DateTime?>("ExpectationExecuteTime");
            }
            set
            {
                SetAttribute<DateTime?>("ExpectationExecuteTime", value);
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

        public async Task Send()
        {
            await _imp.Send(this);
        }

    }

    public interface ICommonMessageClientIMP
    {
        Task Send(CommonMessageClient message);
    }

    [Injection(InterfaceType = typeof(ICommonMessageClientIMP), Scope = InjectionScope.Transient)]
    public class CommonMessageClientIMP : ICommonMessageClientIMP
    {
        private ICommonMessageClientTypeRepositoryCacheProxy _commonMessageClientTypeRepositoryCacheProxy;

        public CommonMessageClientIMP(ICommonMessageClientTypeRepositoryCacheProxy commonMessageClientTypeRepositoryCacheProxy)
        {
            _commonMessageClientTypeRepositoryCacheProxy = commonMessageClientTypeRepositoryCacheProxy;
        }
        public async Task Send(CommonMessageClient message)
        {
            var type=await _commonMessageClientTypeRepositoryCacheProxy.QueryByName(message.Type);
            if (type==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCommonMessageClientType,
                    DefaultFormatting = "找不到类型名称为{0}的通用消息客户端类型",
                    ReplaceParameters = new List<object>() { message.Type }
                };

                throw new UtilityException((int)Errors.NotFoundCommonMessageClientType, fragment);
            }

            CommonMessage commonMessage = new CommonMessage()
            {
                Type = message.Type,
                CreateTime = message.CreateTime,
                Data = message.Data,
                Key = message.Key,
                ExpectationExecuteTime = message.ExpectationExecuteTime
            };

            await type.Endpoint.Product(commonMessage);
        }
    }
}
