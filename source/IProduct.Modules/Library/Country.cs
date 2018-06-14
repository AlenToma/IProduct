using IProduct.Modules.Library.Base_Entity;
using System.Collections.Generic;

namespace IProduct.Modules.Library
{
    public class Country : Entity
    {
        public string Name { get; set; }

        public string CountryCode { get; set; }

        public bool Active { get; set; }

        public List<City> Cities { get; set; }
    }
}
