using System;
using System.Data.Common;

namespace MSLibrary.Workflow.DAL
{
    public class StoreHelper
    {
        /// <summary>
        /// 获取工作流资源数据操作
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetWorkflowResourceStoreSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                              {0}.[type] as [{0}type],
                              {0}.[key] as [{0}key],
                              {0}.[status] as [{0}status],
                              {0}.[initstatus] as [{0}initstatus],
                              {0}.[createtime] as [{0}createtime],
                              {0}.[modifytime] as [{0}modifytime],
                              {0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],
                              [type],
                              [key],
                              [status],
                              [initstatus],
                              [createtime],
                              [modifytime],
                              [sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 赋值工作流资源数据操作
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetWorkflowResourceStoreSelectFields(WorkflowResource resource, DbDataReader reader, string prefix)
        {
            resource.ID = (Guid)reader[string.Format("{0}id", prefix)];
            resource.Type = reader[string.Format("{0}type", prefix)].ToString();
            resource.Key = reader[string.Format("{0}key", prefix)].ToString();
            resource.Status = (int)reader[string.Format("{0}status", prefix)];
            resource.InitStatus = (int)reader[string.Format("{0}initstatus", prefix)];
            resource.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            resource.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }

        /// <summary>
        /// 获取工作流步骤数据操作
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetWorkflowStepStoreSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                              {0}.[resourceid] as [{0}resourceid],
                              {0}.[actionname] as [{0}actionname],
                              {0}.[status] as [{0}status],
                              {0}.[serialno] as [{0}serialno],
                              {0}.[usertype] as [{0}usertype],
                              {0}.[userkey] as [{0}userkey],
                              {0}.[usercount] as [{0}usercount],
                              {0}.[complete] as [{0}complete],
                              {0}.[createtime] as [{0}createtime],
                              {0}.[modifytime] as [{0}modifytime],
                              {0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],
                              [resourceid],
                              [actionname],
                              [status],
                              [serialno],
                              [usertype],
                              [userkey],
                              [usercount],
                              [complete],
                              [createtime],
                              [modifytime],
                              [sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 赋值工作流步骤数据操作
        /// </summary>
        /// <param name="step"></param>
        /// <param name="reader"></param>
        /// <param name="prefixForResource"></param>
        /// <param name="prefixForStep"></param>
        public static void SetWorkflowStepStoreSelectFields(WorkflowStep step, DbDataReader reader, string prefixForResource, string prefixForStep)
        {
            var wfResource = new WorkflowResource();
            SetWorkflowResourceStoreSelectFields(wfResource, reader, prefixForResource);

            step.ID = (Guid)reader[string.Format("{0}id", prefixForStep)];
            step.ResourceID = (Guid)reader[string.Format("{0}resourceid", prefixForStep)];
            step.Resource = wfResource;
            step.ActionName = reader[string.Format("{0}actionname", prefixForStep)].ToString();
            step.Status = (int)reader[string.Format("{0}status", prefixForStep)];
            step.SerialNo = reader[string.Format("{0}serialno", prefixForStep)].ToString();
            step.UserType = reader[string.Format("{0}usertype", prefixForStep)].ToString();
            step.UserKey = reader[string.Format("{0}userkey", prefixForStep)].ToString();
            step.UserCount = (int)reader[string.Format("{0}usercount", prefixForStep)];
            step.Complete = (bool)reader[string.Format("{0}complete", prefixForStep)];
            step.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefixForStep)];
            step.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefixForStep)];
        }

        /// <summary>
        /// 获取工作流用户动作数据操作
        /// </summary>
        /// <returns></returns>
        public static string GetWorkflowStepUserActionStoreSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                              {0}.[stepid] as [{0}stepid],
                              {0}.[userkey] as [{0}userkey],
                              {0}.[result] as [{0}result],
                              {0}.[createtime] as [{0}createtime],
                              {0}.[modifytime] as [{0}modifytime],
                              {0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],
                              [stepid],
                              [userkey],
                              [result],
                              [createtime],
                              [modifytime],
                              [sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 赋值工作流用户动作数据操作
        /// </summary>
        public static void SetWorkflowStepUserActionStoreSelectFields(WorkflowStepUserAction action, DbDataReader reader, string prefix)
        {
            action.ID = (Guid)reader[string.Format("{0}id", prefix)];
            action.StepID = (Guid)reader[string.Format("{0}stepid", prefix)];
            action.UserKey = reader[string.Format("{0}userkey", prefix)].ToString();
            action.Result = (int)reader[string.Format("{0}result", prefix)];
            action.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            action.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }


        /// <summary>
        /// 获取通用审批配置数据操作
        /// </summary>
        /// <returns></returns>
        public static string GetCommonSignConfigurationSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                              {0}.[workflowresourcetype] as [{0}workflowresourcetype],
                              {0}.[entitytype] as [{0}entitytype],          
                              {0}.[workflowresourcedefaultcompletestatus] as [{0}workflowresourcedefaultcompletestatus],
                              {0}.[completeserviceconfiguration] as [{0}completeserviceconfiguration],
                              {0}.[completeservicename] as [{0}completeservicename],
                              {0}.[createtime] as [{0}createtime],
                              {0}.[modifytime] as [{0}modifytime],
                              {0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id]
                               ,[workflowresourcetype]
                               ,[entitytype]
                               ,[workflowresourcedefaultcompletestatus]
                               ,[completeserviceconfiguration]
                               ,[completeservicename]
                               ,[createtime]
                               ,[modifytime]
                                ,[sequence] ";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 赋值通用审批配置数据操作
        /// </summary>
        public static void SetCommonSignConfigurationSelectFields(CommonSignConfiguration data, DbDataReader reader, string prefix)
        {
            data.ID = (Guid)reader[string.Format("{0}id", prefix)];
            data.WorkflowResourceType = reader[string.Format("{0}workflowresourcetype", prefix)].ToString();
            data.EntityType = reader[string.Format("{0}entitytype", prefix)].ToString();
            data.WorkflowResourceDefaultCompleteStatus = (int)reader[string.Format("{0}workflowresourcedefaultcompletestatus", prefix)];
            data.CompleteServiceConfiguration = reader[string.Format("{0}completeserviceconfiguration", prefix)].ToString();
            data.CompleteServiceName = reader[string.Format("{0}completeservicename", prefix)].ToString();
            data.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            data.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }


        /// <summary>
        /// 获取通用审批配置起始动作数据操作
        /// </summary>
        /// <returns></returns>
        public static string GetCommonSignConfigurationRootActionSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                              {0}.[configurationid] as [{0}configurationid],
                              {0}.[actionname] as [{0}actionname],
                              {0}.[workflowresourceinitstatus] as [{0}workflowresourceinitstatus],
                              {0}.[workflowresourcedefaultcompletestatus] as [{0}workflowresourcedefaultcompletestatus],
                              {0}.[entrynodeid] as [{0}entrynodeid],
                              {0}.[entrynodefindserviceconfiguration] as [{0}entrynodefindserviceconfiguration],
                              {0}.[entrynodefindservicename] as [{0}entrynodefindservicename],
                              {0}.[entryserviceconfiguration] as [{0}entryserviceconfiguration],
                              {0}.[entryservicename] as [{0}entryservicename],
                              {0}.[entrygetexecuteusersserviceconfiguration] as [{0}entrygetexecuteusersserviceconfiguration],
                              {0}.[entrygetexecuteusersservicename] as [{0}entrygetexecuteusersservicename],
                              {0}.[createtime] as [{0}createtime],
                              {0}.[modifytime] as [{0}modifytime],
                              {0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id]
                              ,[configurationid]
                              ,[actionname]
                              ,[workflowresourceinitstatus]
                              ,[workflowresourcedefaultcompletestatus]
                              ,[entrynodeid]
                              ,[entrynodefindserviceconfiguration]
                              ,[entrynodefindservicename]
                              ,[entryserviceconfiguration]
                              ,[entryservicename]
                              ,[entrygetexecuteusersserviceconfiguration]
                              ,[entrygetexecuteusersservicename]
                              ,[createtime]
                              ,[modifytime]
                              ,[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 赋值通用审批配置节点数据操作
        /// </summary>
        public static void SetCommonSignConfigurationRootActionSelectFields(CommonSignConfigurationRootAction data, DbDataReader reader, string prefix)
        {
            data.ID = (Guid)reader[string.Format("{0}id", prefix)];
            data.ConfigurationId = (Guid)reader[string.Format("{0}configurationid", prefix)];
            data.ActionName = reader[string.Format("{0}actionname", prefix)].ToString();
            data.WorkflowResourceInitStatus = (int)reader[string.Format("{0}workflowresourceinitstatus", prefix)];
            data.WorkflowResourceDefaultCompleteStatus = (int)reader[string.Format("{0}workflowresourcedefaultcompletestatus", prefix)];
            if (reader[string.Format("{0}entrynodeid", prefix)] != DBNull.Value)
            {
                data.EntryNodeId = (Guid)reader[string.Format("{0}entrynodeid", prefix)];
            }
            if (reader[string.Format("{0}entrynodefindserviceconfiguration", prefix)] != DBNull.Value)
            {
                data.EntryNodeFindServiceConfiguration = reader[string.Format("{0}entrynodefindserviceconfiguration", prefix)].ToString();
            }
            if (reader[string.Format("{0}entrynodefindservicename", prefix)] != DBNull.Value)
            {
                data.EntryNodeFindServiceName = reader[string.Format("{0}entrynodefindservicename", prefix)].ToString();
            }
            if (reader[string.Format("{0}entryserviceconfiguration", prefix)] != DBNull.Value)
            {
                data.EntryServiceConfiguration = reader[string.Format("{0}entryserviceconfiguration", prefix)].ToString();
            }
            if (reader[string.Format("{0}entryservicename", prefix)] != DBNull.Value)
            {
                data.EntryServiceName = reader[string.Format("{0}entryservicename", prefix)].ToString();
            }
            if (reader[string.Format("{0}entrygetexecuteusersserviceconfiguration", prefix)] != DBNull.Value)
            {
                data.EntryGetExecuteUsersServiceConfiguration = reader[string.Format("{0}entrygetexecuteusersserviceconfiguration", prefix)].ToString();
            }
            if (reader[string.Format("{0}entrygetexecuteusersservicename", prefix)] != DBNull.Value)
            {
                data.EntryGetExecuteUsersServiceName = reader[string.Format("{0}entrygetexecuteusersservicename", prefix)].ToString();
            }
            data.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            data.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }


        /// <summary>
        /// 获取通用审批配置节点数据操作
        /// </summary>
        /// <returns></returns>
        public static string GetCommonSignConfigurationNodeSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                              {0}.[configurationid] as [{0}configurationid],
                              {0}.[name] as [{0}name],
                              {0}.[workflowstatus] as [{0}workflowstatus],
                              {0}.[directgoexecuteserviceconfiguration] as [{0}directgoexecuteserviceconfiguration],
                              {0}.[directgoexecuteservicename] as [{0}directgoexecuteservicename],
                              {0}.[workflowstatus] as [{0}workflowstatus],
                              {0}.[Status] as [{0}Status],
                              {0}.[createtime] as [{0}createtime],
                              {0}.[modifytime] as [{0}modifytime],
                              {0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id]
                              ,[configurationid]
                              ,[name]
                              ,[workflowstatus]
                              ,[directgoexecuteserviceconfiguration]
                              ,[directgoexecuteservicename]
                              ,[Status]
                              ,[createtime]
                              ,[modifytime]
                               ,[sequence] ";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 赋值通用审批配置起始动作数据操作
        /// </summary>
        public static void SetCommonSignConfigurationNodeSelectFields(CommonSignConfigurationNode data, DbDataReader reader, string prefix)
        {
            data.ID = (Guid)reader[string.Format("{0}id", prefix)];
            data.ConfigurationId = (Guid)reader[string.Format("{0}configurationid", prefix)];
            data.Name = reader[string.Format("{0}name", prefix)].ToString();
            data.WorkflowStatus = (int)reader[string.Format("{0}workflowstatus", prefix)];
            if (reader[string.Format("{0}directgoexecuteserviceconfiguration", prefix)] != DBNull.Value)
            {
                data.DirectGoExecuteServiceConfiguration = reader[string.Format("{0}directgoexecuteserviceconfiguration", prefix)].ToString();
            }
            if (reader[string.Format("{0}directgoexecuteservicename", prefix)] != DBNull.Value)
            {
                data.DirectGoExecuteServiceName = reader[string.Format("{0}directgoexecuteservicename", prefix)].ToString();
            }
            data.Status = (int)reader[string.Format("{0}Status", prefix)];
            data.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            data.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }

        /// <summary>
        /// 获取通用审批配置节点动作数据操作
        /// </summary>
        /// <returns></returns>
        public static string GetCommonSignConfigurationNodeActionSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                              {0}.[nodeid] as [{0}nodeid],
                              {0}.[actionname] as [{0}actionname],
                              {0}.[executeuserismanual] as [{0}executeuserismanual],
                              {0}.[executeusergetserviceconfiguration] as [{0}executeusergetserviceconfiguration],
                              {0}.[executeusergetservicename] as [{0}executeusergetservicename],
                              {0}.[createclowexecuteserviceconfiguration] as [{0}createclowexecuteserviceconfiguration],
                              {0}.[createclowexecuteservicename] as [{0}createclowexecuteservicename],
                              {0}.[signtype] as [{0}signtype],
                              {0}.[signtypeconfiguration] as [{0}signtypeconfiguration],
                              {0}.[createtime] as [{0}createtime],
                              {0}.[modifytime] as [{0}modifytime],
                              {0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id]
                              ,[nodeid]
                              ,[actionname]
                              ,[executeuserismanual]
                              ,[executeusergetserviceconfiguration]
                              ,[executeusergetservicename]
                              ,[createclowexecuteserviceconfiguration]
                              ,[createclowexecuteservicename]
                              ,[signtype]
                              ,[signtypeconfiguration]
                              ,[createtime]
                              ,[modifytime]
                              ,[sequence] ";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 赋值通用审批配置节点动作数据操作
        /// </summary>
        public static void SetCommonSignConfigurationNodeActionSelectFields(CommonSignConfigurationNodeAction data, DbDataReader reader, string prefix)
        {
            data.ID = (Guid)reader[string.Format("{0}id", prefix)];
            data.NodeId = (Guid)reader[string.Format("{0}nodeid", prefix)];
            data.ActionName = reader[string.Format("{0}actionname", prefix)].ToString();
            data.ExecuteUserIsManual = (bool)reader[string.Format("{0}executeuserismanual", prefix)];
            if (reader[string.Format("{0}executeusergetserviceconfiguration", prefix)] != DBNull.Value)
            {
                data.ExecuteUserGetServiceConfiguration = reader[string.Format("{0}executeusergetserviceconfiguration", prefix)].ToString();
            }
            if (reader[string.Format("{0}executeusergetservicename", prefix)] != DBNull.Value)
            {
                data.ExecuteUserGetServiceName = reader[string.Format("{0}executeusergetservicename", prefix)].ToString();
            }
            if (reader[string.Format("{0}createclowexecuteserviceconfiguration", prefix)] != DBNull.Value)
            {
                data.CreateFlowExecuteServiceConfiguration = reader[string.Format("{0}createclowexecuteserviceconfiguration", prefix)].ToString();
            }
            if (reader[string.Format("{0}createclowexecuteservicename", prefix)] != DBNull.Value)
            {
                data.CreateFlowExecuteServiceName = reader[string.Format("{0}createclowexecuteservicename", prefix)].ToString();
            }
            data.SignType = reader[string.Format("{0}signtype", prefix)].ToString();
            data.SignTypeConfiguration = reader[string.Format("{0}signtypeconfiguration", prefix)].ToString();
            data.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            data.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }
    }
}
