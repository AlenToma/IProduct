using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IProduct.Models
{
    public class TableTreeSettings
    {
        public string Sort { get; set; }

        public string SortColumn { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public int SelectedPage { get; set; }

        public string SearchText { get; set; }

        public object Result { get; set; }
    }
}