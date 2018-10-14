using IProduct.Modules.Library.Base_Entity;

namespace IProduct.Modules.Library
{
    public class Role : Entity
    {

        public string Description { get; set; }

        public string Name { get; set; }

        public Roles RoleType { get; set; }

        public bool System { get; set; }
    }
}
