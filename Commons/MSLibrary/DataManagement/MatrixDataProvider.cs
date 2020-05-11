using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.DataManagement
{

    /// <summary>
    /// 矩阵数据提供方
    /// </summary>
    public class MatrixDataProvider:EntityBase<IMatrixDataProviderIMP>
    {
        private static IFactory<IMatrixDataProviderIMP> _matrixDataProviderIMPFactory;

        public static IFactory<IMatrixDataProviderIMP> MatrixDataProviderIMPFactory
        {
            set
            {
                _matrixDataProviderIMPFactory = value;
            }
        }


        public override IFactory<IMatrixDataProviderIMP> GetIMPFactory()
        {
            return _matrixDataProviderIMPFactory;
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
        /// 类型
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
        /// 配置
        /// </summary>
        public string Configuration
        {
            get
            {
                return GetAttribute<string>("Configuration");
            }
            set
            {
                SetAttribute<string>("Configuration", value);
            }
        }


        /// <summary>
        /// 列中的数据类型映射集合
        /// 列表的index对应行中的每个列
        /// </summary>
        public List<string> TypeMapping
        {
            get
            {
                return GetAttribute<List<string>>("TypeMapping");
            }
            set
            {
                SetAttribute<List<string>>("TypeMapping", value);
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

        public async Task ExecuteAll(int skip, int size, Func<List<MatrixDataRow>, Task<bool>> execute)
        {
            await _imp.ExecuteAll(this, skip, size, execute);
        }
    }

    public interface IMatrixDataProviderIMP
    {
        /// <summary>
        /// 处理所有矩阵行，跳过skip数量的行，每次获取指定size的数据
        /// 当execute返回false时，中止取数操作
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="configuration"></param>
        /// <param name="skip"></param>
        /// <param name="size"></param>
        /// <param name="execute"></param>
        /// <returns></returns>
        Task ExecuteAll(MatrixDataProvider provider,int skip, int size, Func<List<MatrixDataRow>, Task<bool>> execute);
    }


    [Injection(InterfaceType = typeof(IMatrixDataProviderIMP), Scope = InjectionScope.Transient)]
    public class MatrixDataProviderIMP : IMatrixDataProviderIMP
    {
        private static Dictionary<string, IFactory<IMatrixDataProviderService>> _providerServiceFactories = new Dictionary<string, IFactory<IMatrixDataProviderService>>();

        public static IDictionary<string, IFactory<IMatrixDataProviderService>> ProviderServiceFactories
        {
            get
            {
                return _providerServiceFactories;
            }
        }

        private IMatrixDataTypeConvertService _matrixDataTypeConvertService;

        public MatrixDataProviderIMP(IMatrixDataTypeConvertService matrixDataTypeConvertService)
        {
            _matrixDataTypeConvertService = matrixDataTypeConvertService;
        }

        public async Task ExecuteAll(MatrixDataProvider provider, int skip, int size, Func<List<MatrixDataRow>, Task<bool>> execute)
        {
            if (!_providerServiceFactories.TryGetValue(provider.Type,out IFactory<IMatrixDataProviderService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundMatrixDataProviderServiceByType,
                    DefaultFormatting = "找不到类型为{0}的矩阵数据提供方服务，发生位置：{1}",
                    ReplaceParameters = new List<object>() { provider.Type, $"{this.GetType().FullName}.ProviderServiceFactories" }
                };
                throw new UtilityException((int)Errors.NotFoundMatrixDataProviderServiceByType, fragment);
            }

            await serviceFactory.Create().ExecuteAll(provider.Configuration, skip, size, async(rowList)=>
            {
                foreach(var rowItem in rowList)
                {
                    if (rowItem.Columns.Count > provider.TypeMapping.Count)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.MatrixDataRowColumnCountOutTypeMappingCount,
                            DefaultFormatting = "矩阵行的列数为{0}，而该矩阵数据提供方{1}的类型映射配置长度为{2}，两者不匹配，发生位置：{3}",
                            ReplaceParameters = new List<object>() { rowItem.Columns.Count.ToString(), provider.Name, provider.TypeMapping.Count.ToString(), $"{this.GetType().FullName}.ExecuteAll" }
                        };
                        throw new UtilityException((int)Errors.MatrixDataRowColumnCountOutTypeMappingCount, fragment);
                    }

                    for (var index=0;index<= rowItem.Columns.Count-1;index++)
                    {

                        rowItem.Columns[index].Data= await _matrixDataTypeConvertService.Convert(provider.TypeMapping[index], rowItem.Columns[index].Data);                 
                    }                  
                }

                return await execute(rowList);
            });
        }
    }


    public interface IMatrixDataProviderService
    {
        Task ExecuteAll(string configuration, int skip, int size, Func<List<MatrixDataRow>, Task<bool>> execute);
    }

    public class MatrixDataRow
    {
        public List<MatrixDataColumn> Columns = new List<MatrixDataColumn>();
    }

    public class MatrixDataColumn
    {
        public object Data { get; set; }
    }

 
}
