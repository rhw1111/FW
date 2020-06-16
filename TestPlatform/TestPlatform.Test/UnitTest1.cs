using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Unicode;
using System.Linq;
using System.Threading.Tasks;
using MSLibrary.Transaction;
using System.Threading;

namespace TestPlatform.Test
{
    public class Tests
    {
        private static AsyncLocal<Dictionary<string, string>> _connections = new AsyncLocal<Dictionary<string, string>>();
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {



            //IAsyncEnumerable<object> asyncEnumer;


            //var bytes=Convert.FromBase64String("XExBYS06nWWTysT9lzOTnw==");

            //var str= UTF8Encoding.UTF32.GetString(bytes);
            //var str = Convert.ToBase64String(bytes);
            //bytes = Convert.FromBase64String(str);
            //str = UTF8Encoding.UTF8.GetString(bytes);

            await using (var scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { }))
            {

                scope.Complete();
            }

            var aa=DBTransactionScope.InScope();

                Assert.Pass();
        }

        private async Task Do1()
        {
            _connections.Value.Add("1", "1");
            await Task.FromResult(0);
        }

        private async Task Do2()
        {
           var dict= _connections.Value;
            await Task.FromResult(0);
        }
    }
}