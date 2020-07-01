﻿using System;
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
    [Route("api/testhost")]
    [ApiController]
    public class TestHostController : ControllerBase
    {
        private const int _pageSize = 50;

        private readonly IAppQueryTestHost _appQueryTestHost;

        public TestHostController(IAppQueryTestHost appQueryTestHost)
        {
            _appQueryTestHost = appQueryTestHost;
        }

        [HttpGet("QueryAll")]
        public async Task<List<TestHostViewData>> QueryAll()
        {
            return await _appQueryTestHost.Do();
        }
    }
}