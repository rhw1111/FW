using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FW.TestPlatform.Main.Application;
using FW.TestPlatform.Main.DTOModel;
using MSLibrary;

namespace FW.TestPlatform.Portal.Api.Controllers
{
    [Route("api/testdatasource")]
    [ApiController]
    public class TestDataSourceController : ControllerBase
    {
        private const int _pageSize = 50;

        private readonly IAppQueryTestDataSource _appQueryTestDataSource;
        private readonly IAppAddTestDataSource _appAddTestDataSource;

        public TestDataSourceController(IAppQueryTestDataSource appQueryTestDataSource, IAppAddTestDataSource appAddTestDataSource)
        {
            _appQueryTestDataSource = appQueryTestDataSource;
            _appAddTestDataSource = appAddTestDataSource;
        }

        [HttpGet("getbypage")]
        public async Task<QueryResult<TestDataSourceViewData>> GetByPage(string matchName,int page)
        {
            return await _appQueryTestDataSource.Do(matchName, page, _pageSize);
        }

        [HttpPost("add")]
        public async Task<TestDataSourceViewData> Add(TestDataSourceAddModel model)
        {
            return await _appAddTestDataSource.Do(model);
        }
        [HttpPut("update")]
        public async Task<TestDataSourceViewData> Update(TestDataSourceAddModel model)
        {
            return await _appAddTestDataSource.Delete(model);
        }
        [HttpPost("delete")]
        public async Task<TestDataSourceViewData> Delete(TestDataSourceAddModel model)
        {
            return await _appAddTestDataSource.Do(model);
        }
    }
}
