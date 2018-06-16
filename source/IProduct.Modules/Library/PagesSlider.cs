using IProduct.Modules.Library.Base_Entity;
using System;

namespace IProduct.Modules.Library
{
    public class PagesSlider : Entity
    {
        public Guid Pages_Id { get; set; }

        public Guid Files_Id { get; set; }

        public Files Files { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }
    }
}
