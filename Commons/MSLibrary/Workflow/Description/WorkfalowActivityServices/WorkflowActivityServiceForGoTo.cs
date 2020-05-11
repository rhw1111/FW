using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.Serializer;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using System.Runtime.InteropServices;

namespace MSLibrary.Workflow.Description.WorkfalowActivityServices
{
    [Injection(InterfaceType = typeof(WorkflowActivityServiceForGoTo), Scope = InjectionScope.Singleton)]
    public class WorkflowActivityServiceForGoTo : WorkflowActivityServiceBase
    {

        public override async Task<IList<object>> GenerateInputParameters(string configuration, IWorkflowContext context)
        {
            return await Task.FromResult(new List<object> { });
        }

        protected override async ValueTask<string> GetActivityType()
        {
            return await new ValueTask<string>("GoTo");
        }

        protected override async ValueTask<WorkflowActivityParameterDescription> GetParameterDescription(IList<object> inputParameters)
        {
            return await new ValueTask<WorkflowActivityParameterDescription>(
                new WorkflowActivityParameterDescription()
                {
                     Count=4,
                      Items=new List<WorkflowActivityParameterDescriptionItem>()
                      {
                          new WorkflowActivityParameterDescriptionItem()
                          {
                               AllowNull=false,
                                ExpectType=typeof(int)
                          },
                          new WorkflowActivityParameterDescriptionItem()
                          {
                               AllowNull=false,
                                ExpectType=typeof(Guid)
                          },
                          new WorkflowActivityParameterDescriptionItem()
                          {
                               AllowNull=false,
                                ExpectType=typeof(int)
                          },
                          new WorkflowActivityParameterDescriptionItem()
                          {
                               AllowNull=false,
                                ExpectType=typeof(string)
                          },
                      }
                }
                );
        }

        protected override async Task<IWorkfalowActivityResult> RealExecute(IList<object> inputParameters, IWorkflowContext context)
        {
            int type = (int)inputParameters[0];
            Guid blockID=(Guid)inputParameters[1];
            int status= (int)inputParameters[2];
            string actionName= (string)inputParameters[3];


            return await Task.FromResult(new WorkflowActivityResultDefault()
            {
                GoTo = new WorkflowGoToResult()
                {
                    Type = type,
                    BlockID = blockID,
                    Status = status,
                    ActionName = actionName
                }
            });
        }
    }
}
