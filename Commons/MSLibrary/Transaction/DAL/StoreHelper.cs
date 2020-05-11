using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.Transaction.DAL
{
    /// <summary>
    /// 数据存储帮助类
    /// 统一管理查询赋值
    /// </summary>
    public static class StoreHelper
    {

        /// <summary>
        /// 获取分布式操作记录实体的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetDTOperationRecordSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[uniquename] as [{0}uniquename],{0}.[type] as [{0}type],{0}.[typeinfo] as [{0}typeinfo],{0}.[storegroupname] as [{0}storegroupname],{0}.[hashinfo] as [{0}hashinfo],{0}.[errormessage] as [{0}errormessage],{0}.[version] as [{0}version],{0}.[status] as [{0}status],{0}.[timeout] as [{0}timeout],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[uniquename],[type],[typeinfo],[storegroupname],[hashinfo],[errormessage],[version],[status],[timeout],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }


        /// <summary>
        /// 分布式操作记录实体的赋值
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetDTOperationRecordSelectFields(DTOperationRecord record, DbDataReader reader, string prefix)
        {
            record.ID = (Guid)reader[string.Format("{0}id", prefix)];
            record.UniqueName = reader[string.Format("{0}uniquename", prefix)].ToString();
            record.StoreGroupName= reader[string.Format("{0}storegroupname", prefix)].ToString();
            record.HashInfo = reader[string.Format("{0}hashinfo", prefix)].ToString();
            record.Version = reader[string.Format("{0}version", prefix)].ToString();
            record.Status = (int)reader[string.Format("{0}status", prefix)];
            record.Type= reader[string.Format("{0}type", prefix)].ToString();  
            record.TypeInfo = reader[string.Format("{0}typeinfo", prefix)].ToString();
            record.ErrorMessage = reader[string.Format("{0}errormessage", prefix)].ToString();
            record.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            record.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];

        }



        /// <summary>
        /// 获取分布式操作数据实体的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetDTOperationDataSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[recorduniquename] as [{0}recorduniquename],{0}.[name] as [{0}name],{0}.[type] as [{0}type],{0}.[storegroupname] as [{0}storegroupname],{0}.[hashinfo] as [{0}hashinfo],{0}.[data] as [{0}data],{0}.[status] as [{0}status],{0}.[version] as [{0}version],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[recorduniquename],[type],[storegroupname],[hashinfo],[data],[status],[version],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }


        /// <summary>
        /// 分布式操作数据实体的赋值
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetDTOperationDataSelectFields(DTOperationData data, DbDataReader reader, string prefix)
        {
            data.ID = (Guid)reader[string.Format("{0}id", prefix)];
            data.RecordUniqueName = reader[string.Format("{0}recorduniquename", prefix)].ToString();
            data.Name = reader[string.Format("{0}name", prefix)].ToString();
            data.Type = reader[string.Format("{0}type", prefix)].ToString();
            data.Data = reader[string.Format("{0}data", prefix)].ToString();
            data.StoreGroupName= reader[string.Format("{0}storegroupname", prefix)].ToString();
            data.HashInfo = reader[string.Format("{0}hashinfo", prefix)].ToString();
            data.Status = (int)reader[string.Format("{0}status", prefix)];
            data.Version=(byte[])reader[string.Format("{0}version", prefix)];
            data.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            data.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }


        /// <summary>
        /// 获取分布式操作数据cancel日志的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetDTOperationDataCancelLogSelectFields(string prefix)
        {
            var strSelect = @"{0}.[dataid] as [{0}dataid],{0}.[createtime] as [{0}createtime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[dataid],[createtime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }


        /// <summary>
        /// 分布式操作数据cancel日志的赋值
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetDTOperationDataCancelLogSelectFields(DTOperationDataCancelLog log, DbDataReader reader, string prefix)
        {
            log.DataID = (Guid)reader[string.Format("{0}dataid", prefix)];
            log.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
        }


    }
}
