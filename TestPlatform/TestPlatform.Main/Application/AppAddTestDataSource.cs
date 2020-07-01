using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppAddTestDataSource), Scope = InjectionScope.Singleton)]
    public class AppAddTestDataSource : IAppAddTestDataSource
    {
        public async Task<TestDataSourceViewData> Do(TestDataSourceAddModel model, CancellationToken cancellationToken = default)
        {
            TestDataSource source = new TestDataSource()
            {
                Name = model.Name,
                Data = model.Data,
                Type = model.Type
            };

            await source.Add(cancellationToken);

            TestDataSourceViewData result = new TestDataSourceViewData()
            {
                ID = source.ID,
                Type = source.Type,
                Data = source.Data,
                Name = source.Name,
                CreateTime = source.CreateTime.ToCurrentUserTimeZone(),
                ModifyTime = source.ModifyTime.ToCurrentUserTimeZone()
            };

            return result;
        }

        //public async Task<TestDataSourceViewData> Update(TestDataSourceAddModel model, CancellationToken cancellationToken = default)
        //{
        //    TestDataSource source = new TestDataSource()
        //    {
        //        ID = model.ID,
        //        Name = model.Name,
        //        Data = model.Data,
        //        Type = model.Type
        //    };

        //    await source.Update(cancellationToken);

        //    TestDataSourceViewData result = new TestDataSourceViewData()
        //    {
        //        ID = source.ID,
        //        Type = source.Type,
        //        Data = source.Data,
        //        Name = source.Name,
        //        CreateTime = source.CreateTime.ToCurrentUserTimeZone(),
        //        ModifyTime = source.ModifyTime.ToCurrentUserTimeZone()
        //    };

        //    return result;
        //}

        //public async Task<TestDataSourceViewData> Delete(TestDataSourceAddModel model, CancellationToken cancellationToken = default)
        //{
        //    TestDataSource source = new TestDataSource()
        //    {
        //        ID = model.ID,
        //        Name = model.Name,
        //        Data = model.Data,
        //        Type = model.Type
        //    };

        //    await source.Delete(cancellationToken);

        //    TestDataSourceViewData result = new TestDataSourceViewData()
        //    {
        //        ID = source.ID,
        //        Type = source.Type,
        //        Data = source.Data,
        //        Name = source.Name,
        //        CreateTime = source.CreateTime.ToCurrentUserTimeZone(),
        //        ModifyTime = source.ModifyTime.ToCurrentUserTimeZone()
        //    };

        //    return result;
        //}
    }
}
