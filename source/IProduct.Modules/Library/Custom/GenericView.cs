using EntityWorker.Core.FastDeepCloner;
using EntityWorker.Core.Helper;
using IProduct.Modules.Attributes;
using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace IProduct.Modules.Library.Custom
{
    public class GenericView<T> where T : Entity
    {
        private List<string> _errors = new List<string>();
        public bool Success { get; private set; }

        public string SuccessMessage { get; set; } = "The data have been saved";

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

        public GenericView<T> Error(Exception message, IProduct.Modules.Interface.IController controller)
        {
            controller.ExceptionHandled = true;
            return Error(message.InnerException?.Message ?? message.Message);

        }

        public GenericView<T> Validate()
        {
            _errors.Clear();
            _errors.AddRange(Actions.ValidateRequireField(this.View));
            Success = _errors.Count() <= 0;
            return this;
        }



        #region ViewModel Data Joiner


        // join the result of insert, update to the model
        // this is better then to create a viewmodel for each class
        public T Join(T wholeData)
        {
            return Join(wholeData, View);
        }

        private PT Join<PT>(PT wholeData, PT modelView)
        {
            if (modelView == null)
                return wholeData;
            var type = wholeData != null ? wholeData.GetType() : modelView.GetType();
            foreach (var p in DeepCloner.GetFastDeepClonerProperties(type).Where(x =>  x.CanReadWrite && (!x.IsInternalType || x.ContainAttribute<ModelView>())))
            {

                var newValue = p.GetValue(modelView);
                var oldValue = p.GetValue(wholeData);
                if (p.IsInternalType)
                {
                    p.SetValue(wholeData, newValue);
                }
                else
                {
                    if (oldValue == null)
                        continue;
                    if (newValue is IList)
                    {
                        var list = newValue as IList;

                        foreach (var item in list)
                        {
                            var t = item as Entity;
                            if (!t.Id.HasValue)
                            {
                                ((IList)oldValue).Add(item);
                                continue;
                            }
                            var v = find(t.Id.Value, oldValue as IList);
                            if (v != null)
                                Join(v, item);
                            else
                                ((IList)oldValue).Add(item);
                        }
                    }
                    else
                    {
                        if (newValue != null && oldValue == null)
                            p.SetValue(wholeData, newValue);
                        else
                            Join(oldValue, newValue);
                    }

                }

            }
            return wholeData;
        }

        private object find(Guid id, IList data)
        {
            foreach (var item in data)
            {
                var t = item as Entity;
                if (t.Id == id)
                    return t;
            }
            return null;
        }

        #endregion
    }
}