using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Extensions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Context.Application;
using MSLibrary.Logger;
using MSLibrary.AspNet.Middleware.Application;
using MSLibrary.Grpc.Interceptors.Application;
using MSLibrary.Grpc.Context.Application;

namespace MSLibrary.Grpc.Interceptors
{
    public class GrpcExtensionContext : Interceptor
    {
        private const string _grpcContextItemName = "ExtensionContextInits";

        private string _name;
        private string _errorCatalogName;
        private ILoggerFactory _loggerFactory;
        private IAppGrpcExtensionContextExecute _appGrpcExtensionContextExecute;

        public GrpcExtensionContext(string name, string errorCatalogName, ILoggerFactory loggerFactory, IAppGrpcExtensionContextExecute appGrpcExtensionContextExecute)
        {
            _name = name;
            _errorCatalogName = errorCatalogName;
            _loggerFactory = loggerFactory;
            _appGrpcExtensionContextExecute = appGrpcExtensionContextExecute;
        }

        public async override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            TResponse response = null!;
            await execute(context,async()=>
            {
                response=await continuation(request, context);
            });
            return response;
        }

        public async override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            TResponse response = null!;
            await execute(context,async()=>
            {
                response= await continuation(requestStream, context);
            });
            return response;
        }

        public async override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            await execute(context,async()=>
            {
                await continuation(request, responseStream, context);
            });
       
            
        }

        public async override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            await execute(context,async()=>
            {
                await continuation(requestStream, responseStream, context); 
            });
                     
        }



        private async Task execute(ServerCallContext context,Func<Task> action)
        {
            var contextInit = await _appGrpcExtensionContextExecute.Do(context, _name);
            contextInit.Execute();
            Dictionary<string, IGrpcExtensionContextInit> contextInits;
            if (!context.UserState.TryGetValue(_grpcContextItemName, out object? contextInitsObj))
            {
                contextInits = new Dictionary<string, IGrpcExtensionContextInit>();
                context.UserState[_grpcContextItemName] = contextInits;
            }
            else
            {
                contextInits = (Dictionary<string, IGrpcExtensionContextInit>)contextInitsObj;
            }

            contextInits[_name] = contextInit;
            await action();
        }

    }
}
