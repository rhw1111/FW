﻿using System;
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
        private readonly IAppAddTestCaseHistory _appAddTestCaseHistory;

        public TestCaseHistoryController(ILogger<TestCaseHistoryController> logger, IAppAddTestCaseHistory appAddTestCaseHistory)
        {
            _logger = logger;
            _appAddTestCaseHistory = appAddTestCaseHistory;
        }

        [HttpPost("addhistory")]
        public async Task AddHistory(TestCaseHistorySummyAddModel model)
        {
            try
            {
                if (model != null)
                {
                    await _appAddTestCaseHistory.Do(model);
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