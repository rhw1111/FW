using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using Aliyun.OSS;
using Aliyun.OSS.Common;
using Aliyun.OSS.Util;
using Aliyun.Acs.Sts;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Sts.Model.V20150401;
using MSLibrary.Storge;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;
using MSLibrary.Security;
using MSLibrary.FileManagement;
using MSLibrary.Transaction;
using MSLibrary.Thread;

namespace MSLibrary.Ali
{
    /// <summary>
    /// 阿里oss服务终结点
    /// </summary>
    public class OSSEndpoint : EntityBase<IOSSEndpointIMP>
    {

        private static IFactory<IOSSEndpointIMP> _ossEndpointIMPFactory;
        public static IFactory<IOSSEndpointIMP> OssEndpointIMPFactory
        {
            set
            {
                _ossEndpointIMPFactory = value;
            }
        }
        public override IFactory<IOSSEndpointIMP> GetIMPFactory()
        {
            return _ossEndpointIMPFactory;
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {

                return GetAttribute<Guid>("ID");
            }
            set
            {
                SetAttribute<Guid>("ID", value);
            }
        }

        /// <summary>
        /// 终结点名称
        /// </summary>
        public string Name
        {
            get
            {
                return GetAttribute<string>("Name");
            }
            set
            {
                SetAttribute<string>("Name", value);
            }
        }

        /// <summary>
        /// 所处区域名称
        /// </summary>
        public string Region
        {
            get
            {
                return GetAttribute<string>("Region");
            }
            set
            {
                SetAttribute<string>("Region", value);
            }
        }
        

        /// <summary>
        /// 终结点类型
        /// 0:使用区域地址
        /// 1:使用CNAME
        /// </summary>
        public int Type
        {
            get
            {
                return GetAttribute<int>("Type");
            }
            set
            {
                SetAttribute<int>("Type", value);
            }
        }

        /// <summary>
        /// 区域地址
        /// 当Type为0时使用
        /// </summary>
        public string Address
        {
            get
            {
                return GetAttribute<string>("Address");
            }
            set
            {
                SetAttribute<string>("Address", value);
            }
        }


        /// <summary>
        /// 是否是公共读
        /// </summary>
        public bool IsPublic
        {
            get
            {
                return GetAttribute<bool>("IsPublic");
            }
            set
            {
                SetAttribute<bool>("IsPublic", value);
            }
        }


        /// <summary>
        /// CName
        /// 当Type为1时使用
        /// </summary>
        public string CName
        {
            get
            {
                return GetAttribute<string>("CName");
            }
            set
            {
                SetAttribute<string>("CName", value);
            }
        }

        /// <summary>
        /// AccessKeyId
        /// </summary>
        public string AccessKeyId
        {
            get
            {
                return GetAttribute<string>("AccessKeyId");
            }
            set
            {
                SetAttribute<string>("AccessKeyId", value);
            }
        }
        /// <summary>
        /// AccessKeySecret
        /// </summary>
        public string AccessKeySecret
        {
            get
            {
                return GetAttribute<string>("AccessKeySecret");
            }
            set
            {
                SetAttribute<string>("AccessKeySecret", value);
            }
        }


        /// <summary>
        /// STS授权时用到的角色ID
        /// </summary>
        public string STSRole
        {
            get
            {
                return GetAttribute<string>("STSRole");
            }
            set
            {
                SetAttribute<string>("STSRole", value);
            }
        }


        /// <summary>
        /// 要操作的存储空间名称
        /// </summary>
        public string Bucket
        {
            get
            {
                return GetAttribute<string>("Bucket");
            }
            set
            {
                SetAttribute<string>("Bucket", value);
            }
        }
        /// <summary>
        /// 临时文件夹目录
        /// </summary>
        public string TempFolder
        {
            get
            {
                return GetAttribute<string>("TempFolder");
            }
            set
            {
                SetAttribute<string>("TempFolder", value);
            }
        }

        /// <summary>
        /// 永久文件夹目录
        /// </summary>
        public string PermanentFolder
        {
            get
            {
                return GetAttribute<string>("PermanentFolder");
            }
            set
            {
                SetAttribute<string>("PermanentFolder", value);
            }
        }
        
        /// <summary>
        /// 分片上传的起始大小
        /// 只有大于等于这个大小的文件流，才会启用分片上传
        /// </summary>
        public long MultipartStartSize
        {
            get
            {
                return GetAttribute<long>("MultipartMinSize");
            }
            set
            {
                SetAttribute<long>("MultipartMinSize", value);
            }
        }

        /// <summary>
        /// 分片上传的每片大小
        /// </summary>
        public long MultipartPerSize
        {
            get
            {
                return GetAttribute<long>("MultipartPerSize");
            }
            set
            {
                SetAttribute<long>("MultipartPerSize", value);
            }
        }


        /// <summary>
        /// 分片上传的片数
        /// </summary>
        public int MultipartNumber
        {
            get
            {
                return GetAttribute<int>("MultipartNumber");
            }
            set
            {
                SetAttribute<int>("MultipartNumber", value);
            }
        }

        /// <summary>
        /// 启用分片读取的起始大小
        /// </summary>
        public long ReadStartSize
        {
            get
            {
                return GetAttribute<long>("ReadStartSize");
            }
            set
            {
                SetAttribute<long>("ReadStartSize", value);
            }
        }

        /// <summary>
        /// 分片读取时，每次读取的长度
        /// </summary>
        public long ReadPerSize
        {
            get
            {
                return GetAttribute<long>("ReadPerSize");
            }
            set
            {
                SetAttribute<long>("ReadPerSize", value);
            }
        }

        /// <summary>
        /// 分片读取时，分片数量
        /// </summary>
        public long ReadNumber
        {
            get
            {
                return GetAttribute<long>("ReadNumber");
            }
            set
            {
                SetAttribute<long>("ReadNumber", value);
            }
        }


        /// <summary>
        /// 分片操作时的并行度
        /// </summary>
        public int MultipartParallelNumber
        {
            get
            {
                return GetAttribute<int>("MultipartParallelNumber");
            }
            set
            {
                SetAttribute<int>("MultipartParallelNumber", value);
            }
        }


        /// <summary>
        /// 回调地址
        /// </summary>
        public string CallbackUrl
        {
            get
            {
                return GetAttribute<string>("CallbackUrl");
            }
            set
            {
                SetAttribute<string>("CallbackUrl", value);
            }
        }

