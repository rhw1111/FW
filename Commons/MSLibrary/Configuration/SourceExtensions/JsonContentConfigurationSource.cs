using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace MSLibrary.Configuration.SourceExtensions
{
    public class JsonContentConfigurationSource : IConfigurationSource
    {
        private string _content;

        public JsonContentConfigurationSource(string content)
        {
            _content = content;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new JsonContentConfigurationProvider(_content);
        }
    }
}
