using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MongoDB.Driver;

namespace MSLibrary.ExceptionHandle.CheckHandles
{
    [Injection(InterfaceType = typeof(ExceptionRetryCheckHandleForMongoException), Scope = InjectionScope.Singleton)]
    public class ExceptionRetryCheckHandleForMongoException : IExceptionRetryCheckHandle
    {
        public async Task<bool> Check(Exception ex)
        {
            return await Task.FromResult(CheckSync(ex));
        }

        public bool CheckSync(Exception ex)
        {
            MongoException realEx = (MongoException)ex;

            if (realEx.HasErrorLabel("UnknownTransactionCommitResult") || realEx.HasErrorLabel("TransientTransactionError"))
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
