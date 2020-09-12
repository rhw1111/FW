using System;
using System.Collections.Generic;
using System.Text;
using Grpc.Core;

namespace MSLibrary.Grpc
{
    /// <summary>
    /// 系统错误类型与Grpc状态码映射关系
    /// </summary>
    public class UtilityExceptionTypeGrpcStatusCodeMappings
    {
        /// <summary>
        /// 错误类型与StatusCode的映射关系
        /// 键为错误类型，值为StatusCode
        /// </summary>
        public static IDictionary<int, StatusCode> Mappings { get; } = new Dictionary<int, StatusCode>()
        {
            {0,StatusCode.Internal },
            { 1, StatusCode.Unauthenticated},
            { 2, StatusCode.PermissionDenied}
        };
    }
}
