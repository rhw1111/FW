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
        public async Task<TestCaseSlaveHost> AddSlaveHost(TestCaseSlaveHostAddModel slaveHost, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase()
            {
                ID = slaveHost.TestCaseID
            };
            TestCaseSlaveHost testCaseSlaveHost = new TestCaseSlaveHost()
            {
                SlaveName = slaveHost.SlaveName,
                ExtensionInfo = slaveHost.ExtensionInfo,
                HostID = slaveHost.HostID,
                TestCaseID = slaveHost.TestCaseID,
                Count = slaveHost.Count
            };
            await source.AddSlaveHost(testCaseSlaveHost, cancellationToken);
            return testCaseSlaveHost;
        }
        public IAsyncEnumerable<TestCaseSlaveHost> GetAllSlaveHosts(Guid caseId, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase()
            {
                ID = caseId
            };
            return source.GetAllSlaveHosts(cancellationToken);
        }

        public async Task DeleteSlaveHost(Guid slaveHostID, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase();
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

        public async Task<TestCaseHistory?> GetHistory(Guid historyID, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase();
            return await source.GetHistory(historyID, cancellationToken);
        }

        public async Task DeleteHistory(Guid historyID, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase();
            await source.DeleteHistory(historyID, cancellationToken);
        }

        public async Task UpdateSlaveHost(TestCaseSlaveHostAddModel slaveHost, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase()
            {
                ID = slaveHost.TestCaseID
            };
            TestCaseSlaveHost tCaseSlaveHost = new TestCaseSlaveHost() {
                ID = slaveHost.ID,
                TestCaseID = slaveHost.TestCaseID,
                HostID = slaveHost.HostID,
                Count = slaveHost.Count,
                ExtensionInfo = slaveHost.ExtensionInfo,
                SlaveName = slaveHost.SlaveName,
                CreateTime = DateTime.UtcNow,
                ModifyTime = DateTime.UtcNow
            };
            await source.UpdateSlaveHost(tCaseSlaveHost, cancellationToken);
        }
    }
}
