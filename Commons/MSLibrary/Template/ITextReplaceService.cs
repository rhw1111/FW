using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Template
{
    /// <summary>
    /// 文本替换服务
    /// </summary>
    public interface ITextReplaceService
    {
        Task<string> Replace(string content,CancellationToken cancellationToken=default);
    }
}
