using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Entity.FillEntityServices
{
    public class FillEntityServiceForGuidFactory<T> : IFactory<IFillEntityService>
    {
        private FillEntityServiceForValue<T> _fillEntityServiceForValue;

        public FillEntityServiceForGuidFactory(FillEntityServiceForValue<T> fillEntityServiceForValue)
        {
            _fillEntityServiceForValue = fillEntityServiceForValue;
        }

        public IFillEntityService Create()
        {
            return _fillEntityServiceForValue;
        }
    }
}
