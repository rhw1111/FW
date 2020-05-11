using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.DataManagement
{
    [Injection(InterfaceType = typeof(IMatrixDataTypeConvertService), Scope = InjectionScope.Singleton)]
    public class MatrixDataTypeConvertMainService:IMatrixDataTypeConvertService
    {
        private static Dictionary<string, IFactory<IMatrixDataTypeConvertService>> _matrixDataTypeConvertServiceFactories = new Dictionary<string, IFactory<IMatrixDataTypeConvertService>>();

        public static Dictionary<string, IFactory<IMatrixDataTypeConvertService>> MatrixDataTypeConvertServiceFactories
        {
            get
            {
                return _matrixDataTypeConvertServiceFactories;
            }
        }
        public async Task<object> Convert(string type,object value)
        {
            if (!_matrixDataTypeConvertServiceFactories.TryGetValue(type,out IFactory<IMatrixDataTypeConvertService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundMatrixDataTypeConvertServiceByType,
                    DefaultFormatting = "找不到转换类型为{0}的矩阵数据类型转换服务，发生位置：{1}",
                    ReplaceParameters = new List<object>() { type, $"{this.GetType().FullName}.MatrixDataTypeConvertServiceFactories" }
                };
                throw new UtilityException((int)Errors.NotFoundMatrixDataTypeConvertServiceByType, fragment);
            }

            return await serviceFactory.Create().Convert(type, value);
        }
    }

}
