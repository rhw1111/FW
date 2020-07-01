﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using MSLibrary;

namespace FW.TestPlatform.Main.Application
{
    public interface IAppCheckTestCaseStatus
    {
        
        Task<bool> Do(Guid id, CancellationToken cancellationToken = default);
        
    }
}