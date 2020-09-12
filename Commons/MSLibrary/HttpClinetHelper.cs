using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;

namespace MSLibrary
{
    /// <summary>
    /// httpclient辅助类
    /// </summary>
    public static class HttpClinetHelper
    {
        private static Func<IHttpClientFactory> _httpClientFactoryGenerator;

        public static Func<IHttpClientFactory> HttpClientFactoryGenerator
        {
            set
            {
                _httpClientFactoryGenerator = value;
            }
        }

  
        /// <summary>
        /// Post提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Post的对象</param>
        /// <param name="url">服务地址</param>
        public static void Post<T>(T data, string url)
        {
            Post(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Post提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Post的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task PostAsync<T>(T data, string url)
        {
            await PostAsync(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Post提交，无返回值
        /// </summary>
        /// <param name="url"></param>
        public static void Post(string url)
        {
            Post<object>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Post提交，无返回值(异步)
        /// </summary>
        /// <param name="url"></param>
        public static async Task PostAsync(string url)
        {
            await PostAsync<object>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        ///  Post提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Post<T, V>(T data, string url)
        {
            return Post<T, V>(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        ///  Post提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PostAsync<T, V>(T data, string url)
        {
            return await PostAsync<T, V>(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Post提交,返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Post<V>(string url)
        {
            return Post<object, V>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Post提交,返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PostAsync<V>(string url)
        {
            return await PostAsync<object, V>(null, url, null, new HttpErrorHandlerDefault());
        }







        /// <summary>
        /// Post提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Post的对象</param>
        /// <param name="url">服务地址</param>
        public static void Post<T>(T data, string url, IHttpErrorHandler errorHandler)
        {
            Post(data, url, null, errorHandler);
        }

        /// <summary>
        /// Post提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Post的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task PostAsync<T>(T data, string url, IHttpErrorHandler errorHandler)
        {
            await PostAsync(data, url, null, errorHandler);
        }

        /// <summary>
        /// Post提交，无返回值
        /// </summary>
        /// <param name="url"></param>
        public static void Post(string url, IHttpErrorHandler errorHandler)
        {
            Post<object>(null, url, null, errorHandler);
        }

        /// <summary>
        /// Post提交，无返回值(异步)
        /// </summary>
        /// <param name="url"></param>
        public static async Task PostAsync(string url, IHttpErrorHandler errorHandler)
        {
            await PostAsync<object>(null, url, null, errorHandler);
        }

        /// <summary>
        ///  Post提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Post<T, V>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return Post<T, V>(data, url, null, errorHandler);
        }

        /// <summary>
        ///  Post提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PostAsync<T, V>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return await PostAsync<T, V>(data, url, null, errorHandler);
        }

        /// <summary>
        /// Post提交,返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Post<V>(string url, IHttpErrorHandler errorHandler)
        {
            return Post<object, V>(null, url, null, errorHandler);
        }







        /// <summary>
        /// Post提交,返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PostAsync<V>(string url, Dictionary<string, string> httpHeaders)
        {
            return await PostAsync<object, V>(null, url, httpHeaders, new HttpErrorHandlerDefault());
        }


        /// <summary>
        /// Post提交,返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PostAsync<V>(string url, IHttpErrorHandler errorHandler)
        {
            return await PostAsync<object, V>(null, url, null, errorHandler);
        }



        /// <summary>
        /// Put提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static void Put<T>(T data, string url)
        {
            Put(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Put提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task PutAsync<T>(T data, string url)
        {
            await PutAsync(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Put提交，无返回值
        /// </summary>
        /// <param name="url"></param>
        public static void Put(string url)
        {
            Put<object>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Put提交，无返回值(异步)
        /// </summary>
        /// <param name="url"></param>
        public static async Task PutAsync(string url)
        {
            await PutAsync<object>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        ///  Put提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Put<T, V>(T data, string url)
        {
            return Put<T, V>(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        ///  Put提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PutAsync<T, V>(T data, string url)
        {
            return await PutAsync<T, V>(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Put提交,返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Put<V>(string url)
        {
            return Put<object, V>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Put提交,返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PutAsync<V>(string url)
        {
            return await PutAsync<object, V>(null, url, null, new HttpErrorHandlerDefault());
        }








        /// <summary>
        /// Put提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static void Put<T>(T data, string url, IHttpErrorHandler errorHandler)
        {
            Put(data, url, null, errorHandler);
        }

        /// <summary>
        /// Put提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task PutAsync<T>(T data, string url, IHttpErrorHandler errorHandler)
        {
            await PutAsync(data, url, null, errorHandler);
        }

        /// <summary>
        /// Put提交，无返回值
        /// </summary>
        /// <param name="url"></param>
        public static void Put(string url, IHttpErrorHandler errorHandler)
        {
            Put<object>(null, url, null, errorHandler);
        }

        /// <summary>
        /// Put提交，无返回值(异步)
        /// </summary>
        /// <param name="url"></param>
        public static async Task PutAsync(string url, IHttpErrorHandler errorHandler)
        {
            await PutAsync<object>(null, url, null, errorHandler);
        }

        /// <summary>
        ///  Put提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Put<T, V>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return Put<T, V>(data, url, null, errorHandler);
        }

        /// <summary>
        ///  Put提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PutAsync<T, V>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return await PutAsync<T, V>(data, url, null, errorHandler);
        }

        /// <summary>
        /// Put提交,返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Put<V>(string url, IHttpErrorHandler errorHandler)
        {
            return Put<object, V>(null, url, null, errorHandler);
        }

        /// <summary>
        /// Put提交,返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PutAsync<V>(string url, IHttpErrorHandler errorHandler)
        {
            return await PutAsync<object, V>(null, url, null, errorHandler);
        }




        /// <summary>
        /// Patch提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static void Patch<T>(T data, string url)
        {
            Patch(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Patch提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Patch的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task PatchAsync<T>(T data, string url)
        {
            await PatchAsync(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Patch提交，无返回值
        /// </summary>
        /// <param name="url"></param>
        public static void Patch(string url)
        {
            Patch<object>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Patch提交，无返回值(异步)
        /// </summary>
        /// <param name="url"></param>
        public static async Task PatchAsync(string url)
        {
            await PatchAsync<object>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        ///  Patch提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Patch<T, V>(T data, string url)
        {
            return Patch<T, V>(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        ///  Patch提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PatchAsync<T, V>(T data, string url)
        {
            return await PatchAsync<T, V>(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Patch提交,返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Patch<V>(string url)
        {
            return Patch<object, V>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Patch提交,返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PatchAsync<V>(string url)
        {
            return await PatchAsync<object, V>(null, url, null, new HttpErrorHandlerDefault());
        }







        /// <summary>
        /// Patch提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static void Patch<T>(T data, string url, IHttpErrorHandler errorHandler)
        {
            Patch(data, url, null, errorHandler);
        }

        /// <summary>
        /// Patch提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Patch的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task PatchAsync<T>(T data, string url, IHttpErrorHandler errorHandler)
        {
            await PatchAsync(data, url, null, errorHandler);
        }

        /// <summary>
        /// Patch提交，无返回值
        /// </summary>
        /// <param name="url"></param>
        public static void Patch(string url, IHttpErrorHandler errorHandler)
        {
            Patch<object>(null, url, null, errorHandler);
        }

        /// <summary>
        /// Patch提交，无返回值(异步)
        /// </summary>
        /// <param name="url"></param>
        public static async Task PatchAsync(string url, IHttpErrorHandler errorHandler)
        {
            await PatchAsync<object>(null, url, null, errorHandler);
        }

        /// <summary>
        ///  Patch提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Patch<T, V>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return Patch<T, V>(data, url, null, errorHandler);
        }

        /// <summary>
        ///  Patch提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PatchAsync<T, V>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return await PatchAsync<T, V>(data, url, null, errorHandler);
        }

        /// <summary>
        /// Patch提交,返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Patch<V>(string url, IHttpErrorHandler errorHandler)
        {
            return Patch<object, V>(null, url, null, errorHandler);
        }

        /// <summary>
        /// Patch提交,返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PatchAsync<V>(string url, IHttpErrorHandler errorHandler)
        {
            return await PatchAsync<object, V>(null, url, null, errorHandler);
        }






        /// <summary>
        /// Get获取
        /// </summary>
        /// <param name="url">服务地址</param>
        public static void Get(string url)
        {
            Get(url, null, new HttpErrorHandlerDefault());

        }

        /// <summary>
        /// Get获取(异步)
        /// </summary>
        /// <param name="url">服务地址</param>
        public static async Task GetAsync(string url)
        {
            await GetAsync(url, null, new HttpErrorHandlerDefault());

        }


        /// <summary>
        /// get获取
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static T Get<T>(string url)
        {
            return Get<T>(url, null, new HttpErrorHandlerDefault());

        }

        /// <summary>
        /// get获取(异步)
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(string url)
        {
            return await GetAsync<T>(url, null, new HttpErrorHandlerDefault());

        }









        /// <summary>
        /// Get获取
        /// </summary>
        /// <param name="url">服务地址</param>
        public static void Get(string url, IHttpErrorHandler errorHandler)
        {
            Get(url, null, errorHandler);

        }

        /// <summary>
        /// Get获取(异步)
        /// </summary>
        /// <param name="url">服务地址</param>
        public static async Task GetAsync(string url, IHttpErrorHandler errorHandler)
        {
            await GetAsync(url, null, errorHandler);

        }


        /// <summary>
        /// get获取
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static T Get<T>(string url, IHttpErrorHandler errorHandler)
        {
            return Get<T>(url, null, errorHandler);

        }

        /// <summary>
        /// get获取(异步)
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(string url, IHttpErrorHandler errorHandler)
        {
            return await GetAsync<T>(url, null, errorHandler);

        }






        /// <summary>
        /// Delete获取
        /// </summary>
        /// <param name="url">服务地址</param>
        public static void Delete(string url)
        {
            Delete(url, null, new HttpErrorHandlerDefault());

        }

        /// <summary>
        /// Delete获取(异步)
        /// </summary>
        /// <param name="url">服务地址</param>
        public static async Task DeleteAsync(string url)
        {
            await DeleteAsync(url, null, new HttpErrorHandlerDefault());

        }


        /// <summary>
        /// delete获取
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static T Delete<T>(string url)
        {
            return Delete<T>(url, null, new HttpErrorHandlerDefault());

        }

        /// <summary>
        /// delete获取(异步)
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static async Task<T> DeleteAsync<T>(string url)
        {
            return await DeleteAsync<T>(url, null, new HttpErrorHandlerDefault());

        }










        /// <summary>
        /// Delete获取
        /// </summary>
        /// <param name="url">服务地址</param>
        public static void Delete(string url, IHttpErrorHandler errorHandler)
        {
            Delete(url, null, errorHandler);

        }

        /// <summary>
        /// Delete获取(异步)
        /// </summary>
        /// <param name="url">服务地址</param>
        public static async Task DeleteAsync(string url, IHttpErrorHandler errorHandler)
        {
            await DeleteAsync(url, null, errorHandler);

        }


        /// <summary>
        /// delete获取
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static T Delete<T>(string url, IHttpErrorHandler errorHandler)
        {
            return Delete<T>(url, null, errorHandler);

        }

        /// <summary>
        /// delete获取(异步)
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static async Task<T> DeleteAsync<T>(string url, IHttpErrorHandler errorHandler)
        {
            return await DeleteAsync<T>(url, null, errorHandler);

        }






        /// <summary>
        /// Post提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Post的对象</param>
        /// <param name="url">服务地址</param>
        public static void Post<T>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;

            string json = string.Empty;


            


            if (typeof(T).IsValueType || (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T)))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                try
                {
                    response = client.PostAsync(url, httpContent).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Post，Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
            }
        }
        /// <summary>
        /// Post提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Post的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task PostAsync<T>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;

            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                try
                {
                    response = await client.PostAsync(url, httpContent);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Post，Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }


                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }
            }
        }
        /// <summary>
        /// Post提交对象,无内容，无返回值
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static void Post(string url, Dictionary<string, string> httpHeaders)
        {
            Post<object>(null, url, httpHeaders, new HttpErrorHandlerDefault());
        }
        /// <summary>
        ///  Post提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Post<T, V>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpResponseMessage response = null;
                try
                {
                    response = client.PostAsync(url, httpContent).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Post，Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializerHelper.Deserialize<V>(strContent);

                    //return response.Content.ReadAsAsync<V>().Result;
                }

            }
        }
        /// <summary>
        ///  Post提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PostAsync<T, V>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                HttpResponseMessage response = null;

                try
                {
                    response = await client.PostAsync(url, httpContent);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Post，Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializerHelper.Deserialize<V>(strContent);
                    //return await response.Content.ReadAsAsync<V>();
                }

            }
        }
        public static V Post<V>(string url, Dictionary<string, string> httpHeaders)
        {
            return Post<object, V>(null, url, httpHeaders, new HttpErrorHandlerDefault());
        }










        /// <summary>
        /// Post提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Post的对象</param>
        /// <param name="url">服务地址</param>
        public static void Post<T>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                try
                {
                    response = client.PostAsync(url, httpContent).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Post，Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
            }
        }
        /// <summary>
        /// Post提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Post的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task PostAsync<T>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                try
                {
                    response = await client.PostAsync(url, httpContent);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Post，Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }
            }
        }
        /// <summary>
        /// Post提交对象,无内容，无返回值
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static void Post(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            Post<object>(null, url, httpHeaders, errorHandler);
        }
        /// <summary>
        ///  Post提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Post<T, V>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = client.PostAsync(url, httpContent).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Post，Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializerHelper.Deserialize<V>(strContent);

                    //return response.Content.ReadAsAsync<V>().Result;
                }

            }
        }
        /// <summary>
        ///  Post提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PostAsync<T, V>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = await client.PostAsync(url, httpContent);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Post，Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializerHelper.Deserialize<V>(strContent);
                    //return await response.Content.ReadAsAsync<V>();
                }

            }
        }
        public static V Post<V>(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            return Post<object, V>(null, url, httpHeaders, errorHandler);
        }







        /// <summary>
        /// Get获取
        /// </summary>
        /// <param name="url">服务地址</param>
        public static void Get(string url, Dictionary<string, string> httpHeaders)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }

                HttpResponseMessage response = null;
                try
                {
                    response = client.GetAsync(url).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Get，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }

            }

        }

        /// <summary>
        /// Get获取(异步)
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static async Task GetAsync(string url, Dictionary<string, string> httpHeaders)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Get，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }

            }

        }
        /// <summary>
        /// get获取
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static T Get<T>(string url, Dictionary<string, string> httpHeaders)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = client.GetAsync(url).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Get，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializerHelper.Deserialize<T>(strContent);

                    //return response.Content.ReadAsAsync<T>().Result;
                }

            }

        }
        /// <summary>
        /// get获取(异步)
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(string url, Dictionary<string, string> httpHeaders)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Get，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializerHelper.Deserialize<T>(strContent);

