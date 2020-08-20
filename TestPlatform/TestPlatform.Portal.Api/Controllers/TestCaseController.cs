using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using FW.TestPlatform.Main.Application;
using FW.TestPlatform.Main.DTOModel;
using MSLibrary;
using FW.TestPlatform.Main;

namespace FW.TestPlatform.Portal.Api.Controllers
{
    [Route("api/testcase")]
    [ApiController]
    [EnableCors]
    public class TestCaseController : ControllerBase
    {
        private const int _pageSize = 50;

        private readonly IAppQueryTestCase _appQueryTestCase;
        private readonly IAppAddTestCase _appAddTestCase;
        private readonly IAppUpdateTestCase _appUpdateTestCase;
        private readonly IAppDeleteTestCase _appDeleteTestCase;
        private readonly IAppQuerySingleTestCase _appQuerySingleTestCase;
        private readonly IAppRunTestCase _appRunTestCase;
        private readonly IAppStopTestCase _appStopTestCase;
        private readonly IAppCheckTestCaseStatus _appCheckTestCaseStatus;
        private readonly IAppAddSlaveHost _appAddSlaveHost;
        private readonly IAppQueryMasterLog _appQueryMasterLog;
        private readonly IAppQuerySlaveLog _appQuerySlaveLog;
        private readonly IAppQuerySlaveHost _appQuerySlaveHost;
        private readonly IAppQueryTestCaseHistory _appQueryTestCaseHistory;
        private readonly IAppQuerySingleTestCaseHistory _appQuerySingleTestCaseHistory;
        private readonly IAppUpdateSlaveHost _appUpdateSlaveHost;
        private readonly IAppDeleteTestCaseHistory _appDeleteTestCaseHistory;
        private readonly IAppDeleteSlaveHost _appDeleteSlaveHost;
        private readonly IAppDeleteSlaveHosts _appDeleteSlaveHosts;
        private readonly IAppDeleteHistories _appDeleteHistories;
        private readonly IAppQueryTestCaseStatus _appQueryTestCaseStatus;
        private readonly IAppQueryHistoriesByIds _appQueryHistoriesByIds;
        private readonly IAppTransferNetGatewayDataFile _appTransferNetGatewayDataFile;
        private readonly IAppUpdateNetGatewayDataFormat _appUpdateNetGatewayDataFormat;
        private readonly IAppCheckNetGatewayDataAnalysisStatus _appCheckNetGatewayDataAnalysisStatus;
        private readonly IAppGetNetGatewayDataFormatTypes _appGetNetGatewayDataFormatTypes;
        public TestCaseController(IAppQueryTestCase appQueryTestCase, IAppAddTestCase appAddTestCase, IAppQuerySingleTestCase appQuerySingleTestCase, IAppUpdateTestCase appUpdateTestCase,
            IAppDeleteTestCase appDeleteTestCase, IAppRunTestCase appRunTestCase, IAppStopTestCase appStopTestCase, IAppCheckTestCaseStatus appCheckTestCaseStatus, IAppAddSlaveHost appAddSlaveHost,
            IAppQueryMasterLog appQueryMasterLog, IAppQuerySlaveLog appQuerySlaveLog, IAppQuerySlaveHost appQuerySlaveHost, IAppQueryTestCaseHistory appQueryTestCaseHistory, IAppQuerySingleTestCaseHistory appQuerySingleTestCaseHistory, IAppUpdateSlaveHost appUpdateSlaveHost,
            IAppDeleteTestCaseHistory appDeleteTestCaseHistory, IAppDeleteSlaveHost appDeleteSlaveHost, IAppDeleteHistories appDeleteHistories, IAppDeleteSlaveHosts appDeleteSlaveHosts, IAppQueryTestCaseStatus appQueryTestCaseStatus, IAppQueryHistoriesByIds appQueryHistoriesByIds,
            IAppTransferNetGatewayDataFile appTransferNetGatewayDataFile, IAppUpdateNetGatewayDataFormat appUpdateNetGatewayDataFormat, IAppCheckNetGatewayDataAnalysisStatus appCheckNetGatewayDataAnalysisStatus, IAppGetNetGatewayDataFormatTypes appGetNetGatewayDataFormatTypes)
        {
            _appQueryTestCase = appQueryTestCase;
            _appAddTestCase = appAddTestCase;
            _appUpdateTestCase = appUpdateTestCase;
            _appDeleteTestCase = appDeleteTestCase;
            _appQuerySingleTestCase = appQuerySingleTestCase;
            _appRunTestCase = appRunTestCase;
            _appStopTestCase = appStopTestCase;
            _appCheckTestCaseStatus = appCheckTestCaseStatus;
            _appAddSlaveHost = appAddSlaveHost;
            _appQueryMasterLog = appQueryMasterLog;
            _appQuerySlaveLog = appQuerySlaveLog;
            _appQuerySlaveHost = appQuerySlaveHost;
            _appQueryTestCaseHistory = appQueryTestCaseHistory;
            _appQuerySingleTestCaseHistory = appQuerySingleTestCaseHistory;
            _appUpdateSlaveHost = appUpdateSlaveHost;
            _appDeleteTestCaseHistory = appDeleteTestCaseHistory;
            _appDeleteSlaveHost = appDeleteSlaveHost;
            _appDeleteHistories = appDeleteHistories;
            _appDeleteSlaveHosts = appDeleteSlaveHosts;
            _appQueryTestCaseStatus = appQueryTestCaseStatus;
            _appQueryHistoriesByIds = appQueryHistoriesByIds;
            _appTransferNetGatewayDataFile = appTransferNetGatewayDataFile;
            _appUpdateNetGatewayDataFormat = appUpdateNetGatewayDataFormat;
            _appCheckNetGatewayDataAnalysisStatus = appCheckNetGatewayDataAnalysisStatus;
            _appGetNetGatewayDataFormatTypes = appGetNetGatewayDataFormatTypes;
        }
        //查询增加修改执行TestCase
        [HttpGet("querybypage")]
        public async Task<QueryResult<TestCaseListViewData>> GetByPage(string? matchName,int page, int? pageSize)
        {
            if(matchName == null)
            {
                matchName = "";
            }
            if(pageSize == null)
            {
                pageSize = _pageSize;
            }
            return await _appQueryTestCase.Do(matchName, page, (int)pageSize);
        }

