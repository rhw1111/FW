using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;

namespace MSLibrary.ExceptionHandle.CheckHandles
{
    [Injection(InterfaceType = typeof(ExceptionRetryCheckHandleForSqlserver), Scope = InjectionScope.Singleton)]
    public class ExceptionRetryCheckHandleForSqlserver : IExceptionRetryCheckHandle
    {
        public async Task<bool> Check(Exception ex)
        {
            return await Task.FromResult(CheckSync(ex));
        }

        public bool CheckSync(Exception ex)
        {
            SqlException sqlEx = (SqlException)ex;

            if (sqlEx.Number == 41302 || sqlEx.Number == 41305 || sqlEx.Number == 41325 || sqlEx.Number == 41301 || sqlEx.Number == 1205)
            {
                return true;
            }

            return false;
        }
    }
}
