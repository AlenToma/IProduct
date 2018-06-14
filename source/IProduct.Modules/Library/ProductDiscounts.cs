using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProduct.Modules.Library
{
    public class ProductDiscounts : Entity
    {
        public Guid Product_Id { get; set; }

        public Guid Discount_Id { get; set; }

        public Discounts Discounts { get; set; }
    }
}
