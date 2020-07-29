using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using MSLibrary.DI;
using MSLibrary.Thread;

namespace MSLibrary.Template
{
    /// <summary>
    /// 模板服务实现
    /// 模板内容包含标签参数，标签参数格式为
    /// {$参数标签名称(标签参数1,标签参数2,...)}
    /// </summary>
    [Injection(InterfaceType = typeof(ITemplateService), Scope = InjectionScope.Singleton)]
    public class TemplateMainService : ITemplateService
    {
        private const string LabelExecuteParameters = "ExecuteParameters";

        private static int _labelParameterExecuteParallelism = 1;


        /// <summary>
        /// 标签参数运算并行数，默认为1
        /// 在系统初始化时可根据服务器配置做调整
        /// </summary>
        public static int LabelParameterExecuteParallelism
        {
            set
            {
                _labelParameterExecuteParallelism = value;
            }
        }

        private ILabelParameterRepository _labelParameterRepository;

        public TemplateMainService(ILabelParameterRepository labelParameterRepository)
        {
            _labelParameterRepository = labelParameterRepository;
        }

        public async Task<string> Convert(string content, TemplateContext context)
        {
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }
            while (true)
            {
                //通过正则表达式获取标签参数
                Regex reg = new Regex(@"\{(?<!\\)\$((?!(\{(?<!\\)\$[A-Za-z0-9_]+\(.*\)(?<!\\)\})).)+?(?<!\\)\}");
                var matchs = reg.Matches(content);
                int length = matchs.Count;

                if (length == 0)
                {
                    break;
                }

                var matchList = matchs.Cast<Match>();

                Dictionary<int, string> dictResult = new Dictionary<int, string>();

                Dictionary<string, string> dictLabelResult = new Dictionary<string, string>();

                Dictionary<string, SemaphoreSlim> lockObjs = new Dictionary<string, SemaphoreSlim>();


                try
                {
                    //生成锁定对象集
                    foreach (var item in matchList)
                    {
                        lockObjs[item.Value] = new SemaphoreSlim(1, 1);
                    }

                    //设定并行计算

                    await ParallelHelper.ForEach<Match>(matchList, _labelParameterExecuteParallelism, async (match) =>
                    {
                        var label = await GetLabel(match.Value);
                        if (label != null)
                        {
                            //检查标签是否时独立标签
                            if (await label.IsIndividual())
                            {
                                //独立标签直接计算结果
                                dictResult.Add(match.Index, await label.Execute(context, label.GetExtension<string[]>(LabelExecuteParameters)));
                            }
                            else
                            {
                                string matchResult;
                                //非独立标签需要复用计算结果
                                if (!dictLabelResult.TryGetValue(match.Value, out matchResult))
                                {

                                    try
                                    {
                                        await lockObjs[match.Value].WaitAsync();
                                        if (!dictLabelResult.TryGetValue(match.Value, out matchResult))
                                        {
                                            matchResult = await label.Execute(context, label.GetExtension<string[]>(LabelExecuteParameters));
                                            dictLabelResult[match.Value] = matchResult;
                                        }
                                    }
                                    finally
                                    {
                                        lockObjs[match.Value].Release();
                                    }
                                }
                                dictResult.Add(match.Index, matchResult);

                            }
                        }
                    });

                    //为每个匹配项做替换
                    content = reg.Replace(content, (match) =>
                    {

                        if (dictResult.TryGetValue(match.Index, out string matchResult))
                        {
                            return matchResult;
                        }
                        else
                        {
                            return match.Value;
                        }
                    });
                }
                finally
                {
                    foreach (var item in lockObjs)
                    {
                        item.Value.Dispose();
                    }
                }

            }

            content = content.Replace(@"\}", "}").Replace(@"\$", "$").Replace(@"\,", ",").Replace(@"\\", @"\");

            return await Task.FromResult(content);
        }


        private async Task<LabelParameter> GetLabel(string text)
        {

            //分割出标签名和参数
            Regex reg = new Regex(@"\{\$([A-Za-z0-9_]+)\((.*)\)\}", RegexOptions.None);
            var match = reg.Match(text);

            if (match == null || !match.Success)
            {
                return null;
            }

            if (match.Groups.Count != 3)
            {
                return null;
            }

            //获取标签名
            var strLabelName = match.Groups[1].Value;
            //获取参数集
            var strParameters = match.Groups[2].Value;

            //分割多个参数
            Regex regex = new Regex(@"(?<!\\),");
            var arrayParamaters = regex.Split(strParameters);

            // 去空格
            for (int i = 0; i < arrayParamaters.Length; i++)
            {
                arrayParamaters[i] = arrayParamaters[i].Trim();
            }

            var parameter = await _labelParameterRepository.QueryByName(strLabelName);
            parameter.Extensions.Add(LabelExecuteParameters, arrayParamaters);

            return parameter;
        }


    }
}
