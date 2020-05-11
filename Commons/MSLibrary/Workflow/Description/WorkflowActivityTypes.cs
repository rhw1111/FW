using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Workflow.Description
{
    /// <summary>
    /// 工作流活动类型集
    /// </summary>
    public static class WorkflowActivityTypes
    {
        public const string Condition = "Condition";
        public const string While = "While";
        public const string Break = "Break";
        public const string GoTo = "GoTo";
    }
}
