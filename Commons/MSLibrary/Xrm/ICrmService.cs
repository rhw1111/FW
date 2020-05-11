using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Xrm
{
    /// <summary>
    /// 与CRM交互的服务接口
    /// </summary>
    public interface ICrmService
    {
        /// <summary>
        /// 创建实体记录
        /// </summary>
        /// <param name="entity">要创建的实体记录内容</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <returns>创建的实体记录的Id</returns>
        Task<Guid> Create(CrmExecuteEntity entity,Guid? proxyUserId=null);
        /// <summary>
        /// 创建实体记录，返回指定属性集合的记录内容
        /// </summary>
        /// <param name="entity">要创建的实体记录内容</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="headers">附加请求头</param>
        /// <param name="attributes">要返回的属性集合</param>
        /// <returns></returns>
        Task<CrmEntity> Create(CrmExecuteEntity entity, Guid? proxyUserId = null,Dictionary<string, IEnumerable<string>> headers=null, params string[] attributes);
        /// <summary>
        /// 修改实体记录
        /// </summary>
        /// <param name="entity">要修改的实体记录内容</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <returns></returns>
        Task Update(CrmExecuteEntity entity, Guid? proxyUserId = null);


        /// <summary>
        /// 根据唯一键修改实体记录
        /// </summary>
        /// <param name="entity">要修改的实体记录内容</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <returns></returns>
        Task UpdateAlternate(CrmExecuteEntity entity,Dictionary<string,object> alternateKeys, Guid? proxyUserId = null);


        /// <summary>
        /// 修改实体记录，返回指定属性集合的记录内容
        /// </summary>
        /// <param name="entity">要修改的实体记录内容</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="headers">附加请求头</param>
        /// <param name="attributes">要返回的属性集合</param>
        /// <returns></returns>
        Task<CrmEntity> Update(CrmExecuteEntity entity, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null, params string[] attributes);

        /// <summary>
        /// 根据唯一键修改实体记录
        /// </summary>
        /// <param name="entity">要修改的实体记录内容</param>
        /// <param name="alternateKeys">唯一键键值对</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="headers">附加请求头</param>
        /// <param name="attributes">要返回的属性集合</param>
        /// <returns></returns>
        Task<CrmEntity> UpdateAlternate(CrmExecuteEntity entity, Dictionary<string, object> alternateKeys, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null, params string[] attributes);


        /// <summary>
        /// 修改或创建实体记录
        /// </summary>
        /// <param name="entity">要修改或创建的实体记录内容</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <returns></returns>
        Task Upsert(CrmExecuteEntity entity, Guid? proxyUserId = null);


        /// <summary>
        /// 根据唯一键修改或创建实体记录
        /// </summary>
        /// <param name="entity">要修改或创建的实体记录内容</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <returns></returns>
        Task UpsertAlternate(CrmExecuteEntity entity, Dictionary<string, object> alternateKeys, Guid? proxyUserId = null);


        /// <summary>
        /// 修改或创建实体记录，返回指定属性集合的记录内容
        /// </summary>
        /// <param name="entity">要修改或创建的实体记录内容</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="headers">附加请求头</param>
        /// <param name="attributes">要返回的属性集合</param>
        /// <returns></returns>
        Task<CrmEntity> Upsert(CrmExecuteEntity entity, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null, params string[] attributes);

        /// <summary>
        /// 根据唯一键修改或创建实体记录
        /// </summary>
        /// <param name="entity">要修改或创建的实体记录内容</param>
        /// <param name="alternateKeys">唯一键键值对</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="headers">附加请求头</param>
        /// <param name="attributes">要返回的属性集合</param>
        /// <returns></returns>
        Task<CrmEntity> UpsertAlternate(CrmExecuteEntity entity, Dictionary<string, object> alternateKeys, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null, params string[] attributes);



        /// <summary>
        /// 获取指定Id的实体记录
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="entityId">实体记录Id</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="headers">附加请求头</param>
        /// <param name="attributes">要返回的属性集合</param>
        /// <returns>实体记录内容</returns>
        Task<CrmEntity> Retrieve(string entityName, Guid entityId, string queryExpression, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null);
        /// <summary>
        /// 根据唯一键查询
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="alternateKeys">唯一键键值对</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="headers">附加请求头</param>
        /// <param name="attributes">要返回的属性集合</param>
        /// <returns>实体记录内容</returns>
        Task<CrmEntity> RetrieveAlternate(string entityName, Dictionary<string,object> alternateKeys, string queryExpression, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null);
        /// <summary>
        /// 删除指定Id的实体记录
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="entityId">实体记录Id</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <returns></returns>
        Task Delete(string entityName, Guid entityId, Guid? proxyUserId = null);
        /// <summary>
        /// 根据唯一键删除
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="alternateKeys">唯一键键值对</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <returns></returns>
        Task DeleteAlternate(string entityName, Dictionary<string, object> alternateKeys, Guid? proxyUserId = null);
        /// <summary>
        /// 实体记录关联
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="associateEntityName">要关联的实体名称</param>
        /// <param name="relationName">关联关系名称（1:N为集合属性名称，N:N为关联名称）</param>
        /// <param name="entityId">实体记录Id</param>
        /// <param name="associateEntityId">要关联的实体记录Id</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <returns></returns>
        Task Associate(string entityName,string associateEntityName,string relationName,Guid entityId,Guid associateEntityId, Guid? proxyUserId = null);
        /// <summary>
        /// 实体记录取消关联
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="relationName">关联关系名称（1:N为集合属性名称，N:N为关联名称）</param>
        /// <param name="entityId">实体记录Id</param>
        /// <param name="associateEntityId">要关联的实体记录Id</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <returns></returns>
        Task DisAssociate(string entityName, string relationName, Guid entityId, Guid associateEntityId, Guid? proxyUserId = null);
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="queryExpression">查询表达式</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="headers">附加请求头</param>
        /// <returns></returns>
        Task<CrmEntityCollection> RetrieveMultiple(string entityName,string queryExpression, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null);
        /// <summary>
        /// 按指定每页数量查询
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="queryExpression">查询表达式</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="headers">附加请求头</param>
        /// <returns></returns>
        Task<CrmEntityCollection> RetrieveMultiplePage(string entityName, string queryExpression,int pageSize, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null);
        /// <summary>
        /// 根据下一页表达式查询
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="nextLinkExpression">下一页表达式</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="headers">附加请求头</param>
        /// <returns></returns>
        Task<CrmEntityCollection> RetrieveMultipleNextPage(string entityName, string nextLinkExpression, int pageSize, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null);
        /// <summary>
        /// 根据SaveQuery查询
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="saveQueryId">SaveQuery的id</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="headers">附加请求头</param>
        /// <returns></returns>
        Task<CrmEntityCollection> RetrieveMultipleSavedQuery(string entityName, Guid saveQueryId, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null);

        /// <summary>
        /// 根据UserQuery查询
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="userQueryId">UserQuery的Id</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="headers">附加请求头</param>
        /// <returns></returns>
        Task<CrmEntityCollection> RetrieveMultipleUserQuery(string entityName, Guid userQueryId, Guid? proxyUserId = null, Dictionary<string, IEnumerable<string>> headers = null);

        /// <summary>
        /// 执行绑定函数
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="entityId">实体记录Id</param>
        /// <param name="functionName">函数名称</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="parameters">参数集合</param>
        /// <returns></returns>
        Task<JObject> ExecuteBoundFunction(string entityName,Guid entityId,string functionName, Guid? proxyUserId = null, params CrmFunctionParameter[] parameters);
        /// <summary>
        /// 执行非绑定函数
        /// </summary>
        /// <param name="functionName">函数名称</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="parameters">参数集合</param>
        /// <returns></returns>
        Task<JObject> ExecuteUnBoundFunction(string functionName, Guid? proxyUserId = null, params CrmFunctionParameter[] parameters);
        /// <summary>
        /// 执行绑定动作(无返回值)
        /// </summary>
        /// <param name="entityName">函数名称</param>
        /// <param name="entityId">实体记录Id</param>
        /// <param name="actionName">动作名称</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="parameters">参数集合</param>
        /// <returns></returns>
        Task ExecuteBoundActionVoid(string entityName, Guid entityId, string actionName, Guid? proxyUserId = null, params CrmActionParameter[] parameters);

        /// <summary>
        /// 执行绑定动作（有返回值）
        /// </summary>
        /// <param name="entityName">函数名称</param>
        /// <param name="entityId">实体记录Id</param>
        /// <param name="actionName">动作名称</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="parameters">参数集合</param>
        /// <returns></returns>
        Task<JObject> ExecuteBoundAction(string entityName, Guid entityId, string actionName, Guid? proxyUserId = null, params CrmActionParameter[] parameters);

        /// <summary>
        /// 执行无绑定动作(无返回值)
        /// </summary>
        /// <param name="actionName">动作名称</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="parameters">参数集合</param>
        /// <returns></returns>
        Task ExecuteUnBoundActionVoid(string actionName, Guid? proxyUserId = null, params CrmActionParameter[] parameters);

        /// <summary>
        /// 执行无绑定动作（有返回值）
        /// </summary>
        /// <param name="actionName">动作名称</param>
        /// <param name="proxyUserId">要被代理的用户Id</param>
        /// <param name="parameters">参数集合</param>
        /// <returns></returns>
        Task<JObject> ExecuteUnBoundAction(string actionName, Guid? proxyUserId = null, params CrmActionParameter[] parameters);
        /// <summary>
        /// 执行消息
        /// </summary>
        /// <param name="request">请求消息</param>
        /// <returns>响应消息</returns>
        Task<CrmResponseMessage> Execute(CrmRequestMessage request);


        /// <summary>
        /// 为文件类型属性上传文件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fileAttributeName"></param>
        /// <param name="fileName"></param>
        /// <param name="fileStream"></param>
        Task UploadAttributeFile(CrmEntityReference entityID, string fileAttributeName, string fileName, string fileMimeType, Stream fileStream, Guid? proxyUserId = null);
        /// <summary>
        /// 下载文件类型属性的文件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fileAttributeName"></param>
        /// <param name="action"></param>
        Task DownloadAttributeFile(CrmEntityReference entityID, string fileAttributeName, Func<string, Stream,Task> action, Guid? proxyUserId = null);
        /// <summary>
        /// 删除文件类型属性的文件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fileAttributeName"></param>
        Task DeleteAttributeFileData(CrmEntityReference entityID, string fileAttributeName, Guid? proxyUserId = null);


    }



}
