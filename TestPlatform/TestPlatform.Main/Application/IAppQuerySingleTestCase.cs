﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using FW.TestPlatform.Main.DTOModel;

namespace FW.TestPlatform.Main.Application
{
    public interface IAppQuerySingleTestCase
    {
        Task<TestCaseViewData> Do(Guid id, CancellationToken cancellationToken = default);
    }
}
