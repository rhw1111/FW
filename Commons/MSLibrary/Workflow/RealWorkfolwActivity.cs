using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Workflow.RealWorkfolwActivityParameterHandle;
using MSLibrary.Workflow.RealWorkfolwActivityHandle.Resolve;
using MSLibrary.Workflow.RealWorkfolwActivityHandle.Calculate;
using MSLibrary.Workflow.RealWorkfolwConfiguration;

namespace MSLibrary.Workflow
{
    public class RealWorkfolwActivity : EntityBase<IRealWorkfolwActivityIMP>
    {
        private static IFactory<IRealWorkfolwActivityIMP> _realWorkfolwActivityIMPFactory;

        public static IFactory<IRealWorkfolwActivityIMP> RealWorkfolwActivityIMPFactory
        {
            set
            {
                _realWorkfolwActivityIMPFactory = value;
            }
        }
        public override IFactory<IRealWorkfolwActivityIMP> GetIMPFactory()
        {
            return _realWorkfolwActivityIMPFactory;
        }

        /// <summary>
        /// id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>("ID");
            }
            set
            {
                SetAttribute<Guid>("ID", value);
            }
        }

        public string Configuration
        {
            get
            {
                return GetAttribute<string>("Configuration");
            }
            set
            {
                SetAttribute<string>("Configuration", value);
            }
        }



        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }

        public async Task<Dictionary<string, object>> Calculate(RealWorkfolwContext context)
        {
            return await _imp.Calculate(this, context);
        }

        public async Task<Dictionary<string, RealWorkfolwActivityParameter>> GetInputParameters()
        {
            return await _imp.GetInputParameters(this);
        }
        public async Task<Dictionary<string, RealWorkfolwActivityParameter>> GetOutputParameters()
        {
            return await _imp.GetOutputParameters(this);
        }

    }

    public interface IRealWorkfolwActivityIMP
    {
        Task<Dictionary<string, RealWorkfolwActivityParameter>> GetInputParameters(RealWorkfolwActivity activity);
        Task<Dictionary<string, RealWorkfolwActivityParameter>> GetOutputParameters(RealWorkfolwActivity activity);
        Task<Dictionary<string, object>> Calculate(RealWorkfolwActivity activity,RealWorkfolwContext context);
    }

    [Injection(InterfaceType = typeof(IRealWorkfolwActivityIMP), Scope = InjectionScope.Transient)]
    public class RealWorkfolwActivityIMP : IRealWorkfolwActivityIMP
    {
        private IRealWorkfolwActivityCalculate _realWorkfolwActivityCalculate;
        private IRealWorkfolwActivityResolve _realWorkfolwActivityResolve;
        private IRealWorkfolwActivityParameterHandle _realWorkfolwActivityParameterHandle;
        private IActivityConfigurationService _activityConfigurationService;
        public RealWorkfolwActivityIMP(IRealWorkfolwActivityCalculate realWorkfolwActivityCalculate, IRealWorkfolwActivityResolve realWorkfolwActivityResolve, IRealWorkfolwActivityParameterHandle realWorkfolwActivityParameterHandle, IActivityConfigurationService activityConfigurationService)
        {
            _realWorkfolwActivityCalculate = realWorkfolwActivityCalculate;
            _realWorkfolwActivityResolve = realWorkfolwActivityResolve;
            _realWorkfolwActivityParameterHandle = realWorkfolwActivityParameterHandle;
            _activityConfigurationService = activityConfigurationService;
        }
        public async Task<Dictionary<string, object>> Calculate(RealWorkfolwActivity activity, RealWorkfolwContext context)
        {
            /*var inputParameters = await GetInputParameters(activity);
            Dictionary<string, object> inputObjects = new Dictionary<string, object>();
            foreach(var inputItem in inputParameters)
            {
                inputObjects.Add(inputItem.Key, await _realWorkfolwActivityParameterHandle.Execute(inputItem.Value, context));
            }
            */
            RealWorkfolwActivityDescription activityDescription = null;
            if (!activity.Extensions.TryGetValue("description", out object objActivityDescription))
            {
                activityDescription = await _realWorkfolwActivityResolve.Execute(activity.Configuration);
                lock (activity)
                {
                    activity.Extensions["description"] = activityDescription;
                }
            }
            else
            {
                activityDescription = (RealWorkfolwActivityDescription)objActivityDescription;
            }

            var outputObjects=await _realWorkfolwActivityCalculate.Execute(activityDescription, context,new Dictionary<string, object>());

            //context.ActivityOutputParameters[activity.ID.ToString()] = outputObjects;

            return outputObjects;
        }

        public async Task<Dictionary<string, RealWorkfolwActivityParameter>> GetInputParameters(RealWorkfolwActivity activity)
        {
            return await _activityConfigurationService.ResolveActivityInputParameters(activity.Configuration);
        }

        public async Task<Dictionary<string, RealWorkfolwActivityParameter>> GetOutputParameters(RealWorkfolwActivity activity)
        {
            return await _activityConfigurationService.ResolveActivityOutputParameters(activity.Configuration);
        }
    }

    public class RealWorkfolwActivityParameter
    {
        public string Name { get; set; }
        public string DataType { get; set; }

        public string ExpressionType { get; set; }

        public string Configuration { get; set; }   
        public ConcurrentDictionary<string, object> Extensions { get; } = new ConcurrentDictionary<string, object>();
    }
}
