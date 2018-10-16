using EntityWorker.Core.FastDeepCloner;
using System;
using System.Text.RegularExpressions;

namespace IProduct.Modules.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Regex : Attribute
    {
        private string _message;
        private string _regEx;
        public Regex(string regEx, string message)
        {
            _regEx = regEx;
            _message = message;
        }

        public string Validate(object value, IFastDeepClonerProperty property)
        {
            var errorMessage = _message ?? $"Field {property.Name} value is not valid";
            return System.Text.RegularExpressions.Regex.IsMatch((value?.ToString() ?? ""), _regEx, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)) ? null : errorMessage;
        }
    }
}
