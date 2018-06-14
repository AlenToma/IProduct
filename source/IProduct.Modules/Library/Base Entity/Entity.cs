using EntityWorker.Core.Attributes;
using System;

namespace IProduct.Modules.Library.Base_Entity
{
    public abstract class Entity
    {
        [PrimaryKey]
        public Guid? Id { get; set; }

        [Stringify]
        public EnumHelper.ObjectStatus Object_Status { get; set; }
    }
}
