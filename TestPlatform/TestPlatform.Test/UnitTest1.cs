using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Unicode;
using System.Linq;

namespace TestPlatform.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            IAsyncEnumerable<object> asyncEnumer;

           
              var bytes=Convert.FromBase64String("XExBYS06nWWTysT9lzOTnw==");
         
             var str= UTF8Encoding.UTF32.GetString(bytes);
            //var str = Convert.ToBase64String(bytes);
            //bytes = Convert.FromBase64String(str);
            //str = UTF8Encoding.UTF8.GetString(bytes);


            Assert.Pass();
        }
    }
}