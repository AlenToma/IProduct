using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProduct.Modules.Library
{
    public class ProductImages : Entity
    {
        public Guid Product_Id { get; set; }
        public Guid Image_Id { get; set; }
        public Files Images { get; set; }
    }
}
