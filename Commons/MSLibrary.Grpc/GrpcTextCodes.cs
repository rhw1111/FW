using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MSLibrary.Grpc
{
    public static class GrpcTextCodes
    {
        /// <summary>
        /// 找不到指定名称的Grpc请求扩展上下文处理服务工厂
        /// 格式为“找不到名称为{0}的Grpc请求扩展上下文处理服务工厂，发生位置为{1}”
        /// {0}:名称
        /// {1}:发生的位置
        /// </summary>
        public const string NotFountGrpcExtensionContextHandleServiceFactoryByName = "NotFountGrpcExtensionContextHandleServiceFactoryByName";
        /// <summary>
        /// 找不到指定名称的Grpc客户端拦截器描述
        /// 格式为“找不到名称为{0}的Grpc客户端拦截器描述，发生位置为{1}”
        /// {0}：名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundGrpcClientInterceptorDescriptionByName = "NotFoundGrpcClientInterceptorDescriptionByName";
        /// <summary>
        /// 找不到指定名称的Grpc客户端拦截器
        /// 格式为“找不到名称为{0}的Grpc客户端拦截器，发生位置为{1}”
        /// {0}：名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundCertificateGenerateServiceByName = "NotFoundCertificateGenerateServiceByName";
    }
}
