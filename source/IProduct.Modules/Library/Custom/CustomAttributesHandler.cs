using EntityWorker.Core.FastDeepCloner;
using System;
using System.Linq.Expressions;
using EntityWorker.Core.Helper;
using IProduct.Modules.Attributes;

namespace IProduct.Modules.Library.Custom
{
    public class CustomAttributesHandler<T> where T : class
    {
        public CustomAttributesHandler<T> NotNullOrEmpty<TP>(Expression<Func<T, TP>> action, string message = null)
        {
            DeepCloner.GetProperty(typeof(T), action.GetMemberName()).Add(new Required(message));
            return this;
        }

        public CustomAttributesHandler<T> StringLength<TP>(Expression<Func<T, TP>> action, int? length = null, string message = null)
        {
            DeepCloner.GetProperty(typeof(T), action.GetMemberName()).Add(new StringLength(message, length));
            return this;
        }

        public CustomAttributesHandler<T> RegExp<TP>(Expression<Func<T, TP>> action, string expression = null, string message = null)
        {
            DeepCloner.GetProperty(typeof(T), action.GetMemberName()).Add(new Regex(expression, message));
            return this;
        }

        public CustomAttributesHandler<TP> Entity<TP>() where TP : class
        {
            return new CustomAttributesHandler<TP>();
        }
    }
}
