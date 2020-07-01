using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FW.TestPlatform.Main.Application;
using FW.TestPlatform.Main.DTOModel;
using MSLibrary;
using FW.TestPlatform.Main.Entities;
using MSLibrary.Security.RequestController;

namespace FW.TestPlatform.Portal.Api.Controllers
{
    [Route("api/testcase")]
    [ApiController]
    public class TestCaseController : ControllerBase
    {
        private const int _pageSize = 50;

        private readonly IAppQueryTestCase _appQueryTestCase;
        private readonly IAppAddTestCase _appAddTestCase;
        private readonly IAppQueryTestHost _appQueryTestHost;
        private readonly IAppExecuteTestCase _appExecuteTestCase;
        private readonly IAppUpdateTestCase _appUpdateTestCase;
        private readonly IAppDeleteTestCase _appDeleteTestCase;
        private readonly IAppDeleteMultipleTestCase _appDeleteMultipleTestCase;
        private readonly IAppQuerySingleTestCase _appQuerySingleTestCase;
        private readonly IAppRunTestCase _appRunTestCase;
        private readonly IAppStopTestCase _appStopTestCase;
        private readonly IAppCheckTestCaseStatus _appCheckTestCaseStatus;
        private readonly IAppAddSlaveHost _appAddSlaveHost;
        //private readonly IAppAddTestCaseHistory _appAddTestCaseHistory;
        private readonly IAppQueryMasterLog _appQueryMasterLog;
        private readonly IAppQuerySlaveLog _appQuerySlaveLog;
        private readonly IAppQuerySlaveHost _appQuerySlaveHost;
        private readonly IAppQueryTestCaseHistory _appQueryTestCaseHistory;
        private readonly IAppQuerySingleTestCaseHistory _appQuerySingleTestCaseHistory;
        private readonly IAppUpdateSlaveHost _appUpdateSlaveHost;
        private readonly IAppDeleteTestCaseHistory _appDeleteTestCaseHistory;
        private readonly IAppDeleteSlaveHost _appDeleteSlaveHost;
        public TestCaseController(IAppQueryTestCase appQueryTestCase, IAppAddTestCase appAddTestCase, IAppQueryTestHost appQueryTestHost, IAppExecuteTestCase appExecuteTestCase, IAppQuerySingleTestCase appQuerySingleTestCase, IAppUpdateTestCase appUpdateTestCase,
            IAppDeleteMultipleTestCase appDeleteMultipleTestCase, IAppDeleteTestCase appDeleteTestCase, IAppRunTestCase appRunTestCase, IAppStopTestCase appStopTestCase, IAppCheckTestCaseStatus appCheckTestCaseStatus, IAppAddSlaveHost appAddSlaveHost,
            IAppQueryMasterLog appQueryMasterLog, IAppQuerySlaveLog appQuerySlaveLog, IAppQuerySlaveHost appQuerySlaveHost, IAppQueryTestCaseHistory appQueryTestCaseHistory, IAppQuerySingleTestCaseHistory appQuerySingleTestCaseHistory, IAppUpdateSlaveHost appUpdateSlaveHost,
            IAppDeleteTestCaseHistory appDeleteTestCaseHistory, IAppDeleteSlaveHost appDeleteSlaveHost)
        {
            _appQueryTestCase = appQueryTestCase;
            _appAddTestCase = appAddTestCase;
            _appQueryTestHost = appQueryTestHost;
            _appExecuteTestCase = appExecuteTestCase;
            _appUpdateTestCase = appUpdateTestCase;
            _appDeleteTestCase = appDeleteTestCase;
            _appDeleteMultipleTestCase = appDeleteMultipleTestCase;
            _appQuerySingleTestCase = appQuerySingleTestCase;
            _appRunTestCase = appRunTestCase;
            _appStopTestCase = appStopTestCase;
            _appCheckTestCaseStatus = appCheckTestCaseStatus;
            _appAddSlaveHost = appAddSlaveHost;
            _appQueryMasterLog = appQueryMasterLog;
            _appQuerySlaveLog = appQuerySlaveLog;
            _appQuerySlaveHost = appQuerySlaveHost;
            _appQueryTestCaseHistory = appQueryTestCaseHistory;
            _appQuerySingleTestCaseHistory = appQuerySingleTestCaseHistory;
            _appUpdateSlaveHost = appUpdateSlaveHost;
            _appDeleteTestCaseHistory = appDeleteTestCaseHistory;
            _appDeleteSlaveHost = appDeleteSlaveHost;
        }

