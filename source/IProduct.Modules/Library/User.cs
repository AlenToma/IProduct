using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections.Generic;

namespace IProduct.Modules.Library
{
    public class User : Entity
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public Guid Person_Id { get; set; }

        public Person Person { get; set; }

        public Guid Role_Id { get; set; }

        public Role Role { get; set; }

        public List<Invoice> Invoices { get; set; }

        public bool RememberMe { get; set; }

        public bool System { get; set; }
    }
}
