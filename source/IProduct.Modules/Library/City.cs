using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IProduct.Modules.Library
{
    public class City : Entity
    {
        public string Name { get; set; }

        public Guid Country_Id { get; set; }
    }
}
