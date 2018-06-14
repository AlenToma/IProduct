using IProduct.Modules.Library.Base_Entity;
using System;


namespace IProduct.Modules.Library
{
    public class Person : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
            set { }
        }

        public string PhoneNumber { get; set; }

        public Guid Address_Id { get; set; }

        public Address Address { get; set; }

    }
}
