using EntityWorker.Core.FastDeepCloner;
using EntityWorker.Core.Helper;
using IProduct.Modules.Data;
using System;
using System.Linq;
using System.Web;

namespace IProduct.Modules.Library.Custom
{
    public class Cart
    {
        private DbContext _provider;

        public User _user;

        public Cart(DbContext provider)
        {
            _provider = new DbContext();
        }

        private Invoice Invoice
        {
            get
            {
                if (HttpContext.Current.Session["UserCarts"] == null)
                    HttpContext.Current.Session["UserCarts"] = new Invoice();
                return (Invoice)HttpContext.Current.Session["UserCarts"];
            }
            set
            {
                HttpContext.Current.Session["UserCarts"] = value;
            }
        }

        public Cart ApplyUser(Guid user_Id)
        {
            if (_user == null)
            {
                _user = _provider.Get<User>().Where(x => x.Id == user_Id).LoadChildren().ExecuteFirstOrDefault();
                var invoice = _user.Invoices.Find(x => x.InvoiceState == EnumHelper.InvoiceState.Pending);
                if (invoice == null)
                {
                    _user.Invoices.Add(new Invoice()
                    {
                        FullName = string.Format("{0} {1}", _user.Person.FirstName, _user.Person.LastName),
                        DeliveryAddress = _user.Person.Address.AddressLine,
                        State = _user.Person.Address.State,
                        City = _user.Person.Address.City,
                        ZipCode = _user.Person.Address.Code,
                        Country = _user.Person.Address.Country.Name,
                        Email = _user.Email
                    });
                    invoice = _user.Invoices.Last();
                    _provider.Save(_user);
                    _provider.SaveChanges();
                }
                var items = _provider.Clone(invoice.ProductTotalInformations, CloneLevel.Hierarki);
                foreach (var product in items)
                {

                    if (Invoice.ProductTotalInformations.ContainsKey(product.Key))
                    {
                        if (Invoice.ProductTotalInformations[product.Key] != product.Value)
                        {
                            var newValue = invoice.ProductTotalInformations[product.Key];
                            invoice.ProductTotalInformations.Remove(product.Key);

                            newValue += Invoice.ProductTotalInformations[product.Key];
                            invoice.ProductTotalInformations.Add(product.Key, newValue);
                        }
                    }
                }
                foreach (var product in Invoice.Products.Where(x => invoice.Products.All(a => a.Id != x.Id)))
                {
                    invoice.Products.Add(product);
                    invoice.ProductTotalInformations.Add(product.Id.Value, Invoice.ProductTotalInformations[product.Id.Value]);
                }
            }
            Invoice = null;
            return this;
        }

        public Cart Add(Product userCart, decimal total)
        {
            var invoice = _user != null ? _user.Invoices.Find(x => x.InvoiceState == EnumHelper.InvoiceState.Pending) : Invoice;
            if (invoice.Products.Any(x => x.Id == userCart.Id && x.Object_Status == EnumHelper.ObjectStatus.Added))
            {
                var newValue = invoice.ProductTotalInformations[userCart.Id.Value] + total;
                invoice.ProductTotalInformations.Remove(userCart.Id.Value);
                invoice.ProductTotalInformations.Add(userCart.Id.Value, newValue);
            }
            else
            {
                invoice.Products.Add(userCart);
                invoice.ProductTotalInformations.Add(userCart.Id.Value, total);
            }
            var items = _provider.Clone(invoice.ProductTotalInformations, CloneLevel.Hierarki);

            foreach (var item in items)
            {
                if (item.Value <= 0)
                {
                    invoice.Products.RemoveAll(x => x.Id == item.Key);
                    invoice.ProductTotalInformations.Remove(item.Key);
                }
            }
            if (_user != null)
            {
                _provider.Save(_user);
                _user = _provider.Get<User>().Where(x => x.Id == _user.Id).LoadChildren().ExecuteFirstOrDefault();
                _provider.SaveChanges();
            }
            return this;
        }


        public Invoice Get()
        {
            if (_user != null)
                return _user.Invoices.Find(x => x.Object_Status != EnumHelper.ObjectStatus.Removed && x.InvoiceState == EnumHelper.InvoiceState.Pending);
            else return Invoice;
        }

        public void Update(string field, string value)
        {

            var invoice = _user != null ? _user.Invoices.Find(x => x.InvoiceState == EnumHelper.InvoiceState.Pending) : Invoice;

            if (field == "password" && _user == null && !string.IsNullOrEmpty(invoice.Email))
            {
                var user = _provider.Get<User>().Where(x => x.Email == invoice.Email && x.Password == value).LoadChildren().ExecuteFirstOrDefault();
                if (user != null)
                    ApplyUser(user.Id.Value);
            }

            var prop = DeepCloner.GetFastDeepClonerProperties(invoice.GetType()).FirstOrDefault(x => string.Equals(x.Name, field, System.StringComparison.CurrentCultureIgnoreCase));
            if (prop != null)
                prop.SetValue(invoice, value.ConvertValue(prop.PropertyType));

        }
    }
}
