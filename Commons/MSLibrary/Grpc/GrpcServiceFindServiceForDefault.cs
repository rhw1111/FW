using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using MSLibrary.DI;

namespace MSLibrary.Grpc
{
    public class GrpcServiceFindServiceForDefault : IGrpcServiceFindService
    {
        /// <summary>
        /// 被搜索的程序集名称
        /// </summary>
        private string[] _assemblyNames;

        public GrpcServiceFindServiceForDefault(string[] assemblyNames)
        {
            _assemblyNames = assemblyNames;
        }

        public async Task<IList<GrpcTypeData>> Execute()
        {
            List<GrpcTypeData> typeDatas = new List<GrpcTypeData>();
            foreach (var assemblyName in _assemblyNames)
            {
                var assembly = Assembly.Load(assemblyName);
                //检查每个程序集中的类是否是GRPC服务
                var types = assembly.GetTypes();
                foreach (var itemType in types)
                {
                    var grpcServiceAttributes = itemType.GetTypeInfo().GetCustomAttributes<GrpcServiceAttribute>();

                    if (grpcServiceAttributes.Count() > 0)
                    {
                        typeDatas.Add(new GrpcTypeData() { NameSpaceType = grpcServiceAttributes.First().NameSpaceType, ServiceType = itemType });
                    }
                }
            }

            return await Task.FromResult(typeDatas);
        }
    }

    [Injection(InterfaceType = typeof(GrpcServiceFindServiceForDefaultFactory), Scope = InjectionScope.Singleton)]
    public class GrpcServiceFindServiceForDefaultFactory:SingletonFactory<IGrpcServiceFindService>
    {
        private string[] _assemblyNames;
        public GrpcServiceFindServiceForDefaultFactory(string[] assemblyNames):base()
        {
            _assemblyNames = assemblyNames;
        }
        protected override IGrpcServiceFindService RealCreate()
        {
            return new GrpcServiceFindServiceForDefault(_assemblyNames);
        }
    }
}
