using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IProduct.Modules.Library
{
    public class Pages : Entity
    {
        public string Name { get; set; }

        public string Meta { get; set; }

        public Guid? Parent_Id { get; set; }

        public List<Pages> Children { get; set; }

        public List<PagesSlider> PagesSliders { get; set; }

        public List<PageSection> PageSections { get; set; }

        public bool IsActive { get; set; }

        public int Order { get; set; }
    }
}
