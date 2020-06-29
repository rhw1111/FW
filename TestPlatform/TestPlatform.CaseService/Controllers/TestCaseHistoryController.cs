using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FW.TestPlatform.Main.Application;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MSLibrary.Serializer;

namespace FW.TestPlatform.CaseService.Controllers
{
    [Route("api/report")]
    [ApiController]
    public class TestCaseHistoryController : ControllerBase
    {
        private readonly ILogger<TestCaseHistoryController> _logger;
        private readonly IAppAddTestCase _appAddTestCase;

        public TestCaseHistoryController(ILogger<TestCaseHistoryController> logger, IAppAddTestCase appAddTestCase)
        {
            _logger = logger;
            _appAddTestCase = appAddTestCase;
        }

        [HttpPost("addhistory")]
        public async Task AddHistory([FromBody]string data)
        {
            try
            {
                TestCaseHistorySummyAddModel model = JsonSerializerHelper.Deserialize<TestCaseHistorySummyAddModel>(data);

                if (model != null)
                {
                    await _appAddTestCase.AddHistory(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }

    }
}