using EntityWorker.Core.Interface;
using EntityWorker.Core.InterFace;
using EntityWorker.Core.Object.Library;
using IProduct.Modules.Migration;
using System;
using System.Collections.Generic;
using System.Text;

namespace IProduct.Modules
{
    public class MigrationConfig : IMigrationConfig
    {
        public IList<EntityWorker.Core.Object.Library.Migration> GetMigrations(IRepository repository)
        {
            return new List<EntityWorker.Core.Object.Library.Migration>()
            {
                new Startup()
            };
        }
    }
}
