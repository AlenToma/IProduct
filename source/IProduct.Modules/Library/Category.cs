using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IProduct.Modules.Library
{
    public class Category : Entity
    {
        public string Name { get; set; } 

        public string Description { get; set; } 

        public List<Category> Categories { get; set; }

        public Guid? Parent_Id { get; set; }

        public bool Published { get; set; }
    }
}
