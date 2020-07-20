using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using FW.TestPlatform.Main.Application;
using FW.TestPlatform.Main.DTOModel;
using MSLibrary;

namespace FW.TestPlatform.Portal.Api.Controllers
{
    [Route("api/testdatasource")]
    [ApiController]
    [EnableCors]
    public class TestDataSourceController : ControllerBase
    {
        private const int _pageSize = 50;

        private readonly IAppQueryTestDataSource _appQueryTestDataSource;
        private readonly IAppAddTestDataSource _appAddTestDataSource;
        private readonly IAppUpdateTestDataSource _appUpdateTestDataSource;
        private readonly IAppDeleteTestDataSource _appDeleteTestDataSource;
        private readonly IAppQuerySingleTestDataSource _appQuerySingleTestDataSource;
        private readonly IAppDeleteMultipleTestDataSource _appDeleteMultipleTestDataSource;
        private readonly IAppQueryTestDataSources _appQueryTestDataSources;

        public TestDataSourceController(IAppQueryTestDataSource appQueryTestDataSource, IAppAddTestDataSource appAddTestDataSource, IAppUpdateTestDataSource appUpdateTestDataSource, IAppDeleteTestDataSource appDeleteTestDataSource, 
            IAppQuerySingleTestDataSource appQuerySingleTestDataSource, IAppDeleteMultipleTestDataSource appDeleteMultipleTestDataSource, IAppQueryTestDataSources appQueryTestDataSources)
        {
            _appQueryTestDataSource = appQueryTestDataSource;
            _appAddTestDataSource = appAddTestDataSource;
            _appUpdateTestDataSource = appUpdateTestDataSource;
            _appDeleteTestDataSource = appDeleteTestDataSource;
            _appQuerySingleTestDataSource = appQuerySingleTestDataSource;
            _appDeleteMultipleTestDataSource = appDeleteMultipleTestDataSource;
            _appQueryTestDataSources = appQueryTestDataSources;
        }

        [HttpGet("querybypage")]
        public async Task<QueryResult<TestDataSourceViewData>> GetByPage(string? matchName,int page, int? pageSize)
        {
            if (matchName == null)
                matchName = "";
            if(pageSize == null)
            {
                pageSize = _pageSize;
            }
            return await _appQueryTestDataSource.Do(matchName, page, (int)pageSize);
        }

        [HttpGet("testdatasource")]
        public async Task<TestDataSourceViewData> GetTestDataSource(Guid id)
        {
            return await _appQuerySingleTestDataSource.Do(id);
        }

        [HttpPost("add")]
        public async Task<TestDataSourceViewData> Add(TestDataSourceAddModel model)
        {
            return await _appAddTestDataSource.Do(model);
        }
        [HttpPut("update")]
        public async Task<TestDataSourceViewData> Update(TestDataSourceUpdateModel model)
        {
            return await _appUpdateTestDataSource.Do(model);
        }
        [HttpDelete("delete")]
        public async Task Delete(Guid id)
        {
            await _appDeleteTestDataSource.Do(id);
        }

        [HttpDelete("deletemultiple")]
        public async Task DeleteMutiple(List<Guid> ids)
        {
            await _appDeleteMultipleTestDataSource.Do(ids);
        }

        [HttpGet("datasources")]
        public async Task<List<TestDataSourceNameAndIDList>> GetTestDataSources()
        {
            return await _appQueryTestDataSources.Do();
        }
    }
}
