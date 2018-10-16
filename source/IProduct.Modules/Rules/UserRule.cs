using EntityWorker.Core.InterFace;
using IProduct.Modules.Library;
using EntityWorker.Core.Helper;
using System;

namespace IProduct.Modules.Rules
{
    public class UserRule : EntityWorker.Core.Interface.IDbRuleTrigger<User>
    {
        public void AfterSave(IRepository repository, User itemDbEntity, object objectId)
        {

        }

        public void BeforeSave(IRepository repository, User itemDbEntity)
        {
            if (itemDbEntity.Role == null && itemDbEntity.Role_Id.ObjectIsNew())
                itemDbEntity.Role = repository.Get<Role>().Where(x => x.RoleType == Roles.Customers).ExecuteFirstOrDefault();

            if (!itemDbEntity.Id.HasValue && repository.Get<User>().Where(x => x.Email.Contains(itemDbEntity.Email)).ExecuteAny())
                throw new Exception("Email already exist in the system.");
        }
    }
}
