
namespace IProduct.Models.Controls
{
    /// <summary>
    /// this control is for auto fill combobox. 
    /// Extensions.AutoFillFor 
    /// @Html.AutoFillFor
    /// have to add @using IProject.Extensions
    /// </summary>
    public class AutoFillForSettings
    {
        public AutoFillForSettings(string valueField, string textField, object data, object selectedValue = null)
        {
            ValueField = valueField;
            TextField = textField;
            Data = data;
            SelectedValue = selectedValue;
        }

        public AutoFillForSettings(string valueField, string textField, string ajaxUrl, object selectedValue = null)
        {
            ValueField = valueField;
            TextField = textField;
            AjaxUrl = ajaxUrl;
            SelectedValue = selectedValue;
        }

        public object SelectedValue { get; set; }

        public string ValueField { get; set; }

        public string TextField { get; set; }

        //Use Data or ajax. 
        public string AjaxUrl { get; set; }

        //Use Data or ajax. 
        public object Data { get; set; }

        public bool Required { get; set; }
    }
}