using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace MSLibrary.Grpc.Interceptors
{
    public static class ServerCallContextExtension
    {
        private const string _onceStateName = "once";

        public static async Task OnceRun(this ServerCallContext context, string name,Func<Task> action)
        {
            Dictionary<string, bool> onceDict;
            if (!context.UserState.ContainsKey(_onceStateName))
            {
                onceDict = new Dictionary<string, bool>();
                context.UserState[_onceStateName] = onceDict;
            }
            else
            {
                onceDict = (Dictionary<string, bool>)context.UserState[_onceStateName];
            }

            if (!onceDict.ContainsKey(name))
            {
                await action();
                onceDict[name] = true;
            }
        }
    }
}
