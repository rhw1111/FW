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

        public TestCaseController(IAppQueryTestCase appQueryTestCase, IAppAddTestCase appAddTestCase, IAppQueryTestHost appQueryTestHost, IAppExecuteTestCase appExecuteTestCase)
        {
            _appQueryTestCase = appQueryTestCase;
            _appAddTestCase = appAddTestCase;
            _appQueryTestHost = appQueryTestHost;
            _appExecuteTestCase = appExecuteTestCase;
        }

        [HttpGet("getbypage")]
        public async Task<QueryResult<TestCaseViewData>> GetByPage(string matchName,int page)
        {
            return await _appQueryTestCase.Do(matchName, page, _pageSize);
        }

        [HttpGet("getcase")]
        public async Task<TestCaseViewData?> GetCase(Guid id)
        {
            return await _appQueryTestCase.GetCase(id);
        }

        [HttpPost("add")]
        public async Task<TestCaseViewData> Add(TestCaseAddModel model)
        {
            return await _appAddTestCase.Do(model);
        }

        [HttpPut("update")]
        public async Task<TestCaseViewData> Update(TestCaseAddModel model)
        {
            return await _appAddTestCase.Update(model);
        }

        [HttpDelete("delete")]
        public async Task<TestCaseViewData> Delete(TestCaseAddModel model)
        {
            try
            {
                return await _appAddTestCase.Delete(model);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("gethosts")]
        public async Task<QueryResult<TestHostViewData>> GetHosts()
        {
            try
            {
                return await _appQueryTestHost.GetHosts();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost("run")]
        public async Task Run(TestCaseAddModel model)
        {
            await _appExecuteTestCase.Run(model);
        }
        [HttpPost("stop")]
        public async Task Stop(TestCaseAddModel model)
        {
            await _appExecuteTestCase.Run(model);
        }
        [HttpPost("isenginerun")]
        public async Task<TestCaseViewData> IsEngineRun(TestCaseAddModel model)
        {
            return await _appExecuteTestCase.IsEngineRun(model);
        }
        [HttpPost("addslavehost")]
        public async Task AddSlaveHost(TestCaseSlaveHost slaveHost)
        {
            await _appExecuteTestCase.AddSlaveHost(slaveHost);
        }
        [HttpPost("GetAllSlaveHosts")]
        public IAsyncEnumerable<TestCaseSlaveHost> GetAllSlaveHosts(TestCaseAddModel tCase)
        {
            return _appExecuteTestCase.GetAllSlaveHosts(tCase);
        }
        [HttpPut("UpdateSlaveHost")]
        public async Task UpdateSlaveHost(TestCaseSlaveHost slaveHost)
        {
            await _appExecuteTestCase.UpdateSlaveHost(slaveHost);
        }
        [HttpGet("GetHistories")]
        public async Task<QueryResult<TestCaseHistory>> GetHistories(Guid caseID, int page, int pageSize)
        {
            return await _appExecuteTestCase.GetHistories(caseID, page, pageSize);
        }
        [HttpDelete("DeleteHistory")]
        public async Task DeleteHistory(Guid historyID)
        {
            TestCase tCase = new TestCase();
            await _appExecuteTestCase.DeleteHistory(tCase, historyID);
        }
        [HttpDelete("DeleteSlaveHost")]
        public async Task DeleteSlaveHost(TestCaseSlaveHost tCaseSlaveHost)
        {
            TestCase tCase = new TestCase()
            {
                ID = tCaseSlaveHost.TestCaseID,
                Status = tCaseSlaveHost.TestCase.Status
            };
            await _appExecuteTestCase.DeleteSlaveHost(tCase, tCaseSlaveHost.ID);
        }
        [HttpDelete("GetHistory")]
        public async Task<TestCaseHistory?> GetHistory(Guid historyID)
        {
            TestCase tCase = new TestCase();
            return await _appExecuteTestCase.GetHistory(tCase, historyID);
        }
    }
}
