using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Data.SqlClient;
using MSLibrary.Transaction;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.DAL
{
    /// <summary>
    /// SqlServer中的应用锁帮助器
    /// </summary>
    public static class SqlServerApplicationLockHelper
    {
        /// <summary>
        /// 执行串行化(同步)
        /// </summary>
        /// <param name="lockName">资源名称</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="timeout">超时时间（-1为永不超时）,单位毫秒</param>
        public static void ExecuteSync(SqlConnection conn, SqlTransaction transaction, string lockName, Action callBack, int timeout = -1)
        {
            try
            {
                LockSync(conn, transaction, lockName, timeout);
                callBack();
            }
            finally
            {
                UnLockSync(conn, transaction, lockName);
            }

        }

        /// <summary>
        /// 执行串行化
        /// </summary>
        /// <param name="lockName">资源名称</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="timeout">超时时间（-1为永不超时），单位毫秒</param>
        public static async Task Execute(SqlConnection conn,SqlTransaction transaction, string lockName, Func<Task> callBack, int timeout = -1)
        {
                try
                {
                    await Lock(conn, transaction, lockName, timeout);
                    await callBack();
                }
                finally
                {
                    await UnLock(conn, transaction, lockName);
                }
        }



        private static async Task Lock(SqlConnection conn,SqlTransaction transaction,string lockName, int timeout)
        {
                    string strTimeout;
                    if (timeout == -1)
                    {
                        strTimeout = "null";
                    }
                    else
                    {
                        strTimeout = timeout.ToString();
                    }
                    string strSql = string.Format(@"declare @result int,@resource nvarchar(300),@timeout int,@message nvarchar(300)
                                      set @result = 0;
		                              set @resource='{0}';
                                      set @timeout={1}
                                      begin
			                            if @@TRANCOUNT>0
		                                    begin
			                                    if @timeout is null
				                                    EXEC @result = sp_getapplock @Resource = @resource, 
					                                @LockMode = 'Exclusive'
			                                    else
					                                EXEC @result = sp_getapplock @Resource = @resource, 
					                                @LockMode = 'Exclusive',@LockTimeout=@timeout			
		                                    end
                                        else
		                                    begin
			                                    if @timeout is null
				                                    EXEC @result = sp_getapplock @Resource = @resource, 
					                                @LockMode = 'Exclusive',@LockOwner='Session'
			                                    else
					                                EXEC @result = sp_getapplock @Resource = @resource, 
					                                @LockMode = 'Exclusive',@LockOwner='Session',@LockTimeout=@timeout			
		                                    end	

                                        if @result<0
		                                    begin
			                                    set @message=N'applock加锁失败，失败码:'+convert(nvarchar(20),@result)+',详细信息：'+ERROR_MESSAGE();
			                                    throw 50001,@message,1
		                                    end
                                       end
                            
                        ", lockName, strTimeout);

                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = transaction;
                    }

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandTimeout = 300;
                        command.Connection = conn;
                        command.CommandType = CommandType.Text;
                        command.CommandText = strSql;
                        command.Transaction = sqlTran;
                        command.Prepare();
                        try
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 50001)
                            {
                                var fragment = new TextFragment()
                                {
                                    Code = TextCodes.ExistLicationLock,
                                    DefaultFormatting = "当前请求{0}已被锁定",
                                    ReplaceParameters = new List<object>() { lockName }
                                };

                                throw new UtilityException((int)Errors.ExistLicationLock, fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }

                    }

        }


        private static void LockSync(SqlConnection conn, SqlTransaction transaction, string lockName, int timeout)
        {

            string strTimeout;
            if (timeout == -1)
            {
                strTimeout = "null";
            }
            else
            {
                strTimeout = timeout.ToString();
            }
            string strSql = string.Format(@"declare @result int,@resource nvarchar(300),@timeout int,@message nvarchar(300)
                                      set @result = 0;
		                              set @resource='{0}';
                                      set @timeout={1}
                                      begin
			                            if @@TRANCOUNT>0
		                                    begin
			                                    if @timeout is null
				                                    EXEC @result = sp_getapplock @Resource = @resource, 
					                                @LockMode = 'Exclusive'
			                                    else
					                                EXEC @result = sp_getapplock @Resource = @resource, 
					                                @LockMode = 'Exclusive',@LockTimeout=@timeout			
		                                    end
                                        else
		                                    begin
			                                    if @timeout is null
				                                    EXEC @result = sp_getapplock @Resource = @resource, 
					                                @LockMode = 'Exclusive',@LockOwner='Session'
			                                    else
					                                EXEC @result = sp_getapplock @Resource = @resource, 
					                                @LockMode = 'Exclusive',@LockOwner='Session',@LockTimeout=@timeout			
		                                    end	

                                        if @result<0
		                                    begin
			                                    set @message=N'applock加锁失败，失败码:'+convert(nvarchar(20),@result)+',详细信息：'+ERROR_MESSAGE();
			                                    throw 50001,@message,1
		                                    end
                                       end
                            
                        ", lockName, strTimeout);

            SqlTransaction sqlTran = null;
            if (transaction != null)
            {
                sqlTran = transaction;
            }

            using (SqlCommand command = new SqlCommand())
            {
                command.CommandTimeout = 300;
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                command.CommandText = strSql;
                command.Transaction = sqlTran;
                command.Prepare();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 50001)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.ExistLicationLock,
                            DefaultFormatting = "当前请求{0}已被锁定",
                            ReplaceParameters = new List<object>() { lockName }
                        };

                        throw new UtilityException((int)Errors.ExistLicationLock, fragment);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

        }

        private static async Task UnLock(SqlConnection conn, SqlTransaction transaction, string lockName)
        {
            string strSql = string.Format(@"declare @result int,@resource nvarchar(300),@message nvarchar(300)
                                      set @result = 0;
		                              set @resource='{0}';
                                      begin

		                                begin try
			                                EXEC @result = sp_releaseapplock @Resource = @resource
		                                end try
		                                begin catch
		                                end catch

                                        if @result<0
		                                    begin
			                                    set @message=N'applock解锁失败，失败码:'+convert(nvarchar(20),@result)+N',详细信息：'+ERROR_MESSAGE();
			                                    throw 50001,@message,1
		                                    end
                                       end
                            
                        ", lockName);

            SqlTransaction sqlTran = null;
            if (transaction != null)
            {
                sqlTran = transaction;
            }

            using (SqlCommand command = new SqlCommand())
            {
                command.CommandTimeout = 300;
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                command.CommandText = strSql;
                command.Transaction = sqlTran;
                command.Prepare();
                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 50001)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.ExistLicationLock,
                            DefaultFormatting = "当前请求{0}已被锁定",
                            ReplaceParameters = new List<object>() { lockName }
                        };

                        throw new UtilityException((int)Errors.ExistLicationLock, fragment);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

        }

        private static void UnLockSync(SqlConnection conn, SqlTransaction transaction, string lockName)
        {

                    string strSql = string.Format(@"declare @result int,@resource nvarchar(300),@message nvarchar(300)
                                      set @result = 0;
		                              set @resource='{0}';
                                      begin

		                                begin try
			                                EXEC @result = sp_releaseapplock @Resource = @resource
		                                end try
		                                begin catch
		                                end catch

                                        if @result<0
		                                    begin
			                                    set @message=N'applock解锁失败，失败码:'+convert(nvarchar(20),@result)+N',详细信息：'+ERROR_MESSAGE();
			                                    throw 50001,@message,1
		                                    end
                                       end
                            
                        ", lockName);

                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = transaction;
                    }

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandTimeout = 300;
                        command.Connection = conn;
                        command.CommandType = CommandType.Text;
                        command.CommandText = strSql;
                        command.Transaction = sqlTran;
                        command.Prepare();
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 50001)
                            {
                                var fragment = new TextFragment()
                                {
                                    Code = TextCodes.ExistLicationLock,
                                    DefaultFormatting = "当前请求{0}已被锁定",
                                    ReplaceParameters = new List<object>() { lockName }
                                };

                                throw new UtilityException((int)Errors.ExistLicationLock, fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }

        }


    }
}
