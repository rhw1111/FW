using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Template;
using FW.TestPlatform.Main.Entities.DAL;
using System.Linq;

namespace FW.TestPlatform.Main.Entities
{
    public class ScriptTemplate : EntityBase<IScriptTemplateIMP>
    {
        private static IFactory<IScriptTemplateIMP>? _scriptTemplateIMPFactory;

        public static IFactory<IScriptTemplateIMP>? ScriptTemplateIMPFactory
        {
            set
            {
                _scriptTemplateIMPFactory = value;
            }
        }
        public override IFactory<IScriptTemplateIMP>? GetIMPFactory()
        {
            return _scriptTemplateIMPFactory;
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
        /// 脚本内容
        /// </summary>
        public string Content
        {
            get
            {

                return GetAttribute<string>(nameof(Content));
            }
            set
            {
                SetAttribute<string>(nameof(Content), value);
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


        public async Task<string> GenerateScript( IDictionary<string, object> parameters, CancellationToken cancellationToken = default)
        {
            return await _imp.GenerateScript(this, parameters, cancellationToken);
        }
    }

    public interface IScriptTemplateIMP
    {
        Task<string> GenerateScript(ScriptTemplate template, IDictionary<string, object> parameters, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(IScriptTemplateIMP), Scope = InjectionScope.Transient)]
    public class ScriptTemplateIMP : IScriptTemplateIMP
    {
        private readonly ITemplateService _templateService;

        public ScriptTemplateIMP(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        public async Task<string> GenerateScript(ScriptTemplate template, IDictionary<string, object> parameters, CancellationToken cancellationToken = default)
        {
            TemplateContext context = new TemplateContext(1033, parameters.ToDictionary((kv)=>kv.Key,(kv)=>kv.Value));
            return await _templateService.Convert(template.Content, context);
        }
    }
}
