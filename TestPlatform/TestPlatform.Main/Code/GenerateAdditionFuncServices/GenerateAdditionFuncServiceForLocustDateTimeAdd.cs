using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    /// <summary>
    /// 针对Locust的附件函数生成服务
    /// </summary>
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustDateTimeAdd), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustDateTimeAdd : IGenerateAdditionFuncService
    {
        public async Task<string> Generate()
        {
            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("def DateTimeAdd(dt, years=0, months=0, weeks=0, days=0, hours=0, minutes=0, seconds=0, microseconds=0, milliseconds=0):");
            sbCode.AppendLine("    # print(\"DateTimeAdd\")");
            sbCode.AppendLine("    import datetime");
            sbCode.AppendLine("    from dateutil.relativedelta import relativedelta");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if not dt:");
            sbCode.AppendLine("        return None");
            sbCode.AppendLine("");
            sbCode.AppendLine("    if type(dt) == datetime.datetime:");
            sbCode.AppendLine("        if years:");
            sbCode.AppendLine("            return dt + relativedelta(years=years)");
            sbCode.AppendLine("        elif months:");
            sbCode.AppendLine("            return dt + relativedelta(months=months)");
            sbCode.AppendLine("        elif weeks:");
            sbCode.AppendLine("            return dt + datetime.timedelta(weeks=weeks)");
            sbCode.AppendLine("        elif days:");
            sbCode.AppendLine("            return dt + datetime.timedelta(days=days)");
            sbCode.AppendLine("        elif hours:");
            sbCode.AppendLine("            return dt + datetime.timedelta(hours=hours)");
            sbCode.AppendLine("        elif minutes:");
            sbCode.AppendLine("            return dt + datetime.timedelta(minutes=minutes)");
            sbCode.AppendLine("        elif seconds:");
            sbCode.AppendLine("            return dt + datetime.timedelta(seconds=seconds)");
            sbCode.AppendLine("        elif microseconds:");
            sbCode.AppendLine("            return dt + datetime.timedelta(microseconds=microseconds)");
            sbCode.AppendLine("        elif milliseconds:");
            sbCode.AppendLine("            return dt + datetime.timedelta(milliseconds=milliseconds)");
            sbCode.AppendLine("");

            return await Task.FromResult(sbCode.ToString());
        }
    }
}
