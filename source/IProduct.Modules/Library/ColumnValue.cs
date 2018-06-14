using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProduct.Modules.Library
{
    public class ColumnValue : Entity
    {
        public string Value { get; set; }

        public Guid Country_Id { get; set; }

        public Country Country { get; set; }

        public Guid Column_Id { get; set; }
    }
}
