using EntityWorker.Core.FastDeepCloner;
using EntityWorker.Core.Helper;
using IProduct.Models.Controls;
using IProduct.Modules.Data;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
namespace IProduct.Models
{
    public static class Extensions
    {

        /// <summary>
        /// Call on JSX class
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="attributes"></param>
        /// <param name="ifTerms"> if (1=1)</param>
        /// <returns></returns>
        public static MvcHtmlString JSX<TModel>(this HtmlHelper<TModel> htmlHelper, string name, dynamic data, dynamic attributes = null)
        {
            var variable = name.Split('=').Count() > 1 ? name.Split('=').First() : null;
            var className = name.Split('=').Last();
            var id = Guid.NewGuid().ToString();
            var div = "<div";
            if (attributes != null)
            {
                foreach (IFastDeepClonerProperty prop in DeepCloner.GetFastDeepClonerProperties(attributes.GetType()))
                {
                    if (prop.Name.ToLower() != "id")
                        div += $" {prop.Name}='{prop.GetValue(attributes)}'";
                }
            }
            div += $" id='{id}'> </div>";
            var script = new StringBuilder($"{div}<script>");
            var option = ((object)data).ToJson();
            if (variable != null)
                script.Append($"{variable} = ReactDOM.render(React.createElement({className},{option}),");
            else script.Append($"ReactDOM.render(React.createElement({className},{option}),");
            script.Append($"document.getElementById('{id}'));");
            script.Append("</script>");

            return new MvcHtmlString(script.ToString());
        }

        /// <summary>
        /// Create Autofill
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
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