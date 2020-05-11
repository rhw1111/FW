using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.DAL;
using MSLibrary.Transaction;
using MSLibrary.Collections.Hash;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Thread.DAL
{
    [Injection(InterfaceType = typeof(IApplicationLockStore), Scope = InjectionScope.Singleton)]
    public class ApplicationLockStore : IApplicationLockStore
    {
        private IThreadConnectionFactory _dbConnectionFactory;
        private IHashGroupRepository _hashGroupRepository;
        private IStoreInfoResolveService _storeInfoResolveService;


        public ApplicationLockStore(IThreadConnectionFactory dbConnectionFactory, IHashGroupRepository hashGroupRepository, IStoreInfoResolveService storeInfoResolveService)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _hashGroupRepository = hashGroupRepository;
            _storeInfoResolveService = storeInfoResolveService;
        }

        public async Task Lock(DBConnectionNames connNames,string lockName, int timeout)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForApplicationLock(connNames,lockName),
                async (conn, transaction) =>
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
                        sqlTran = (SqlTransaction)transaction;
                    }

                    await using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandTimeout = 300;
                        command.Connection = (SqlConnection)conn;
                        command.CommandType = CommandType.Text;
                        command.CommandText = strSql;
                        command.Transaction = sqlTran;
                        await command.PrepareAsync();
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
            );
        }


        public void LockSync(DBConnectionNames connNames,string lockName, int timeout)
        {


            DBTransactionHelper.SqlTransactionWork(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForApplicationLock(connNames,lockName),
                (conn, transaction) =>
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
                        sqlTran = (SqlTransaction)transaction;
                    }

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandTimeout = 300;
                        command.Connection = (SqlConnection)conn;
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
                                    ReplaceParameters = new List<object>() {lockName }
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
            );
        }

        public async Task UnLock(DBConnectionNames connNames,string lockName)
        {

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForApplicationLock(connNames,lockName),
                async (conn, transaction) =>
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
                        sqlTran = (SqlTransaction)transaction;
                    }

                    await using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandTimeout = 300;
                        command.Connection = (SqlConnection)conn;
                        command.CommandType = CommandType.Text;
                        command.CommandText = strSql;
                        command.Transaction = sqlTran;
                        await command.PrepareAsync();
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
                                    ReplaceParameters = new List<object>() {lockName }
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
            );
        }

        public void UnLockSync(DBConnectionNames connNames,string lockName)
        {
            
            DBTransactionHelper.SqlTransactionWork(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForApplicationLock(connNames,lockName),
                (conn, transaction) =>
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
                        sqlTran = (SqlTransaction)transaction;
                    }

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandTimeout = 300;
                        command.Connection = (SqlConnection)conn;
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

                                throw new UtilityException((int)Errors.ExistLicationLock,fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
            );
        }


    }

}
