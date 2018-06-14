using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProduct.Modules.Library
{
    public class Discounts : Entity
    {
        public string Code { get; set; }

        public decimal DiscountPrice { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }
    }
}
