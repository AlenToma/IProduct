using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IProduct.Modules.Library
{
    public class Product : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        public string Keywords { get; set; }

        public string Meta { get; set; }

        public List<ProductCategories> ProductCategories { get; set; }

        public decimal Price { get; set; }

        public Guid? PriceCode { get; set; }

        public Country Country { get; set; }

        public DateTime? Available { get; set; }

        public EnumHelper.Status Status { get; set; }

        public List<ProductImages> Images { get; set; }

        public List<ProductTabs> Tabs { get; set; }

        public List<ProductDiscounts> ProductDiscounts { get; set; }
    }
}
