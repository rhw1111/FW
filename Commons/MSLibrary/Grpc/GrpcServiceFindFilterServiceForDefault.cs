using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MSLibrary.DI;
using MSLibrary.Grpc.Filters;

namespace MSLibrary.Grpc
{
    public class GrpcServiceFindFilterServiceForDefault : IGrpcServiceFindFilterService
    {
        public async Task<IList<FilterBase>> Execute(Type serviceType)
        {
            List<FilterBase> filters = new List<FilterBase>();
            var filterAttributes=serviceType.GetTypeInfo().GetCustomAttributes<FilterBase>();
            foreach(var attributeItem in filterAttributes)
            {
                filters.Add(attributeItem);
            }

            return await Task.FromResult(filters);
        }
    }

    [Injection(InterfaceType = typeof(GrpcServiceFindFilterServiceForDefaultFactory), Scope = InjectionScope.Singleton)]
    public class GrpcServiceFindFilterServiceForDefaultFactory:SingletonFactory<IGrpcServiceFindFilterService>
    {
        protected override IGrpcServiceFindFilterService RealCreate()
        {
            return new GrpcServiceFindFilterServiceForDefault();
        }
    }
}
