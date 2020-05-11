using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MSLibrary.DI;

namespace MSLibrary.ExceptionHandle.CheckHandles
{
    [Injection(InterfaceType = typeof(ExceptionRetryCheckHandleForDbUpdateConcurrencyException), Scope = InjectionScope.Singleton)]
    public class ExceptionRetryCheckHandleForDbUpdateConcurrencyException : IExceptionRetryCheckHandle
    {
        public async Task<bool> Check(Exception ex)
        {
            return await Task.FromResult(CheckSync(ex));
        }

        public bool CheckSync(Exception ex)
        {
            DbUpdateConcurrencyException realEx = (DbUpdateConcurrencyException)ex;
            var error = ex.InnerException as SqlException;
            if (error == null)
            {
                return false;
            }

            if (error.Number == 41302 || error.Number == 41305 || error.Number == 41325 || error.Number == 41301 || error.Number == 1205)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
