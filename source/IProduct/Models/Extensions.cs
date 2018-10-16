using EntityWorker.Core.Helper;
using IProduct.Models.Controls;
using IProduct.Modules.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IProduct.Models
{
    public static class Extensions
    {
        public static MvcHtmlString AutoFillFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, AutoFillForSettings settings)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            settings.SelectedValue = settings.SelectedValue ?? htmlHelper.Value(expressionText);
            var id = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(expressionText);
            var name = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);

            var idd = DateTime.Now.ToFileTimeUtc().ToString();
            var script = new StringBuilder($"<input type='hidden' value='{settings.SelectedValue}' id='{id}' name='{name}'  /><input required='{settings.Required}' type='text' id='{idd}' ");
            script.Append("/><script>$('#" + idd + "').autofill({");
            script.Append($"textField:'{settings.TextField}', valueField:'{settings.ValueField}', selectedValue:'{settings.SelectedValue}',");
            script.Append("onselect:function(selecteditem) {");
            script.Append($"$('input#{id}').val(selecteditem['{settings.ValueField}']);");
            script.Append("},");
            using (var db = new DbContext())
            {
                if (settings.Data != null)
                    script.Append($"data:{ settings.Data.ToJson() }" + "});</script>");
                else
                    script.Append($"ajaxUrl:'{ settings.AjaxUrl }'" + "});</script>");
            }
            return new MvcHtmlString(script.ToString());
        }

    }
}