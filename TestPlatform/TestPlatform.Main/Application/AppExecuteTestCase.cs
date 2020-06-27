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
    [Injection(InterfaceType = typeof(IAppExecuteTestCase), Scope = InjectionScope.Singleton)]
    public class AppExecuteTestCase : IAppExecuteTestCase
    {
        public async Task<TestCaseViewData> Run(TestCaseAddModel model, CancellationToken cancellationToken = default)
        {
            TestCaseViewData result;
            try
            {
                TestCase source = new TestCase()
                {
                    Name = model.Name,
                    OwnerID = model.OwnerID,
                    EngineType = model.EngineType,
                    MasterHostID = model.MasterHostID,
                    Configuration = model.Configuration,
                    Status = model.Status
                };               
                await source.Run();

                result = new TestCaseViewData()
                {
                    ID = source.ID,
                    EngineType = source.EngineType,
                    Configuration = source.Configuration,
                    Name = source.Name,
                    CreateTime = source.CreateTime.ToCurrentUserTimeZone(),
                    ModifyTime = source.ModifyTime.ToCurrentUserTimeZone()
                };
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            

            return result;
        }

        public async Task<TestCaseViewData> Stop(TestCaseAddModel model, CancellationToken cancellationToken = default)
        {
            TestCaseViewData result;
            try
            {
                TestCase source = new TestCase()
                {
                    ID = model.ID,
                    Name = model.Name,
                    OwnerID = model.OwnerID,
                    EngineType = model.EngineType,
                    MasterHostID = model.MasterHostID,
                    Configuration = model.Configuration,
                    Status = model.Status
                };
                await source.Stop(cancellationToken);

                result = new TestCaseViewData()
                {
                    EngineType = source.EngineType,
                    Configuration = source.Configuration,
                    Name = source.Name,
                    ModifyTime = source.ModifyTime.ToCurrentUserTimeZone()
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public async Task<TestCaseViewData> IsEngineRun(TestCaseAddModel model, CancellationToken cancellationToken = default)
        {
            TestCaseViewData result;
            TestCase source = new TestCase()
            {
                ID = model.ID,
                Name = model.Name,
                OwnerID = model.OwnerID,
                EngineType = model.EngineType,
                MasterHostID = model.MasterHostID,
                Configuration = model.Configuration,
                Status = model.Status
            };
            await source.IsEngineRun(cancellationToken);

            result = new TestCaseViewData()
            {
                EngineType = source.EngineType,
                Configuration = source.Configuration,
                Name = source.Name,
                ModifyTime = source.ModifyTime.ToCurrentUserTimeZone()
            };
            return result;
        }
        public async Task AddSlaveHost(TestCaseSlaveHost slaveHost, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase()
            {
                ID = slaveHost.TestCaseID
            };
            await source.AddSlaveHost(slaveHost, cancellationToken);
        }
        public IAsyncEnumerable<TestCaseSlaveHost> GetAllSlaveHosts(TestCaseAddModel model, CancellationToken cancellationToken = default)
        {
            //TestCaseSlaveHostViewData viewData = new TestCaseSlaveHostViewData()
            //{
            //    ID = item.ID,
            //    SlaveName = item.SlaveName,
            //    Count = item.Count,
            //    ExtensionInfo = item.ExtensionInfo,
            //    Host = item.Host,
            //    TestCase = item.TestCase,
            //    ModifyTime = item.ModifyTime,
            //    CreateTime = item.CreateTime
            //};
            TestCase source = new TestCase()
            {
                ID = model.ID,
                Name = model.Name,
                OwnerID = model.OwnerID,
                EngineType = model.EngineType,
                MasterHostID = model.MasterHostID,
                Configuration = model.Configuration,
                Status = model.Status
            };
            return source.GetAllSlaveHosts(cancellationToken);
        }

        public async Task DeleteSlaveHost(TestCase model, Guid slaveHostID, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase()
            {
                ID = model.ID,
                Name = model.Name,
                OwnerID = model.OwnerID,
                EngineType = model.EngineType,
                MasterHostID = model.MasterHostID,
                Configuration = model.Configuration,
                Status = model.Status
            };
            await source.DeleteSlaveHost(slaveHostID, cancellationToken);
        }

        public async Task<QueryResult<TestCaseHistory>> GetHistories(Guid caseID, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase()
            {
                ID = caseID
            };
           return await source.GetHistories(caseID, page, pageSize, cancellationToken);
        }

        public async Task<TestCaseHistory?> GetHistory(TestCase tCase, Guid historyID, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase();
            return await source.GetHistory(historyID, cancellationToken);
        }

        public async Task DeleteHistory(TestCase tCase, Guid historyID, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase();
            await source.DeleteHistory(historyID, cancellationToken);
        }

        public async Task UpdateSlaveHost(TestCaseSlaveHost slaveHost, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase()
            {
                ID = slaveHost.TestCaseID,
                Status = slaveHost.TestCase.Status
            };
            await source.UpdateSlaveHost(slaveHost, cancellationToken);
        }
    }
}
