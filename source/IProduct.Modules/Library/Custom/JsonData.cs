using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProduct.Modules.Library.Custom
{
   public class JsonData
    {
        public bool Success { get; set; }

        public int Status { get; set; }

        public string Description { get; set; }

        public object Data { get; set; }
    }
}
