using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;

namespace IdentityCenter.Main.IdentityServer
{
    public class SecretData
    {
        public string Value { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime? Expiration { get; set; }
    }
}
