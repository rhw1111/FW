using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateDataSourceFuncServices
{
    public class GenerateDataSourceFuncServiceForLocustJson : IGenerateDataSourceFuncService
    {
        public async Task<string> Generate(string funcUniqueName, string data)
        {
            //throw new NotImplementedException();

            // 格式:{$datasourcefunc()}

            string code = data;

            return code;
        }
    }
}
