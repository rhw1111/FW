using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Security
{
    /// <summary>
    /// 安全服务
    /// 系统所有需要加密签名的操作都需要使用该接口
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content">要加密的信息</param>
        /// <returns>加密过后的信息</returns>
        string Encrypt(string content);
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="content">已经加密的信息</param>
        /// <returns>解密后的信息</returns>
        string Decrypt(string content);
        /// <summary>
        /// 使用证书的私钥加密
        /// </summary>
        /// <param name="content">要加密的信息</param>
        /// <param name="thumbprint">证书指纹</param>
        /// <returns>加密后的信息</returns>
        string EncryptByCertificatePrivateKey(string content, string thumbprint);

        /// <summary>
        /// 使用证书的公钥加密
        /// </summary>
        /// <param name="content">要加密的信息</param>
        /// <param name="thumbprint">证书指纹</param>
        /// <returns>加密后的信息</returns>
        string EncryptByCertificatePublicKey(string content, string thumbprint);

        /// <summary>
        /// 使用证书的私钥解密
        /// </summary>
        /// <param name="content">已经加密的信息</param>
        /// <param name="thumbprint">证书指纹</param>
        /// <returns>解密后的信息</returns>
        string DecryptByCertificatePrivateKey(string content, string thumbprint);

        /// <summary>
        /// 哈希
        /// </summary>
        /// <param name="content">要哈希的信息</param>
        /// <returns>哈希过后的信息</returns>
        string Hash(string content);

        /// <summary>
        /// 使用证书的私钥签名
        /// 证书存放位置为本机，受信根目录
        /// </summary>
        /// <param name="content">要签名的内容</param>
        /// <param name="thumbprint">证书指纹</param>
        /// <returns>签名</returns>
        string SignByCertificatePrivateKey(string content, string thumbprint);

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="content">要签名的内容</param>
        /// <param name="key">密钥</param>
        /// <returns>签名</returns>
        string SignByKey(string content, string key);
        /// <summary>
        /// 使用证书的公钥验证签名
        /// 证书存放位置为本机，受信根目录
        /// </summary>
        /// <param name="content">签名的内容</param>
        /// <param name="thumbprint">证书指纹</param>
        /// <returns>是否合法</returns>
        bool VerifySignByCertificatePublicKey(string content,string signContent, string thumbprint);

        /// <summary>
        /// 使用证书的私钥验证签名
        /// 证书存放位置为本机，受信根目录
        /// </summary>
        /// <param name="content">签名的内容</param>
        /// <param name="thumbprint">证书指纹</param>
        /// <returns>是否合法</returns>
        bool VerifySignByCertificatePrivateKey(string content, string signContent, string thumbprint);

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="signContent">签名的内容</param>
        /// <param name="key">密钥</param>
        /// <returns>是否合法</returns>
        bool VerifySignByKey(string content,string signContent, string key);

        /// <summary>
        /// 根据公钥签名指定内容
        /// </summary>
        /// <param name="content">要签名的内容</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>签名的内容</returns>
        string SignByPublicKey(string content, string publicKey);
        /// <summary>
        /// 根据私钥签名指定内容
        /// </summary>
        /// <param name="content">要签名的内容</param>
        /// <param name="privateKey">公钥</param>
        /// <returns>签名的内容</returns>
        string SignByPivateKey(string content, string privateKey);

        /// <summary>
        /// 根据公钥验证内容
        /// </summary>
        /// <param name="content">要验证的内容</param>
        /// <param name="signContent">签名内容</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        bool VerifySignByPublicKey(string content, string signContent, string publicKey);

        /// <summary>
        /// 根据私钥验证内容
        /// </summary>
        /// <param name="content">要验证的内容</param>
        /// <param name="signContent">签名内容</param>
        /// <param name="privateKey">私6钥</param>
        /// <returns></returns>
        bool VerifySignByPrivateKey(string content, string signContent, string privateKey);

        /// <summary>
        /// 根据私钥加密内容
        /// </summary>
        /// <param name="content">要加密的内容</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>加密后的内容</returns>
        string EncryptByPrivateKey(string content,string privateKey);
        /// <summary>
        /// 根据公钥加密内容
        /// </summary>
        /// <param name="content">要加密的内容</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>加密后的内容</returns>
        string EncryptByPublicKey(string content, string publicKey);

        /// <summary>
        /// 根据私钥解密
        /// </summary>
        /// <param name="content">加密后的内容</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>解密后的内容</returns>
        string DecryptByPrivateKey(string content,string privateKey);

        /// <summary>
        /// 生成JWT
        /// </summary>
        /// <param name="key">签名密钥</param>
        /// <param name="values">需要附加到JWT的键值对</param>
        /// <param name="expireSecond">JWT过期秒数</param>
        /// <returns></returns>
        string GenerateJWT(string key, Dictionary<string, string> values, int expireSecond);
        /// <summary>
        /// 验证JWT
        /// </summary>
        /// <param name="key">签名密钥</param>
        /// <param name="strJWT">JWT字符串</param>
        /// <returns></returns>
        JWTValidateResult ValidateJWT(string key,string strJWT);
        /// <summary>
        /// 获取JWT的Playload部分
        /// </summary>
        /// <param name="strJWT"></param>
        /// <returns></returns>
        JWTValidateResult GetPlayloadFromJWT(string strJWT);

    }

    /// <summary>
    /// JWT字符串验证结果
    /// </summary>
    public class JWTValidateResult
    {
        /// <summary>
        /// 验证结果
        /// </summary>
        public ValidateResult ValidateResult { get; set; }
        /// <summary>
        /// Playload部分的键值对
        /// </summary>
        public Dictionary<string, string> Playload { get; set; }
    }
}
