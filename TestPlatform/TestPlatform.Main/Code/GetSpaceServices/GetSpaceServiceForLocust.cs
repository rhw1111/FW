using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Code.GetSpaceServices
{
    [Injection(InterfaceType = typeof(GetSpaceServiceForLocust), Scope = InjectionScope.Singleton)]
    public class GetSpaceServiceForLocust : IGetSpaceService
    {
        public async Task<string> GetSpace(int n)
        {
            string result = $" ";
            result = this.RepeatString(result, n);

            return await Task.FromResult(result);
        }

        private string RepeatString(string str, int n)
        {
            char[] arr = str.ToCharArray();
            char[] arrDest = new char[arr.Length * n];

            for (int i = 0; i < n; i++)
            {
                Buffer.BlockCopy(arr, 0, arrDest, i * arr.Length * 2, arr.Length * 2);
            }

            return new string(arrDest);
        }
    }
}
