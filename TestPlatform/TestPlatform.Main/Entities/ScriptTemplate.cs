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
using MSLibrary.Transaction;

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

        public async Task Add()
        {
            await _imp.Add(this);
        }

        public async Task Update()
        {
            await _imp.Update(this);
        }

        public async Task Delete()
        {
            await _imp.Delete(this);
        }

        public async Task Add(CancellationToken cancellationToken = default)
        {
            await _imp.Add(this, cancellationToken);
        }

        public async Task Update(CancellationToken cancellationToken = default)
        {
            await _imp.Update(this, cancellationToken);
        }

        public async Task Delete(CancellationToken cancellationToken = default)
        {
            await _imp.Delete(this, cancellationToken);
        }

        public async Task<string> GenerateScript( IDictionary<string, object> parameters, CancellationToken cancellationToken = default)
        {
            return await _imp.GenerateScript(this, parameters, cancellationToken);
        }
    }

    public interface IScriptTemplateIMP
    {
        Task Add(ScriptTemplate template, CancellationToken cancellationToken = default);
        Task Update(ScriptTemplate template, CancellationToken cancellationToken = default);
        Task Delete(ScriptTemplate template, CancellationToken cancellationToken = default);
        Task<ScriptTemplate?> QueryByID(Guid id, CancellationToken cancellationToken = default);
        Task<ScriptTemplate?> QueryByName(string name, CancellationToken cancellationToken = default);
        Task<string> GenerateScript(ScriptTemplate template, IDictionary<string, object> parameters, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(IScriptTemplateIMP), Scope = InjectionScope.Transient)]
    public class ScriptTemplateIMP : IScriptTemplateIMP
    {
        private readonly ITemplateService _templateService;
        private readonly IScriptTemplateStore _scriptTemplateStore;

        public ScriptTemplateIMP(ITemplateService templateService, IScriptTemplateStore scriptTemplateStore)
        {
            _templateService = templateService;
            _scriptTemplateStore = scriptTemplateStore;
        }

        public async Task Add(ScriptTemplate template, CancellationToken cancellationToken = default)
        {
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                await _scriptTemplateStore.Add(template, cancellationToken);
                //检查是否有名称重复的

                scope.Complete();
            }
        }

        public async Task Delete(ScriptTemplate template, CancellationToken cancellationToken = default)
        {
            await _scriptTemplateStore.Delete(template.ID, cancellationToken);
        }

        public async Task Update(ScriptTemplate template, CancellationToken cancellationToken = default)
        {
            ScriptTemplate scriptTemplate = await _scriptTemplateStore.QueryByID(template.ID, cancellationToken);

            if(scriptTemplate != null)
            {
                await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
                {
                    await _scriptTemplateStore.Update(template, cancellationToken);
                    //检查是否有名称重复的

                    scope.Complete();
                }
            }
        }

        public async Task<ScriptTemplate?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            return await _scriptTemplateStore.QueryByID(id, cancellationToken);
        }

        public async Task<ScriptTemplate?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _scriptTemplateStore.QueryByName(name, cancellationToken);
        }

        public async Task<string> GenerateScript(ScriptTemplate template, IDictionary<string, object> parameters, CancellationToken cancellationToken = default)
        {
            TemplateContext context = new TemplateContext(1033, parameters.ToDictionary((kv) => kv.Key, (kv) => kv.Value));
            return await _templateService.Convert(template.Content, context);
        }
    }
}
