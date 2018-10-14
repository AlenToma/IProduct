using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IProduct.Modules.Library
{
    public class Invoice : Entity
    {
        public Invoice()
        {
            InvoiceDate = DateTime.Now;
        }
        public DateTime InvoiceDate { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// Containes productId and total ordered items for each product
        /// </summary>
        public Dictionary<Guid, decimal> ProductTotalInformations { get; set; } = new Dictionary<Guid, decimal>();

        public Guid User_Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string DeliveryAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }

        public InvoiceState InvoiceState { get; set; }

        public decimal Total
        {
            get
            {
                return ProductTotalInformations?.Select(x => x.Value * Products.Find(a=> a.Id == x.Key).Price).Sum() ?? 0;
            }
        }

    }
}
