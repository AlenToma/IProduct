using IProduct.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IProduct.Controllers.Shared
{
    [PAuthorize]
    public class ProtectedController : SharedController
    {
    }
}