using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MSLibrary.Serializer;
using MSLibrary.DI;

namespace MSLibrary.DataManagement.DAL
{
    [Injection(InterfaceType = typeof(IMatrixDataHandlerStore), Scope = InjectionScope.Singleton)]
    public class MatrixDataHandlerStoreForConfigurationFile:IMatrixDataHandlerStore
    {
        private static Dictionary<string, MatrixDataHandler> _datas;
        static MatrixDataHandlerStoreForConfigurationFile()
        {
            var strDatas= File.ReadAllText($"Configurations{Path.PathSeparator}MatrixDataHandlerData.json");
            _datas=JsonSerializerHelper.Deserialize<Dictionary<string, MatrixDataHandler>>(strDatas);
        }
        public async Task<MatrixDataHandler> QueryByName(string name)
        {
            if (!_datas.TryGetValue(name,out MatrixDataHandler data))
            {
                data = null;
            }

            return await Task.FromResult(data);
        }
    }

}
