using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.LanguageTranslate;


namespace MSLibrary.FileManagement
{
    /// <summary>
    /// 文件后缀名与文件类型映射容器
    /// </summary>
    public static class FileSuffixFileTypeMapContainer
    {
        private static FileSuffixFileTypeMapProvider _provider = new FileSuffixFileTypeMapProvider();

        public static FileSuffixFileTypeMapProvider Provider
        {
            get
            {
                return _provider;
            }
        }


        /// <summary>
        /// 根据后缀名获取文件的默认类型
        /// </summary>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static FileType GetFileDefaultType(string suffix)
        {
            if (string.IsNullOrEmpty(suffix))
            {
                return FileType.Other;
            }
            if (suffix[0] == '.')
            {
                suffix = suffix.Remove(0,1);
            }
            if (!_provider.DefaultProvider.TryGetValue(suffix.ToLower(), out FileType result))
            {
                //throw new UtilityException((int)Errors.NotFoundFileSuffixFileTypeMapBySyffix, string.Format(StringLanguageTranslate.Translate(TextCodes.NotFoundFileSuffixFileTypeMapBySyffix, "找不到后缀名为{0}的文件后缀名与文件类型映射"), suffix.ToLower()));
                return FileType.Other;
            }

            return result;
        }

        /// <summary>
        /// 获得指定文件类型对应的后缀名列表
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static List<string> GetSuffixs(FileType fileType)
        {
            if (!_provider.FileTypeListProvider.TryGetValue(fileType, out List<string> result))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundSuffixsByFileType,
                    DefaultFormatting = "找不到文件类型为{0}的后缀名列表",
                    ReplaceParameters = new List<object>() { fileType.ToString() }
                };

                throw new UtilityException((int)Errors.NotFoundSuffixsByFileType, fragment);
            }

            return result;
        }
    }


    public class FileSuffixFileTypeMapProvider
    {
        private Dictionary<string, FileType> _defaultProvider = new Dictionary<string, FileType>()
        {
            { "avi", FileType.Video },
            { "bmp", FileType.Image },
            { "cmx",FileType.Image },
            { "cod",FileType.Image },
            { "gif",FileType.Image },
            { "ico",FileType.Image },
            { "ief",FileType.Image },
            { "jfif",FileType.Image },
            { "jpe",FileType.Image },
            { "jpeg",FileType.Image },
            { "jpg",FileType.Image },
            { "lsf",FileType.Video },
            { "lsx",FileType.Video },
            { "mov",FileType.Video },
            { "movie",FileType.Video },
            { "mp2",FileType.Video },
            { "mp3",FileType.Audio },
            { "mp4",FileType.Video },
            { "mpa",FileType.Video },
            { "mpe",FileType.Video },
            { "mpeg",FileType.Video },
            { "mpg",FileType.Video },
            { "mpv2",FileType.Video },
            { "pbm", FileType.Image },
            { "pgm",FileType.Image },
            { "pnm",FileType.Image },
            { "ppm",FileType.Image },
            { "png",FileType.Image },
            { "qt",FileType.Video },
            { "ra",FileType.Audio },
            { "ram",FileType.Audio },
            { "ras",FileType.Image },
            { "rgb",FileType.Image },
            { "snd",FileType.Audio },
            { "svg",FileType.Image },
            { "tif",FileType.Image },
            { "tiff",FileType.Image },
            { "wav", FileType.Audio },
            { "xbm", FileType.Image },
            { "xpm",FileType.Image },
            { "xwd",FileType.Image },
            { "pdf",FileType.Other },
            { "doc",FileType.Other },
            { "docx",FileType.Other },
            { "pptx",FileType.Other },
            { "ppt",FileType.Other },
            { "xls",FileType.Other },
            { "xlsx",FileType.Other },
            { "zip",FileType.Other },
            { "rar",FileType.Other },
            { "7z",FileType.Other },
            { "txt",FileType.Other },
            { "json",FileType.Other },
        };
        private Dictionary<FileType, List<string>> _fileTypeListProvider = new Dictionary<FileType, List<string>>()
        {
            {
                FileType.Image,new List<string>(){ "bmp", "cmx", "cod","gif","ico","ief", "jfif", "jpe", "jpeg", "jpg", "pbm", "pgm", "pnm", "ppm","png", "ras","rgb" ,"svg","tif","tiff","xbm","xpm","xwd"}
            },
            {
                FileType.Audio,new List<string>(){ "avi","mp3","ra","ram","snd","wav" }
            },
            {
                FileType.Video,new List<string>(){ "avi","lsf","lsx","mov","movie","mp2","mp4","mpa","mpe","mpeg","mpg","mpv2","qt" }
            },
            {
                FileType.Other,new List<string>(){ "pdf","doc","docx","ppt","pptx","xls","xlsx","zip","rar","7z","txt","json" }
            }
        };

        /// <summary>
        /// 后缀名默认的文件类型提供者
        /// </summary>
        public Dictionary<string, FileType> DefaultProvider
        {
            get
            {
                return _defaultProvider;
            }
        }

        /// <summary>
        /// 文件类型对应的后缀名列表提供者
        /// </summary>
        public Dictionary<FileType, List<string>> FileTypeListProvider
        {
            get
            {
                return _fileTypeListProvider;
            }
        }
    }
}
