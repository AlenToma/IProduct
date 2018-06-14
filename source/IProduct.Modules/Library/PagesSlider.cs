using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProduct.Modules.Library
{
    public class PagesSlider : Entity
    {
        public Guid Pages_Id { get; set; }

        public Guid Files_Id { get; set; }

        public Files Files { get; set; }
    }
}
