using IProduct.Modules.Library.Base_Entity;
using System;
namespace IProduct.Modules.Library
{
    public class Files : Entity
    {
        public byte[] File { get; set; }

        public string FriendlyName { get; set; }

        public string Type { get; set; }

        public Guid Mapp_Id { get; set; }
    }
}