        /// <summary>
        /// CDN域【读取的时候需要替换的域名】
        /// </summary>
        public string ReadDomain
        {
            get
            {
                return GetAttribute<string>("ReadDomain");
            }
            set
            {
                SetAttribute<string>("ReadDomain", value);
            }
        }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }


    }

    public interface IOSSEndpointIMP
    {
        /// <summary>
        /// 创建文件名称
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task<string> CreateFileName(OSSEndpoint endpoint);
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="filePath"></param>
        /// <param name="credentialInfo"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task Write(OSSEndpoint endpoint, string filePath, string credentialInfo, string displayName, string suffix, Stream stream, ObjectMetadata objMetadata=null);
        /// <summary>
        /// 读取指定位置范围的文件
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="filePath"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        Task<Stream> Read(OSSEndpoint endpoint, string filePath, long start, long? end);
        /// <summary>
        /// 分片读取文件（如果不符合分片条件，则完整读取一次）
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        Task Read(OSSEndpoint endpoint, string filePath, Func<(int,int,Stream), Task> action);
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task Delete(OSSEndpoint endpoint, string filePath);
        /// <summary>
        /// 批量删除文件
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="filePaths"></param>
        /// <returns></returns>
        Task DeleteBatch(OSSEndpoint endpoint, IList<string> filePaths);
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="sourceBucket"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="targetFilePath"></param>
        /// <param name="credentialInfo"></param>
        /// <param name="newMetadata"></param>
        /// <returns></returns>
        Task Copy(OSSEndpoint endpoint, string sourceBucket, string sourceFilePath, string targetFilePath, string credentialInfo, ObjectMetadata newMetadata = null);
        /// <summary>
        /// 获取文件元数据
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<ObjectMetadata> GetMetadata(OSSEndpoint endpoint, string filePath);
        /// <summary>
        /// 修改文件元数据
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="key"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        Task ModifyMetadata(OSSEndpoint endpoint, string filePath, ObjectMetadata metadata);

        /// <summary>
        /// 创建授权Uri
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="filePath"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        Task<string> CreatePresignedUri(OSSEndpoint endpoint, string filePath, SignHttpMethod method);
        /// <summary>
        /// 创建授权uri（指定过期时间）
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="filePath"></param>
        /// <param name="method"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        Task<string> CreatePresignedUri(OSSEndpoint endpoint, string filePath, SignHttpMethod method, long minutes);
        /// <summary>
        /// 创建回调参数
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="extensionInfo"></param>
        /// <returns></returns>
        Task<Dictionary<string,string>> CreatePostCallbackParameters(OSSEndpoint endpoint, string filePath, Dictionary<string, string> extensionInfos);
        /// <summary>
        /// 创建分片上传完成时回调的header信息列表
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<Dictionary<string,string>> CreateMultipartUploadCompleteHeaderCallbackParameters(OSSEndpoint endpoint, MultipartStorgeInfo storgeInfo, Dictionary<string,string> extensionInfos);

        /// <summary>
        /// 创建分片复制完成时回调的header信息列表
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> CreateMultipartCopyCompleteHeaderCallbackParameters(OSSEndpoint endpoint, MultipartStorgeInfo storgeInfo, Dictionary<string, string> extensionInfos);

        /// <summary>
        /// 创建分片完成时的Body数据
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="storgeInfo"></param>
        /// <returns></returns>
        Task<string>  CreateMultipartCompleteBody(OSSEndpoint endpoint, MultipartStorgeInfo storgeInfo);
        /// <summary>
        /// 创建分片上传明细的header信息列表
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<Dictionary<MultipartStorgeInfoDetail, Dictionary<string, string>>> CreateMultipartUploadDetailHeaderParameters(OSSEndpoint endpoint, MultipartStorgeInfo storgeInfo);

        /// <summary>
        /// 创建分片上传明细的header信息列表
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="infoID"></param>
        /// <returns></returns>
        Task<Dictionary<MultipartStorgeInfoDetail, Dictionary<string, string>>> CreateMultipartUploadDetailHeaderParameters(OSSEndpoint endpoint, Guid infoID);

        /// <summary>
        /// 创建分片复制明细的header信息列表
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="infoID"></param>
        /// <returns></returns>
        Task<Dictionary<MultipartStorgeInfoDetail, Dictionary<string, string>>> CreateMultipartCopyDetailHeaderParameters(OSSEndpoint endpoint, Guid infoID);


        /// <summary>
        /// 创建分片上传
        /// 返回生成的分片存储信息以及所属明细的Header信息列表
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="filePath"></param>
        /// <param name="displayName"></param>
        /// <param name="credentialInfo"></param>
        /// <param name="suffix"></param>
        /// <param name="size"></param>
        /// <param name="objMetadata"></param>
        /// <returns></returns>
        Task<(MultipartStorgeInfo,Dictionary<MultipartStorgeInfoDetail, Dictionary<string, string>>)> CreateMultipartUpload(OSSEndpoint endpoint, string filePath, string displayName, string suffix, string credentialInfo, long size, ObjectMetadata objMetadata = null);

        Task<(MultipartStorgeInfo, Dictionary<MultipartStorgeInfoDetail, Dictionary<string, string>>)> CreateMultipartCopy(OSSEndpoint endpoint, string filePath, string sourceBucket, string sourceObject, string credentialInfo);

        /// <summary>
        /// 创建分片复制明细的header信息列表
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<Dictionary<MultipartStorgeInfoDetail, Dictionary<string, string>>> CreateMultipartCopyDetailHeaderParameters(OSSEndpoint endpoint, MultipartStorgeInfo storgeInfo);



        /// <summary>
        /// 判断是否需要分片
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="size">文件大小</param>
        /// <returns></returns>
        Task<bool> NeedMultipart(OSSEndpoint endpoint, long size);

        Task<MultipartStorgeInfo> CreateMultipartUploadInfo(OSSEndpoint endpoint, string filePath,string displayName,string suffix, string credentialInfo, long size, ObjectMetadata objMetadata=null);

        Task<MultipartStorgeInfo> GetMultipartUploadInfo(OSSEndpoint endpoint, Guid infoID);

        Task<QueryResult<MultipartStorgeInfo>> GetMultipartUploadInfo(OSSEndpoint endpoint, int page,int size);


        Task<MultipartStorgeInfo> CreateMultipartCopyInfo(OSSEndpoint endpoint, string filePath, string sourceBucket, string sourceObject,string credentialInfo, ObjectMetadata newMetadata = null);

        Task<MultipartStorgeInfo> GetMultipartCopyInfo(OSSEndpoint endpoint, Guid infoID);

        Task<QueryResult<MultipartStorgeInfo>> GetMultipartCopyInfo(OSSEndpoint endpoint, int page, int size);

        Task DeleteMultipart(OSSEndpoint endpoint, Guid infoID);

        Task CompleteMultipartUploadInfoDetail(OSSEndpoint endpoint, Guid infoID,Guid detailID,string etag);
        Task CompleteMultipartCopyInfoDetail(OSSEndpoint endpoint, Guid infoID, Guid detailID, string etag);


        Task<string> UploadMultipart(OSSEndpoint endpoint, string filePath, string uploadID,int number,byte[] bytes);

        Task<string> CopyMultipart(OSSEndpoint endpoint, string filePath,string sourceBucket, string sourceObject, string uploadID, long start, long size, int number);


        Task CompleteMultipart(OSSEndpoint endpoint, string filePath, string uploadID,List<(int,string)> parts);

        /// <summary>
        /// 完成分片上传
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="infoID"></param>
        /// <returns></returns>
        Task CompletMultipartUpload(OSSEndpoint endpoint, Guid infoID);
        /// <summary>
        /// 完成分片复制
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="infoID"></param>
        /// <returns></returns>
        Task CompletMultipartCopy(OSSEndpoint endpoint, Guid infoID);

        Task AbortMultipart(OSSEndpoint endpoint, string filePath, string uploadID);

        Task ClearMultipart(OSSEndpoint endpoint);
    }

    public class OSSEndpointIMP : IOSSEndpointIMP
    {
        /// <summary>
        /// 最大分片数量
        /// </summary>
        private const int _maxMultipartNumer = 10000;
        /// <summary>
        /// 分片每片最大大小（字节）
        /// </summary>
        private const long _maxMultipartPerSize = (long)5 * 1024 * 1024 * 1024;
        /// <summary>
        /// 分片每片最小大小（字节）
        /// </summary>
        private const int _minMultipartPerSize = 100*1024;
        /// <summary>
        /// 最大总大小（字节）
        /// </summary>
        private const long _maxTotalSize = (long)48.8 * 1024* 1024 * 1024 * 1024;

        private IMultipartStorgeInfoRepository _multipartStorgeInfoRepository;
        private ISecurityService _securityService;

        public OSSEndpointIMP(IMultipartStorgeInfoRepository multipartStorgeInfoRepository, ISecurityService securityService)
        {
            _multipartStorgeInfoRepository = multipartStorgeInfoRepository;
            _securityService = securityService;
        }
        public async Task CompleteMultipart(OSSEndpoint endpoint, string filePath, string uploadID, List<(int, string)> parts)
        {
            var client= getOssClient(endpoint);
            CompleteMultipartUploadRequest request = new CompleteMultipartUploadRequest(endpoint.Bucket, filePath, uploadID);
            foreach(var item in parts)
            {
                request.PartETags.Add(new PartETag(item.Item1, item.Item2));
            }

            client.CompleteMultipartUpload(request);
            await Task.FromResult(0);
        }

        public async Task CompleteMultipartUploadInfoDetail(OSSEndpoint endpoint, Guid infoID, Guid detailID, string etag)
        {
            var strSourceInfo = generateUploadPartSourceInfo(endpoint);
            var storgeInfo=await _multipartStorgeInfoRepository.QueryBySourceID(strSourceInfo, infoID);
            if (storgeInfo==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundMultipartStorgeInfoBySourceInfoAndID,
                    DefaultFormatting = "找不到源信息为{0}，Id为{1}的分片存储信息",
                    ReplaceParameters = new List<object>() { strSourceInfo, infoID.ToString()}
                };

                throw new UtilityException((int)Errors.NotFoundMultipartStorgeInfoBySourceInfoAndID, fragment);
            }

            await storgeInfo.CompleteDetail(detailID, etag);
       
        }

        public async Task Copy(OSSEndpoint endpoint,string sourceBucket, string sourceFilePath, string targetFilePath, string credentialInfo, ObjectMetadata newMetadata = null)
        {
            //获取源文件的元数据
            var sourceMeatdata = await getObjMetadata(endpoint, sourceBucket, sourceFilePath);
            if (sourceMeatdata == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.AliOSSNotFoundObject,
                    DefaultFormatting = "阿里OSS中找不到Bucket为{0}，Key为{1}的文件",
                    ReplaceParameters = new List<object>() { sourceBucket, sourceFilePath }
                };

                throw new UtilityException((int)Errors.AliOSSNotFoundObject, fragment);
            }

            //判断是否需要分片复制
            var size = sourceMeatdata.ContentLength;
            var needPart = await NeedMultipart(endpoint, size);
            var client = getOssClient(endpoint);
            if (!needPart)
            {
                //直接复制
                var task = new TaskCompletionSource<int>();
                CopyObjectRequest request = new CopyObjectRequest(endpoint.Bucket, sourceFilePath, endpoint.Bucket, targetFilePath);
                if (newMetadata != null)
                {
                    request.NewObjectMetadata = newMetadata;
                }
                var asyncResult = client.BeginCopyObject(request, result =>
                {
                    var response = client.EndCopyResult(result);
                    task.SetResult(0);
                }, null);

                await task.Task;
                return;
            }
            else
            {
                bool needCreate = false;
                //分片复制
                //检查是否已经存在分片存储信息
                var copySourceInfo = generateCopyPartSourceInfo(endpoint);
                var storgeInfo = await _multipartStorgeInfoRepository.QueryRunByName(targetFilePath);

                if (storgeInfo.SourceInfo != copySourceInfo)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.ExistRunMultipartStorgeInfoByName,
                        DefaultFormatting = "已经存在名称为{0}的未完成分片存储信息",
                        ReplaceParameters = new List<object>() { targetFilePath }
                    };
                    throw new UtilityException((int)Errors.ExistRunMultipartStorgeInfoByName, fragment);
                }


                if (storgeInfo != null)
                {
                    //如果已经存在
                    //如果状态是已删除，则抛出异常，因为已删除的存储信息无法进行处理
                    /*if (storgeInfo.Status == 2)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.AliOSSMultiparStorgeInfoStatusNotAllowCopy,
                            DefaultFormatting = "阿里OSS分片存储信息的状态不允许复制，分片存储Id：{0}，状态：{1}",
                            ReplaceParameters = new List<object>() { storgeInfo.ID.ToString(), storgeInfo.Status.ToString() }
                        };

                        throw new UtilityException((int)Errors.AliOSSMultiparStorgeInfoStatusNotAllowCopy, fragment);
                    }

                    //如果状态为已完成，则不再做处理
                    if (storgeInfo.Status == 1)
                    {
                        return;
                    }
                    */
                    //判断文件长度是否相等，如果不相等，执行删除操作
                    if (storgeInfo.Size != size)
                    {
                        needCreate = true;
                        await DeleteMultipart(endpoint, storgeInfo.ID);
                    }

                }

                if (needCreate)
                {
                    //创建新的storgeInfo
                    storgeInfo = await CreateMultipartCopyInfo(endpoint, targetFilePath,sourceBucket, sourceFilePath,credentialInfo, newMetadata);
                }

                //对storgeInfo执行复制操作
                await copyMultipartStorgeInfo(endpoint, storgeInfo);
            }




        }

        public async Task<string> CreateFileName(OSSEndpoint endpoint)
        {
            return await Task.FromResult(Guid.NewGuid().ToString());
        }

        public async Task<Dictionary<string, string>> CreateMultipartUploadCompleteHeaderCallbackParameters(OSSEndpoint endpoint, MultipartStorgeInfo storgeInfo, Dictionary<string, string> extensionInfos)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            //构建callback参数
            StringBuilder strExtension = new StringBuilder();
            foreach (var item in extensionInfos)
            {
                strExtension.Append($"&{item.Key}={item.Value.ToUrlEncode()}");
            }

            String strBody = $"bucket=${{bucket}}&object=${{object}}&etag=${{etag}}&size=${{size}}&mimeType=${{mimeType}}{strExtension.ToString()}";
            JObject jObj = new JObject();
            jObj.Add("callbackUrl", JToken.FromObject(endpoint.CallbackUrl));
            jObj.Add("callbackBody", JToken.FromObject(strBody));

            var strCallback = JsonSerializerHelper.Serializer(jObj);

            result.Add("x-oss-callback", strCallback);

            //构建Authorization参数
            //首先生成sts
            string strPolicy = $@"{{
                                ""Statement"": [
                                    {{
                                    ""Action"": [
                                        ""oss:PutObject""
                                       ],
                                    ""Effect"": ""Allow"",
                                    ""Resource"": [""acs:oss:{endpoint.Region}:*:{endpoint.Bucket}/{storgeInfo.Name}""]
                                    }}
                                    ],
                                ""Version"": ""1""
                                }}";

            (string stsAccessKeyId, string stsAccessKeySecret, string stsToken) = generateSTS(endpoint, endpoint.AccessKeyId, endpoint.AccessKeySecret, endpoint.STSRole, "MSLibrary", strPolicy, ProtocolType.HTTPS, 20 * 60);


            //用sts的id、Secret、token生成签名
            var authorization = await generateAuthorization(endpoint, stsAccessKeyId, stsAccessKeySecret, "PUT", string.Empty, new Dictionary<string, string>()
                {
                    { "x-oss-security-token",stsToken}
                },
            $"/{endpoint.Bucket}/{storgeInfo.Name}?uploadId={storgeInfo.ExtensionInfo}"
            );


            result.Add("Authorization", authorization);
            //加入Date参数
            result.Add("Date", DateTime.UtcNow.ToString("r"));


            return result;
        }

        public async Task<string> CreateMultipartCompleteBody(OSSEndpoint endpoint, MultipartStorgeInfo storgeInfo)
        {
            XDocument resultDoc = XDocument.Parse("<CompleteMultipartUpload></CompleteMultipartUpload>");
            
            //获取所有已经完成的明细
            await storgeInfo.GetDetailAll(1, async (detail) =>
             {
                 XElement newNode;
                 var node=XElement.Parse("<Part></Part>");

                 newNode = XElement.Parse($"<PartNumber>{detail.Number.ToString()}</PartNumber>");
                 node.Add(newNode);

                 newNode = XElement.Parse($"<ETag>{detail.CompleteExtensionInfo}</ETag>");
                 node.Add(newNode);

                 resultDoc.Add(node);
                 await Task.FromResult(0);
             });

            return resultDoc.ToString();
        }
        public async Task<Dictionary<MultipartStorgeInfoDetail, Dictionary<string, string>>> CreateMultipartUploadDetailHeaderParameters(OSSEndpoint endpoint, MultipartStorgeInfo storgeInfo)
        {
            //构建Authorization参数
            //首先生成sts
            string strPolicy = $@"{{
                                ""Statement"": [
                                    {{
                                    ""Action"": [
                                        ""oss:PutObject""
                                       ],
                                    ""Effect"": ""Allow"",
                                    ""Resource"": [""acs:oss:{endpoint.Region}:*:{endpoint.Bucket}/{storgeInfo.Name}""]
                                    }}
                                    ],
                                ""Version"": ""1""
                                }}";

            (string stsAccessKeyId, string stsAccessKeySecret, string stsToken) = generateSTS(endpoint, endpoint.AccessKeyId, endpoint.AccessKeySecret, endpoint.STSRole, "MSLibrary", strPolicy, ProtocolType.HTTPS, 20 * 60);


            Dictionary<MultipartStorgeInfoDetail, Dictionary<string, string>> result = new Dictionary<MultipartStorgeInfoDetail, Dictionary<string, string>>();
            //获取尚未完成的分片存储明细
            await storgeInfo.GetDetailAll(0, async (detail) =>
            {
                Dictionary<string, string> header = new Dictionary<string, string>();
                //用sts的id、Secret、token生成签名
                var authorization=await generateAuthorization(endpoint, stsAccessKeyId, stsAccessKeySecret, "PUT", string.Empty, new Dictionary<string, string>()
                {
                    { "x-oss-security-token",stsToken}
                },
                $"/{endpoint.Bucket}/{storgeInfo.Name}?uploadId={storgeInfo.ExtensionInfo}&partNumber={detail.Number.ToString()}"
                );
                header.Add("Authorization", authorization);


                result.Add(detail, header);
            });

            return result;

        }
        public async Task<MultipartStorgeInfo> CreateMultipartUploadInfo(OSSEndpoint endpoint, string filePath, string displayName,string suffix, string credentialInfo, long size, ObjectMetadata objMetadata=null)
        {
            if (size>_maxTotalSize)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.AliOSSMultipartExceedTotalMaxSize,
                    DefaultFormatting = "阿里OSS分片单一文件超出最大限制，最大限制：{0}字节，文件名称：{1}，实际大小：{2}",
                    ReplaceParameters = new List<object>() { _maxTotalSize, filePath, size }
                };
                throw new UtilityException((int)Errors.AliOSSMultipartExceedTotalMaxSize, fragment);
            }
            //执行InitiateMultipartUpload
            var client = getOssClient(endpoint);

            if (objMetadata != null)
            {
                objMetadata.ContentType = FileSuffixMimeMapContainer.GetMime(suffix);
                objMetadata.ContentDisposition = $"{displayName}.{suffix}";
            }
            else
            {
                objMetadata = new ObjectMetadata();
                objMetadata.ContentType = FileSuffixMimeMapContainer.GetMime(suffix);
                objMetadata.ContentDisposition = $"{displayName}.{suffix}";
            }

            InitiateMultipartUploadRequest request = new InitiateMultipartUploadRequest(endpoint.Bucket, filePath, objMetadata);
            var response=client.InitiateMultipartUpload(request);
            //创建MultipartStorgeInfo

            MultipartStorgeInfo storgeInfo = new MultipartStorgeInfo()
            {
                Name = filePath,
                DisplayName = displayName,
                Suffix = suffix,
                Size = size,
                ExtensionInfo = response.UploadId,
                CredentialInfo = credentialInfo,
                SourceInfo = generateUploadPartSourceInfo(endpoint),
                Status = 0
            };

            //计算分片数量,优先按MultipartPerSize计算
            var number = size / endpoint.MultipartPerSize;
           
            if (number% endpoint.MultipartPerSize>0)
            {
                number++;
            }
            if (number > endpoint.MultipartNumber)
            {
                number = endpoint.MultipartNumber;
            }

            if (number>_maxMultipartNumer)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.AliOSSMultipartExceedMaxNumer,
                    DefaultFormatting = "阿里OSS分片数量超出最大限制，最大限制：{0}，文件名称：{1}，实际分片数量：{2}",
                    ReplaceParameters = new List<object>() { _maxTotalSize, filePath,number }
                };
                throw new UtilityException((int)Errors.AliOSSMultipartExceedMaxNumer, fragment);
            }

            var perSize = size / number;
            if (perSize>_maxMultipartPerSize)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.AliOSSMultipartExceedMaxPerSize,
                    DefaultFormatting = "阿里OSS分片每片大小超出最大限制，最大限制：{0}，文件名称：{1}，实际每片大小：{2}",
                    ReplaceParameters = new List<object>() { _maxMultipartPerSize, filePath, perSize }
                };
                throw new UtilityException((int)Errors.AliOSSMultipartExceedMaxPerSize, fragment);
            }

            if (perSize < _minMultipartPerSize)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.AliOSSMultipartLessMaxPerSize,
                    DefaultFormatting = "阿里OSS分片每片大小低于最小限制，最小限制：{0}，文件名称：{1}，实际每片大小：{2}",
                    ReplaceParameters = new List<object>() { _minMultipartPerSize, filePath, perSize }
                };
                throw new UtilityException((int)Errors.AliOSSMultipartLessMaxPerSize, fragment);
            }

            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                await storgeInfo.Add();
                await storgeInfo.AddDetails(size, (int)number);

                scope.Complete();
            }



            return storgeInfo;
        }

        public async Task<Dictionary<string, string>> CreatePostCallbackParameters(OSSEndpoint endpoint, string filePath, Dictionary<string, string> extensionInfos)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            //构建callback参数
            StringBuilder strExtension = new StringBuilder();
            foreach(var item in extensionInfos)
            {
                strExtension.Append($"&{item.Key}={item.Value.ToUrlEncode()}");
            }
            
            String strBody = $"bucket=${{bucket}}&object=${{object}}&etag=${{etag}}&size=${{size}}&mimeType=${{mimeType}}{strExtension.ToString()}";
            JObject jObj = new JObject();
            jObj.Add("callbackUrl", JToken.FromObject(endpoint.CallbackUrl));
            jObj.Add("callbackBody", JToken.FromObject(strBody));

            var strCallback = JsonSerializerHelper.Serializer(jObj);

            result.Add("callback", strCallback);

            //构建OSSAccessKeyId参数
            result.Add("OSSAccessKeyId", endpoint.AccessKeyId);

            //构建policy参数
            var strPolicy = await generatePostCallbackPolicypublic(endpoint, filePath, strCallback);
            result.Add("policy", strPolicy);

            //构建Signature参数
            var strSignPolicy=_securityService.SignByKey(strPolicy, endpoint.AccessKeySecret);
            result.Add("Signature", strSignPolicy);

            //构建key参数
            result.Add("key", filePath);

            return result;
        }


        public async Task<string> CreatePresignedUri(OSSEndpoint endpoint, string filePath, SignHttpMethod method)
        {
            var client = getOssClient(endpoint);
            var request = new GeneratePresignedUriRequest(endpoint.Bucket, filePath, method)
            {
                
                Expiration = DateTime.UtcNow.AddMinutes(20)
            };

            var uri = client.GeneratePresignedUri(request);

            string strUri;
            if (endpoint.IsPublic)
            {
                strUri = uri.ToString().Replace(uri.Query, string.Empty);
            }
            else
            {
                strUri = uri.ToString();
            }
            return await Task.FromResult(strUri);
        }

        public async Task<string> CreatePresignedUri(OSSEndpoint endpoint, string filePath, SignHttpMethod method, long minutes)
        {
            DateTime expire = DateTime.UtcNow;
            if (minutes >= Double.MaxValue)
            {
                expire = expire.AddYears((int)(minutes / 60 / 24 / 365));
            }
            else
            {
                expire = expire.AddMinutes(minutes);
            }
            var client = getOssClient(endpoint);
            var request = new GeneratePresignedUriRequest(endpoint.Bucket, filePath, method)
            {
                Expiration = expire
            };
            var uri = client.GeneratePresignedUri(request);

            string strUri;
            if (endpoint.IsPublic)
            {
                strUri = uri.ToString().Replace(uri.Query, string.Empty);

            }
            else
            {
                strUri = uri.ToString();
            }
            if (!string.IsNullOrEmpty(endpoint.ReadDomain))
            {
                strUri = strUri.Replace(endpoint.CName, endpoint.ReadDomain);
            }
            return await Task.FromResult(strUri);
        }

        public async Task Delete(OSSEndpoint endpoint, string filePath)
        {
            var client = getOssClient(endpoint);
            //DeleteObjectsRequest request = new DeleteObjectsRequest(endpoint.Bucket, new List<string>() { filePath });
            client.DeleteObject(endpoint.Bucket, filePath);
            await Task.CompletedTask;

        }

        public async Task<ObjectMetadata> GetMetadata(OSSEndpoint endpoint, string filePath)
        {
            return await getObjMetadata(endpoint, endpoint.Bucket, filePath);
        }

        public async Task<MultipartStorgeInfo> GetMultipartUploadInfo(OSSEndpoint endpoint, Guid infoID)
        {
            var sourceInfo=generateUploadPartSourceInfo(endpoint);
            return await _multipartStorgeInfoRepository.QueryBySourceID(sourceInfo, infoID);
        }

        public async Task<QueryResult<MultipartStorgeInfo>> GetMultipartUploadInfo(OSSEndpoint endpoint, int page, int size)
        {
            var sourceInfo = generateUploadPartSourceInfo(endpoint);
            return await _multipartStorgeInfoRepository.QueryBySourcePage(sourceInfo, 0, page, size);
        }

        public async Task ModifyMetadata(OSSEndpoint endpoint, string filePath, ObjectMetadata metadata)
        {
            var client = getOssClient(endpoint);
            client.ModifyObjectMeta(endpoint.Bucket, filePath, metadata);
            await Task.FromResult(0);
        }

        public async Task<bool> NeedMultipart(OSSEndpoint endpoint, long size)
        {
            if (size>endpoint.MultipartStartSize)
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<Stream> Read(OSSEndpoint endpoint, string filePath, long start, long? end)
        {
            TaskCompletionSource<Stream> task = new TaskCompletionSource<Stream>();

            var client = getOssClient(endpoint);
            GetObjectRequest request = new GetObjectRequest(endpoint.Bucket, filePath);
            if (end != null)
            {
                request.SetRange(start, end.Value);
            }
            client.BeginGetObject(request, (result) =>
            {
                var objResult = client.EndGetObject(result);

                task.SetResult(objResult.Content);
            }, null);

            var stream = await task.Task;
            return stream;
        }

        public async Task Read(OSSEndpoint endpoint,string filePath, Func<(int,int,Stream), Task> action)
        {
            //获取文件元数据
            var metadata = await GetMetadata(endpoint, filePath);
            if (metadata == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.AliOSSNotFoundObject,
                    DefaultFormatting = "阿里OSS中找不到Bucket为{0}，Key为{1}的文件",
                    ReplaceParameters = new List<object>() { endpoint.Bucket, filePath }
                };

                throw new UtilityException((int)Errors.AliOSSNotFoundObject, fragment);
            }

            //判断是否需要分片读取
            if (metadata.ContentLength>endpoint.ReadStartSize)
            {
                //需要分片读取
                var number = metadata.ContentLength / endpoint.ReadPerSize;
                if (metadata.ContentLength% endpoint.ReadPerSize>0)
                {
                    number++;
                }

                if (number>endpoint.ReadNumber)
                {
                    number = endpoint.ReadNumber;
                }

                List<(int,long, long)> items = new System.Collections.Generic.List<(int,long, long)>();
                long perSize;
                if (metadata.ContentLength % number>0)
                {
                    perSize = metadata.ContentLength / (number - 1);
                }
                else
                {
                    perSize=metadata.ContentLength / number;
                }

                long start = 0;
                for(var index=0;index<=number-1;index++)
                {
                    if (index < number - 1)
                    {
                        items.Add((index,start, start + perSize-1));
                    }
                    else
                    {
                        items.Add((index,start, start + (metadata.ContentLength % number)-1));
                    }

                    start = start + perSize;
                }

                await ParallelHelper.ForEach(items, endpoint.MultipartParallelNumber, async (item) =>
                  {
                      TaskCompletionSource<Stream> task = new TaskCompletionSource<Stream>();
                      //一次读取
                      var client = getOssClient(endpoint);
                      GetObjectRequest request = new GetObjectRequest(endpoint.Bucket, filePath);
                      request.SetRange(item.Item2, item.Item3);
                      client.BeginGetObject(request, (result) =>
                      {
                          var objResult = client.EndGetObject(result);

                          task.SetResult(objResult.Content);
                      }, null);

                      var stream = await task.Task;
                      await action(((int)number, item.Item1, stream));
                  });


            }
            else
            {
                TaskCompletionSource<Stream> task = new TaskCompletionSource<Stream>();
                //一次读取
                var client = getOssClient(endpoint);
                GetObjectRequest request = new GetObjectRequest(endpoint.Bucket, filePath);
                client.BeginGetObject(request, (result) =>
                 {
                     var objResult=client.EndGetObject(result);
                  
                     task.SetResult(objResult.Content);
                 }, null);

                var stream=await task.Task;
                await action((1,1,stream));
            }
        }

        public async Task<string> UploadMultipart(OSSEndpoint endpoint, string filePath, string uploadID, int number, byte[] bytes)
        {
            var client = getOssClient(endpoint);

            var task = new TaskCompletionSource<string>();

            using (MemoryStream stream=new MemoryStream(bytes))
            {
                stream.Position = 0;
                UploadPartRequest request = new UploadPartRequest(endpoint.Bucket, filePath, uploadID)
                {
                    PartNumber = number,
                    InputStream = stream,
                    PartSize = bytes.Length
                };

                client.BeginUploadPart(request,
                    result =>
                    {
                        var uploadResult=client.EndUploadPart(result);
                        task.SetResult(uploadResult.PartETag.ETag);
                    }, null);
                stream.Close();
            }

            return await task.Task;
        }

        public async Task Write(OSSEndpoint endpoint, string filePath, string credentialInfo, string displayName, string suffix, Stream stream, ObjectMetadata objMetadata=null)
        {
            if (objMetadata != null)
            {
                objMetadata.ContentType = FileSuffixMimeMapContainer.GetMime(suffix);
                objMetadata.ContentDisposition = $"{displayName}.{suffix}";
            }
            else
            {
                objMetadata = new ObjectMetadata();
                objMetadata.ContentType = FileSuffixMimeMapContainer.GetMime(suffix);
                objMetadata.ContentDisposition = $"{displayName}.{suffix}";
            }

            var taskComplete = new TaskCompletionSource<int>();

            //判断是否需要分片上传
            var size=stream.Length;
            var needPart = await NeedMultipart(endpoint, size);
            var client = getOssClient(endpoint);
            if (!needPart)
            {
                //直接上传
                PutObjectRequest request = new PutObjectRequest(endpoint.Bucket, filePath, stream);
                                           
                request.Metadata = objMetadata;

                client.BeginPutObject(request, (result) =>
                 {
                     var response = client.EndPutObject(result);
                     taskComplete.SetResult(0);
                 }, null);

                await taskComplete.Task;
                return;
            }
            else
            {
                bool needCreate = false;
                //分片上传
                //检查是否已经存在分片存储信息
                var uploadSourceInfo= generateUploadPartSourceInfo(endpoint);
                var storgeInfo=await _multipartStorgeInfoRepository.QueryRunByName(filePath);

                if (storgeInfo.SourceInfo!= uploadSourceInfo)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.ExistRunMultipartStorgeInfoByName,
                        DefaultFormatting = "已经存在名称为{0}的未完成分片存储信息",
                        ReplaceParameters = new List<object>() { filePath }
                    };
                    throw new UtilityException((int)Errors.ExistRunMultipartStorgeInfoByName, fragment);
                }


                if (storgeInfo != null)
                {
                    /*//如果已经存在
                    //如果状态是已删除，则抛出异常，因为已删除的存储信息无法进行处理
                    if (storgeInfo.Status == 2)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.AliOSSMultiparStorgeInfoStatusNotAllowUpload,
                            DefaultFormatting = "阿里OSS分片存储信息的状态不允许上传，分片存储Id：{0}，状态：{1}",
                            ReplaceParameters = new List<object>() { storgeInfo.ID.ToString(), storgeInfo.Status.ToString() }
                        };

                        throw new UtilityException((int)Errors.AliOSSMultiparStorgeInfoStatusNotAllowUpload, fragment);
                    }

                    //如果状态为已完成，则不再做处理
                    if (storgeInfo.Status == 1)
                    {
                        return;
                    }
                    */
                    //判断文件长度是否相等，如果不相等，执行删除方法
                    if (storgeInfo.Size != size)
                    {
                        needCreate = true;

                        await DeleteMultipart(endpoint, storgeInfo.ID);
                    }

                }

                if (needCreate)
                {
                    //创建新的storgeInfo
                    storgeInfo = await CreateMultipartUploadInfo(endpoint, filePath, displayName, suffix,credentialInfo, size,objMetadata);
                }

                //对storgeInfo执行上传操作
                await uploadMultipartStorgeInfo(endpoint, storgeInfo, stream);
            }
        }


        private OssClient getOssClient(OSSEndpoint endpoint)
        {
            var conf = new ClientConfiguration();
            OssClient client;
            if (endpoint.Type == 0)
            {
                conf.IsCname = false;
                client = new OssClient(endpoint.Address, endpoint.AccessKeyId, endpoint.AccessKeySecret, conf);
            }
            else
            {

                conf.IsCname = true;
                client = new OssClient(endpoint.CName, endpoint.AccessKeyId, endpoint.AccessKeySecret, conf);
            }

            return client;
        }

        private string generateUploadPartSourceInfo(OSSEndpoint endpoint)
        {
            return $"AliOSS-Upload-{endpoint.ID.ToString()}";
        }

        private string generateCopyPartSourceInfo(OSSEndpoint endpoint)
        {
            return $"AliOSS-Copy-{endpoint.ID.ToString()}";
        }


        private (string,string,string) generateSTS(OSSEndpoint endpoint, String accessKeyId, String accessKeySecret, String roleArn,
            String roleSessionName, String policy, ProtocolType protocolType, long durationSeconds)
        {
            DefaultProfile.AddEndpoint("", "", "Sts", "sts.aliyuncs.com");
            // 创建一个 Aliyun Acs Client, 用于发起 OpenAPI 请求
            IClientProfile profile = DefaultProfile.GetProfile("", accessKeyId, accessKeySecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);

            // 创建一个 AssumeRoleRequest 并设置请求参数
            AssumeRoleRequest request = new AssumeRoleRequest();
            //request.Version = STS_API_VERSION;
            request.Method = MethodType.POST;
            request.Protocol = protocolType;

            request.RoleArn = roleArn;
            request.RoleSessionName = roleSessionName;
            request.Policy = policy;
            request.DurationSeconds = durationSeconds;

            // 发起请求，并得到response
            AssumeRoleResponse response = client.GetAcsResponse(request);

            return (response.Credentials.AccessKeyId,response.Credentials.AccessKeySecret,response.Credentials.SecurityToken);


        }


        private async Task<string> generatePostCallbackPolicypublic (OSSEndpoint endpoint,string filePath, string callback)
        {
            var client = getOssClient(endpoint);
            PolicyConditions conds = new PolicyConditions();
            //bucket精确匹配终结点中的Bucket
            conds.AddConditionItem(MatchMode.Exact, "bucket", endpoint.Bucket);
            //文件名起始匹配终结点的临时文件夹
            conds.AddConditionItem(MatchMode.Exact, "key", filePath);
            //文件大小范围0~1G
            conds.AddConditionItem("content-length-range", 0, 1073741824);
            //callback精确匹配该终结点的callback参数
            conds.AddConditionItem(MatchMode.Exact, "callback", callback);

            //Content-Type起始匹配空，目的是为了保证存在该域
            conds.AddConditionItem(MatchMode.StartWith, "Content-Type", "");
            //Content-Disposition起始匹配空，目的是为了保证存在该域
            conds.AddConditionItem(MatchMode.StartWith, "Content-Disposition", "");

            var strPolicy = client.GeneratePostPolicy(DateTime.UtcNow.AddMinutes(20), conds);

            await Task.CompletedTask;
            return strPolicy;
        }

        private async Task uploadMultipartStorgeInfo(OSSEndpoint endpoint,MultipartStorgeInfo storgeInfo,Stream stream)
        {
            //获取未上传的明细信息
            List<(MultipartStorgeInfoDetail, byte[])> details = new List<(MultipartStorgeInfoDetail, byte[])>();
            await storgeInfo.GetDetailAll(0, async (detail) =>
            {
                //获取每个分片的字节
                byte[] bytes = new byte[detail.End - detail.Start];
                stream.Position = detail.Start;
                var length = await stream.ReadAsync(bytes, 0, bytes.Length);

                if (length != bytes.Length)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.AliOSSMultiparStorgeInfoDetailDataPositionNotCorrect,
                        DefaultFormatting = "阿里OSS分片存储信息明细中的数据位置不正确，分片存储Id：{0}，分片存储明细编号：{1}，实际文件长度：{2}",
                        ReplaceParameters = new List<object>() { storgeInfo.ID.ToString(), detail.Number.ToString(), stream.Length.ToString() }
                    };
                    throw new UtilityException((int)Errors.AliOSSMultiparStorgeInfoDetailDataPositionNotCorrect, fragment);
                }

                details.Add((detail,bytes));
            });

            //为每个未上传的明细信息执行上传操作
            await ParallelHelper.ForEach(details, endpoint.MultipartParallelNumber, async (data) =>
              {
                  var eTag= await UploadMultipart(endpoint, storgeInfo.Name, storgeInfo.ExtensionInfo, data.Item1.Number, data.Item2);
                  await storgeInfo.CompleteDetail(data.Item1.ID, eTag);
              });

            List<(int,string)> completeParts = new List<(int, string)>();
            //重新获取所有已经完成的明细
            await storgeInfo.GetDetailAll(1,
                async(detail)=>
                {
                    completeParts.Add((detail.Number, detail.CompleteExtensionInfo));
                    await Task.FromResult(0);
                }
                );
            try
            {
                //执行完成上传
                await CompleteMultipart(endpoint, storgeInfo.Name, storgeInfo.ExtensionInfo, completeParts);

            }
            catch (OssException ex)
            {
                if (ex.ErrorCode.ToLower() != "nosuchupload")
                {
                    throw;
                }
            }
            //执行storgeInfo的完成操作
            await storgeInfo.Complete(string.Empty);
        }

        private async Task copyMultipartStorgeInfo(OSSEndpoint endpoint, MultipartStorgeInfo storgeInfo)
        {
            //获取未复制的明细信息
            List<MultipartStorgeInfoDetail> details = new List<MultipartStorgeInfoDetail>();
            await storgeInfo.GetDetailAll(0, async (detail) =>
            {
                details.Add((detail));
                await Task.CompletedTask;
            });

            //获取分片复制扩展信息
            var copyExtensionInfo = JsonSerializerHelper.Deserialize<MultipartCopyExtensionInfo>(storgeInfo.ExtensionInfo);

            //为每个未复制的明细信息执行复制操作
            await ParallelHelper.ForEach(details, endpoint.MultipartParallelNumber, async (data) =>
            {

                var eTag = await CopyMultipart(endpoint,storgeInfo.Name, copyExtensionInfo.SourceBucket, copyExtensionInfo.SourceFilePath, copyExtensionInfo.UploadID,data.Start,data.End-data.Start,data.Number);
                await storgeInfo.CompleteDetail(data.ID, eTag);
            });

            List<(int, string)> completeParts = new List<(int, string)>();
            //重新获取所有已经完成的明细
            await storgeInfo.GetDetailAll(1,
                async (detail) =>
                {
                    completeParts.Add((detail.Number, detail.CompleteExtensionInfo));
                    await Task.FromResult(0);
                }
                );
            try
            {
                //执行完成上传
                await CompleteMultipart(endpoint, storgeInfo.Name, copyExtensionInfo.UploadID, completeParts);

            }
            catch (OssException ex)
            {
                if (ex.ErrorCode.ToLower() != "nosuchupload")
                {
                    throw;
                }
            }
            //执行storgeInfo的完成操作
            await storgeInfo.Complete(string.Empty);
        }
        private async Task<ObjectMetadata> getObjMetadata(OSSEndpoint endpoint,string bucket,string filePath)
        {
            ObjectMetadata metadata = null;
            var client = getOssClient(endpoint);
            try
            {
                 metadata = client.GetObjectMetadata(bucket, filePath);
            }
            catch(OssException ex)
            {
                if (ex.ErrorCode.ToLower()!= "nosuchkey")
                {
                    throw;
                }
            }
            return await Task.FromResult(metadata);
        }

        private async Task checkRunMulitipartStorgeInfoNotExistByName(OSSEndpoint endpoint, string name)
        {
            var info=await _multipartStorgeInfoRepository.QueryRunBySourceName(generateUploadPartSourceInfo(endpoint), name);
            if (info!=null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.ExistRunMultipartStorgeInfoByName,
                    DefaultFormatting = "已经存在名称为{0}的未完成分片存储信息",
                    ReplaceParameters = new List<object>() { name }
                };
                throw new UtilityException((int)Errors.ExistRunMultipartStorgeInfoByName, fragment);
            }
        }

        public async Task<string> generateAuthorization(OSSEndpoint endpoint,string accessKeyId,string accessKeySecret,string method,string strContentMD5,Dictionary<string,string> headers,string resource)
        {
            //拼装header
            StringBuilder strHeader = new StringBuilder();
            var headerList=headers.OrderBy(pair => pair.Key);
            foreach(var item in headerList)
            {
                strHeader.Append($"{item.Key}:{item.Value}\n");
            }

            var strSignature = _securityService.SignByKey($"{method}\n{DateTime.UtcNow.ToString("r")}\n{strHeader.ToString()}{resource}",endpoint.AccessKeySecret);

            return await Task.FromResult($"OSS {accessKeyId}:{strSignature}");
        }

        public async Task<Dictionary<string, string>> CreateMultipartCopyCompleteHeaderCallbackParameters(OSSEndpoint endpoint, MultipartStorgeInfo storgeInfo, Dictionary<string, string> extensionInfos)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            //构建callback参数
            StringBuilder strExtension = new StringBuilder();
            foreach (var item in extensionInfos)
            {
                strExtension.Append($"&{item.Key}={item.Value.ToUrlEncode()}");
            }

            String strBody = $"bucket=${{bucket}}&object=${{object}}&etag=${{etag}}&size=${{size}}&mimeType=${{mimeType}}{strExtension.ToString()}";
            JObject jObj = new JObject();
            jObj.Add("callbackUrl", JToken.FromObject(endpoint.CallbackUrl));
            jObj.Add("callbackBody", JToken.FromObject(strBody));

            var strCallback = JsonSerializerHelper.Serializer(jObj);

            result.Add("x-oss-callback", strCallback);

            //构建Authorization参数
            //首先生成sts
            string strPolicy = $@"{{
                                ""Statement"": [
                                    {{
                                    ""Action"": [
                                        ""oss:PutObject""
                                       ],
                                    ""Effect"": ""Allow"",
                                    ""Resource"": [""acs:oss:{endpoint.Region}:*:{endpoint.Bucket}/{storgeInfo.Name}""]
                                    }}
                                    ],
                                ""Version"": ""1""
                                }}";

            (string stsAccessKeyId, string stsAccessKeySecret, string stsToken) = generateSTS(endpoint, endpoint.AccessKeyId, endpoint.AccessKeySecret, endpoint.STSRole, "MSLibrary", strPolicy, ProtocolType.HTTPS, 20 * 60);


            //用sts的id、Secret、token生成签名
            var authorization = await generateAuthorization(endpoint, stsAccessKeyId, stsAccessKeySecret, "PUT", string.Empty, new Dictionary<string, string>()
                {
                    { "x-oss-security-token",stsToken}
                },
            $"/{endpoint.Bucket}/{storgeInfo.Name}?uploadId={storgeInfo.ExtensionInfo}"
            );


            result.Add("Authorization", authorization);
            //加入Date参数
            result.Add("Date", DateTime.UtcNow.ToString("r"));


            return result;
        }

        public async Task<Dictionary<MultipartStorgeInfoDetail, Dictionary<string, string>>> CreateMultipartCopyDetailHeaderParameters(OSSEndpoint endpoint, MultipartStorgeInfo storgeInfo)
        {
            //获取CopyEntensionInfo
            var copyExtensionInfo = JsonSerializerHelper.Deserialize<MultipartCopyExtensionInfo>(storgeInfo.ExtensionInfo);
            //构建Authorization参数
            //首先生成sts
            string strPolicy = $@"{{
                                ""Statement"": [
                                    {{
                                    ""Action"": [
                                        ""oss:PutObject""
                                       ],
                                    ""Effect"": ""Allow"",
                                    ""Resource"": [""acs:oss:{endpoint.Region}:*:{endpoint.Bucket}/{storgeInfo.Name}""]
                                    }},
                                    {{
                                    ""Action"": [
                                        ""oss:GetObject""
                                       ],
                                    ""Effect"": ""Allow"",
                                    ""Resource"": [""acs:oss:{endpoint.Region}:*:{copyExtensionInfo.SourceBucket}/{copyExtensionInfo.SourceFilePath}""]
                                    }}
                                    ],
                                ""Version"": ""1""
                                }}";

            (string stsAccessKeyId, string stsAccessKeySecret, string stsToken) = generateSTS(endpoint, endpoint.AccessKeyId, endpoint.AccessKeySecret, endpoint.STSRole, "MSLibrary", strPolicy, ProtocolType.HTTPS, 20 * 60);


            Dictionary<MultipartStorgeInfoDetail, Dictionary<string, string>> result = new Dictionary<MultipartStorgeInfoDetail, Dictionary<string, string>>();
            //获取尚未完成的分片存储明细
            await storgeInfo.GetDetailAll(0, async (detail) =>
            {
                var strCopySource = $"/{copyExtensionInfo.SourceBucket}/{copyExtensionInfo.SourceFilePath}";
                var strCopySourceRange = $"bytes={detail.Start.ToString()}-{(detail.End - detail.Start).ToString()}";

                Dictionary<string, string> header = new Dictionary<string, string>();
                //用sts的id、Secret、token生成签名
                var authorization = await generateAuthorization(endpoint, stsAccessKeyId, stsAccessKeySecret, "PUT", string.Empty, new Dictionary<string, string>()
                {
                    { "x-oss-security-token",stsToken},
                    { "x-oss-copy-source",strCopySource},
                    { "x-oss-copy-source-range",strCopySourceRange}
                },
                $"/{endpoint.Bucket}/{storgeInfo.Name}?uploadId={storgeInfo.ExtensionInfo}&partNumber={detail.Number.ToString()}"
                );
                header.Add("Authorization", authorization);

                header.Add("Content-Length", $"{(detail.End - detail.Start).ToString()}");

                header.Add("x-oss-copy-source", strCopySource);

                header.Add("x-oss-copy-source-range", strCopySourceRange);

                result.Add(detail, header);
            });

            return result;
        }

        public async Task<MultipartStorgeInfo> CreateMultipartCopyInfo(OSSEndpoint endpoint, string filePath, string sourceBucket, string sourceObject,string credentialInfo, ObjectMetadata newMetadata = null)
        {
            var client = getOssClient(endpoint);
            
            //获取源目标信息
            var sourceMeatdata = await getObjMetadata(endpoint, sourceBucket, sourceObject);
            if (sourceMeatdata==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.AliOSSNotFoundObject,
                    DefaultFormatting = "阿里OSS中找不到Bucket为{0}，Key为{1}的文件",
                    ReplaceParameters = new List<object>() { sourceBucket, sourceObject }
                };

                throw new UtilityException((int)Errors.AliOSSNotFoundObject, fragment);
            }

            if (newMetadata==null)
            {
                newMetadata = sourceMeatdata;
            }
            InitiateMultipartUploadRequest request = new InitiateMultipartUploadRequest(endpoint.Bucket, filePath, newMetadata);
            var response = client.InitiateMultipartUpload(request);
            //创建MultipartStorgeInfo

            string suffix = string.Empty;
            string displayName = string.Empty;
            if (sourceMeatdata.ContentDisposition!=null )
            {
                var arrayDisplayName = sourceMeatdata.ContentDisposition.Split('.');
                if (arrayDisplayName.Length>1)
                {
                    displayName = arrayDisplayName[0];
                    suffix= arrayDisplayName[1];
                }
                else
                {
                    displayName = sourceMeatdata.ContentDisposition;
                }
            }
            MultipartCopyExtensionInfo copyExtensionInfo = new MultipartCopyExtensionInfo()
            {
                SourceBucket = sourceBucket,
                SourceFilePath = sourceObject,
                UploadID = response.UploadId
            };
            MultipartStorgeInfo storgeInfo = new MultipartStorgeInfo()
            {
                Name = filePath,
                DisplayName = displayName,
                Suffix = suffix,
                Size = sourceMeatdata.ContentLength,
                ExtensionInfo = JsonSerializerHelper.Serializer(copyExtensionInfo),
                CredentialInfo = credentialInfo,
                SourceInfo = generateCopyPartSourceInfo(endpoint),
                Status = 0
            };

            //计算分片数量,优先按MultipartPerSize计算
            var number = storgeInfo.Size / endpoint.MultipartPerSize;

            if (number % endpoint.MultipartPerSize > 0)
            {
                number++;
            }
            if (number > endpoint.MultipartNumber)
            {
                number = endpoint.MultipartNumber;
            }

            if (number > _maxMultipartNumer)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.AliOSSMultipartExceedMaxNumer,
                    DefaultFormatting = "阿里OSS分片数量超出最大限制，最大限制：{0}，文件名称：{1}，实际分片数量：{2}",
                    ReplaceParameters = new List<object>() { _maxTotalSize, filePath, number }
                };
                throw new UtilityException((int)Errors.AliOSSMultipartExceedMaxNumer, fragment);
            }

            var perSize = storgeInfo.Size / number;
            if (perSize > _maxMultipartPerSize)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.AliOSSMultipartExceedMaxPerSize,
                    DefaultFormatting = "阿里OSS分片每片大小超出最大限制，最大限制：{0}，文件名称：{1}，实际每片大小：{2}",
                    ReplaceParameters = new List<object>() { _maxMultipartPerSize, filePath, perSize }
                };
                throw new UtilityException((int)Errors.AliOSSMultipartExceedMaxPerSize, fragment);
            }

            if (perSize < _minMultipartPerSize)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.AliOSSMultipartLessMaxPerSize,
                    DefaultFormatting = "阿里OSS分片每片大小低于最小限制，最小限制：{0}，文件名称：{1}，实际每片大小：{2}",
                    ReplaceParameters = new List<object>() { _minMultipartPerSize, filePath, perSize }
                };
                throw new UtilityException((int)Errors.AliOSSMultipartLessMaxPerSize, fragment);
            }

            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                await storgeInfo.Add();
                await storgeInfo.AddDetails(storgeInfo.Size, (int)number);

                scope.Complete();
            }


            return storgeInfo;
        }

        public async Task<MultipartStorgeInfo> GetMultipartCopyInfo(OSSEndpoint endpoint, Guid infoID)
        {
            var copyPartSourceInfo= generateCopyPartSourceInfo(endpoint);
            return await _multipartStorgeInfoRepository.QueryBySourceID(copyPartSourceInfo, infoID);
        }

        public async Task<QueryResult<MultipartStorgeInfo>> GetMultipartCopyInfo(OSSEndpoint endpoint, int page, int size)
        {
            var copyPartSourceInfo = generateCopyPartSourceInfo(endpoint);
            return await _multipartStorgeInfoRepository.QueryBySourcePage(copyPartSourceInfo, 0, page, size);
        }

        public async Task AbortMultipart(OSSEndpoint endpoint, string filePath, string uploadID)
        {
            var client = getOssClient(endpoint);
            AbortMultipartUploadRequest request = new AbortMultipartUploadRequest(endpoint.Bucket, filePath, uploadID);
            client.AbortMultipartUpload(request);

            await Task.FromResult(0);
        }

        public async Task CompleteMultipartCopyInfoDetail(OSSEndpoint endpoint, Guid infoID, Guid detailID, string etag)
        {
            var strSourceInfo = generateCopyPartSourceInfo(endpoint);
            var storgeInfo = await _multipartStorgeInfoRepository.QueryBySourceID(strSourceInfo, infoID);
            if (storgeInfo == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundMultipartStorgeInfoBySourceInfoAndID,
                    DefaultFormatting = "找不到源信息为{0}，Id为{1}的分片存储信息",
                    ReplaceParameters = new List<object>() { strSourceInfo, infoID.ToString() }
                };

                throw new UtilityException((int)Errors.NotFoundMultipartStorgeInfoBySourceInfoAndID, fragment);
            }

            await storgeInfo.CompleteDetail(detailID, etag);
        }

        public Task ClearMultipart(OSSEndpoint endpoint)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteMultipart(OSSEndpoint endpoint, Guid infoID)
        {
            //查找Copy的StorgeInfo
            var storgeInfo=await _multipartStorgeInfoRepository.QueryBySourceID(generateCopyPartSourceInfo(endpoint), infoID);
            if (storgeInfo!=null)
            {
               
                //获取分片复制扩展信息
                var copyExtensionInfo=JsonSerializerHelper.Deserialize<MultipartCopyExtensionInfo>(storgeInfo.ExtensionInfo);
                //执行storgeInfo.Cancel
                await storgeInfo.Cancel();
                //执行取消操作，忽略OssException中的NoSuchUpload
                try
                {
                    await AbortMultipart(endpoint, storgeInfo.Name, copyExtensionInfo.UploadID);
                }
                catch(OssException ex)
                {
                    if (ex.ErrorCode.ToLower()!="nosuchupload")
                    {
                        throw;
                    }
                }

                //删除storgeInfo
                await storgeInfo.Delete();
            }

            //查找Upload的StorgeInfo
            storgeInfo = await _multipartStorgeInfoRepository.QueryBySourceID(generateUploadPartSourceInfo(endpoint), infoID);
            if (storgeInfo != null)
            {
                //执行storgeInfo.Cancel
                await storgeInfo.Cancel();
                //执行取消操作，忽略OssException中的NoSuchUpload
                try
                {
                    await AbortMultipart(endpoint, storgeInfo.Name, storgeInfo.ExtensionInfo);
                }
                catch (OssException ex)
                {
                    if (ex.ErrorCode.ToLower() != "nosuchupload")
                    {
                        throw;
                    }
                }

                //删除storgeInfo
                await storgeInfo.Delete();
            }


        }

        public async Task<string> CopyMultipart(OSSEndpoint endpoint, string filePath, string sourceBucket, string sourceObject, string uploadID, long start, long size, int number)
        {
            var client = getOssClient(endpoint);

            var task = new TaskCompletionSource<string>();

            var request = new UploadPartCopyRequest(endpoint.Bucket, filePath, sourceBucket, sourceObject, uploadID)
            {
                PartSize = size,
                PartNumber = number,
                BeginIndex = start
            };

            client.BeginUploadPartCopy(request,
                result =>
                {
                    var copyResult = client.EndUploadPartCopy(result);
                    task.SetResult(copyResult.PartETag.ETag);
                }, null);


            return await task.Task;
        }

        public async Task<(MultipartStorgeInfo, Dictionary<MultipartStorgeInfoDetail, Dictionary<string, string>>)> CreateMultipartUpload(OSSEndpoint endpoint, string filePath, string displayName, string suffix, string credentialInfo, long size, ObjectMetadata objMetadata = null)
        {
            //检查是否已经存在相同名称的未完成分片存储信息
            await checkRunMulitipartStorgeInfoNotExistByName(endpoint, filePath);
            //创建分片存储信息
            var info=await CreateMultipartUploadInfo(endpoint, filePath, displayName, suffix, credentialInfo, size, objMetadata);
            //获取分片存储信息明细的头信息列表键值对
            var details=await CreateMultipartUploadDetailHeaderParameters(endpoint, info);

            return (info, details);
        }

        public async Task<Dictionary<MultipartStorgeInfoDetail, Dictionary<string, string>>> CreateMultipartUploadDetailHeaderParameters(OSSEndpoint endpoint, Guid infoID)
        {
            //查询对应的StorgeInfo
            var storgeInfo= await GetMultipartUploadInfo(endpoint, infoID);
            if (storgeInfo==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundMultipartStorgeInfoBySourceInfoAndID,
                    DefaultFormatting = "找不到源信息为{0}，Id为{1}的分片存储信息",
                    ReplaceParameters = new List<object>() {generateUploadPartSourceInfo(endpoint), infoID.ToString() }
                };

                throw new UtilityException((int)Errors.NotFoundMultipartStorgeInfoBySourceInfoAndID, fragment);
            }

            //判断storgeInfo的status，如果是1或2，抛出异常
            if (storgeInfo.Status==1 || storgeInfo.Status==2)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.AliOSSMultiparStorgeInfoStatusNotAllowUpload,
                    DefaultFormatting = "阿里OSS分片存储信息的状态不允许上传，分片存储Id：{0}，状态：{1}",
                    ReplaceParameters = new List<object>() { storgeInfo.ID.ToString(), storgeInfo.Status.ToString() }
                };

                throw new UtilityException((int)Errors.AliOSSMultiparStorgeInfoStatusNotAllowUpload, fragment);
            }

            var details = await CreateMultipartUploadDetailHeaderParameters(endpoint, storgeInfo);

            return details;
        }

        public async Task CompletMultipartUpload(OSSEndpoint endpoint, Guid infoID)
        {
            //查询对应的StorgeInfo
            var storgeInfo = await GetMultipartUploadInfo(endpoint, infoID);
            if (storgeInfo == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundMultipartStorgeInfoBySourceInfoAndID,
                    DefaultFormatting = "找不到源信息为{0}，Id为{1}的分片存储信息",
                    ReplaceParameters = new List<object>() { generateUploadPartSourceInfo(endpoint), infoID.ToString() }
                };

                throw new UtilityException((int)Errors.NotFoundMultipartStorgeInfoBySourceInfoAndID, fragment);
            }

            //判断storgeInfo的status，如果是1或2，抛出异常
            if (storgeInfo.Status == 1 || storgeInfo.Status == 2)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.AliOSSMultiparStorgeInfoStatusNotAllowComplete,
                    DefaultFormatting = "阿里OSS分片存储信息的状态不允许执行完成操作，分片存储Id：{0}，状态：{1}",
                    ReplaceParameters = new List<object>() { storgeInfo.ID.ToString(), storgeInfo.Status.ToString() }
                };

                throw new UtilityException((int)Errors.AliOSSMultiparStorgeInfoStatusNotAllowComplete, fragment);
            }

            var unDetail = await storgeInfo.GetDetailTop(0, 1);
            if (unDetail != null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.AliOSSMultiparStorgeInfoNotAllowCompleteForUnDoDetail,
                    DefaultFormatting = "阿里OSS分片存储信息不允许执行完成操作,原因为包含有未完成的明细，分片存储Id：{0}",
                    ReplaceParameters = new List<object>() { storgeInfo.ID.ToString() }
                };

                throw new UtilityException((int)Errors.AliOSSMultiparStorgeInfoNotAllowCompleteForUnDoDetail, fragment);
            }



            List<(int, string)> completeParts = new List<(int, string)>();
            //获取所有已经完成的明细
            await storgeInfo.GetDetailAll(1,
                async (detail) =>
                {
                    completeParts.Add((detail.Number, detail.CompleteExtensionInfo));
                    await Task.FromResult(0);
                }
                );
            try
            {
                //执行完成上传
                await CompleteMultipart(endpoint, storgeInfo.Name, storgeInfo.ExtensionInfo, completeParts);

            }
            catch (OssException ex)
            {
                if (ex.ErrorCode.ToLower() != "nosuchupload")
                {
                    throw;
                }
            }
            //执行storgeInfo的完成操作
            await storgeInfo.Complete(string.Empty);
        }

        public async Task<(MultipartStorgeInfo, Dictionary<MultipartStorgeInfoDetail, Dictionary<string, string>>)> CreateMultipartCopy(OSSEndpoint endpoint, string filePath, string sourceBucket, string sourceObject, string credentialInfo)
        {
            //检查是否已经存在相同名称的未完成分片存储信息
            await checkRunMulitipartStorgeInfoNotExistByName(endpoint, filePath);
            //创建分片存储信息
            var info = await CreateMultipartCopyInfo(endpoint, filePath, sourceBucket,sourceObject, credentialInfo);
            //获取分片存储信息明细的头信息列表键值对
            var details = await CreateMultipartCopyDetailHeaderParameters(endpoint, info);

            return (info, details);
        }

        public async Task<Dictionary<MultipartStorgeInfoDetail, Dictionary<string, string>>> CreateMultipartCopyDetailHeaderParameters(OSSEndpoint endpoint, Guid infoID)
        {
            //查询对应的StorgeInfo
            var storgeInfo = await GetMultipartCopyInfo(endpoint, infoID);
            if (storgeInfo == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundMultipartStorgeInfoBySourceInfoAndID,
                    DefaultFormatting = "找不到源信息为{0}，Id为{1}的分片存储信息",
                    ReplaceParameters = new List<object>() { generateCopyPartSourceInfo(endpoint), infoID.ToString() }
                };

                throw new UtilityException((int)Errors.NotFoundMultipartStorgeInfoBySourceInfoAndID, fragment);
            }

            //判断storgeInfo的status，如果是1或2，抛出异常
            if (storgeInfo.Status == 1 || storgeInfo.Status == 2)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.AliOSSMultiparStorgeInfoStatusNotAllowCopy,
                    DefaultFormatting = "阿里OSS分片存储信息的状态不允许复制，分片存储Id：{0}，状态：{1}",
                    ReplaceParameters = new List<object>() { storgeInfo.ID.ToString(), storgeInfo.Status.ToString() }
                };

                throw new UtilityException((int)Errors.AliOSSMultiparStorgeInfoStatusNotAllowCopy, fragment);
            }

            var details = await CreateMultipartCopyDetailHeaderParameters(endpoint, storgeInfo);

            return details;
        }

        public async Task CompletMultipartCopy(OSSEndpoint endpoint, Guid infoID)
        {
            //查询对应的StorgeInfo
            var storgeInfo = await GetMultipartCopyInfo(endpoint, infoID);
            if (storgeInfo == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundMultipartStorgeInfoBySourceInfoAndID,
                    DefaultFormatting = "找不到源信息为{0}，Id为{1}的分片存储信息",
                    ReplaceParameters = new List<object>() { generateCopyPartSourceInfo(endpoint), infoID.ToString() }
                };

                throw new UtilityException((int)Errors.NotFoundMultipartStorgeInfoBySourceInfoAndID, fragment);
            }

            //判断storgeInfo的status，如果是1或2，抛出异常
            if (storgeInfo.Status == 1 || storgeInfo.Status == 2)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.AliOSSMultiparStorgeInfoStatusNotAllowComplete,
                    DefaultFormatting = "阿里OSS分片存储信息的状态不允许执行完成操作，分片存储Id：{0}，状态：{1}",
                    ReplaceParameters = new List<object>() { storgeInfo.ID.ToString(), storgeInfo.Status.ToString() }
                };

                throw new UtilityException((int)Errors.AliOSSMultiparStorgeInfoStatusNotAllowComplete, fragment);
            }

            var unDetail = await storgeInfo.GetDetailTop(0, 1);
            if (unDetail != null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.AliOSSMultiparStorgeInfoNotAllowCompleteForUnDoDetail,
                    DefaultFormatting = "阿里OSS分片存储信息不允许执行完成操作,原因为包含有未完成的明细，分片存储Id：{0}",
                    ReplaceParameters = new List<object>() { storgeInfo.ID.ToString() }
                };

                throw new UtilityException((int)Errors.AliOSSMultiparStorgeInfoNotAllowCompleteForUnDoDetail, fragment);
            }



            List<(int, string)> completeParts = new List<(int, string)>();
            //获取所有已经完成的明细
            await storgeInfo.GetDetailAll(1,
                async (detail) =>
                {
                    completeParts.Add((detail.Number, detail.CompleteExtensionInfo));
                    await Task.FromResult(0);
                }
                );
            try
            {
                //执行完成上传
                await CompleteMultipart(endpoint, storgeInfo.Name, storgeInfo.ExtensionInfo, completeParts);

            }
            catch (OssException ex)
            {
                if (ex.ErrorCode.ToLower() != "nosuchupload")
                {
                    throw;
                }
            }
            //执行storgeInfo的完成操作
            await storgeInfo.Complete(string.Empty);
        }

        public async Task DeleteBatch(OSSEndpoint endpoint, IList<string> filePaths)
        {
            var client = getOssClient(endpoint);
            DeleteObjectsRequest request = new DeleteObjectsRequest(endpoint.Bucket, filePaths);
            client.DeleteObjects(request);
            await Task.FromResult(0);
        }

        /// <summary>
        /// 分片复制扩展信息
        /// </summary>
        [DataContract]
        private class MultipartCopyExtensionInfo
        {
            /// <summary>
            /// 源Bucket
            /// </summary>
            [DataMember]
            public string SourceBucket { get; set; }
            /// <summary>
            /// 源FilePath
            /// </summary>
            [DataMember]
            public string SourceFilePath { get; set; }
            /// <summary>
            /// 上传ID
            /// </summary>
            [DataMember]
            public string UploadID { get; set; }
        }
    }
}
