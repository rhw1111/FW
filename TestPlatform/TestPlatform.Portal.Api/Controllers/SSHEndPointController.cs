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
    [Route("api/sshendpoint")]
    [ApiController]
    [EnableCors]
    public class SSHEndpointController : ControllerBase
    {
        private const int _pageSize = 50;

        private readonly IAppAddSSHEndPoint _appAddSSHEndPoint;
        private readonly IAppQuerySingleSSHEndPoint _appQuerySingleSSHEndPoint;

        public SSHEndpointController(IAppAddSSHEndPoint appAddSSHEndPoint, IAppQuerySingleSSHEndPoint appQuerySingleSSHEndPoint)
        {
            _appAddSSHEndPoint = appAddSSHEndPoint;
            _appQuerySingleSSHEndPoint = appQuerySingleSSHEndPoint;
        }

        //[HttpGet("querybypage")]
        //public async Task<QueryResult<TestDataSourceViewData>> GetByPage(string? matchName,int page, int? pageSize)
        //{
        //    if (matchName == null)
        //        matchName = "";
        //    if(pageSize == null)
        //    {
        //        pageSize = _pageSize;
        //    }
        //    return await _appQueryTestDataSource.Do(matchName, page, (int)pageSize);
        //}

        [HttpGet("sshendpoint")]
        public async Task<SSHEndPointViewData> GetSSHEndpoint(Guid id)
        {
            return await _appQuerySingleSSHEndPoint.Do(id);
        }

        [HttpPost("add")]
        public async Task<SSHEndPointViewData> Add(SSHEndPointAddModel model)
        {
             return await _appAddSSHEndPoint.Do(model);
        }
        //[HttpPut("update")]
        //public async Task<TestDataSourceViewData> Update(TestDataSourceUpdateModel model)
        //{
        //    return await _appUpdateTestDataSource.Do(model);
        //}
        //[HttpDelete("delete")]
        //public async Task Delete(Guid id)
        //{
        //    await _appDeleteTestDataSource.Do(id);
        //}

        //[HttpDelete("deletemultiple")]
        //public async Task DeleteMutiple(List<Guid> ids)
        //{
        //    await _appDeleteMultipleTestDataSource.Do(ids);
        //}
    }
}
