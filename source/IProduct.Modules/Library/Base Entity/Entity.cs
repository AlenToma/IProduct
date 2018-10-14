using EntityWorker.Core.Attributes;
using System;

namespace IProduct.Modules.Library.Base_Entity
{
    public abstract class Entity
    {
        [PrimaryKey]
        public virtual Guid? Id { get; set; }

        [Stringify]
        public ObjectStatus Object_Status { get; set; }
    }
}
