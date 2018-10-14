using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth.Security.Modules.Library
{
    public class UserRole
    {
        public Guid User_Id { get; set; }

        public Guid Role_Id { get; set; }

        public Role Role { get; set; }

        public User User { get; set; }
    }
}
