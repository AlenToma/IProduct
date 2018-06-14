using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections.Generic;

namespace IProduct.Modules.Library
{
    public class Mapps : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool System { get; set; }

        public List<Files> Files { get; set; }

        public Guid? Parent_Id { get; set; }

        public List<Mapps> Children { get; set; }
    }
}
