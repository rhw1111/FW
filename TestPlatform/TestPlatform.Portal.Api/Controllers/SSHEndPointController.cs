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
        private readonly IAppDeleteSSHEndPoints _appDeleteSSHEndPoints;
        private readonly IAppQuerySSHEndPointByPage _appQuerySSHEndPointByPage;
        private readonly IAppUpdateSSHEndpoint _appUpdateSSHEndpoint;
        private readonly IAppDeleteSSHEndPoint _appDeleteSSHEndPoint;

        public SSHEndpointController(IAppAddSSHEndPoint appAddSSHEndPoint, IAppQuerySingleSSHEndPoint appQuerySingleSSHEndPoint, IAppDeleteSSHEndPoints appDeleteSSHEndPoints, IAppQuerySSHEndPointByPage appQuerySSHEndPointByPage, 
            IAppUpdateSSHEndpoint appUpdateSSHEndpoint, IAppDeleteSSHEndPoint appDeleteSSHEndPoint)
        {
            _appAddSSHEndPoint = appAddSSHEndPoint;
            _appQuerySingleSSHEndPoint = appQuerySingleSSHEndPoint;
            _appDeleteSSHEndPoints = appDeleteSSHEndPoints;
            _appQuerySSHEndPointByPage = appQuerySSHEndPointByPage;
            _appUpdateSSHEndpoint = appUpdateSSHEndpoint;
            _appDeleteSSHEndPoint = appDeleteSSHEndPoint;
        }

        [HttpGet("querybypage")]
        public async Task<QueryResult<SSHEndPointViewData>> GetByPage(string? matchName, int page, int? pageSize)
        {
            if (matchName == null)
                matchName = "";
            if (pageSize == null)
            {
                pageSize = _pageSize;
            }
            return await _appQuerySSHEndPointByPage.Do(matchName, page, (int)pageSize);
        }

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
        [HttpPut("update")]
        public async Task<SSHEndPointViewData> Update(SSHEndPointUpdateModel model)
        {
            return await _appUpdateSSHEndpoint.Do(model);
        }
        [HttpDelete("delete")]
        public async Task Delete(Guid id)
        {
            await _appDeleteSSHEndPoint.Do(id);
        }

        [HttpDelete("deletemultiple")]
        public async Task DeleteMutiple(List<Guid> ids)
        {
            await _appDeleteSSHEndPoints.Do(ids);
        }
    }
}
