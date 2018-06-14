using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProduct.Modules.Library
{
    public class PageCategories : Entity
    {
        public Guid Category_Id { get; set; }

        public Category Category { get; set; }

        public Guid Section_Id { get; set; }
    }
}
