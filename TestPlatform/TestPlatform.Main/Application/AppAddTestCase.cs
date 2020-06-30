using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using System.Linq;
using System.Diagnostics.Tracing;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppAddTestCase), Scope = InjectionScope.Singleton)]
    public class AppAddTestCase : IAppAddTestCase
    {
        public async Task<TestCaseViewData> Do(TestCaseAddModel model, CancellationToken cancellationToken = default)
        {
            TestCaseViewData result;
            TestCase source = new TestCase()
            {
                Name = model.Name,
                EngineType = model.EngineType,
                MasterHostID = model.MasterHostID,
                Configuration = model.Configuration,
                Status = TestCaseStatus.NoRun,
                CreateTime = DateTime.UtcNow,
                ModifyTime = DateTime.UtcNow
            };               
            await source.Add();

            result = new TestCaseViewData()
            {
                ID = source.ID,
                EngineType = source.EngineType,
                Configuration = source.Configuration,
                Name = source.Name,
                Status = source.Status,
                MasterHostID = source.MasterHostID,
                CreateTime = source.CreateTime.ToCurrentUserTimeZone(),
                ModifyTime = source.ModifyTime.ToCurrentUserTimeZone()
            };
            return result;
        }

        //public async Task<TestCaseViewData> Update(TestCaseAddModel model, CancellationToken cancellationToken = default)
        //{
        //    TestCaseViewData result;
        //    try
        //    {
        //        TestCase source = new TestCase()
        //        {
        //            ID = model.ID,
        //            Name = model.Name,
        //            OwnerID = model.OwnerID,
        //            EngineType = model.EngineType,
        //            MasterHostID = model.MasterHostID,
        //            Configuration = model.Configuration,
        //            Status = model.Status
        //        };
        //        await source.Update(cancellationToken);

        //        result = new TestCaseViewData()
        //        {
        //            EngineType = source.EngineType,
        //            Configuration = source.Configuration,
        //            Name = source.Name,
        //            ModifyTime = source.ModifyTime.ToCurrentUserTimeZone()
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }


        //    return result;
        //}

        //public async Task<TestCaseViewData> Delete(TestCase model, CancellationToken cancellationToken = default)
        //{
        //    TestCase source = new TestCase()
        //    {
        //        ID = model.ID
        //    };
        //    await source.Delete(cancellationToken);
        //    TestCaseViewData viewData = new TestCaseViewData
        //    {
        //        ID = model.ID
        //    };
        //    return viewData;
        //}

        //public async Task DeleteMutiple(List<TestCaseAddModel> list, CancellationToken cancellationToken = default)
        //{
        //    TestCase source = new TestCase();
        //    List<TestCase> array = new List<TestCase>();
        //    foreach (TestCaseAddModel item in list)
        //    {
        //        TestCase tCase = new TestCase()
        //        {
        //            ID = item.ID
        //        };
        //        array.Add(tCase);
        //    }
        //    await source.DeleteMultiple(array.ToList(), cancellationToken);
        //}

        //public async Task AddHistory(TestCaseHistorySummyAddModel model, CancellationToken cancellationToken = default)
        //{
        //    TestCase source = new TestCase()
        //    {
        //        ID = model.CaseID
        //    };

        //    await source.AddHistory(model);
        //}
    }
}
