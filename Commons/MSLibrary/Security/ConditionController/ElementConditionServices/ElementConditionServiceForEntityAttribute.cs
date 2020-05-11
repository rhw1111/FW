using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using MSLibrary.LanguageTranslate;
using MSLibrary.DI;
using MSLibrary.Entity;

namespace MSLibrary.Security.ConditionController.ElementConditionServices
{
    /// <summary>
    /// 判断指定实体类型的数据里面的指定属性的值和设定值的关系是否符合指定的操作符
    /// 需要从参数中获取EntityKeyForEntityAttribute，表示实体记录的唯一关键字，将根据该关键字查找记录
    /// 格式<entityattribute entitytype="要检查的实体类型" attributename="要检查的实体属性名称,支持级联，如XXX.XXX,这种情况下，属性必须也是一个基于ModelBase的类型" operator="来自MSLibrary.Operators" valuetype="来自于来自MSLibrary.EntityAttributeValueTypes" value="属性的值，会根据" parameterindex="如果条件中有多个同名规则，该属性表明本规则该从参数数组中获取参数的位置，从0开始，如果条件中不存在多个同名规则，不要设置该属性" invertresult="false"/>
    /// </summary>
    [Injection(InterfaceType = typeof(ElementConditionServiceForEntityAttribute), Scope = InjectionScope.Singleton)]
    public class ElementConditionServiceForEntityAttribute : IElementConditionService
    {
        private static Dictionary<string, IFactory<IEntityAttributeValueValidateService>> _entityAttributeValueValidateServiceFactories = new Dictionary<string, IFactory<IEntityAttributeValueValidateService>>();

        /// <summary>
        /// 实体属性值检查服务工厂键值对
        /// 键为操作符(MSLibrary.Operators)
        /// 该属性需要初始化
        /// </summary>
        public static Dictionary<string, IFactory<IEntityAttributeValueValidateService>> EntityAttributeValueValidateServiceFactories
        {
            get
            {
                return _entityAttributeValueValidateServiceFactories;
            }
        }

        private IEntityRepository _entityRepository;

        public ElementConditionServiceForEntityAttribute(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
        public async Task<ValidateResult> Validate(XElement securityElement, Dictionary<string, object> parameters)
        {
           
            var entityTypeElement = securityElement.Attribute("entitytype");
            var attributeNameElement = securityElement.Attribute("attributename");
            var operatorElement = securityElement.Attribute("operator");
            var valueTypeElement = securityElement.Attribute("valuetype");
            var valueElement = securityElement.Attribute("value");
            var parameterIndexElement = securityElement.Attribute("parameterindex");
            var invertResultElement = securityElement.Attribute("invertresult");
            TextFragment fragment;

            if (entityTypeElement==null)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundAttributeInConditionElement,
                    DefaultFormatting = "在条件元素{0}中，找不到名称为{1}的属性",
                    ReplaceParameters = new List<object>() { "entityattribute", "entitytype" }
                };

                throw new UtilityException((int)Errors.NotFoundAttributeInConditionElement, fragment);
            }

            if (attributeNameElement == null)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundAttributeInConditionElement,
                    DefaultFormatting = "在条件元素{0}中，找不到名称为{1}的属性",
                    ReplaceParameters = new List<object>() { "entityattribute", "attributename" }
                };

