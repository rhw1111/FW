using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public enum CrmAttributeTypeCode
    {
        Boolean = 0,
        Customer = 1,
        DateTime = 2,
        Decimal = 3,
        Double = 4,
        Integer = 5,
        Lookup = 6,
        Memo = 7,
        Money = 8,
        Owner = 9,
        PartyList = 10,
        Picklist = 11,
        State = 12,
        Status = 13,
        String = 14,
        Uniqueidentifier = 15,
        CalendarRules = 16,
        Virtual = 17,
        BigInt = 18,
        ManagedProperty = 19,
        EntityName = 20
    }
}
