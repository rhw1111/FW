using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.DAL
{
    /// <summary>
    /// 数据操作帮助
    /// </summary>
    public static class DBOperateHelper
    {
        /// <summary>
        /// Sql异常处理
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task SqlExceptionHandler(Func<Task> action)
        {
            int reply = 5;
            while (true)
            {
                try
                {
                    await action();
                    break;
                }
                catch (SqlException ex)
                {

                    if (reply > 0 && (ex.Number == 41302 || ex.Number == 41305 || ex.Number == 41325 || ex.Number == 41301 || ex.Number == 1205))
                    {
                        reply--;
                        System.Threading.Thread.Sleep(10);
                    }
                    else if (ex.Number == -2)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.SqlExecuteTimeout,
                            DefaultFormatting = "Sql执行超时",
                            ReplaceParameters = new List<object>() { }
                        };

                        throw new UtilityException((int)Errors.SqlExecuteTimeout, fragment);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 实体框架异常处理
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task DbContextExceptionHandler(Func<Task> action)
        {
            int reply = 5;
            while (true)
            {
                try
                {
                    await action();
                    break;
                }
                catch (Exception ex)
                {

                    if (ex is DbUpdateConcurrencyException || ex is DbUpdateException)
                    {
                        var error = ex.InnerException as SqlException;
                        if (error == null)
                        {
                            throw;
                        }

                        if (reply > 0 && (error.Number == 41302 || error.Number == 41305 || error.Number == 41325 || error.Number == 41301 || error.Number == 1205))
                        {
                            reply--;
                            System.Threading.Thread.Sleep(10);
                        }
                        else if (error.Number == -2)
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.SqlExecuteTimeout,
                                DefaultFormatting = "Sql执行超时",
                                ReplaceParameters = new List<object>() { }
                            };

                            throw new UtilityException((int)Errors.SqlExecuteTimeout, fragment);
                        }
                        else
                        {
                            throw;
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Sql异常处理(同步)
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static void SqlExceptionHandlerSync(Action action)
        {
            int reply = 5;
            while (true)
            {
                try
                {
                    action();
                    break;
                }
                catch (SqlException ex)
                {

                    if (reply > 0 && (ex.Number == 41302 || ex.Number == 41305 || ex.Number == 41325 || ex.Number == 41301 || ex.Number == 1205))
                    {
                        reply--;
                        System.Threading.Thread.Sleep(10);
                    }
                    else if (ex.Number == -2)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.SqlExecuteTimeout,
                            DefaultFormatting = "Sql执行超时",
                            ReplaceParameters = new List<object>() { }
                        };

                        throw new UtilityException((int)Errors.SqlExecuteTimeout, fragment);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 实体框架异常处理(同步)
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static void DbContextExceptionHandlerSync(Action action)
        {
            int reply = 5;
            while (true)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception ex)
                {
                    
                    if (ex is DbUpdateConcurrencyException || ex is DbUpdateException)
                    {
                        var error = ex.InnerException as SqlException;
                        if (error == null)
                        {
                            throw;
                        }

                        if (reply > 0 && (error.Number == 41302 || error.Number == 41305 || error.Number == 41325 || error.Number == 41301 || error.Number == 1205))
                        {
                            reply--;
                            System.Threading.Thread.Sleep(10);
                        }
                        else if (error.Number == -2)
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.SqlExecuteTimeout,
                                DefaultFormatting = "Sql执行超时",
                                ReplaceParameters = new List<object>() { }
                            };

                            throw new UtilityException((int)Errors.SqlExecuteTimeout,fragment);
                        }
                        else
                        {
                            throw;
                        }
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