                    //return await response.Content.ReadAsAsync<T>();
                }

            }

        }









        /// <summary>
        /// Get获取
        /// </summary>
        /// <param name="url">服务地址</param>
        public static void Get(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = client.GetAsync(url).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Get，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }

            }

        }

        /// <summary>
        /// Get获取(异步)
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static async Task GetAsync(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Get，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }

            }

        }
        /// <summary>
        /// get获取
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static T Get<T>(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }

                HttpResponseMessage response = null;
                try
                {
                    response = client.GetAsync(url).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Get，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializerHelper.Deserialize<T>(strContent);

                    //return response.Content.ReadAsAsync<T>().Result;
                }

            }

        }
        /// <summary>
        /// get获取(异步)
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Get，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializerHelper.Deserialize<T>(strContent);

                    //return await response.Content.ReadAsAsync<T>();
                }

            }
        }






        /// <summary>
        /// Put提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static void Put<T>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                try
                {
                    response = client.PutAsync(url, httpContent).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Put,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
            }
        }
        /// <summary>
        /// Put提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task PutAsync<T>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                try
                {
                    response = await client.PutAsync(url, httpContent);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Put,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }
            }
        }
        /// <summary>
        /// Put提交对象,无内容，无返回值
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static void Put(string url, Dictionary<string, string> httpHeaders)
        {
            Put<object>(null, url, httpHeaders, new HttpErrorHandlerDefault());
        }
        /// <summary>
        ///  Put提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Put<T, V>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = client.PutAsync(url, httpContent).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Put,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializerHelper.Deserialize<V>(strContent);

                    //return response.Content.ReadAsAsync<V>().Result;
                }

            }
        }
        /// <summary>
        ///  Put提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PutAsync<T, V>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = await client.PutAsync(url, httpContent);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Put,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializerHelper.Deserialize<V>(strContent);
                    //return await response.Content.ReadAsAsync<V>();
                }

            }
        }
        public static V Put<V>(string url, Dictionary<string, string> httpHeaders)
        {
            return Put<object, V>(null, url, httpHeaders, new HttpErrorHandlerDefault());
        }








        /// <summary>
        /// Put提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static void Put<T>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                try
                {
                    response = client.PutAsync(url, httpContent).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Put,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
            }
        }
        /// <summary>
        /// Put提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task PutAsync<T>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                try
                {
                    response = await client.PutAsync(url, httpContent);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Put,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }
            }
        }
        /// <summary>
        /// Put提交对象,无内容，无返回值
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static void Put(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            Put<object>(null, url, httpHeaders, errorHandler);
        }
        /// <summary>
        ///  Put提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Put<T, V>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpResponseMessage response = null;
                try
                {
                    response = client.PutAsync(url, httpContent).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Put,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializerHelper.Deserialize<V>(strContent);

                    //return response.Content.ReadAsAsync<V>().Result;
                }

            }

        }
        /// <summary>
        ///  Put提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PutAsync<T, V>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpResponseMessage response = null;
                try
                {
                    response = await client.PutAsync(url, httpContent);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Put,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializerHelper.Deserialize<V>(strContent);
                    //return await response.Content.ReadAsAsync<V>();
                }

            }
        }
        public static V Put<V>(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            return Put<object, V>(null, url, httpHeaders, errorHandler);
        }






        /// <summary>
        /// Patch提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Patch的对象</param>
        /// <param name="url">服务地址</param>
        public static void Patch<T>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = httpContent
                };
                try
                {
                    response = client.SendAsync(request).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Patch,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
            }
        }

        /// <summary>
        /// Patch提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Patch的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task PatchAsync<T>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage { Method = new HttpMethod("PATCH"), RequestUri = new Uri(url) };
                request.Content = httpContent;
                try
                {
                    response = await client.SendAsync(request);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Patch,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }
            }
        }

        /// <summary>
        /// Patch提交对象,无内容，无返回值
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static void Patch(string url, Dictionary<string, string> httpHeaders)
        {
            Patch<object>(null, url, httpHeaders, new HttpErrorHandlerDefault());
        }
        /// <summary>
        ///  Patch提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Patch<T, V>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = httpContent
                };
                HttpResponseMessage response = null;
                try
                {
                    response = client.SendAsync(request).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Patch,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializerHelper.Deserialize<V>(strContent);

                    //return response.Content.ReadAsAsync<V>().Result;
                }

            }
        }
        /// <summary>
        ///  Patch提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PatchAsync<T, V>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = httpContent
                };

                HttpResponseMessage response = null;

                try
                {
                    response = await client.SendAsync(request);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Patch,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializerHelper.Deserialize<V>(strContent);
                    //return await response.Content.ReadAsAsync<V>();
                }

            }
        }
        public static V Patch<V>(string url, Dictionary<string, string> httpHeaders)
        {
            return Patch<object, V>(null, url, httpHeaders, new HttpErrorHandlerDefault());
        }







        /// <summary>
        /// Patch提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Patch的对象</param>
        /// <param name="url">服务地址</param>
        public static void Patch<T>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = httpContent
                };
                try
                {
                    response = client.SendAsync(request).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Patch,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
            }
        }

        /// <summary>
        /// Patch提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Patch的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task PatchAsync<T>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage { Method = new HttpMethod("PATCH"), RequestUri = new Uri(url) };
                request.Content = httpContent;
                try
                {
                    response = await client.SendAsync(request);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Patch,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }
            }
        }

        /// <summary>
        /// Patch提交对象,无内容，无返回值
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static void Patch(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            Patch<object>(null, url, httpHeaders, errorHandler);
        }
        /// <summary>
        ///  Patch提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static V Patch<T, V>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = httpContent
                };
                HttpResponseMessage response = null;
                try
                {
                    response = client.SendAsync(request).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Patch,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializerHelper.Deserialize<V>(strContent);

                    //return response.Content.ReadAsAsync<V>().Result;
                }

            }
        }
        /// <summary>
        ///  Patch提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<V> PatchAsync<T, V>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = httpContent
                };
                HttpResponseMessage response = null;
                try
                {
                    response = await client.SendAsync(request);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Patch,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializerHelper.Deserialize<V>(strContent);
                    //return await response.Content.ReadAsAsync<V>();
                }

            }
        }
        public static V Patch<V>(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            return Patch<object, V>(null, url, httpHeaders, errorHandler);
        }




        /// <summary>
        /// Delete获取
        /// </summary>
        /// <param name="url">服务地址</param>
        public static void Delete(string url, Dictionary<string, string> httpHeaders)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = client.DeleteAsync(url).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Delete，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }

            }

        }
        /// <summary>
        /// Delete获取(异步)
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static async Task DeleteAsync(string url, Dictionary<string, string> httpHeaders)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;

                try
                {
                    response = await client.DeleteAsync(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Delete，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }

            }

        }
        /// <summary>
        /// Delete获取
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static T Delete<T>(string url, Dictionary<string, string> httpHeaders)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = client.DeleteAsync(url).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Delete，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializerHelper.Deserialize<T>(strContent);

                    //return response.Content.ReadAsAsync<T>().Result;
                }

            }

        }
        /// <summary>
        /// Delete获取(异步)
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        /// <returns></returns>
        public static async Task<T> DeleteAsync<T>(string url, Dictionary<string, string> httpHeaders)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;

                try
                {
                    response = await client.DeleteAsync(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Delete，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializerHelper.Deserialize<T>(strContent);

                    //return await response.Content.ReadAsAsync<T>();
                }

            }

        }





        /// <summary>
        /// Delete获取
        /// </summary>
        /// <param name="url">服务地址</param>
        public static void Delete(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = client.DeleteAsync(url).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Delete，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }

            }

        }
        /// <summary>
        /// Delete获取(异步)
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static async Task DeleteAsync(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = await client.DeleteAsync(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Delete，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }

            }

        }
        /// <summary>
        /// Delete获取
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static T Delete<T>(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = client.DeleteAsync(url).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Delete，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializerHelper.Deserialize<T>(strContent);

                    //return response.Content.ReadAsAsync<T>().Result;
                }

            }

        }
        /// <summary>
        /// Delete获取(异步)
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        /// <returns></returns>
        public static async Task<T> DeleteAsync<T>(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = await client.DeleteAsync(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Delete，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializerHelper.Deserialize<T>(strContent);

                    //return await response.Content.ReadAsAsync<T>();
                }

            }

        }

































        /// <summary>
        /// Post提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Post的对象</param>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage PostWithResponse<T>(T data, string url)
        {
            return PostWithResponse(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Post提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Post的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task<HttpResponseMessage> PostWithResponseAsync<T>(T data, string url)
        {
            return await PostWithResponseAsync(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Post提交，无返回值
        /// </summary>
        /// <param name="url"></param>
        public static HttpResponseMessage PostWithResponse(string url)
        {
            return PostWithResponse<object>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Post提交，无返回值(异步)
        /// </summary>
        /// <param name="url"></param>
        public static async Task<HttpResponseMessage> PostWithResponseAsync(string url)
        {
            return await PostWithResponseAsync<object>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        ///  Post提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PostWithResponse<T, V>(T data, string url)
        {
            return PostWithResponse<T, V>(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        ///  Post提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PostWithResponseAsync<T, V>(T data, string url)
        {
            return await PostWithResponseAsync<T, V>(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Post提交,返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PostWithResponse<V>(string url)
        {
            return PostWithResponse<object, V>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Post提交,返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PostWithResponseAsync<V>(string url)
        {
            return await PostWithResponseAsync<object, V>(null, url, null, new HttpErrorHandlerDefault());
        }







        /// <summary>
        /// Post提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Post的对象</param>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage PostWithResponse<T>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return PostWithResponse(data, url, null, errorHandler);
        }

        /// <summary>
        /// Post提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Post的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task<HttpResponseMessage> PostWithResponseAsync<T>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return await PostWithResponseAsync(data, url, null, errorHandler);
        }

        /// <summary>
        /// Post提交，无返回值
        /// </summary>
        /// <param name="url"></param>
        public static HttpResponseMessage PostWithResponse(string url, IHttpErrorHandler errorHandler)
        {
            return PostWithResponse<object>(null, url, null, errorHandler);
        }

        /// <summary>
        /// Post提交，无返回值(异步)
        /// </summary>
        /// <param name="url"></param>
        public static async Task<HttpResponseMessage> PostWithResponseAsync(string url, IHttpErrorHandler errorHandler)
        {
            return await PostWithResponseAsync<object>(null, url, null, errorHandler);
        }

        /// <summary>
        ///  Post提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PostWithResponse<T, V>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return PostWithResponse<T, V>(data, url, null, errorHandler);
        }

        /// <summary>
        ///  Post提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PostWithResponseAsync<T, V>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return await PostWithResponseAsync<T, V>(data, url, null, errorHandler);
        }

        /// <summary>
        /// Post提交,返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PostWithResponse<V>(string url, IHttpErrorHandler errorHandler)
        {
            return PostWithResponse<object, V>(null, url, null, errorHandler);
        }







        /// <summary>
        /// Post提交,返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PostWithResponseAsync<V>(string url, Dictionary<string, string> httpHeaders)
        {
            return await PostWithResponseAsync<object, V>(null, url, httpHeaders, new HttpErrorHandlerDefault());
        }


        /// <summary>
        /// Post提交,返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PostWithResponseAsync<V>(string url, IHttpErrorHandler errorHandler)
        {
            return await PostWithResponseAsync<object, V>(null, url, null, errorHandler);
        }



        /// <summary>
        /// Put提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage PutWithResponse<T>(T data, string url)
        {
            return PutWithResponse(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Put提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task<HttpResponseMessage> PutWithResponseAsync<T>(T data, string url)
        {
            return await PutWithResponseAsync(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Put提交，无返回值
        /// </summary>
        /// <param name="url"></param>
        public static HttpResponseMessage PutWithResponse(string url)
        {
            return PutWithResponse<object>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Put提交，无返回值(异步)
        /// </summary>
        /// <param name="url"></param>
        public static async Task<HttpResponseMessage> PutWithResponseAsync(string url)
        {
            return await PutWithResponseAsync<object>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        ///  Put提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PutWithResponse<T, V>(T data, string url)
        {
            return PutWithResponse<T, V>(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        ///  Put提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PutWithResponseAsync<T, V>(T data, string url)
        {
            return await PutWithResponseAsync<T, V>(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Put提交,返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PutWithResponse<V>(string url)
        {
            return PutWithResponse<object, V>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Put提交,返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PutWithResponseAsync<V>(string url)
        {
            return await PutWithResponseAsync<object, V>(null, url, null, new HttpErrorHandlerDefault());
        }








        /// <summary>
        /// Put提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage PutWithResponse<T>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return PutWithResponse(data, url, null, errorHandler);
        }

        /// <summary>
        /// Put提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task<HttpResponseMessage> PutWithResponseAsync<T>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return await PutWithResponseAsync(data, url, null, errorHandler);
        }

        /// <summary>
        /// Put提交，无返回值
        /// </summary>
        /// <param name="url"></param>
        public static HttpResponseMessage PutWithResponse(string url, IHttpErrorHandler errorHandler)
        {
            return PutWithResponse<object>(null, url, null, errorHandler);
        }

        /// <summary>
        /// Put提交，无返回值(异步)
        /// </summary>
        /// <param name="url"></param>
        public static async Task<HttpResponseMessage> PutWithResponseAsync(string url, IHttpErrorHandler errorHandler)
        {
            return await PutWithResponseAsync<object>(null, url, null, errorHandler);
        }

        /// <summary>
        ///  Put提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PutWithResponse<T, V>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return PutWithResponse<T, V>(data, url, null, errorHandler);
        }

        /// <summary>
        ///  Put提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PutWithResponseAsync<T, V>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return await PutWithResponseAsync<T, V>(data, url, null, errorHandler);
        }

        /// <summary>
        /// Put提交,返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PutWithResponse<V>(string url, IHttpErrorHandler errorHandler)
        {
            return PutWithResponse<object, V>(null, url, null, errorHandler);
        }

        /// <summary>
        /// Put提交,返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PutWithResponseAsync<V>(string url, IHttpErrorHandler errorHandler)
        {
            return await PutWithResponseAsync<object, V>(null, url, null, errorHandler);
        }




        /// <summary>
        /// Patch提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage PatchWithResponse<T>(T data, string url)
        {
            return PatchWithResponse(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Patch提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Patch的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task<HttpResponseMessage> PatchWithResponseAsync<T>(T data, string url)
        {
            return await PatchWithResponseAsync(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Patch提交，无返回值
        /// </summary>
        /// <param name="url"></param>
        public static HttpResponseMessage PatchWithResponse(string url)
        {
            return PatchWithResponse<object>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Patch提交，无返回值(异步)
        /// </summary>
        /// <param name="url"></param>
        public static async Task<HttpResponseMessage> PatchWithResponseAsync(string url)
        {
            return await PatchWithResponseAsync<object>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        ///  Patch提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PatchWithResponse<T, V>(T data, string url)
        {
            return PatchWithResponse<T, V>(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        ///  Patch提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PatchWithResponseAsync<T, V>(T data, string url)
        {
            return await PatchWithResponseAsync<T, V>(data, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Patch提交,返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PatchWithResponse<V>(string url)
        {
            return PatchWithResponse<object, V>(null, url, null, new HttpErrorHandlerDefault());
        }

        /// <summary>
        /// Patch提交,返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PatchWithResponseAsync<V>(string url)
        {
            return await PatchWithResponseAsync<object, V>(null, url, null, new HttpErrorHandlerDefault());
        }







        /// <summary>
        /// Patch提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage PatchWithResponse<T>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return PatchWithResponse(data, url, null, errorHandler);
        }

        /// <summary>
        /// Patch提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Patch的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task<HttpResponseMessage> PatchWithResponseAsync<T>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return await PatchWithResponseAsync(data, url, null, errorHandler);
        }

        /// <summary>
        /// Patch提交，无返回值
        /// </summary>
        /// <param name="url"></param>
        public static HttpResponseMessage PatchWithResponse(string url, IHttpErrorHandler errorHandler)
        {
            return PatchWithResponse<object>(null, url, null, errorHandler);
        }

        /// <summary>
        /// Patch提交，无返回值(异步)
        /// </summary>
        /// <param name="url"></param>
        public static async Task<HttpResponseMessage> PatchWithResponseAsync(string url, IHttpErrorHandler errorHandler)
        {
            return await PatchWithResponseAsync<object>(null, url, null, errorHandler);
        }

        /// <summary>
        ///  Patch提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PatchWithResponse<T, V>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return PatchWithResponse<T, V>(data, url, null, errorHandler);
        }

        /// <summary>
        ///  Patch提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PatchWithResponseAsync<T, V>(T data, string url, IHttpErrorHandler errorHandler)
        {
            return await PatchWithResponseAsync<T, V>(data, url, null, errorHandler);
        }

        /// <summary>
        /// Patch提交,返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PatchWithResponse<V>(string url, IHttpErrorHandler errorHandler)
        {
            return PatchWithResponse<object, V>(null, url, null, errorHandler);
        }

        /// <summary>
        /// Patch提交,返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PatchWithResponseAsync<V>(string url, IHttpErrorHandler errorHandler)
        {
            return await PatchWithResponseAsync<object, V>(null, url, null, errorHandler);
        }






        /// <summary>
        /// Get获取
        /// </summary>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage GetWithResponse(string url)
        {
            return GetWithResponse(url, null, new HttpErrorHandlerDefault());

        }

        /// <summary>
        /// Get获取(异步)
        /// </summary>
        /// <param name="url">服务地址</param>
        public static async Task<HttpResponseMessage> GetWithResponseAsync(string url)
        {
            return await GetWithResponseAsync(url, null, new HttpErrorHandlerDefault());

        }


        /// <summary>
        /// get获取
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static HttpResult<T> GetWithResponse<T>(string url)
        {
            return GetWithResponse<T>(url, null, new HttpErrorHandlerDefault());

        }

        /// <summary>
        /// get获取(异步)
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static async Task<HttpResult<T>> GetWithResponseAsync<T>(string url)
        {
            return await GetWithResponseAsync<T>(url, null, new HttpErrorHandlerDefault());

        }









        /// <summary>
        /// Get获取
        /// </summary>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage GetWithResponse(string url, IHttpErrorHandler errorHandler)
        {
            return GetWithResponse(url, null, errorHandler);

        }

        /// <summary>
        /// Get获取(异步)
        /// </summary>
        /// <param name="url">服务地址</param>
        public static async Task<HttpResponseMessage> GetWithResponseAsync(string url, IHttpErrorHandler errorHandler)
        {
            return await GetWithResponseAsync(url, null, errorHandler);

        }


        /// <summary>
        /// get获取
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static HttpResult<T> GetWithResponse<T>(string url, IHttpErrorHandler errorHandler)
        {
            return GetWithResponse<T>(url, null, errorHandler);

        }

        /// <summary>
        /// get获取(异步)
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static async Task<HttpResult<T>> GetWithResponseAsync<T>(string url, IHttpErrorHandler errorHandler)
        {
            return await GetWithResponseAsync<T>(url, null, errorHandler);

        }






        /// <summary>
        /// Delete获取
        /// </summary>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage DeleteWithResponse(string url)
        {
            return DeleteWithResponse(url, null, new HttpErrorHandlerDefault());

        }

        /// <summary>
        /// Delete获取(异步)
        /// </summary>
        /// <param name="url">服务地址</param>
        public static async Task<HttpResponseMessage> DeleteWithResponseAsync(string url)
        {
            return await DeleteWithResponseAsync(url, null, new HttpErrorHandlerDefault());

        }


        /// <summary>
        /// delete获取
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static HttpResult<T> DeleteWithResponse<T>(string url)
        {
            return DeleteWithResponse<T>(url, null, new HttpErrorHandlerDefault());

        }

        /// <summary>
        /// delete获取(异步)
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static async Task<HttpResult<T>> DeleteWithResponseAsync<T>(string url)
        {
            return await DeleteWithResponseAsync<T>(url, null, new HttpErrorHandlerDefault());

        }










        /// <summary>
        /// Delete获取
        /// </summary>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage DeleteWithResponse(string url, IHttpErrorHandler errorHandler)
        {
            return DeleteWithResponse(url, null, errorHandler);

        }

        /// <summary>
        /// Delete获取(异步)
        /// </summary>
        /// <param name="url">服务地址</param>
        public static async Task<HttpResponseMessage> DeleteWithResponseAsync(string url, IHttpErrorHandler errorHandler)
        {
            return await DeleteWithResponseAsync(url, null, errorHandler);

        }


        /// <summary>
        /// delete获取
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static HttpResult<T> DeleteWithResponse<T>(string url, IHttpErrorHandler errorHandler)
        {
            return DeleteWithResponse<T>(url, null, errorHandler);

        }

        /// <summary>
        /// delete获取(异步)
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static async Task<HttpResult<T>> DeleteWithResponseAsync<T>(string url, IHttpErrorHandler errorHandler)
        {
            return await DeleteWithResponseAsync<T>(url, null, errorHandler);

        }






        /// <summary>
        /// Post提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Post的对象</param>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage PostWithResponse<T>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;

            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                try
                {
                    response = client.PostAsync(url, httpContent).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Post，Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }

                return response;
            }
        }
        /// <summary>
        /// Post提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Post的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task<HttpResponseMessage> PostWithResponseAsync<T>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;

            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                try
                {
                    response = await client.PostAsync(url, httpContent);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Post，Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }


                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }

                return response;
            }
        }
        /// <summary>
        /// Post提交对象,无内容，无返回值
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static HttpResponseMessage PostWithResponse(string url, Dictionary<string, string> httpHeaders)
        {
            return PostWithResponse<object>(null, url, httpHeaders, new HttpErrorHandlerDefault());
        }
        /// <summary>
        ///  Post提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PostWithResponse<T, V>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpResponseMessage response = null;
                try
                {
                    response = client.PostAsync(url, httpContent).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Post，Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return new HttpResult<V>(JsonSerializerHelper.Deserialize<V>(strContent), response);

                    //return response.Content.ReadAsAsync<V>().Result;
                }

            }
        }
        /// <summary>
        ///  Post提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PostWithResponseAsync<T, V>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                HttpResponseMessage response = null;

                try
                {
                    response = await client.PostAsync(url, httpContent);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Post，Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return new HttpResult<V>(JsonSerializerHelper.Deserialize<V>(strContent), response);
                    //return await response.Content.ReadAsAsync<V>();
                }

            }
        }
        public static HttpResult<V> PostWithResponse<V>(string url, Dictionary<string, string> httpHeaders)
        {
            return PostWithResponse<object, V>(null, url, httpHeaders, new HttpErrorHandlerDefault());
        }










        /// <summary>
        /// Post提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Post的对象</param>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage PostWithResponse<T>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                try
                {
                    response = client.PostAsync(url, httpContent).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Post，Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }

                return response;
            }
        }
        /// <summary>
        /// Post提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Post的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task<HttpResponseMessage> PostWithResponseAsync<T>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                try
                {
                    response = await client.PostAsync(url, httpContent);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Post，Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }

                return response;
            }
        }
        /// <summary>
        /// Post提交对象,无内容，无返回值
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static HttpResponseMessage PostWithResponse(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            return PostWithResponse<object>(null, url, httpHeaders, errorHandler);
        }
        /// <summary>
        ///  Post提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PostWithResponse<T, V>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = client.PostAsync(url, httpContent).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Post，Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return new HttpResult<V>(JsonSerializerHelper.Deserialize<V>(strContent), response);

                    //return response.Content.ReadAsAsync<V>().Result;
                }

            }
        }
        /// <summary>
        ///  Post提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PostWithResponseAsync<T, V>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }

                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }

                }
                HttpResponseMessage response = null;
                try
                {
                    response = await client.PostAsync(url, httpContent);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Post，Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return new HttpResult<V>(JsonSerializerHelper.Deserialize<V>(strContent), response);
                    //return await response.Content.ReadAsAsync<V>();
                }

            }
        }
        public static HttpResult<V> PostWithResponse<V>(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            return PostWithResponse<object, V>(null, url, httpHeaders, errorHandler);
        }







        /// <summary>
        /// Get获取
        /// </summary>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage GetWithResponse(string url, Dictionary<string, string> httpHeaders)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }

                HttpResponseMessage response = null;
                try
                {
                    response = client.GetAsync(url).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Get，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }

                return response;
            }

        }

        /// <summary>
        /// Get获取(异步)
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static async Task<HttpResponseMessage> GetWithResponseAsync(string url, Dictionary<string, string> httpHeaders)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Get，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }

                return response;
            }

        }
        /// <summary>
        /// get获取
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static HttpResult<T> GetWithResponse<T>(string url, Dictionary<string, string> httpHeaders)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = client.GetAsync(url).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Get，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return new HttpResult<T>(JsonSerializerHelper.Deserialize<T>(strContent), response);

                    //return response.Content.ReadAsAsync<T>().Result;
                }

            }

        }
        /// <summary>
        /// get获取(异步)
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        /// <returns></returns>
        public static async Task<HttpResult<T>> GetWithResponseAsync<T>(string url, Dictionary<string, string> httpHeaders)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Get，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return new HttpResult<T>(JsonSerializerHelper.Deserialize<T>(strContent), response);

                    //return await response.Content.ReadAsAsync<T>();
                }

            }

        }









        /// <summary>
        /// Get获取
        /// </summary>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage GetWithResponse(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = client.GetAsync(url).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Get，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }

                return response;
            }

        }

        /// <summary>
        /// Get获取(异步)
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static async Task<HttpResponseMessage> GetWithResponseAsync(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Get，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }

                return response;
            }

        }
        /// <summary>
        /// get获取
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static HttpResult<T> GetWithResponse<T>(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }

                HttpResponseMessage response = null;
                try
                {
                    response = client.GetAsync(url).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Get，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return new HttpResult<T>(JsonSerializerHelper.Deserialize<T>(strContent), response);

                    //return response.Content.ReadAsAsync<T>().Result;
                }

            }

        }
        /// <summary>
        /// get获取(异步)
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        /// <returns></returns>
        public static async Task<HttpResult<T>> GetWithResponseAsync<T>(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Get，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return new HttpResult<T>(JsonSerializerHelper.Deserialize<T>(strContent), response);

                    //return await response.Content.ReadAsAsync<T>();
                }

            }
        }






        /// <summary>
        /// Put提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage PutWithResponse<T>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                try
                {
                    response = client.PutAsync(url, httpContent).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Put,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }

                return response;
            }
        }
        /// <summary>
        /// Put提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task<HttpResponseMessage> PutWithResponseAsync<T>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                try
                {
                    response = await client.PutAsync(url, httpContent);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Put,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }

                return response;
            }
        }
        /// <summary>
        /// Put提交对象,无内容，无返回值
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static HttpResponseMessage PutWithResponse(string url, Dictionary<string, string> httpHeaders)
        {
            return PutWithResponse<object>(null, url, httpHeaders, new HttpErrorHandlerDefault());
        }
        /// <summary>
        ///  Put提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PutWithResponse<T, V>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = client.PutAsync(url, httpContent).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Put,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return new HttpResult<V>(JsonSerializerHelper.Deserialize<V>(strContent), response);

                    //return response.Content.ReadAsAsync<V>().Result;
                }

            }
        }
        /// <summary>
        ///  Put提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PutWithResponseAsync<T, V>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = await client.PutAsync(url, httpContent);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Put,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return new HttpResult<V>(JsonSerializerHelper.Deserialize<V>(strContent), response);
                    //return await response.Content.ReadAsAsync<V>();
                }

            }
        }
        public static HttpResult<V> PutWithResponse<V>(string url, Dictionary<string, string> httpHeaders)
        {
            return PutWithResponse<object, V>(null, url, httpHeaders, new HttpErrorHandlerDefault());
        }








        /// <summary>
        /// Put提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage PutWithResponse<T>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                try
                {
                    response = client.PutAsync(url, httpContent).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Put,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }

                return response;
            }
        }
        /// <summary>
        /// Put提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Put的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task<HttpResponseMessage> PutWithResponseAsync<T>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }
                try
                {
                    response = await client.PutAsync(url, httpContent);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Put,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }

                return response;
            }
        }
        /// <summary>
        /// Put提交对象,无内容，无返回值
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static HttpResponseMessage PutWithResponse(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            return PutWithResponse<object>(null, url, httpHeaders, errorHandler);
        }
        /// <summary>
        ///  Put提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PutWithResponse<T, V>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpResponseMessage response = null;
                try
                {
                    response = client.PutAsync(url, httpContent).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Put,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return new HttpResult<V>(JsonSerializerHelper.Deserialize<V>(strContent), response);

                    //return response.Content.ReadAsAsync<V>().Result;
                }

            }

        }
        /// <summary>
        ///  Put提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PutWithResponseAsync<T, V>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpResponseMessage response = null;
                try
                {
                    response = await client.PutAsync(url, httpContent);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Put,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return new HttpResult<V>(JsonSerializerHelper.Deserialize<V>(strContent), response);
                    //return await response.Content.ReadAsAsync<V>();
                }

            }
        }
        public static HttpResult<V> PutWithResponse<V>(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            return PutWithResponse<object, V>(null, url, httpHeaders, errorHandler);
        }






        /// <summary>
        /// Patch提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Patch的对象</param>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage PatchWithResponse<T>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = httpContent
                };
                try
                {
                    response = client.SendAsync(request).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Patch,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }

                return response;
            }
        }

        /// <summary>
        /// Patch提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Patch的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task<HttpResponseMessage> PatchWithResponseAsync<T>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage { Method = new HttpMethod("PATCH"), RequestUri = new Uri(url) };
                request.Content = httpContent;
                try
                {
                    response = await client.SendAsync(request);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Patch,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }

                return response;
            }
        }

        /// <summary>
        /// Patch提交对象,无内容，无返回值
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static HttpResponseMessage PatchWithResponse(string url, Dictionary<string, string> httpHeaders)
        {
            return PatchWithResponse<object>(null, url, httpHeaders, new HttpErrorHandlerDefault());
        }
        /// <summary>
        ///  Patch提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PatchWithResponse<T, V>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = httpContent
                };
                HttpResponseMessage response = null;
                try
                {
                    response = client.SendAsync(request).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Patch,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return new HttpResult<V>(JsonSerializerHelper.Deserialize<V>(strContent), response);

                    //return response.Content.ReadAsAsync<V>().Result;
                }

            }
        }
        /// <summary>
        ///  Patch提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PatchWithResponseAsync<T, V>(T data, string url, Dictionary<string, string> httpHeaders)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = httpContent
                };

                HttpResponseMessage response = null;

                try
                {
                    response = await client.SendAsync(request);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Patch,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return new HttpResult<V>(JsonSerializerHelper.Deserialize<V>(strContent), response);
                    //return await response.Content.ReadAsAsync<V>();
                }

            }
        }
        public static HttpResult<V> PatchWithResponse<V>(string url, Dictionary<string, string> httpHeaders)
        {
            return PatchWithResponse<object, V>(null, url, httpHeaders, new HttpErrorHandlerDefault());
        }







        /// <summary>
        /// Patch提交对象，无返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Patch的对象</param>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage PatchWithResponse<T>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = httpContent
                };
                try
                {
                    response = client.SendAsync(request).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Patch,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }

                return response;
            }
        }

        /// <summary>
        /// Patch提交对象，无返回值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Patch的对象</param>
        /// <param name="url">服务地址</param>
        public static async Task<HttpResponseMessage> PatchWithResponseAsync<T>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            HttpResponseMessage response = null;
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }

            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage { Method = new HttpMethod("PATCH"), RequestUri = new Uri(url) };
                request.Content = httpContent;
                try
                {
                    response = await client.SendAsync(request);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Patch,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }
                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }
                return response;
            }
        }

        /// <summary>
        /// Patch提交对象,无内容，无返回值
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static HttpResponseMessage PatchWithResponse(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            return PatchWithResponse<object>(null, url, httpHeaders, errorHandler);
        }
        /// <summary>
        ///  Patch提交对象，返回值类型为V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpResult<V> PatchWithResponse<T, V>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = httpContent
                };
                HttpResponseMessage response = null;
                try
                {
                    response = client.SendAsync(request).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Patch,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return new HttpResult<V>(JsonSerializerHelper.Deserialize<V>(strContent), response);

                    //return response.Content.ReadAsAsync<V>().Result;
                }

            }
        }
        /// <summary>
        ///  Patch提交对象，返回值类型为V(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResult<V>> PatchWithResponseAsync<T, V>(T data, string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            StringContent httpContent = null;
            string json = string.Empty;
            if (typeof(T).IsValueType || (!typeof(T).IsValueType && !EqualityComparer<T>.Default.Equals(data, default(T))))
            {
                json = JsonSerializerHelper.Serializer(data);
                httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
            }


            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    if (httpContent != null)
                    {
                        foreach (var headerItem in httpHeaders)
                        {
                            if (headerItem.Key.StartsWith("_Content_"))
                            {
                                httpContent.Headers.Add(headerItem.Key.Substring(9, headerItem.Key.Length - 8), headerItem.Value);
                            }
                        }
                    }
                    foreach (var headerItem in httpHeaders)
                    {
                        if (!headerItem.Key.StartsWith("_Content_"))
                        {
                            client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                        }
                    }
                }

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = httpContent
                };
                HttpResponseMessage response = null;
                try
                {
                    response = await client.SendAsync(request);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Patch,Data：{json}，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return new HttpResult<V>(JsonSerializerHelper.Deserialize<V>(strContent), response);
                    //return await response.Content.ReadAsAsync<V>();
                }

            }
        }
        public static HttpResult<V> PatchWithResponse<V>(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            return PatchWithResponse<object, V>(null, url, httpHeaders, errorHandler);
        }




        /// <summary>
        /// Delete获取
        /// </summary>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage DeleteWithResponse(string url, Dictionary<string, string> httpHeaders)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = client.DeleteAsync(url).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Delete，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }

                return response;
            }

        }
        /// <summary>
        /// Delete获取(异步)
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static async Task<HttpResponseMessage> DeleteWithResponseAsync(string url, Dictionary<string, string> httpHeaders)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;

                try
                {
                    response = await client.DeleteAsync(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Delete，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }

                return response;
            }

        }
        /// <summary>
        /// Delete获取
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static HttpResult<T> DeleteWithResponse<T>(string url, Dictionary<string, string> httpHeaders)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = client.DeleteAsync(url).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Delete，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return new HttpResult<T>(JsonSerializerHelper.Deserialize<T>(strContent), response);

                    //return response.Content.ReadAsAsync<T>().Result;
                }

            }

        }
        /// <summary>
        /// Delete获取(异步)
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        /// <returns></returns>
        public static async Task<HttpResult<T>> DeleteWithResponseAsync<T>(string url, Dictionary<string, string> httpHeaders)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;

                try
                {
                    response = await client.DeleteAsync(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Delete，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await new HttpErrorHandlerDefault().Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return new HttpResult<T>(JsonSerializerHelper.Deserialize<T>(strContent), response);

                    //return await response.Content.ReadAsAsync<T>();
                }

            }

        }





        /// <summary>
        /// Delete获取
        /// </summary>
        /// <param name="url">服务地址</param>
        public static HttpResponseMessage DeleteWithResponse(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = client.DeleteAsync(url).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Delete，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }

                return response;

            }

        }
        /// <summary>
        /// Delete获取(异步)
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        public static async Task<HttpResponseMessage> DeleteWithResponseAsync(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = await client.DeleteAsync(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Delete，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }
                return response;

            }

        }
        /// <summary>
        /// Delete获取
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <returns></returns>
        public static HttpResult<T> DeleteWithResponse<T>(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = client.DeleteAsync(url).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Delete，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = errorHandler.Do(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    throw exception;
                }
                else
                {
                    var strContent = response.Content.ReadAsStringAsync().Result;
                    return new HttpResult<T>(JsonSerializerHelper.Deserialize<T>(strContent), response);

                    //return response.Content.ReadAsAsync<T>().Result;
                }

            }

        }
        /// <summary>
        /// Delete获取(异步)
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="url">服务地址</param>
        /// <param name="httpHeaders">http头信息</param>
        /// <returns></returns>
        public static async Task<HttpResult<T>> DeleteWithResponseAsync<T>(string url, Dictionary<string, string> httpHeaders, IHttpErrorHandler errorHandler)
        {
            using (HttpClient client = _httpClientFactoryGenerator().CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (httpHeaders != null)
                {
                    foreach (var headerItem in httpHeaders)
                    {
                        client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    }
                }
                HttpResponseMessage response = null;
                try
                {
                    response = await client.DeleteAsync(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http请求出错，Url：{url}，Method：Delete，详细信息：{ex.Message},{ex.StackTrace}");
                }

                //Logger.WriteLog("SerialNumberServiceProxy StatusCode:"+response.StatusCode.ToString(), System.Diagnostics.EventLogEntryType.Warning);
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await errorHandler.Do(response).ConfigureAwait(false);
                    throw exception;
                }
                else
                {
                    var strContent = await response.Content.ReadAsStringAsync();
                    return new HttpResult<T>(JsonSerializerHelper.Deserialize<T>(strContent), response);

                    //return await response.Content.ReadAsAsync<T>();
                }

            }

        }
    }



    /// <summary>
    /// http响应的错误处理
    /// </summary>
    public interface IHttpErrorHandler
    {
        /// <summary>
        /// 将错误响应转换为异常
        /// </summary>
        /// <param name="response">错误响应</param>
        /// <returns></returns>
        Task<Exception> Do(HttpResponseMessage response);
    }

    /// <summary>
    /// 默认的http响应错误处理
    /// 首先尝试将错误反序列化为ErrorMessage,如果成功，则转换成UtilityException返回
    /// 否则转换成Exception，内容为response的body的内容
    /// </summary>
    public class HttpErrorHandlerDefault : IHttpErrorHandler
    {
        public async Task<Exception> Do(HttpResponseMessage response)
        {
            ErrorMessage errorResult = null;
            try
            {

                var strContent = await response.Content.ReadAsStringAsync();

                errorResult = JsonSerializerHelper.Deserialize<ErrorMessage>(strContent);
                if (errorResult == null)
                {
                    errorResult = new ErrorMessage()
                    {
                        Code = -1,
                        Message = response.ReasonPhrase
                    };
                }
            }
            catch
            {

                string errorMessage = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = response.ReasonPhrase;
                }
                return await Task.FromResult(new Exception(errorMessage));

            }

            if (errorResult.Message == null)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = response.ReasonPhrase;
                }
                return await Task.FromResult(new Exception(errorMessage));
            }
            else
            {
                var fragment = new TextFragment()
                {
                    Code = string.Empty,
                    DefaultFormatting = errorResult.Message,
                    ReplaceParameters = new List<object>() { }
                };

                return await Task.FromResult(new UtilityException(errorResult.Code, fragment, errorResult.Level, errorResult.Type));
            }

        }
    }


    public class HttpResult<T>
    {
        public HttpResult(T value, HttpResponseMessage response)
        {
            Value = value;
            Response = response;
        }
        public T Value { get; private set; }
        public HttpResponseMessage Response { get; private set; }
    }
}
