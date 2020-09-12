using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using MSLibrary.LanguageTranslate;

namespace MSLibrary
{
    /// <summary>
    /// 异常扩展类
    /// </summary>
    public static class ExceptionExtension
    {
        public static string EnhancedStackTrace(this Exception ex)
        {
            return EnhancedStackTrace(new StackTrace(ex, true));
        }

        public static string ToStackTraceString(this Exception ex)
        {
            string strError = $"message:{ex.Message},stack:{ex.StackTrace}";
            var innerEx = ex;
            while(innerEx.InnerException!=null)
            {
                innerEx = innerEx.InnerException;
            }

            if (innerEx!=null)
            {
                strError = $"inner message:{innerEx.Message},inner stack:{innerEx.StackTrace}";
            }

            return strError;
        }


        private static string EnhancedStackTrace(StackTrace st)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append("---- Stack Trace ----");
            sb.Append(Environment.NewLine);
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                MemberInfo mi = sf.GetMethod();
                sb.Append(StackFrameToString(sf));
            }

            sb.Append(Environment.NewLine);
            return sb.ToString();
        }
        private static string StackFrameToString(StackFrame sf)
        {
            StringBuilder sb = new StringBuilder();
            int intParam;
            MemberInfo mi = sf.GetMethod();
            sb.Append("   ");
            sb.Append(mi.DeclaringType.Namespace);
            sb.Append(".");
            sb.Append(mi.DeclaringType.FullName);
            sb.Append(".");
            sb.Append(mi.Name);

            // -- build method params
            sb.Append("(");
            intParam = 0;

            foreach (ParameterInfo param in sf.GetMethod().GetParameters())
            {
                intParam += 1;
                sb.Append(param.Name);
                sb.Append(" As ");
                sb.Append(param.ParameterType.Name);
            }
            sb.Append(")");
            sb.Append(Environment.NewLine);

            // -- if source code is available, append location info
            sb.Append("       ");
            if (string.IsNullOrEmpty(sf.GetFileName()))
            {
                sb.Append("(unknown file)");
                //-- native code offset is always available
                sb.Append(": N ");
                sb.Append(String.Format("{0:#00000}", sf.GetNativeOffset()));
            }
            else
            {
                sb.Append(System.IO.Path.GetFileName(sf.GetFileName()));
                sb.Append(": line ");
                sb.Append(String.Format("{0:#0000}", sf.GetFileLineNumber()));
                sb.Append(", col ");
                sb.Append(String.Format("{0:#00}", sf.GetFileColumnNumber()));
                if (sf.GetILOffset() != StackFrame.OFFSET_UNKNOWN)
                {
                    sb.Append(", IL ");
                    sb.Append(String.Format("{0:#0000}", sf.GetILOffset()));

                }

            }
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

        /// <summary>
        /// 获取基于当前用户语言的错误内容
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static async Task<string> GetCurrentLcidMessage(this Exception exception)
        {
            if (exception is UtilityException && ((UtilityException)exception).Fragment!=null)
            {
                var lcid = ContextContainer.GetValue<int>(ContextTypes.CurrentUserLcid);
                return await ((UtilityException)exception).Fragment.GetLanguageText(lcid);
            }
            else
            {
                return await Task.FromResult(exception.Message);
            }
        }

        /// <summary>
        /// 获取基于当前用户语言的错误内容（同步）
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string GetCurrentLcidMessageSync(this Exception exception)
        {
            if (exception is UtilityException && ((UtilityException)exception).Fragment != null)
            {
                var lcid = ContextContainer.GetValue<int>(ContextTypes.CurrentUserLcid);
                return ((UtilityException)exception).Fragment.GetLanguageTextSync(lcid);
            }
            else
            {
                return exception.Message;
            }
        }
    }
}
