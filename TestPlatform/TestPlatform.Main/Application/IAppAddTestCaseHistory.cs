﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.Application
{
    public interface IAppAddTestCaseHistory
    {
        Task Do(TestCaseHistorySummyAddModel model, CancellationToken cancellationToken = default);

        Task DoForJmeter(TestCaseHistorySummyAddModel model, CancellationToken cancellationToken = default);
    }
}
