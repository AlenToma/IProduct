using System;

namespace OAuth.Security.Modules.Library
{
    public class Role
    {
        public Guid? Id { get; set; }

        public virtual string Name { get; set; }

        public Roles Position { get; set; }
    }
}