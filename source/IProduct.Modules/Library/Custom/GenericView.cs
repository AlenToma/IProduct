using EntityWorker.Core.Helper;
using System.Collections.Generic;

namespace IProduct.Modules.Library.Custom
{
    public class GenericView<T> where T : class
    {
        private List<string> _errors = new List<string>();
        public bool Success { get; private set; }

        public T View { get; private set; }

        public string Option_1 { get; set; }

        public GenericView()
        {
            View = (T)typeof(T).CreateInstance();
        }

        public GenericView(T view)
        {
            View = view;
        }

        public string GetHtmlError()
        {
            if (!Success && _errors.Any())
                return string.Join("</br>", _errors);
            return null;
        }

        public GenericView<T> Error(string message)
        {
            Success = false;
            _errors.Add(message);
            return this;
        }

        public GenericView<T> Validate()
        {
            _errors.Clear();
            _errors.AddRange(Actions.ValidateRequireField(this.View));
            Success = _errors.Count() <= 0;
            return this;
        }
    }
}