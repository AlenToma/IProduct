using EntityWorker.Core.Helper;
using System.Collections.Generic;

namespace IProduct.Models
{
    public class GenericView<T> where T : class
    {
        public T View { get; private set; }

        public List<string> Errors { get; set; }

        public List<string> Warnings { get; set; }

        public string Option_1 { get; set; }

        public string Option_2 { get; set; }

        public string Option_3 { get; set; }

        public GenericView(T view)
        {
            View = view;
        }

        public GenericView()
        {
            View = (T)typeof(T).CreateInstance();
        }

    }
}