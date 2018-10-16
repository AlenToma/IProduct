using EntityWorker.Core.Attributes;
using EntityWorker.Core.FastDeepCloner;
using System;

namespace IProduct.Modules.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class StringLength : Attribute
    {
        private int? _length;

        private string _message;
        public StringLength(string message = null, int? length = null)
        {
            _length = length;
            _message = message;
        }

        public string Validate(object value, IFastDeepClonerProperty property)
        {
            if(!_length.HasValue)
                _length = property.ContainAttribute<ToBase64String>() || property.ContainAttribute<DataEncode>() ? 7 : 3;
            var errorMessage = _message ?? $"Minimum length for field {property.Name} has not been reached. the minimum length is {_length.Value} character";
            if(value == null || value?.ToString().Length < _length)
                return errorMessage;
            return null;
        }
    }
}
