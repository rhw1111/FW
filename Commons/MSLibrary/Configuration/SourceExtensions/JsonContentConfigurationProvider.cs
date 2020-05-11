using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace MSLibrary.Configuration.SourceExtensions
{
    /// <summary>
    /// 基于Json文本内容的配置提供方
    /// </summary>
    public class JsonContentConfigurationProvider: ConfigurationProvider
    {
        private string _content;
        private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Stack<string> _context = new Stack<string>();
        private string _currentPath;

        public JsonContentConfigurationProvider(string content)
        {
            _content = content;
        }
        public override void Load()
        {
            _data.Clear();
            var jsonDocumentOptions = new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            };

            using (JsonDocument doc = JsonDocument.Parse(_content, jsonDocumentOptions))
            {
                if (doc.RootElement.ValueKind != JsonValueKind.Object)
                {
                    throw new FormatException($"the content is not json format, content is {_content}");
                }
                VisitElement(doc.RootElement);
            }

            this.Data = _data;

            base.Load();
        }

        private void VisitElement(JsonElement element)
        {
            foreach (var property in element.EnumerateObject())
            {
                EnterContext(property.Name);
                VisitValue(property.Value);
                ExitContext();
            }
        }

        private void VisitValue(JsonElement value)
        {
            switch (value.ValueKind)
            {
                case JsonValueKind.Object:
                    VisitElement(value);
                    break;

                case JsonValueKind.Array:
                    var index = 0;
                    foreach (var arrayElement in value.EnumerateArray())
                    {
                        EnterContext(index.ToString());
                        VisitValue(arrayElement);
                        ExitContext();
                        index++;
                    }
                    break;

                case JsonValueKind.Number:
                case JsonValueKind.String:
                case JsonValueKind.True:
                case JsonValueKind.False:
                case JsonValueKind.Null:
                    var key = _currentPath;
                    if (_data.ContainsKey(key))
                    {
                        throw new FormatException($"the key {key} is duplicate" );
                    }
                    _data[key] = value.ToString();
                    break;

                default:
                    throw new FormatException($"the value {value.ToString()} is not json format ");
            }
        }

        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }

        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }


    }
}
