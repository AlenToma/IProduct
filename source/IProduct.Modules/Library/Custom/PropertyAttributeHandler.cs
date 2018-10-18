using EntityWorker.Core.FastDeepCloner;
using IProduct.Modules.Attributes;
using System;
using System.Linq.Expressions;
namespace IProduct.Modules.Library.Custom
{
    public class PropertyAttributeHandler<T> : CustomAttributesHandler<T> where T : class
    {
        private IFastDeepClonerProperty _property;
        public PropertyAttributeHandler(IFastDeepClonerProperty prop)
        {
            _property = prop;
        }
        public PropertyAttributeHandler<T> NotNullOrEmpty(string message = null)
        {
            _property.Add(new Required(message));
            return this;
        }

        public PropertyAttributeHandler<T> StringLength(int? length = null, string message = null)
        {
            _property.Add(new StringLength(message, length));
            return this;
        }

        public PropertyAttributeHandler<T> RegExp(string expression = null, string message = null)
        {
            _property.Add(new Regex(expression, message));
            return this;
        }

        public PropertyAttributeHandler<T> ModelView()
        {
            _property.Add(new ModelView());
            return this;
        }
    }
}
