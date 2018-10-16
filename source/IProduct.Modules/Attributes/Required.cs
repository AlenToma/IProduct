using EntityWorker.Core.FastDeepCloner;
using EntityWorker.Core.Helper;
using System;
using System.Collections;

namespace IProduct.Modules.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class Required : Attribute
    {
        private string _message;

        /// <summary>
        /// Isnullorempty
        /// </summary>
        /// <param name="validationType"></param>
        /// <param name="message"></param>
        public Required(string message = null)
        {
            _message = message;
        }

        public string Validate(object value, IFastDeepClonerProperty property)
        {
            // Default message
            var errorMessage = _message ?? $"Field {property.Name} can not be empty";
            if(value == null)
                return errorMessage;
            if(string.IsNullOrEmpty(value.ToString()))
                return errorMessage;
            return null;
        }

    }
}
