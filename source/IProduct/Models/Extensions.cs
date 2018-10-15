using EntityWorker.Core.Helper;
using IProduct.Modules.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IProduct.Models
{
    public static class Extensions
    {

        /// <summary>
        /// Load AutoFill script
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="match"></param>
        /// <param name="textFieldName"></param>
        /// <param name="valueFieldName"></param>
        /// <param name="name"></param>
        /// <param name="selectedValue"></param>
        /// <param name="loadChildren"></param>
        /// <returns></returns>
        public static HtmlString GetAutoFill<T>(Expression<Predicate<T>> match, string textFieldName, string valueFieldName, string name, string selectedValue, bool required= false, bool loadChildren = false)
        {
            var id = DateTime.Now.ToFileTimeUtc().ToString();
            var script = new StringBuilder($"<input type='hidden' value='{selectedValue}' name='{name}'  /><input required='{required}' type='text' id='{id}' ");
            script.Append("/><script>$('#" + id + "').autofill({");
            script.Append($"textField:'{textFieldName}', valueField:'{valueFieldName}', selectedValue:'{selectedValue}',");
            script.Append("onselect:function(selecteditem) {");
            script.Append($"$('input[name={name}]').val(selecteditem['{valueFieldName}']);");
            script.Append("},");
            using(var db = new DbContext())
            {
                script.Append($"data:{(match != null ? db.Get<T>().Where(match).Take(300).Execute() : db.Get<T>().Take(300).Execute()).ToJson() }");
                script.Append("});</script>");
            }

            return new HtmlString(script.ToString());
        }
    }
}