using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Thread;

namespace MSLibrary.DataManagement
{
    /// <summary>
    /// 矩阵数据处理方
    /// </summary>
    public class MatrixDataHandler:EntityBase<IMatrixDataHandlerIMP>
    {
        private static IFactory<IMatrixDataHandlerIMP> _matrixDataHandlerIMPFactory;

        public static IFactory<IMatrixDataHandlerIMP> MatrixDataHandlerIMPFactory
        {
            set
            {
                _matrixDataHandlerIMPFactory = value;
            }
        }


        public override IFactory<IMatrixDataHandlerIMP> GetIMPFactory()
        {
            return _matrixDataHandlerIMPFactory;
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
        /// 数据提供方
        /// </summary>
        public MatrixDataProvider Provider
        {
            get
            {
                return GetAttribute<MatrixDataProvider>("Provider");
            }
            set
            {
                SetAttribute<MatrixDataProvider>("Provider", value);
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
    }

    public interface IMatrixDataHandlerIMP
    {
        Task Execute(MatrixDataHandler handler,int skip,int size, int paralleNumber, Func<MatrixDataRow,int,Task> rowCompleteAction, Func<MatrixDataRow, int,Exception, Task> rowExceptionAction);
    }

    [Injection(InterfaceType = typeof(IMatrixDataHandlerIMP), Scope = InjectionScope.Transient)]
    public class MatrixDataHandlerIMP: IMatrixDataHandlerIMP
    {
        private static Dictionary<string, IFactory<IMatrixDataHandlerService>> _matrixDataHandlerServiceFactories = new Dictionary<string, IFactory<IMatrixDataHandlerService>>();

        public static IDictionary<string, IFactory<IMatrixDataHandlerService>> MatrixDataHandlerServiceFactories
        {
            get
            {
                return _matrixDataHandlerServiceFactories;
            }

        }
        public async Task Execute(MatrixDataHandler handler,int skip, int size,int paralleNumber, Func<MatrixDataRow, int, Task> rowCompleteAction, Func<MatrixDataRow, int, Exception, Task> rowExceptionAction)
        {
            if (!_matrixDataHandlerServiceFactories.TryGetValue(handler.Type, out IFactory<IMatrixDataHandlerService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundMatrixDataHandlerServiceByType,
                    DefaultFormatting = "找不到类型为{0}的矩阵数据处理方服务，发生位置：{1}",
                    ReplaceParameters = new List<object>() { handler.Type, $"{this.GetType().FullName}.MatrixDataHandlerServiceFactories" }
                };
                throw new UtilityException((int)Errors.NotFoundMatrixDataHandlerServiceByType, fragment);
            }

            var service=serviceFactory.Create();
            MatrixDataHandlerContext context = new MatrixDataHandlerContext();
            await service.PreExecute(context);
            await ParallelHelper.RunCircle(paralleNumber, async (int index) =>
             {
                 int start = index * size + skip;
                 bool result = true;
                 await handler.Provider.ExecuteAll(start, size, async (rowList) =>
                    {
                        if (rowList.Count<size)
                        {
                            result = false;
                        }

                        if (rowList.Count>0)
                        {
                            for(var i=0;i<= rowList.Count-1;i++)
                            {
                                start++;
                                bool executeResult = true;
                                try
                                {
                                    await service.Execute(context, handler.Configuration, rowList[i]);
                                }
                                catch(Exception ex)
                                {
                                    executeResult = false;
                                    await rowExceptionAction(rowList[i], start, ex);
                                }

                                if (executeResult)
                                {
                                    await rowCompleteAction(rowList[i], start);
                                }
                            }                        
                        }

                        return false;
                    });

                 return await Task.FromResult(result);

             });

            await service.PostExecute(context);
        }
    }


    /// <summary>
    /// 矩阵数据处理服务
    /// </summary>
    public interface IMatrixDataHandlerService
    {
        /// <summary>
        /// 处理前被调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task PreExecute(MatrixDataHandlerContext context);
        /// <summary>
        /// 每行矩阵数据并行调用该方法执行处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="handlerConfiguration"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        Task<bool> Execute(MatrixDataHandlerContext context,string handlerConfiguration,MatrixDataRow row);
        /// <summary>
        /// 处理后被调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task PostExecute(MatrixDataHandlerContext context);
    }

    public class MatrixDataHandlerContext
    {
        public IDictionary<string, object> Datas { get; } = new ConcurrentDictionary<string, object>();
    }
}
