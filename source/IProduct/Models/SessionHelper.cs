using IProduct.Modules.Library;
using IProduct.Modules.Library.Custom;
using System.Web;

namespace IProduct.Models
{
    public static class SessionHelper
    {
        public static Cart Cart
        {
            get
            {
                if (HttpContext.Current.Session["Cart"] == null)
                    HttpContext.Current.Session["Cart"] = new Cart(new IProduct.Modules.Data.DbContext());
                return (Cart)HttpContext.Current.Session["Cart"];
            }
            set
            {
                HttpContext.Current.Session["Cart"] = value;
            }
        }
    }
}