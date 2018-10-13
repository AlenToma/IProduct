using EntityWorker.Core.InterFace;
using IProduct.Modules.Library;
using System;
using System.IO;
using System.Linq;

namespace IProduct.Modules.Rules
{
    public class MappRules : EntityWorker.Core.Interface.IDbRuleTrigger<Mapps>
    {
        public void AfterSave(IRepository repository, Mapps itemDbEntity, object objectId)
        {
            var path = Path.Combine(GlobalConfigration.FileBasePath, itemDbEntity.Name);
            if (itemDbEntity.Object_Status == EnumHelper.ObjectStatus.Removed)
            {
                Directory.Delete(path);
            }
            else
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
        }

        public void BeforeSave(IRepository repository, Mapps itemDbEntity)
        {
            if (itemDbEntity.Object_Status == EnumHelper.ObjectStatus.Removed) // ok lets try to remove it
            {
                var path = Path.Combine(GlobalConfigration.FileBasePath, itemDbEntity.Name);
                var items = Directory.GetFiles(path);
                if (items.Count() > 0)
                    throw new Exception("Mapps cant be deleted, it containes items that may are used in other objects/products");

            }
        }
    }
}
