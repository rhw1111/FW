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
    [Route("api/testcase")]
    [ApiController]
    public class TestCaseController : ControllerBase
    {
        private const int _pageSize = 50;

        private readonly IAppQueryTestCase _appQueryTestCase;
        private readonly IAppAddTestCase _appAddTestCase;

        public TestCaseController(IAppQueryTestCase appQueryTestCase, IAppAddTestCase appAddTestCase)
        {
            _appQueryTestCase = appQueryTestCase;
            _appAddTestCase = appAddTestCase;
        }

        [HttpGet("getbypage")]
        public async Task<QueryResult<TestCaseViewData>> GetByPage(string matchName,int page)
        {
            return await _appQueryTestCase.Do(matchName, page, _pageSize);
        }

        [HttpPost("add")]
        public async Task<TestCaseViewData> Add(TestCaseAddModel model)
        {
            return await _appAddTestCase.Do(model);
        }
    }
}
