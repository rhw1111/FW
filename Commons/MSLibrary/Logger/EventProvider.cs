using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace MSLibrary.Logger
{
    public class EventProvider: ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new EventLogger(categoryName);
        }



        public void Dispose()
        {
        }
    }
}
