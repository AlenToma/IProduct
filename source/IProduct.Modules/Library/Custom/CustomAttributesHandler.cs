using EntityWorker.Core.FastDeepCloner;
using System;
using System.Linq.Expressions;
using EntityWorker.Core.Helper;
using IProduct.Modules.Attributes;

namespace IProduct.Modules.Library.Custom
{
    public class CustomAttributesHandler<T> where T : class
    {
        public PropertyAttributeHandler<T> Property<TP>(Expression<Func<T, TP>> action)
        {
            return new PropertyAttributeHandler<T>(DeepCloner.GetProperty(typeof(T), action.GetMemberName()));
        }

        public CustomAttributesHandler<TP> Entity<TP>() where TP : class
        {
            return new CustomAttributesHandler<TP>();
        }


    }
}
