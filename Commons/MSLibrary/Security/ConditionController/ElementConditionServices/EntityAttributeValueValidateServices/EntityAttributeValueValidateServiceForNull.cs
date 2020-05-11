using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Security.ConditionController.ElementConditionServices.EntityAttributeValueValidateServices
{
    /// <summary>
    /// 针对Null运算符的实体属性值检查服务
    /// </summary>
    [Injection(InterfaceType = typeof(EntityAttributeValueValidateServiceForNull), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueValidateServiceForNull : IEntityAttributeValueValidateService
    {
        public async Task<EntityAttributeValueValidateServiceResult> Validate(string entityType, string entityKey, string attributeName, object attributeValue, string valueType, string checkValue)
        {

            string correctText = string.Format(StringLanguageTranslate.Translate(TextCodes.EntityAttributeValueValidateOperatorNullErrorText, "实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值不为Null"), entityType, entityKey, attributeName);
            string errorText = string.Format(StringLanguageTranslate.Translate(TextCodes.EntityAttributeValueValidateOperatorNullCorrectText, "实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值为Null"), entityType, entityKey, attributeName);

            //如果属性值为null，返回true
            if (attributeValue == null)
            {
                return await Task.FromResult(new EntityAttributeValueValidateServiceResult(correctText, errorText, true));
            }
            else
            {
                return await Task.FromResult(new EntityAttributeValueValidateServiceResult(correctText, errorText, false));
            }

        }
    }
}
