using IProduct.Modules.Library.Base_Entity;
using System;
using System.Web.Mvc;

namespace IProduct.Modules.Library
{
    public class ProductTabs : Entity
    {
        public string Name { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        public Guid Product_Id { get; set; }
    }
}
