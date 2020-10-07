using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Grpc
{
    public enum GrpcErrorCodes
    {
        /// <summary>
        /// 找不到指定名称的Grpc请求扩展上下文处理服务工厂
        /// </summary>
        NotFountGrpcExtensionContextHandleServiceFactoryByName = 324610001,
        /// <summary>
        /// 找不到指定名称的Grpc客户端拦截器描述
        /// </summary>
        NotFoundGrpcClientInterceptorDescriptionByName = 324610002,
        /// <summary>
        /// 找不到指定名称的证书生成服务
        /// </summary>
        NotFoundCertificateGenerateServiceByName = 324610003,
    }
}