        [HttpGet("testcase")]
        public async Task<TestCaseViewData> GetCase(Guid id)
        {
            return await _appQuerySingleTestCase.Do(id);
        }

        [HttpPost("add")]
        public async Task<TestCaseViewData> Add(TestCaseAddModel model)
        {
            return await _appAddTestCase.Do(model);
        }

        [HttpPut("update")]
        public async Task<TestCaseViewData> Update(TestCaseUpdateModel model)
        {
            return await _appUpdateTestCase.Do(model);
        }

        [HttpDelete("delete")]
        public async Task Delete(Guid id)
        {
            await _appDeleteTestCase.Do(id);
        }

        //[HttpDelete("deletemultiple")]
        //public async Task DeleteMultiple(List<Guid> list)
        //{
        //   await _appDeleteMultipleTestCase.Do(list);
        //}   

        [HttpPost("run")]
        public async Task Run(Guid caseId)
        {
            await _appRunTestCase.Do(caseId);
        }

        [HttpPost("stop")]
        public async Task Stop(Guid caseId)
        {
            await _appStopTestCase.Do(caseId);
        }

        [HttpGet("checkstatus")]
        public async Task<bool> CheckStatus(Guid caseId)
        {
            return await _appCheckTestCaseStatus.Do(caseId);
        }

        [HttpGet("querytestcasestatus")]
        public async Task<TestCaseStatus> QueryTestCaseStatus(Guid caseId)
        {
            return await _appQueryTestCaseStatus.Do(caseId);
        }

