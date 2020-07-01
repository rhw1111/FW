using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FW.TestPlatform.Main;
using FW.TestPlatform.Main.Application;
using FW.TestPlatform.Main.DTOModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MSLibrary.Serializer;
using MSLibrary.StreamingDB.InfluxDB;


namespace FW.TestPlatform.CaseService.Controllers
{
    [Route("api/monitor")]
    [ApiController]
    public class MonitorController : ControllerBase
    {
        private readonly IAppCreateMonitorDB _appCreateMonitorDB;
        private readonly IAppAddMonitorMasterData _appAddMonitorMasterData;
        private readonly IAppAddMonitorSlaveData _appAddMonitorSlaveData;
        private readonly ILogger<MonitorController> _logger;

        public MonitorController(ILogger<MonitorController> logger, IAppCreateMonitorDB appCreateMonitorDB, IAppAddMonitorMasterData appAddMonitorMasterData, IAppAddMonitorSlaveData appAddMonitorSlaveData)
        {
            _appCreateMonitorDB = appCreateMonitorDB;
            _appAddMonitorMasterData = appAddMonitorMasterData;
            _appAddMonitorSlaveData = appAddMonitorSlaveData;
            _logger = logger;
        }

        [HttpPost("createdatabase")]
        public async Task CreateDataBase()
        {
            await _appCreateMonitorDB.Do();
        }

        [HttpPost("addmasterdata")]
        public async Task AddMasterData(MonitorMasterDataAddModel model)
        {
            await _appAddMonitorMasterData.Do(model);
        }

        [HttpPost("addslavedata")]
        public async Task AddSlaveData(IList<MonitorSlaveDataAddModel> modelList)
        {
            await _appAddMonitorSlaveData.Do(modelList);
        }
    }
}