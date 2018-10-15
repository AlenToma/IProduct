using EntityWorker.Core.InterFace;
using IProduct.Modules.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProduct.Modules.Rules
{
    public class UserRule : EntityWorker.Core.Interface.IDbRuleTrigger<User>
    {
        public void AfterSave(IRepository repository, User itemDbEntity, object objectId)
        {
            throw new NotImplementedException();
        }

        public void BeforeSave(IRepository repository, User itemDbEntity)
        {
            throw new NotImplementedException();
        }
    }
}
