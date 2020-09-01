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
        /// String,JsonKV,JsonList
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
        /// 树结点Id
        /// </summary>
        public Guid? TreeID
        {
            get
            {

                return GetAttribute<Guid?>(nameof(TreeID));
            }
            set
            {
                SetAttribute<Guid?>(nameof(TreeID), value);
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
            //检查是否有名称重复的
            //var newId = await _testDataSourceStore.QueryByNameNoLock(source.Name, cancellationToken);
            //if (newId!=null)
            //{
            //    var fragment = new TextFragment()
            //    {
            //        Code = TestPlatformTextCodes.ExistTestDataSourceByName,
            //        DefaultFormatting = "已经存在名称为{0}的测试数据源",
            //        ReplaceParameters = new List<object>() { source.Name }
            //    };

            //    throw new UtilityException((int)TestPlatformErrorCodes.ExistTestDataSourceByName, fragment, 1, 0);

            //}
            //await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            //{
            //    await _testDataSourceStore.Add(source, cancellationToken);
            //    检查是否有名称重复的
            //    newId = await _testDataSourceStore.QueryByNameNoLock(source.Name, cancellationToken);
            //    if (source.ID != newId)
            //    {
            //        var fragment = new TextFragment()
            //        {
            //            Code = TestPlatformTextCodes.ExistTestDataSourceByName,
            //            DefaultFormatting = "已经存在名称为{0}的测试数据源",
            //            ReplaceParameters = new List<object>() { source.Name }
            //        };

            //        throw new UtilityException((int)TestPlatformErrorCodes.ExistTestDataSourceByName, fragment, 1, 0);
            //    }
            //    scope.Complete();
            //}
        }

        public async Task Delete(TestDataSource source, CancellationToken cancellationToken = default)
        {
            await _testDataSourceStore.Delete(source.ID, cancellationToken);
        }

        public async Task Update(TestDataSource source, CancellationToken cancellationToken = default)
        {
            await _testDataSourceStore.Update(source, cancellationToken);
            ////检查是否有名称重复的
            //var newId = await _testDataSourceStore.QueryByNameNoLock(source.Name, cancellationToken);
            //if (newId != null && source.ID != newId)
            //{
            //    var fragment = new TextFragment()
            //    {
            //        Code = TestPlatformTextCodes.ExistTestDataSourceByName,
            //        DefaultFormatting = "已经存在名称为{0}的测试数据源",
            //        ReplaceParameters = new List<object>() { source.Name }
            //    };

            //    throw new UtilityException((int)TestPlatformErrorCodes.ExistTestDataSourceByName, fragment, 1, 0);

            //}

            //await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            //{
            //    await _testDataSourceStore.Update(source, cancellationToken);
            //    //检查是否有名称重复的
            //    newId = await _testDataSourceStore.QueryByNameNoLock(source.Name, cancellationToken);
            //    if (newId != null && source.ID != newId)
            //    {
            //        var fragment = new TextFragment()
            //        {
            //            Code = TestPlatformTextCodes.ExistTestDataSourceByName,
            //            DefaultFormatting = "已经存在名称为{0}的测试数据源",
            //            ReplaceParameters = new List<object>() { source.Name }
            //        };

            //        throw new UtilityException((int)TestPlatformErrorCodes.ExistTestDataSourceByName, fragment, 1, 0);
            //    }
            //    scope.Complete();
            //}

        }
       
    }
}
