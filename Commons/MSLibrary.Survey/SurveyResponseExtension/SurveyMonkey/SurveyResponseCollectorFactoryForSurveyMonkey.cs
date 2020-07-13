using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Survey.SurveyMonkey;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Survey.SurveyResponseExtension.SurveyMonkey
{
    public class SurveyResponseCollectorFactoryForSurveyMonkey : ISurveyResponseCollectorFactory
    {
        public Task<SurveyResponseCollector> Create(string collectorData, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
