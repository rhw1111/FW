using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm
{
    [DataContract]
    public class CrmLabel
    {

        [DataMember]
        public CrmLocalizedLabelCollection LocalizedLabels { get; set; }

        [DataMember]
        public CrmLocalizedLabel UserLocalizedLabel { get; set; }

    }
}
