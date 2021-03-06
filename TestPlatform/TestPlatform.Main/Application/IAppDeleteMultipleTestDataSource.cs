﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FW.TestPlatform.Main.DTOModel;

namespace FW.TestPlatform.Main.Application
{
    public interface IAppDeleteMultipleTestDataSource
    {
        Task Do(List<Guid> list, CancellationToken cancellationToken = default);
    }
}