        //获取主机或者从机Log
        [HttpGet("getmasterlog")]
        public async Task<string> GetMasterLog(Guid caseId)
        {
             return await _appQueryMasterLog.Do(caseId);
        }

        [HttpGet("getslavelog")]
        public async Task<string> GetSlaveLog(Guid caseId, Guid slaveHostId, int idx)
        {
            return await _appQuerySlaveLog.Do(caseId, slaveHostId, idx);
        }

        //查询增加修改删除SlaveHost
        [EnableCors]
        [HttpGet("queryslavehosts")]
        public async Task<List<TestCaseSlaveHostViewData>> GetAllSlaveHosts(Guid caseId)
        {
            return await _appQuerySlaveHost.Do(caseId);
        }

        [HttpPost("addslavehost")]
        public async Task<TestCaseSlaveHostViewData> AddSlaveHost(TestCaseSlaveHostAddModel slaveHost)
        {
            return await _appAddSlaveHost.Do(slaveHost);
        }

        [HttpPut("updateslavehost")]
        public async Task<TestCaseSlaveHostViewData> UpdateSlaveHost(TestCaseSlaveHostUpdateModel slaveHost)
        {
            return await _appUpdateSlaveHost.Do(slaveHost);
        }

        [HttpDelete("deleteslavehost")]
        public async Task DeleteSlaveHost(Guid caseId,Guid id)
        {
            await _appDeleteSlaveHost.Do(caseId, id);
        }

        [HttpDelete("deleteslavehosts")]
        public async Task DeleteSlaveHosts(MultipleDeleteModel model)
        {
            await _appDeleteSlaveHosts.Do(model.CaseID, model.IDS);
        }

        //获得历史记录列表
        [HttpGet("histories")]
        public async Task<QueryResult<TestCaseHistoryListViewData>> GetHistories(Guid caseID, int page, int pageSize)
        {
            return await _appQueryTestCaseHistory.Do(caseID, page, pageSize);
        }

        //获得历史记录列表
        [HttpPost("selectedhistories")]
        public async Task<List<TestCaseHistoryDetailViewData>> GetHistoriesByIds(MultipleDeleteModel model)
        {
            return await _appQueryHistoriesByIds.Do(model.CaseID, model.IDS);
        }

        [HttpGet("history")]
        public async Task<TestCaseHistoryDetailViewData> GetHistory(Guid caseId, Guid historyId)
        {
            return await _appQuerySingleTestCaseHistory.Do(caseId, historyId);
        }

        [HttpDelete("deletehistory")]
        public async Task DeleteHistory(Guid caseId, Guid historyId)
        {
            await _appDeleteTestCaseHistory.Do(caseId, historyId);
        }

        [HttpDelete("deletehistories")]
        public async Task DeleteMultipleHistories(MultipleDeleteModel model)
        {
            await _appDeleteHistories.Do(model.CaseID, model.IDS);
        }

        [HttpGet("transfernetgatewaydatafile")]
        public async Task<int> TransferNetGatewayDataFile(Guid caseId, Guid historyId)
        {
            return await _appTransferNetGatewayDataFile.Do(caseId, historyId);
        }

        [HttpGet("checkdataanalysisstatus")]
        public async Task<NetGatewayDataFileStatus> CheckNetGatewayDataAnalysisStatus(Guid caseId, Guid historyId)
        {
            return await _appCheckNetGatewayDataAnalysisStatus.Do(caseId, historyId);
        }

        [HttpPost("updatenetgatewaydataformat")]
        public async Task UpdateNetGatewayDataFormat(TestCaseHistoryUpdateData data)
        {
            await _appUpdateNetGatewayDataFormat.Do(data);
        }

        [HttpGet("getnetgatewaydataformattypes")]
        public List<string> GetNetGatewayDataFormatTypes()
        {
            return _appGetNetGatewayDataFormatTypes.Do();
        }
    }
}
