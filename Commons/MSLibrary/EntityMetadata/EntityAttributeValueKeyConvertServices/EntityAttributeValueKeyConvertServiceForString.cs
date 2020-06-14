using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.EntityMetadata.EntityAttributeValueKeyConvertServices
{
    /// <summary>
    /// 针对字符串类型的实体属性值转换成关键字字符串服务
    /// </summary>
    [Injection(InterfaceType = typeof(EntityAttributeValueKeyConvertServiceForString), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueKeyConvertServiceForString : IEntityAttributeValueKeyConvertService
    {
        public async Task<object> ConvertFrom(EntityAttributeInfo attributeInfo, string strKey)
        {
            if (strKey == null)
            {
                return await Task.FromResult<string>(null);
            }

            if (attributeInfo.Type != EntityAttributeTypes.String)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.EntityAttributeMetadataValueTypeNotMatchEntityAttributeValueKeyConvertServiceRequire,
                    DefaultFormatting = "实体类型为{0}的实体元数据的属性{1}的值类型为{2}，但实体属性值转换成关键字字符串服务{3}期待的类型为{4}，两者不匹配,发生位置为{5}",
                    ReplaceParameters = new List<object>() { attributeInfo.EntityInfo.EntityType, attributeInfo.Name, attributeInfo.Type, typeof(EntityAttributeValueKeyConvertServiceForString).FullName, typeof(string).FullName, $"{typeof(EntityAttributeValueKeyConvertServiceForString).FullName},ConvertFrom" }
                };

                throw new UtilityException((int)Errors.EntityAttributeMetadataValueTypeNotMatchEntityAttributeValueKeyConvertServiceRequire, fragment);
            }

            return await Task.FromResult(strKey);

        }

        public async Task<string> ConvertTo(EntityAttributeInfo attributeInfo, object value)
        {
            if (value==null)
            {
                return await Task.FromResult(string.Empty);
            }
            if (!(value is string))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.EntityAttributeMetadataValueTypeNotMatchEntityAttributeValueKeyConvertService,
                    DefaultFormatting = "实体类型为{0}的实体元数据的属性{1}的值的实际类型为{2}，但实体属性值转换成关键字字符串服务{3}期待的类型为{4}，两者不匹配，发生位置为{5}",
                    ReplaceParameters = new List<object>() { attributeInfo.EntityInfo.EntityType, attributeInfo.Name, value.GetType().FullName, typeof(EntityAttributeValueKeyConvertServiceForString).FullName, typeof(string).FullName, $"{typeof(EntityAttributeValueKeyConvertServiceForString).FullName},ConvertTo" }
                };

                throw new UtilityException((int)Errors.EntityAttributeMetadataValueTypeNotMatchEntityAttributeValueKeyConvertService, fragment);
            }

            if (attributeInfo.Type!=EntityAttributeTypes.String)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.EntityAttributeMetadataValueTypeNotMatchActual,
                    DefaultFormatting = "实体类型为{0}的实体元数据的属性{1}的值类型为{2}，但实际值类型为{3}，两者不匹配，发生位置为{4}",
                    ReplaceParameters = new List<object>() { attributeInfo.EntityInfo.EntityType, attributeInfo.Name, attributeInfo.Type, value.GetType().FullName, $"{typeof(EntityAttributeValueKeyConvertServiceForString).FullName},ConvertTo" }
                };

                throw new UtilityException((int)Errors.EntityAttributeMetadataValueTypeNotMatchActual, fragment);
            }

            return await Task.FromResult((string)value);
        }
    }
}
