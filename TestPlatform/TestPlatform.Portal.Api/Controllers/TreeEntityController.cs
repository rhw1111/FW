﻿using System;
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
    [Route("api/treeentity")]
    [ApiController]
    [EnableCors]
    public class TreeEntityController : ControllerBase
    {
        private const int _pageSize = 50;

        private readonly IAppQueryTreeEntityChildren _appQueryTreeEntityChildren;
        private readonly IAppQueryTreeEntities _appQueryTreeEntities;
        private readonly IAppGoBackPrevious _appGoBackPrevious;
        private readonly IAppAddTreeEntity _appAddTreeEntity;
        private readonly IAppUpdateTreeEntityParent _appUpdateTreeEntityParent;
        private readonly IAppUpdateTreeEntityName _appUpdateTreeEntityName;
        private readonly IAppQueryChild _appQueryChild;
        private readonly IAppDeleteTreeEntity _appDeleteTreeEntity;

        public TreeEntityController(IAppQueryTreeEntityChildren appQueryTreeEntityChildren, IAppQueryTreeEntities appQueryTreeEntities, IAppGoBackPrevious appGoBackPrevious, IAppAddTreeEntity appAddTreeEntity,
            IAppUpdateTreeEntityParent appUpdateTreeEntityParent, IAppUpdateTreeEntityName appUpdateTreeEntityName, IAppQueryChild appQueryChild, IAppDeleteTreeEntity appDeleteTreeEntity)
        {
            _appQueryTreeEntityChildren = appQueryTreeEntityChildren;
            _appQueryTreeEntities = appQueryTreeEntities;
            _appGoBackPrevious = appGoBackPrevious;
            _appAddTreeEntity = appAddTreeEntity;
            _appUpdateTreeEntityParent = appUpdateTreeEntityParent;
            _appUpdateTreeEntityName = appUpdateTreeEntityName;
            _appQueryChild = appQueryChild;
            _appDeleteTreeEntity = appDeleteTreeEntity;
        }

        [HttpGet("querybypage")]
        public async Task<QueryResult<TreeEntityViewModel>> QueryByPage(string? matchName,int? type, int page, int? pageSize)
        {
            if (matchName == null)
                matchName = "";
            if (pageSize == null)
            {
                pageSize = _pageSize;
            }
            return await _appQueryTreeEntities.Do(matchName,type, page, (int)pageSize);
        }

        [HttpGet("querychildren")]
        public async Task<QueryResult<TreeEntityViewModel>> QueryChildren(Guid? parentId, string? matchName, int? type, int page, int? pageSize)
        {
            if (matchName == null)
                matchName = "";
            if (pageSize == null)
            {
                pageSize = _pageSize;
            }
            return await _appQueryTreeEntityChildren.Do(parentId, matchName, type, page, (int)pageSize);
        }

        [HttpGet("gobackprevious")]
        public async Task<QueryResult<TreeEntityViewModel>> GoBackPrevious(Guid parentId, int page, int? pageSize)
        {
            if (pageSize == null)
            {
                pageSize = _pageSize;
            }
            return await _appGoBackPrevious.Do(parentId, page, (int)pageSize);
        }

        [HttpPost("add")]
        public async Task<TreeEntityViewModel> Add(TreeEntityAddModel model)
        {
            return await _appAddTreeEntity.Do(model);
        }

        [HttpGet("querychild")]
        public async Task<TreeEntityViewModel?> QueryChild(Guid? parentId, string name)
        {
            return await _appQueryChild.Do(parentId, name);
        }

        [HttpDelete("delete")]
        public async Task delete(Guid id)
        {
            await _appDeleteTreeEntity.Do(id);
        }

        [HttpPut("UpdateParent")]
        public async Task UpdateParent(Guid id, Guid? parentId)
        {
            await _appUpdateTreeEntityParent.Do(id, parentId);
        }

        [HttpPut("UpdateName")]
        public async Task UpdateName(Guid id, string name)
        {
            await _appUpdateTreeEntityName.Do(id, name);
        }
    }
}
