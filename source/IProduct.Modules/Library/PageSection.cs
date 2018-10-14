using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IProduct.Modules.Library
{
    public class PageSection : Entity
    {
        public string SectionName { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        public PageType PageType { get; set; }

        public ProductShow ProductShow { get; set; }

        public List<PageCategories> PageCategories { get; set; }

        public Guid Page_Id { get; set; }

        public int Order { get; set; }
    }
}
