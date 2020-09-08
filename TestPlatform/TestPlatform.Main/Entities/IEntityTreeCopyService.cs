using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Entities
{
    public interface IEntityTreeCopyService
    {
        Task Execute(string type, Guid entityID, Guid parentTreeID);
    }
}
