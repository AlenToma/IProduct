using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace OAuth.Security.Interface
{
    public interface ISecuritySettings
    {
        T Save<T>(T item);

        void Remove<T>(T item);

        List<T> Get<T>(Expression<Predicate<T>> match);
    }
}
