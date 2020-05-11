using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Runtime.Serialization;
using ExcelDataReader;
using ExcelDataReader.Core;
using MSLibrary.DI;
using MSLibrary.Serializer;

namespace MSLibrary.DataManagement.MatrixDataProviderServices
{
    /// <summary>
    /// 针对通用Xlsx类型数据文件的提供方
    /// configuration格式为
    /// {
    ///     "FilePath":"文件带路径名称"，
    ///     "HasTitle":"第一行是否是标题"，
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(MatrixDataProviderServiceForCommonXlsx), Scope = InjectionScope.Singleton)]
    public class MatrixDataProviderServiceForCommonXlsx:IMatrixDataProviderService
    {
        public async Task ExecuteAll(string configuration, int skip, int size, Func<List<MatrixDataRow>, Task<bool>> execute)
        {
            List<MatrixDataRow> rowList = new List<MatrixDataRow>();
            CommonXlsxConfiguration commonXlsxConfiguration = JsonSerializerHelper.Deserialize<CommonXlsxConfiguration>(configuration);
            using (FileStream stream = new FileStream(commonXlsxConfiguration.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                {
                    DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = commonXlsxConfiguration.HasTitle
                        }
                    });
                    var columnCount = excelReader.FieldCount;


                    while (true)
                    {
                        for (var index = 0; index <= size - 1; index++)
                        {
                            if (skip + index > excelReader.RowCount - 1)
                            {
                                break;
                            }
                            var dataRow = result.Tables[0].Rows[skip + index];
                            var newRow = new MatrixDataRow();
                            for (var columnIndex = 0; columnIndex <= columnCount - 1; columnIndex++)
                            {
                                newRow.Columns.Add(new MatrixDataColumn() { Data = dataRow[columnIndex].ToString() });
                            }
                            rowList.Add(newRow);
                        }

                        var executeResult=await execute(rowList);
                        if (!executeResult)
                        {
                            break;
                        }

                        if (rowList.Count<size)
                        {
                            break;
                        }

                        skip = skip + rowList.Count;
                    }
                
                }

                stream.Close();
            }


        }

        [DataContract]
        private class CommonXlsxConfiguration
        {
            [DataMember]
            public string FilePath { get; set; }
            [DataMember]
            public bool HasTitle { get; set; }
        }
    }
}
