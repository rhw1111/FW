using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Collections;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;
using FW.TestPlatform.Main.Entities.DAL;

namespace FW.TestPlatform.Main.Entities
{
    /// <summary>
    /// 参与测试的数据源
    /// </summary>
    public class TestDataSource : EntityBase<ITestDataSourceIMP>
    {
        private static IFactory<ITestDataSourceIMP>? _testDataSourceIMPFactory;

        public IFactory<ITestDataSourceIMP> TestDataSourceIMPFactory
        {
            set
            {
                _testDataSourceIMPFactory = value;
            }
        }
        public override IFactory<ITestDataSourceIMP>? GetIMPFactory()
        {
            return _testDataSourceIMPFactory;
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
        /// 类型
        /// </summary>
        public string Type
        {
            get
            {

                return GetAttribute<string>(nameof(Type));
            }
            set
            {
                SetAttribute<string>(nameof(Type), value);
            }
        }

        /// <summary>
        /// 数据
        /// </summary>
        public string Data
        {
            get
            {

                return GetAttribute<string>(nameof(Data));
            }
            set
            {
                SetAttribute<string>(nameof(Data), value);
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
            await _imp.Delete(this,cancellationToken);
        }

    }

    public interface ITestDataSourceIMP
    {
        Task Add(TestDataSource source, CancellationToken cancellationToken = default);
        Task Update(TestDataSource source, CancellationToken cancellationToken = default);
        Task Delete(TestDataSource source, CancellationToken cancellationToken = default);
    }




    [Injection(InterfaceType = typeof(ITestDataSourceIMP), Scope = InjectionScope.Transient)]
    public class TestDataSourceIMP : ITestDataSourceIMP
    {
        private ITestDataSourceStore _testDataSourceStore;

        public TestDataSourceIMP(ITestDataSourceStore testDataSourceStore)
        {
            _testDataSourceStore = testDataSourceStore;
        }

        public async Task Add(TestDataSource source, CancellationToken cancellationToken = default)
        {
            await _testDataSourceStore.Add(source, cancellationToken);
        }

        public async Task Delete(TestDataSource source, CancellationToken cancellationToken = default)
        {
            await _testDataSourceStore.Delete(source.ID, cancellationToken);
        }

        public async Task Update(TestDataSource source, CancellationToken cancellationToken = default)
        {
            await _testDataSourceStore.Update(source, cancellationToken);
        }
       
    }
}
