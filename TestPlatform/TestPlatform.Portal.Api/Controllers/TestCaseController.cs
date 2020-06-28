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

        [HttpGet("gettestcsesbypage")]
        public async Task<QueryResult<TestCaseViewData>> GetByPage(int page, int pageSize)
        {
            return await _appQueryTestCase.GetByPage(page, _pageSize);
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
        public async Task<TestCaseViewData> Delete(Guid id)
        {
            try
            {
                TestCase source = new TestCase()
                {
                    ID = id
                };
                return await _appAddTestCase.Delete(source);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("gethosts")]
        public async Task<List<TestHostViewData>> GetHosts()
        {
            try
            {
                //RequestResult<QueryResult<TestHostViewData>> requestRel = new RequestResult<QueryResult<TestHostViewData>>();
                
                List<TestHostViewData> result = await _appQueryTestHost.GetHosts();
                return result;
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
        public async Task AddSlaveHost(TestCaseSlaveHostAddModel slaveHost)
        {
            await _appExecuteTestCase.AddSlaveHost(slaveHost);
        }
        [HttpPost("GetAllSlaveHosts")]
        public IAsyncEnumerable<TestCaseSlaveHost> GetAllSlaveHosts(Guid caseId)
        {
            return _appExecuteTestCase.GetAllSlaveHosts(caseId);
        }
        [HttpPut("UpdateSlaveHost")]
        public async Task UpdateSlaveHost(TestCaseSlaveHostAddModel slaveHost)
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
            await _appExecuteTestCase.DeleteHistory(historyID);
        }
        [HttpDelete("DeleteSlaveHost")]
        public async Task DeleteSlaveHost(Guid slaveHostId)
        {
            await _appExecuteTestCase.DeleteSlaveHost(slaveHostId);
        }
        [HttpDelete("GetHistory")]
        public async Task<TestCaseHistory?> GetHistory(Guid historyID)
        {
            return await _appExecuteTestCase.GetHistory(historyID);
        }
    }
}
