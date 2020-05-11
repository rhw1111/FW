using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MSLibrary.Serializer
{
    /// <summary>
    /// 二进制序列化帮助器
    /// </summary>
    public static class BinarySerializerHelper
    {
        public static string Serializer<T>(T obj)
        {
            string strInfo = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, obj);
                byte[] byteArray = stream.ToArray();
                strInfo = Convert.ToBase64String(byteArray);
                stream.Close();
            }
            return strInfo;
        }

        public static string Serializer(object obj, Type type)
        {
            string strInfo = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, obj);
                byte[] byteArray = stream.ToArray();
                strInfo = Convert.ToBase64String(byteArray);
                stream.Close();
            }
            return strInfo;
        }


        public static T Deserialize<T>(string strInfo)
        {
            T obj = default(T);
            if (string.IsNullOrEmpty(strInfo))
            {
                return obj;
            }
            byte[] byteArray = Convert.FromBase64String(strInfo);
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                BinaryFormatter bf = new BinaryFormatter();

                obj = (T)bf.Deserialize(stream);
                stream.Close();
            }
            return obj;
        }


        public static object Deserialize(string strInfo, Type type)
        {
            object obj = new object();
            byte[] byteArray = Convert.FromBase64String(strInfo);
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                BinaryFormatter bf = new BinaryFormatter();

                obj = bf.Deserialize(stream);
                stream.Close();
            }
            return obj;
        }
    }
}
