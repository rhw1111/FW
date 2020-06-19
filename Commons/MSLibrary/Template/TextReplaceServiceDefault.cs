using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Template
{
    /// <summary>
    /// 文本替换默认服务
    /// 使用{xxx}模式来标注要替换的内容，
    /// 通过映射文本替换内容生成服务来获取实际内容，完成替换
    /// </summary>
    [Injection(InterfaceType = typeof(TextReplaceServiceDefault), Scope = InjectionScope.Transient)]
    public class TextReplaceServiceDefault : ITextReplaceService
    {
        public IDictionary<string, ITextReplaceContentGenerateService> TextReplaceContentGenerateServices = new Dictionary<string, ITextReplaceContentGenerateService>();
        public async Task<string> Replace(string content,CancellationToken cancellationToken)
        {
            Regex regex = new Regex(@"(?<!\\)\{(\S+?)}");
            var matchs= regex.Matches(content);
            bool isMatch = false;
            if (matchs.Count>0)
            {
                isMatch = true;
            }
            foreach(Match item in matchs)
            {
                if (!TextReplaceContentGenerateServices.TryGetValue(item.Groups[1].Value, out ITextReplaceContentGenerateService generateService))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.NotFoundReplaceContentGenerateServiceByLabel,
                        DefaultFormatting = "找不到标签为{0}的替换内容生成服务，发生位置：{1}",
                        ReplaceParameters = new List<object>() { item.Groups[1].Value, $"{this.GetType().FullName}.TextReplaceContentGenerateServices" }
                    };

                    throw new UtilityException((int)Errors.NotFoundReplaceContentGenerateServiceByLabel, fragment);
                }

                var replaceContent=await generateService.Gererate(cancellationToken);
                content=content.Replace($"{{{item.Groups[1].Value}}}", replaceContent);
            }

            if (isMatch)
            {
                content = content.Replace(@"\{", "{");
            }

            return content;
        }
    }

    /// <summary>
    /// 文本替换内容生成服务
    /// </summary>
    public interface ITextReplaceContentGenerateService
    {
        Task<string> Gererate(CancellationToken cancellationToken=default);
    }
}
