using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Grpc
{
    /// <summary>
    /// 用来标记服务为Grpc服务
    /// </summary>
    public class GrpcServiceAttribute:Attribute
    {
        private Type _nameSpaceType;
        public GrpcServiceAttribute(Type nameSpaceType)
        {
            _nameSpaceType = nameSpaceType;
        }

        public Type NameSpaceType
        {
            get
            {
                return _nameSpaceType;
            }
        }
    }
}
