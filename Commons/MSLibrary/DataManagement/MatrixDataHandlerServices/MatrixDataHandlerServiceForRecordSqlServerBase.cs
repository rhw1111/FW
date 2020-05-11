using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using MSLibrary;

namespace MSLibrary.DataManagement.MatrixDataHandlerServices
{
    /// <summary>
    /// 将处理过的矩阵数据记录到SqlServer数据库中的处理基类
    /// </summary>
    public abstract class MatrixDataHandlerServiceForRecordSqlServerBase: IMatrixDataHandlerService
    {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <returns></returns>
        public abstract Task<string> GetConnectionString();
        /// <summary>
        /// 获取存放处理数据的表名
        /// </summary>
        /// <returns></returns>
        public abstract Task<string> GetDataTableName();
        /// <summary>
        /// 获取存放Guid的字段的名称
        /// </summary>
        /// <returns></returns>
        public abstract Task<string> GetGuidColumnName();
        /// <summary>
        /// 获取存放状态的字段的名称
        /// </summary>
        /// <returns></returns>
        public abstract Task<string> GetStatusColumnName();
        /// <summary>
        /// 获取存放错误的字段的名称
        /// </summary>
        /// <returns></returns>
        public abstract Task<string> GetErrorColumnName();
        /// <summary>
        /// 获取创建表的脚本
        /// </summary>
        /// <returns></returns>
        public abstract Task<string> GetTableGenerateScript();

        /// <summary>
        /// 获取新增数据记录的列名称脚本
        /// 格式为
        /// xx,xx,xx
        /// </summary>
        /// <returns></returns>
        public abstract Task<string> GetInsertColumnNameScript();
        /// <summary>
        /// 获取新增数据记录的列数据脚本
        /// </summary>
        /// <returns></returns>
        public abstract Task<string> GetInsertColumnValueScript();

        /// <summary>
        /// 实际执行的业务逻辑
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="handlerConfiguration"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public abstract Task<bool> RealExecute(MatrixDataHandlerContext context, string handlerConfiguration, MatrixDataRow row);

        /// <summary>
        /// 实际执行的Pre动作
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract Task RealPreExecute(MatrixDataHandlerContext context);

        /// <summary>
        /// 实际执行的Post动作
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract Task RealPostExecute(MatrixDataHandlerContext context);


        public async Task PreExecute(MatrixDataHandlerContext context)
        {
            //检查表是否存在，如果存在，则清空数据，如果不存在，则创建数据
            var tableName=await GetDataTableName();
            var tableGenerateScript = await GetTableGenerateScript();

            using (SqlConnection conn = new SqlConnection(await GetConnectionString()))
            {
                conn.Open();
                var tableCheck =await checkTableExist(conn, tableName);

                if (tableCheck)
                {
                    await clearTable(conn, tableName);
                }
                else
                {
                    await createTable(conn, tableGenerateScript);
                }
                
                conn.Close();
            }

            await RealPreExecute(context);

        }

        public async Task<bool> Execute(MatrixDataHandlerContext context, string handlerConfiguration, MatrixDataRow row)
        {
            var strConn=await GetConnectionString();
            var tableName = await GetDataTableName();
            var guidColumn = await GetGuidColumnName();
            var statusColumn = await GetStatusColumnName();
            var errorColumn = await GetErrorColumnName();
            var insertColumnNameScript=await GetInsertColumnNameScript();
            var insertColumnValueScript = await GetInsertColumnValueScript();


            Guid id = Guid.NewGuid();
            //处理前先新增一条状态为0（待处理）的记录
            await dbExecute(strConn, async (conn) =>
             {
                 await insertRecord(conn, tableName, id, guidColumn, statusColumn, 0, insertColumnNameScript, insertColumnValueScript, string.Empty);
             });

            //执行处理
            bool result;
           
            try
            {
                result = await RealExecute(context, handlerConfiguration, row);
            }
            catch(Exception ex)
            {
                //处理后，如果发生错误，修改之前新增的记录的状态为2（处理失败）
                await dbExecute(strConn, async (conn) =>
                {
                    await updateRecordStatus(conn, tableName, guidColumn,id, statusColumn, 2,errorColumn, ex.ToString());
                });
               
                throw;
            }

            //处理后，如果未发生错误，修改之前新增的记录的状态为1（处理成功）
            await dbExecute(strConn, async (conn) =>
            {
                await updateRecordStatus(conn, tableName, guidColumn, id, statusColumn, 1, errorColumn, string.Empty);
            });

            return result;
        }

        public async Task PostExecute(MatrixDataHandlerContext context)
        {
            await RealPostExecute(context);
        }

        private async Task dbExecute(string connString,Func<SqlConnection, Task> action)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                await action(conn);
                conn.Close();
            }
        }

        private async Task<bool> checkTableExist(SqlConnection conn,string tableName)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = $"if exists (select * from dbo.sysobjects where id = object_id(N'{tableName}') and OBJECTPROPERTY(id, N'IsUserTable') = 1) select 1 else select 0";
                var result=await command.ExecuteScalarAsync();

                if ((int)result==1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private async Task clearTable(SqlConnection conn,string tableName)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = $"truncate {tableName}";
                await command.ExecuteNonQueryAsync();

            }
        }


        private async Task createTable(SqlConnection conn,string generateScript)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText =generateScript;
                await command.ExecuteNonQueryAsync();
            }
        }

        private async Task insertRecord(SqlConnection conn,string tableName,Guid id,string guidColumn,string statusColumn,int status,string columnNameScript,string columnValueScript,string errorColumn)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = $"insert into {tableName}({guidColumn},{columnNameScript},{statusColumn},{errorColumn}) values('{id.ToString()}',{columnValueScript},{status.ToString()},'')";
                await command.ExecuteNonQueryAsync();
            }
        }

        private async Task updateRecordStatus(SqlConnection conn,string tableName, string guidColumn,Guid id,  string statusColumn, int status,string errorColumn,string error)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = $"update {tableName} set {statusColumn}={status},{errorColumn}='{error.ToSql()}' where {guidColumn}='{id.ToString()}'";
                await command.ExecuteNonQueryAsync();
            }
        }

    }
}
