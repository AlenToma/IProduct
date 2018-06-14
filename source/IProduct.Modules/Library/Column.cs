using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProduct.Modules.Library
{
    public class Column : Entity
    {
        public string Key { get; set; }

        public List<ColumnValue> ColumnValues { get; set; }

    }
}
