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
            try
            {
                await _appCreateMonitorDB.Do();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        [HttpPost("addmasterdata")]
        public async Task AddMasterData([FromBody]string data)
        {
            try
            {
                MonitorMasterDataAddModel model = JsonSerializerHelper.Deserialize<MonitorMasterDataAddModel>(data);

                if (model != null)
                {
                    await _appAddMonitorMasterData.Do(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost("addslavedata")]
        public async Task AddSlaveData([FromBody]string data)
        {
            try
            {
                IList<MonitorSlaveDataAddModel> modelList = JsonSerializerHelper.Deserialize<IList<MonitorSlaveDataAddModel>>(data);
                if (modelList != null)
                {
                    await _appAddMonitorSlaveData.Do(modelList);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
    }
}