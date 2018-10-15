using IProduct.Modules.Library.Base_Entity;
using System;

namespace IProduct.Modules.Library
{
    public class Address : Entity
    {
        public string AddressLine { get; set; }

        public string AddressLine2 { get; set; }

        public string Code { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public Guid Country_Id { get; set; }

        public Country Country { get; set; }
    }
}
