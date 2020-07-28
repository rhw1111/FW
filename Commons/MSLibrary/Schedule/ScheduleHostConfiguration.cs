using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Schedule
{
    public class ScheduleHostConfiguration : EntityBase<IScheduleHostConfigurationIMP>
    {
        private static IFactory<IScheduleHostConfigurationIMP>? _scheduleHostConfigurationIMPFactory;

        public static IFactory<IScheduleHostConfigurationIMP>? ScheduleHostConfigurationIMPFactory
        {
            set
            {
                _scheduleHostConfigurationIMPFactory = value;
            }
        }
        public override IFactory<IScheduleHostConfigurationIMP>? GetIMPFactory()
        {
            return _scheduleHostConfigurationIMPFactory;
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>(nameof(ID));
            }
            set
            {
                SetAttribute<Guid>(nameof(ID), value);
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return GetAttribute<string>(nameof(Name));
            }
            set
            {
                SetAttribute<string>(nameof(Name), value);
            }
        }

        /// <summary>
        /// 调度组名称
        /// </summary>
        public string ScheduleGroupName
        {
            get
            {
                return GetAttribute<string>(nameof(ScheduleGroupName));
            }
            set
            {
                SetAttribute<string>(nameof(ScheduleGroupName), value);
            }
        }

        /// <summary>
        /// 环境声明生成器名称
        /// </summary>
        public string EnvironmentClaimGeneratorName
        {
            get
            {
                return GetAttribute<string>(nameof(EnvironmentClaimGeneratorName));
            }
            set
            {
                SetAttribute<string>(nameof(EnvironmentClaimGeneratorName), value);
            }
        }

        /// <summary>
        /// 声明上下文生成器名称
        /// </summary>
        public string ClaimContextGeneratorName
        {
            get
            {
                return GetAttribute<string>(nameof(ClaimContextGeneratorName));
            }
            set
            {
                SetAttribute<string>(nameof(ClaimContextGeneratorName), value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(CreateTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(CreateTime), value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(ModifyTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(ModifyTime), value);
            }
        }
    }

    public interface IScheduleHostConfigurationIMP
    {

    }

    [Injection(InterfaceType = typeof(IScheduleHostConfigurationIMP), Scope = InjectionScope.Transient)]
    public class ScheduleHostConfigurationIMP : IScheduleHostConfigurationIMP
    {

    }
}
