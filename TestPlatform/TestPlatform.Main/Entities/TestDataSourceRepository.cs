﻿using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using FW.TestPlatform.Main.Entities.DAL;

namespace FW.TestPlatform.Main.Entities
{
    [Injection(InterfaceType = typeof(ITestDataSourceRepository), Scope = InjectionScope.Singleton)]
    public class TestDataSourceRepository : ITestDataSourceRepository
    {
        private readonly ITestDataSourceStore _testDataSourceStore;

        public TestDataSourceRepository(ITestDataSourceStore testDataSourceStore)
        {
            _testDataSourceStore = testDataSourceStore;
        }

        public async Task<TestDataSource?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            return await _testDataSourceStore.QueryByID(id, cancellationToken);
        }

        public async Task<TestDataSource?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _testDataSourceStore.QueryByName(name, cancellationToken);
        }

        public async Task<IList<TestDataSource>> QueryByNames(IList<string> names, CancellationToken cancellationToken = default)
        {
            return await _testDataSourceStore.QueryByNames(names, cancellationToken);
        }

        public async Task<QueryResult<TestDataSource>> QueryByPage(string matchName, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _testDataSourceStore.QueryByPage(matchName, page, pageSize, cancellationToken);
        }
    }
}