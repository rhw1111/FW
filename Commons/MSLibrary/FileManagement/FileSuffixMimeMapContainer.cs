using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.StaticFiles;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.FileManagement
{
    /// <summary>
    /// 文件后缀名和Mime映射容器
    /// </summary>
    public static class FileSuffixMimeMapContainer
    {
        private static FileExtensionContentTypeProvider _fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();

        /// <summary>
        /// FileExtensionContentTypeProvider属性
        /// 系统初始化时可以向它加入新的映射
        /// </summary>
        public static FileExtensionContentTypeProvider Provider
        {
            get
            {
                return _fileExtensionContentTypeProvider;
            }
        }
        /// <summary>
        /// 根据后缀名获取Mime
        /// </summary>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string GetMime(string suffix)
        {
            if (string.IsNullOrEmpty(suffix))
            {
                return "application/octet-stream";
            }
            if (suffix[0] != '.')
            {
                suffix = "." + suffix;
            }
            if (!_fileExtensionContentTypeProvider.Mappings.TryGetValue(suffix.ToLower(), out string result))
            {
                return "application/octet-stream";
                //throw new UtilityException((int)Errors.NotFoundFileSuffixMimeMapBySyffix, string.Format(StringLanguageTranslate.Translate(TextCodes.NotFoundFileSuffixMimeMapBySyffix, "找不到后缀名为{0}的文件后缀名与Mime映射"), suffix.ToLower()));
            }

            return result;
        }
    }
}
