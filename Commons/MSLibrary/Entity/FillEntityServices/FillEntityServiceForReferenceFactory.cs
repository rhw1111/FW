using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Entity.FillEntityServices
{

    public class FillEntityServiceForReferenceFactory<T> : IFactory<IFillEntityService>
    {
        private FillEntityServiceForReference<T> _fillEntityServiceForReference;

        public FillEntityServiceForReferenceFactory(FillEntityServiceForReference<T> fillEntityServiceForReference)
        {
            _fillEntityServiceForReference = fillEntityServiceForReference;
        }
        public IFillEntityService Create()
        {
            return _fillEntityServiceForReference;
        }
    }
}
