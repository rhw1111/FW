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

        [HttpDelete("gethosts")]
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
        [HttpDelete("run")]
        public async Task Run(TestCaseAddModel model)
        {
            await _appExecuteTestCase.Run(model);
        }
        public async Task Stop(TestCaseAddModel model)
        {
            await _appExecuteTestCase.Run(model);
        }
        public async Task<TestCaseViewData> IsEngineRun(TestCaseAddModel model)
        {
            return await _appExecuteTestCase.IsEngineRun(model);
        }
        //public async Task AddSlaveHost(TestCase tCase, TestCaseSlaveHost slaveHost)
        //{
        //    await _appExecuteTestCase.AddSlaveHost(tCase, slaveHost);
        //}
        //public IAsyncEnumerable<TestCaseSlaveHost> GetAllSlaveHosts(TestCaseAddModel tCase)
        //{
        //    return _appExecuteTestCase.GetAllSlaveHosts(tCase);
        //}
        //public async Task UpdateSlaveHost(TestCase tCase, TestCaseSlaveHost slaveHost)
        //{
        //    await _appExecuteTestCase.UpdateSlaveHost(tCase, slaveHost);
        //}
        //public async Task<QueryResult<TestCaseHistory>> GetHistories(Guid caseID, int page, int pageSize)
        //{
        //    return await _appExecuteTestCase.GetHistories(caseID, page, pageSize);
        //}
        //public async Task DeleteHistory(TestCase tCase, Guid historyID)
        //{
        //    await _appExecuteTestCase.DeleteHistory(tCase, historyID);
        //}
        //public async Task DeleteSlaveHost(TestCase tCase ,Guid slaveHostID)
        //{
        //    await _appExecuteTestCase.DeleteSlaveHost(tCase, slaveHostID);
        //}

        //public async Task<TestCaseHistory?> GetHistory(TestCase tCase, Guid historyID)
        //{
        //    return await _appExecuteTestCase.GetHistory(tCase, historyID);
        //}
    }
}