                throw new UtilityException((int)Errors.NotFoundAttributeInConditionElement, fragment);
            }

            if (operatorElement == null)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundAttributeInConditionElement,
                    DefaultFormatting = "在条件元素{0}中，找不到名称为{1}的属性",
                    ReplaceParameters = new List<object>() { "entityattribute", "operator" }
                };

                throw new UtilityException((int)Errors.NotFoundAttributeInConditionElement, fragment);
            }



            bool isInvert = false;
            ValidateResult result = new ValidateResult { Result = false };

            if (invertResultElement != null && Convert.ToBoolean(invertResultElement.Value))
            {
                isInvert = true;
            }

            //获取工作流资源用户权限中的实体关键字参数
            if (!parameters.TryGetValue(ConditionServiceParameterBaseNames.EntityKeyForEntityAttribute, out object objEntityKey))
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterFromConditionParametersByName,
                    DefaultFormatting = "在条件元素{0}的参数列表中，找不到名称为{1}的参数",
                    ReplaceParameters = new List<object>() { "entityattribute", ConditionServiceParameterBaseNames.EntityKeyForEntityAttribute }
                };

                throw new UtilityException((int)Errors.NotFoundParameterFromConditionParametersByName, fragment);
            }


            string entityKey;
            //判断是否参数是数组
            if (parameterIndexElement == null)
            {
                entityKey = objEntityKey as string;
                if (entityKey == null)
                {
                    fragment = new TextFragment()
                    {
                        Code = TextCodes.ParameterFromConditionParametersTypeNotMatchByName,
                        DefaultFormatting = "在条件元素{0}的参数列表中，名称为{1}的参数的期望类型为{2}，但实际类型为{3}",
                        ReplaceParameters = new List<object>() { "entityattribute", ConditionServiceParameterBaseNames.EntityKeyForEntityAttribute, typeof(string).FullName, objEntityKey.GetType().FullName }
                    };

                    throw new UtilityException((int)Errors.ParameterFromConditionParametersTypeNotMatchByName, fragment);
                }
            }
            else
            {
                int parameterIndex = Convert.ToInt32(parameterIndexElement.Value);

                if (!(objEntityKey is string[]))
                {
                    fragment = new TextFragment()
                    {
                        Code = TextCodes.ParameterFromConditionParametersTypeNotMatchByName,
                        DefaultFormatting = "在条件元素{0}的参数列表中，名称为{1}的参数的期望类型为{2}，但实际类型为{3}",
                        ReplaceParameters = new List<object>() { "entityattribute", ConditionServiceParameterBaseNames.EntityKeyForEntityAttribute, typeof(string[]).FullName, objEntityKey.GetType().FullName }
                    };

                    throw new UtilityException((int)Errors.ParameterFromConditionParametersTypeNotMatchByName, fragment);
                }
                var arrayEntityKey = objEntityKey as string[];
                if (parameterIndex > arrayEntityKey.Length - 1)
                {
                    fragment = new TextFragment()
                    {
                        Code = TextCodes.ParameterFromConditionParametersIndexOut,
                        DefaultFormatting = "在条件元素{0}的参数列表中，元素属性{1}的值{2}做为参数{3}的数组索引，索引越界，参数的数组长度为{4}",
                        ReplaceParameters = new List<object>() { "entityattribute", "parameterindex", parameterIndexElement.Value, ConditionServiceParameterBaseNames.EntityKeyForEntityAttribute, arrayEntityKey.Length }
                    };

                    throw new UtilityException((int)Errors.ParameterFromConditionParametersIndexOut, fragment);
                }

                entityKey = arrayEntityKey[parameterIndex];
            }

            //查询实体记录
            var entity=await _entityRepository.QueryByTypeAndKey(entityTypeElement.Value, entityKey);
            if (entity==null)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEntityByEntityTypeAndKey,
                    DefaultFormatting = "找不到实体类型为{0}，实体关键字为{1}的实体记录",
                    ReplaceParameters = new List<object>() { entityTypeElement.Value, entityKey }
                };

                throw new UtilityException((int)Errors.NotFoundEntityByEntityTypeAndKey, fragment);
            }

            object value = null;
            
            StringBuilder strAttribute = new StringBuilder();
            //获取属性值
            var attributeNameList= attributeNameElement.Value.Split('.').ToList();
            for(var index=0;index<=attributeNameList.Count-1;index++)
            {
                strAttribute.Append(attributeNameList[index]);

                if (index != attributeNameList.Count - 1)
                {
                    if (!entity.Attributes.TryGetValue(attributeNameList[index], out value) || value==null)
                    {
                        fragment = new TextFragment()
                        {
                            Code = TextCodes.EntityAttributeCanNotArrive,
                            DefaultFormatting = "实体类型为{0}、实体关键字为{1}的实体记录，无法获取属性{2}的值，因为当执行到属性{3}时，值为null",
                            ReplaceParameters = new List<object>() { entityTypeElement.Value, entityKey, attributeNameElement.Value, strAttribute.ToString() }
                        };

                        throw new UtilityException((int)Errors.EntityAttributeCanNotArrive, fragment);
                    }

                    if (!(value is ModelBase))
                    {
                        fragment = new TextFragment()
                        {
                            Code = TextCodes.EntityAttributeNotModelBase,
                            DefaultFormatting = "实体类型为{0}、实体关键字为{1}的实体记录，属性{2}的值的类型不是基于ModelBase，实际类型为{3}",
                            ReplaceParameters = new List<object>() { entityTypeElement.Value, entityKey, strAttribute.ToString(), value.GetType().FullName }
                        };

                        throw new UtilityException((int)Errors.EntityAttributeNotModelBase, fragment);
                    }

                    entity = (ModelBase)value;
                }
                else
                {
                    value = null;
                    if (entity.Attributes.TryGetValue(attributeNameList[index], out value))
                    {

                    }
                }
            }

            //根据不同的运算符执行运算
            var service = GetEntityAttributeValueValidateService(operatorElement.Value);

            string valueType = null;
            if (valueTypeElement!=null)
            {
                valueType = valueTypeElement.Value;
            }

            string checkValue = null;
            if (valueElement!=null)
            {
                checkValue= valueElement.Value;
            }

            EntityAttributeValueValidateServiceResult serviceResult=null;

            try
            {
                serviceResult = await service.Validate(entityTypeElement.Value, entityKey, attributeNameElement.Value, value, valueType, checkValue);
            }
            catch(UtilityException ex)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.ConditionElementError,
                    DefaultFormatting = "条件元素{0}执行发生错误，详细信息：{1}",
                    ReplaceParameters = new List<object>() { securityElement.Value, ((UtilityException)ex).Fragment }
                };

                throw new UtilityException((int)Errors.ConditionElementError, fragment);
            }

            result.Result = serviceResult.Result;

            if (isInvert)
            {
                result.Result = !result.Result;
            }

            if (!result.Result)
            {
                if (isInvert)
                {
                    result.Description = serviceResult.CorrectText;
                }
                else
                {
                    result.Description = serviceResult.ErrorText;
                }
            }

            return result;
        }

        private IEntityAttributeValueValidateService GetEntityAttributeValueValidateService(string strOperator)
        {
            if (!_entityAttributeValueValidateServiceFactories.TryGetValue(strOperator, out IFactory<IEntityAttributeValueValidateService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEntityAttributeValueValidateServiceByOperator,
                    DefaultFormatting = "找不到操作符为{0}的EntityAttributeValueValidateService，位置为{1}",
                    ReplaceParameters = new List<object>() { strOperator, "MSLibrary.Security.ConditionController.ElementConditionServices.ElementConditionServiceForEntityAttribute" }
                };

                throw new UtilityException((int)Errors.NotFoundEntityAttributeValueValidateServiceByOperator, fragment);
            }

            return serviceFactory.Create();
        }
    }



    /// <summary>
    /// 实体属性值检查服务
    /// </summary>
    public interface IEntityAttributeValueValidateService
    {
        /// <summary>
        /// 执行检查
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityKey">实体关键字</param>
        /// <param name="attributeName">实体属性名称</param>
        /// <param name="attributeValue">实体属性的值</param>
        /// <param name="valueType">属性值的类型</param>
        /// <param name="checkValue">要和实体属性的值相比较的值</param>
        /// <returns></returns>
        Task<EntityAttributeValueValidateServiceResult> Validate(string entityType,string entityKey,string attributeName,object attributeValue,string valueType,string checkValue);
    }

    /// <summary>
    /// 实体属性值检查服务结果
    /// </summary>
    public class EntityAttributeValueValidateServiceResult
    {
        private string _correctText;
        private string _errorText;
        private bool _result;

        public EntityAttributeValueValidateServiceResult(string correctText, string errorText, bool result)
        {
            _correctText = correctText;
            _errorText = errorText;
            _result = result;
        }

        /// <summary>
        /// 检查正确时显示的文本
        /// </summary>
        public string CorrectText
        {
            get
            {
                return _correctText;
            }
        }

        /// <summary>
        /// 检查错误时显示的文本
        /// </summary>
        public string ErrorText
        {
            get
            {
                return _errorText;
            }
        }

        /// <summary>
        /// 检查结果是否正确
        /// </summary>
        public bool Result
        {
            get
            {
                return _result;
            }
        }
    }


    

}
