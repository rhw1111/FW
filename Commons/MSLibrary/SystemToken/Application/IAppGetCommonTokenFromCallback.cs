using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MSLibrary.SystemToken.Application
{
    /// <summary>
    /// 应用层从第三方认证系统回调中获取重定向到接入方系统的url
    /// 重定向到认证中心的url格式为认证中心接收回调的地址+sysname=系统登录终结点名称&authname=验证终结点名称&returnurl=接入方系统的重定向地址
    /// </summary>
    public interface IAppGetCommonTokenFromCallback
    {
        Task<string> Do(HttpRequest request);
    }
}
