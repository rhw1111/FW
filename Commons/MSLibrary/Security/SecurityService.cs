using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Security
{
    [Injection(InterfaceType = typeof(ISecurityService), Scope = InjectionScope.Singleton)]
    public class SecurityService : ISecurityService
    {
        private ISecurityConfigurationService _securityConfigurationService;

        public SecurityService(ISecurityConfigurationService securityConfigurationService)
        {
            _securityConfigurationService = securityConfigurationService;
        }

        public string Decrypt(string content)
        {
            var arrayContent=content.Split('*');
            var strEKeyIV = arrayContent[0];
            var strEContent = arrayContent[1];

            var strDKeyIV = Decrypt(strEKeyIV, _securityConfigurationService.GetEncryptKey(), _securityConfigurationService.GetEncryptIV());

            var strKey = strDKeyIV.Substring(0, 24);
            var strIV = strDKeyIV.Substring(24, 12);

            return Decrypt(strEContent, strKey, strIV);
        }

        public string DecryptByCertificatePrivateKey(string content, string thumbprint)
        {
            string result;
            //使用证书私钥解密
            var cert = GetCertificate(Regex.Replace(thumbprint, @"[^\da-zA-z]", string.Empty).ToUpper());
            using (var rsa = cert.GetRSAPrivateKey())
            {
                result = DecryptByKey(content, rsa);
            }

            return result;
        }

        public string Encrypt(string content)
        {

            using (var desProvider = TripleDES.Create())
            {


                desProvider.KeySize = 128;

                desProvider.GenerateIV();
                desProvider.GenerateKey();

                var iv = Convert.ToBase64String(desProvider.IV);
                var key = Convert.ToBase64String(desProvider.Key);

                var strKeyIV = Encrypt(key + iv, _securityConfigurationService.GetEncryptKey(), _securityConfigurationService.GetEncryptIV());

                return strKeyIV +"*"+ Encrypt(content, key, iv);

            }
        }

        public string EncryptByCertificatePrivateKey(string content, string thumbprint)
        {
            string result;
            //使用证书私钥加密
            var cert = GetCertificate(Regex.Replace(thumbprint, @"[^\da-zA-z]", string.Empty).ToUpper());
            using (var rsa = cert.GetRSAPrivateKey())
            {
                result=EncryptByKey(content, rsa);
            }

            return result;
        }

        public string Hash(string content)
        {
            byte[] byteList = null; ;
            using (SHA256 s256 = SHA256.Create())
            {
                byteList = s256.ComputeHash(new UTF8Encoding().GetBytes(content));
            }

            return Convert.ToBase64String(byteList);
        }

        public string SignByKey(string content, string key)
        {
            var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(key));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(content));
            var signature = Convert.ToBase64String(hashBytes);

            return signature;
        }



        public bool VerifySignByCertificatePublicKey(string content,string signContent, string thumbprint)
        {
            bool result = false;
            var cert = GetCertificate(Regex.Replace(thumbprint, @"[^\da-zA-z]", string.Empty).ToUpper());
            using (var rsa = cert.GetRSAPublicKey())
            {
                result = rsa.VerifyData(UTF8Encoding.UTF8.GetBytes(content), Convert.FromBase64String(signContent), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            }
            return result;
        }



        public bool VerifySignByKey(string content,string signContent, string key)
        {

            var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(key));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(content));
            var signature = Convert.ToBase64String(hashBytes);
            if (signature==signContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }






        private string Encrypt(string content, string strKey, string strIV)
        {
            content = Convert.ToBase64String(new UTF8Encoding().GetBytes(content));
            byte[] key = Convert.FromBase64String(strKey);
            byte[] iv = Convert.FromBase64String(strIV);

            byte[] byteContent = Convert.FromBase64String(content);
            byte[] byteEncryptionContent;
            string strEncryptionContent;

            using (MemoryStream stream = new MemoryStream())
            {
                using (var desProvider = TripleDES.Create())
                {
                    desProvider.KeySize = 128;

                    CryptoStream encStream = new CryptoStream(stream, desProvider.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                    encStream.Write(byteContent, 0, byteContent.Length);
                    encStream.FlushFinalBlock();
                    stream.Position = 0;
                    byteEncryptionContent = new byte[stream.Length];
                    stream.Read(byteEncryptionContent, 0, (int)stream.Length);



                }
            }

            strEncryptionContent = Convert.ToBase64String(byteEncryptionContent);

            return strEncryptionContent;
        }




        private string Decrypt(string content, string strKey, string strIV)
        {
            byte[] key = Convert.FromBase64String(strKey);
            byte[] iv = Convert.FromBase64String(strIV);

            byte[] byteContent = Convert.FromBase64String(content);
            byte[] byteEncryptionContent;
            string strEncryptionContent;

            using (MemoryStream stream = new MemoryStream())
            {

                using (var desProvider = TripleDES.Create())
                {


                    CryptoStream encStream = new CryptoStream(stream, desProvider.CreateDecryptor(key, iv), CryptoStreamMode.Write);
                    encStream.Write(byteContent, 0, byteContent.Length);
                    encStream.FlushFinalBlock();
                    stream.Position = 0;
                    byteEncryptionContent = new byte[stream.Length];
                    stream.Read(byteEncryptionContent, 0, (int)stream.Length);


                }

                strEncryptionContent = new UTF8Encoding().GetString(byteEncryptionContent);
                //strEncryptionContent = Convert.ToBase64String(byteEncryptionContent);
                //strEncryptionContent = new UTF8Encoding().GetString(Convert.FromBase64String(strEncryptionContent));

                return strEncryptionContent;
            }
        }

        public string SignByPublicKey(string content, string publicKey)
        {
            byte[] resultBytes;
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(publicKey);

                resultBytes = rsa.SignData(UTF8Encoding.UTF8.GetBytes(content), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);

            }
            return Convert.ToBase64String(resultBytes);
        }

        public string SignByPivateKey(string content, string privateKey)
        {
            byte[] resultBytes;
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(privateKey);

                resultBytes = rsa.SignData(UTF8Encoding.UTF8.GetBytes(content), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);

            }
            return Convert.ToBase64String(resultBytes);
        }

        public bool VerifySignByPublicKey(string content, string signContent, string publicKey)
        {
            bool result = false;
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(publicKey);
                result = rsa.VerifyData(UTF8Encoding.UTF8.GetBytes(content), Convert.FromBase64String(signContent), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            }
            return result;
        }

        public bool VerifySignByPrivateKey(string content, string signContent, string privateKey)
        {
            bool result = false;
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(privateKey);
                result = rsa.VerifyData(UTF8Encoding.UTF8.GetBytes(content), Convert.FromBase64String(signContent), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            }
            return result;
        }

        public string EncryptByPrivateKey(string content, string privateKey)
        {
            string result;
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(privateKey);
                result = EncryptByKey(content, rsa);
            }
            return result;
        }

        public string EncryptByPublicKey(string content, string publicKey)
        {
            string result;
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(publicKey);
                result = EncryptByKey(content, rsa);
            }
            return result;
        }

        public string DecryptByPrivateKey(string content, string privateKey)
        {
            string result;
            //解密
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(privateKey);
                result = DecryptByKey(content, rsa);
            }
            return result;
        }


        private X509Certificate2 GetCertificate(string thumbprint)
        {
            X509Certificate2 result = null;

            using (X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadWrite);
                X509Certificate2Collection storecollection = (X509Certificate2Collection)store.Certificates;
                foreach (X509Certificate2 x509 in storecollection)
                {
                    if (x509.Thumbprint.ToLower() == thumbprint.ToLower())
                    {
                        result = x509;
                    }
                }
            }
            return result;

        }


        private string EncryptByKey(string content, RSA rsa)
        {
            //首先获取用来实际加密数据的对称密钥
            string iv, key;
            using (var desProvider = TripleDES.Create())
            {
                desProvider.KeySize = 128;
                desProvider.GenerateIV();
                desProvider.GenerateKey();

                iv = Convert.ToBase64String(desProvider.IV);
                key = Convert.ToBase64String(desProvider.Key);
            }

            string strKey;


            //使用密钥加密对称密钥
            var strKeyBytes = rsa.Encrypt(UTF8Encoding.UTF8.GetBytes(key + iv), RSAEncryptionPadding.OaepSHA1);
            strKey = Convert.ToBase64String(strKeyBytes);


            //使用对称密钥加密内容
            var strContent = Encrypt(content, key, iv);

            return strKey+"*" + strContent;
        }



        private string DecryptByKey(string content, RSA rsa)
        {
            string strKeyIV;

            var arrayContent = content.Split('*');
            var strEKeyIV = arrayContent[0];
            var strEContent = arrayContent[1];

            //解密

            
            //使用密钥解密对称密钥
            var byteKeyIV = rsa.Decrypt(Convert.FromBase64String(strEKeyIV), RSAEncryptionPadding.OaepSHA1);
            strKeyIV = UTF8Encoding.UTF8.GetString(byteKeyIV);


            var strKey = strKeyIV.Substring(0, 24);
            var strIV = strKeyIV.Substring(24);

            return Decrypt(strEContent, strKey, strIV);
        }

        public string EncryptByCertificatePublicKey(string content, string thumbprint)
        {
            string result;
            //使用证书公钥加密
            var cert = GetCertificate(Regex.Replace(thumbprint, @"[^\da-zA-z]", string.Empty).ToUpper());
            using (var rsa = cert.GetRSAPublicKey())
            {
                result = EncryptByKey(content, rsa);
            }

            return result;
        }

        public string SignByCertificatePrivateKey(string content, string thumbprint)
        {
            byte[] resultBytes;
            //使用证书公钥加密
            var cert = GetCertificate(Regex.Replace(thumbprint, @"[^\da-zA-z]", string.Empty).ToUpper());
            using (var rsa = cert.GetRSAPrivateKey())
            {
                resultBytes = rsa.SignData(UTF8Encoding.UTF8.GetBytes(content), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            }

            return Convert.ToBase64String(resultBytes);
        }

        public bool VerifySignByCertificatePrivateKey(string content, string signContent, string thumbprint)
        {
            bool result = false;
            var cert = GetCertificate(Regex.Replace(thumbprint, @"[^\da-zA-z]", string.Empty).ToUpper());
            using (var rsa = cert.GetRSAPrivateKey())
            {
                result = rsa.VerifyData(UTF8Encoding.UTF8.GetBytes(content), Convert.FromBase64String(signContent), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            }
            return result;
        }

        public string GenerateJWT(string key, Dictionary<string, string> values, int expireSecond)
        {
            //生成JWT的header部分
            var jsonObj = new JObject();
            jsonObj.Add("typ", "JWT");
            jsonObj.Add("alg", "HS256");
            var strHeader = JsonSerializerHelper.Serializer<JObject>(jsonObj);

            //生成JWT的playload部分,包含通用令牌中的固定键值对和自定义键值对
            var utcNow = DateTime.UtcNow;
            jsonObj = new JObject();
            jsonObj.Add("iat", utcNow);
            jsonObj.Add("exp", utcNow.AddSeconds(expireSecond));
            foreach (var item in values)
            {

                if (item.Key != "iat" && item.Key != "exp")
                {
                    jsonObj.Add(item.Key,item.Value);
                }
            }
            var strPlayload = JsonSerializerHelper.Serializer<JObject>(jsonObj);

            //生成JWT的signature部分
            var strBase64Header = strHeader.Base64UrlEncode();
            var strBase64Playload = strPlayload.Base64UrlEncode();
            var strSignature = SignByKey(strBase64Header + "." + strBase64Playload, key);

            return $"{strBase64Header}.{strBase64Playload}.{strSignature}";
        }

        public JWTValidateResult ValidateJWT(string key, string strJWT)
        {
            JWTValidateResult result = new JWTValidateResult()
            {
                ValidateResult = new ValidateResult()
                {
                    Result = true
                },
                Playload = new Dictionary<string, string>()
            };


            //验证签名
            var arrayJWT = strJWT.Split('.');
            if (arrayJWT.Length != 3)
            {
                result.ValidateResult.Result = false;
                result.ValidateResult.Description = string.Format(StringLanguageTranslate.Translate(TextCodes.JWTFormatError, "JWT字符串{0}的格式不正确"), strJWT);

                return result;
            }

            var strSignature = SignByKey($"{arrayJWT[0]}.{arrayJWT[1]}", key);

            if (strSignature != arrayJWT[2])
            {
                result.ValidateResult.Result = false;
                result.ValidateResult.Description = string.Format(StringLanguageTranslate.Translate(TextCodes.JWTSignError, "JWT字符串{0}的签名验证失败"), strJWT);
                return result;
            }

            //获取playload部分信息
            var jsonObj = JsonSerializerHelper.Deserialize<JObject>(arrayJWT[1].Base64UrlDecode());

            foreach (var item in jsonObj)
            {
                result.Playload.Add(item.Key, item.Value.Value<string>());
            }

            //检查是否过期
            var exp = jsonObj["exp"];
            if (exp != null)
            {
                if (exp.Value<DateTime>() <= DateTime.UtcNow)
                {
                    result.ValidateResult.Result = false;
                    result.ValidateResult.Description = string.Format(StringLanguageTranslate.Translate(TextCodes.JWTExpire, "JWT字符串{0}已经过期，过期时间为{1}"), strJWT, exp.Value<DateTime>().ToString());
                    return result;
                }
            }



            return result;
        }

        public JWTValidateResult GetPlayloadFromJWT(string strJWT)
        {
            JWTValidateResult result = new JWTValidateResult()
            {
                ValidateResult = new ValidateResult()
                {
                    Result = true
                },
                Playload = new Dictionary<string, string>()
            };


            //验证签名
            var arrayJWT = strJWT.Split('.');
            if (arrayJWT.Length != 3)
            {
                result.ValidateResult.Result = false;
                result.ValidateResult.Description = string.Format(StringLanguageTranslate.Translate(TextCodes.JWTFormatError, "JWT字符串{0}的格式不正确"), strJWT);

                return result;
            }


            //获取playload部分信息
            var jsonObj = JsonSerializerHelper.Deserialize<JObject>(arrayJWT[1].Base64UrlDecode());

            foreach (var item in jsonObj)
            {
                result.Playload.Add(item.Key, item.Value.Value<string>());
            }

            //检查是否过期
            var exp = jsonObj["exp"];
            if (exp != null)
            {
                if (exp.Value<DateTime>() <= DateTime.UtcNow)
                {
                    result.ValidateResult.Result = false;
                    result.ValidateResult.Description = string.Format(StringLanguageTranslate.Translate(TextCodes.JWTExpire, "JWT字符串{0}已经过期，过期时间为{1}"), strJWT, exp.Value<DateTime>().ToString());
                    return result;
                }
            }



            return result;
        }
    }
}
