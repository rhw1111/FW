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
    [Route("api/testhost")]
    [ApiController]
    [EnableCors]
    public class TestHostController : ControllerBase
    {
        private const int _pageSize = 50;

        private readonly IAppQueryTestHost _appQueryTestHost;
        private readonly IAppAddTestHost _appAddTestHost;
        private readonly IAppUpdateTestHost _appUpdateTestHost;
        private readonly IAppDeleteTestHost _appDeleteTestHost;
        private readonly IAppQueryTestHostByPage _appQueryTestHostByPage;
        private readonly IAppQuerySingleTestHost _appQuerySingleTestHost;
        private readonly IAppDeleteTestHosts _appDeleteTestHosts;

        public TestHostController(IAppQueryTestHost appQueryTestHost, IAppAddTestHost appAddTestHost, IAppUpdateTestHost appUpdateTestHost, IAppDeleteTestHost appDeleteTestHost,
            IAppQueryTestHostByPage appQueryTestHostByPage, IAppQuerySingleTestHost appQuerySingleTestHost, IAppDeleteTestHosts appDeleteTestHosts)
        {
            _appQueryTestHost = appQueryTestHost;
            _appAddTestHost = appAddTestHost;
            _appUpdateTestHost = appUpdateTestHost;
            _appDeleteTestHost = appDeleteTestHost;
            _appQueryTestHostByPage = appQueryTestHostByPage;
            _appQuerySingleTestHost = appQuerySingleTestHost;
            _appDeleteTestHosts = appDeleteTestHosts;
        }

        [HttpGet("querybypage")]
        public async Task<QueryResult<TestHostViewData>> GetByPage(string? matchName, int page, int? pageSize)
        {
            if (matchName == null)
                matchName = "";
            if (pageSize == null)
            {
                pageSize = _pageSize;
            }
            return await _appQueryTestHostByPage.Do(matchName, page, (int)pageSize);
        }

        [HttpGet("testhost")]
        public async Task<TestHostViewData> GetTestHost(Guid id)
        {
            return await _appQuerySingleTestHost.Do(id);
        }

        [HttpPost("add")]
        public async Task<TestHostViewData> Add(TestHostAddModel model)
        {
            return await _appAddTestHost.Do(model);
        }
        [HttpPut("update")]
        public async Task<TestHostViewData> Update(TestHostUpdateModel model)
        {
            return await _appUpdateTestHost.Do(model);
        }
        [HttpDelete("delete")]
        public async Task Delete(Guid id)
        {
            await _appDeleteTestHost.Do(id);
        }

        [HttpDelete("deletemultiple")]
        public async Task DeleteMutiple(List<Guid> ids)
        {
            await _appDeleteTestHosts.Do(ids);
        }

        [HttpGet("queryall")]
        public async Task<List<TestHostViewData>> QueryAll()
        {
            return await _appQueryTestHost.Do();
        }
    }
}
