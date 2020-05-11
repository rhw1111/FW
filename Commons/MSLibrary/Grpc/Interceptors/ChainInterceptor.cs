using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace MSLibrary.Grpc.Interceptors
{
    /// <summary>
    /// 链式拦截器
    /// </summary>
    public class ChainInterceptor : Interceptor
    {
        private IList<Interceptor> _filterList;

        public ChainInterceptor(IList<Interceptor> filterList)
        {
            _filterList = filterList.Reverse().ToList();
        }

        public IList<Interceptor> Filters
        {
            get
            {
                return _filterList;
            }
        }

        public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            var innerContinuation = continuation;
            foreach (var filterItem in _filterList)
            {
                innerContinuation = getAsyncClientStreamingCallChain(filterItem, innerContinuation);
            }

            return base.AsyncClientStreamingCall(context, innerContinuation);
        }

        public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            var innerContinuation = continuation;
            foreach (var filterItem in _filterList)
            {
                innerContinuation = getAsyncDuplexStreamingCallChain(filterItem, innerContinuation);
            }

            return base.AsyncDuplexStreamingCall(context, innerContinuation);
        }

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var innerContinuation = continuation;
            foreach (var filterItem in _filterList)
            {
                innerContinuation = getUnaryServerHandlerChain(filterItem, innerContinuation);
            }

            return base.UnaryServerHandler(request, context, innerContinuation);
        }

        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            var innerContinuation = continuation;
            foreach (var filterItem in _filterList)
            {
                innerContinuation = getAsyncServerStreamingCallChain(filterItem, innerContinuation);
            }

            return base.AsyncServerStreamingCall(request, context, innerContinuation);
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var innerContinuation = continuation;
            foreach (var filterItem in _filterList)
            {
                innerContinuation = getAsyncUnaryCallChain(filterItem, innerContinuation);
            }

            return base.AsyncUnaryCall(request, context, innerContinuation);
        }

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var innerContinuation = continuation;
            foreach (var filterItem in _filterList)
            {
                innerContinuation = getBlockingUnaryCallChain(filterItem, innerContinuation);
            }
            return base.BlockingUnaryCall(request, context, innerContinuation);
        }

        public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            var innerContinuation = continuation;
            foreach (var filterItem in _filterList)
            {
                innerContinuation = getClientStreamingServerHandlerChain(filterItem, innerContinuation);
            }

            return base.ClientStreamingServerHandler(requestStream, context, innerContinuation);
        }

        public override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            var innerContinuation = continuation;
            foreach (var filterItem in _filterList)
            {
                innerContinuation = getDuplexStreamingServerHandlerChain(filterItem, innerContinuation);
            }

            return base.DuplexStreamingServerHandler(requestStream, responseStream, context, innerContinuation);
        }


        public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {

            var innerContinuation = continuation;
            foreach (var filterItem in _filterList)
            {
                innerContinuation = getServerStreamingServerHandlerChain(filterItem, innerContinuation);
            }
            return base.ServerStreamingServerHandler(request, responseStream, context, innerContinuation);
        }




        private AsyncClientStreamingCallContinuation<TRequest, TResponse> getAsyncClientStreamingCallChain<TRequest, TResponse>(Interceptor interceptor, AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    where TRequest : class
    where TResponse : class
        {
            AsyncClientStreamingCallContinuation<TRequest, TResponse> fun = (fContext) =>
            {
                return interceptor.AsyncClientStreamingCall(fContext, continuation);
            };

            return fun;
        }

        private AsyncDuplexStreamingCallContinuation<TRequest, TResponse> getAsyncDuplexStreamingCallChain<TRequest, TResponse>(Interceptor interceptor, AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
    where TRequest : class
    where TResponse : class
        {
            AsyncDuplexStreamingCallContinuation<TRequest, TResponse> fun = (fContext) =>
            {
                return interceptor.AsyncDuplexStreamingCall(fContext, continuation);
            };

            return fun;
        }


        private UnaryServerMethod<TRequest, TResponse> getUnaryServerHandlerChain<TRequest, TResponse>(Interceptor interceptor, UnaryServerMethod<TRequest, TResponse> continuation)
            where TRequest : class
            where TResponse : class
        {
            UnaryServerMethod<TRequest, TResponse> fun = (fRequest, fContext) =>
            {
                return interceptor.UnaryServerHandler(fRequest, fContext, continuation);
            };

            return fun;
        }



        private AsyncServerStreamingCallContinuation<TRequest, TResponse> getAsyncServerStreamingCallChain<TRequest, TResponse>(Interceptor interceptor, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    where TRequest : class
    where TResponse : class
        {
            AsyncServerStreamingCallContinuation<TRequest, TResponse> fun = (fRequest, fContext) =>
            {
                return interceptor.AsyncServerStreamingCall(fRequest, fContext, continuation);
            };

            return fun;
        }



        private AsyncUnaryCallContinuation<TRequest, TResponse> getAsyncUnaryCallChain<TRequest, TResponse>(Interceptor interceptor, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
where TRequest : class
where TResponse : class
        {
            AsyncUnaryCallContinuation<TRequest, TResponse> fun = (fRequest, fContext) =>
            {
                return interceptor.AsyncUnaryCall(fRequest, fContext, continuation);
            };

            return fun;
        }

        private BlockingUnaryCallContinuation<TRequest, TResponse> getBlockingUnaryCallChain<TRequest, TResponse>(Interceptor interceptor, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
where TRequest : class
where TResponse : class
        {
            BlockingUnaryCallContinuation<TRequest, TResponse> fun = (fRequest, fContext) =>
            {
                return interceptor.BlockingUnaryCall(fRequest, fContext, continuation);
            };

            return fun;
        }

        private ClientStreamingServerMethod<TRequest, TResponse> getClientStreamingServerHandlerChain<TRequest, TResponse>(Interceptor interceptor, ClientStreamingServerMethod<TRequest, TResponse> continuation)
where TRequest : class
where TResponse : class
        {
            ClientStreamingServerMethod<TRequest, TResponse> fun = (fRequest, fContext) =>
            {
                return interceptor.ClientStreamingServerHandler(fRequest, fContext, continuation);
            };

            return fun;
        }


        private DuplexStreamingServerMethod<TRequest, TResponse> getDuplexStreamingServerHandlerChain<TRequest, TResponse>(Interceptor interceptor, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
where TRequest : class
where TResponse : class
        {
            DuplexStreamingServerMethod<TRequest, TResponse> fun = (fRequest, fResponse, fContext) =>
            {
                return interceptor.DuplexStreamingServerHandler(fRequest, fResponse, fContext, continuation);
            };

            return fun;
        }


        private ServerStreamingServerMethod<TRequest, TResponse> getServerStreamingServerHandlerChain<TRequest, TResponse>(Interceptor interceptor, ServerStreamingServerMethod<TRequest, TResponse> continuation)
where TRequest : class
where TResponse : class
        {
            ServerStreamingServerMethod<TRequest, TResponse> fun = (fRequest, fResponse, fContext) =>
            {
                return interceptor.ServerStreamingServerHandler(fRequest, fResponse, fContext, continuation);
            };

            return fun;
        }


    }
}