        [HttpGet("getbypage")]
        public async Task<QueryResult<TestCaseViewData>> GetByPage(string matchName,int page)
        {
            return await _appQueryTestCase.Do(matchName, page, _pageSize);
        }

        [HttpGet("getcase")]
        public async Task<TestCaseViewData?> GetCase(Guid id)
        {
            return await _appQuerySingleTestCase.Do(id);
        }

        [HttpPost("add")]
        public async Task<TestCaseViewData> Add(TestCaseAddModel model)
        {
            return await _appAddTestCase.Do(model);
        }

        [HttpPut("update")]
        public async Task Update(TestCaseAddModel model)
        {
            await _appUpdateTestCase.Do(model);
        }

        [HttpDelete("delete")]
        public async Task Delete(Guid id)
        {
            await _appDeleteTestCase.Do(id);
        }

        [HttpDelete("deletemultiple")]
        public async Task DeleteMultiple(List<Guid> list)
        {
           await _appDeleteMultipleTestCase.Do(list);
        }
        
        [HttpPost("run")]
        public async Task Run(Guid caseId)
        {
            await _appRunTestCase.Do(caseId);
        }
        [HttpPost("stop")]
        public async Task Stop(Guid caseId)
        {
            await _appStopTestCase.Do(caseId);
        }
        [HttpPost("CheckTestStatus")]
        public async Task<bool> CheckTestStatus(Guid caseId)
        {
            return await _appCheckTestCaseStatus.Do(caseId);
        }
        [HttpPost("GetMasterLog")]
        public async Task<string> GetMasterLog(Guid caseId)
        {
             return await _appQueryMasterLog.Do(caseId);
        }
        [HttpPost("GetSlaveLog")]
        public async Task<string> GetSlaveLog(Guid caseId, Guid slaveHostId)
        {
            return await _appQuerySlaveLog.Do(caseId, slaveHostId);
        }
        [HttpPost("addslavehost")]
        public async Task<TestCaseSlaveHost> AddSlaveHost(TestCaseSlaveHostAddModel slaveHost)
        {
            return await _appAddSlaveHost.Do(slaveHost);
        }
        [HttpGet("GetAllSlaveHosts")]
        public IAsyncEnumerable<TestCaseSlaveHost> GetAllSlaveHosts(Guid caseId)
        {
            return _appQuerySlaveHost.Do(caseId);
        }
        [HttpPut("UpdateSlaveHost")]
        public async Task UpdateSlaveHost(TestCaseSlaveHostAddModel slaveHost)
        {
            await _appUpdateSlaveHost.Do(slaveHost);
        }
        [HttpGet("GetHistories")]
        public async Task<QueryResult<TestCaseHistory>> GetHistories(Guid caseID, int page, int pageSize)
        {
            return await _appQueryTestCaseHistory.Do(caseID, page, pageSize);
        }
        [HttpDelete("DeleteHistory")]
        public async Task DeleteHistory(Guid id)
        {
            await _appDeleteTestCaseHistory.Do(id);
        }
        [HttpDelete("DeleteSlaveHost")]
        public async Task DeleteSlaveHost(Guid id)
        {
            await _appDeleteSlaveHost.Do(id);
        }
        [HttpGet("GetHistory")]
        public async Task<TestCaseHistoryViewModel> GetHistory(Guid caseId, Guid historyId)
        {
            return await _appQuerySingleTestCaseHistory.Do(caseId, historyId);
        }

        //[HttpGet("gethosts")]
        //public async Task<List<TestHostViewData>> GetHosts()
        //{
        //    try
        //    {
        //        List<TestHostViewData> result = await _appQueryTestHost.Do();
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
