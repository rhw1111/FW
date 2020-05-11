using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MSLibrary.LanguageTranslate;

namespace MSLibrary
{
    /// <summary>
    /// 系统错误
    /// 必须使用该类来抛出系统错误
    /// </summary>
    public class UtilityException : Exception
    {
        private Dictionary<string, object> _data = new Dictionary<string, object>();
        private TextFragment _fragment;

        /// <summary>
        /// 错误级别
        /// 大于0的表示为可以暴露给调用方的业务错误
        /// </summary>
        public int Level { get; set; } = -1;

        /// <summary>
        /// 错误类型
        /// </summary>
        public int Type { get; set; } = 0;

        public int Code { get; set; }
        public UtilityException(int code, TextFragment fragment)
            : base(fragment.GetDefaultTextSync())
        {
            Code = code;
            _fragment = fragment;
        }

        public UtilityException(int code,string message)
            :base(message)
        {
            Code = code;
        }

        public UtilityException(int code, TextFragment fragment,int level,int type): base(fragment.GetDefaultTextSync())
        {
            Code = code;
            _fragment = fragment;
            Level = level;
            Type = type;
        }

        public UtilityException(int code, string message, int level, int type)
            : base(message)
        {
            Code = code;
            Level = level;
            Type = type;
        }

        public TextFragment Fragment
        {
            get
            {
                return _fragment;
            }
        }

        public override IDictionary Data
        {
            get
            {
                return _data;
            }
        }
    }

    public static class UtilityExceptionTypes
    {
        public const string Unauthorized = "Unauthorized";
    }


    /// <summary>
    /// 系统错误类型与Http状态码映射关系
    /// </summary>
    public static class UtilityExceptionTypeStatusCodeMappings
    {
        /// <summary>
        /// 错误类型与StatusCode的映射关系
        /// 键为错误类型，值为StatusCode
        /// </summary>
        public static IDictionary<int, int> Mappings { get; } = new Dictionary<int, int>()
        {
            {0,500 },
            { 1, 401},
            { 2, 403}
        };
    }
}
