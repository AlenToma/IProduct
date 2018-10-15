using IProduct.Modules.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace IProduct.Models.ViewModels
{
    public class Google
    {
        public string Name { get; set; }

        public string RoleName { get; set; }

        public string Email { get; set; }

    }
}