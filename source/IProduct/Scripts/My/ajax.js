(function ($) {

    jQuery.fn.center = function (parent) {
        var w = !parent || $(parent).prop("tagName") == "BODY" ? $(window) : $(parent)
        this.css({
            'position': 'absolute',
            'top': Math.abs(((w.height() - $(this).outerHeight()) / 2) + w.scrollTop()),
            'left': Math.abs(((w.width() - $(this).outerWidth()) / 2) + w.scrollLeft())
        });
        return this;
    }

    $.fn.ax = function (options) {
        var settings = $.extend({
            type: "POST",
            url: undefined,
            data: undefined,
            async: true,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            loader: true
        }, options);
        settings.timestamp = new Date().getTime().toString();
      
        var loader = $("<div class='loader " + settings.timestamp + "'><img src='Content/Image/Gear.gif' /></div>").hide();
        if (settings.loader)
            $(this).append(loader);
        var timeout = setTimeout(function () { if (loader) loader.show() }, 800);
        var st = $.extend({}, settings);
        st.success = function (data) {
            clearTimeout(timeout);
            settings.success(data);
            $("." + settings.timestamp).remove();
        }
        st.error = function (err, ee) {
            clearTimeout(timeout);
            $("." + settings.timestamp).remove();
            if (ee == "parsererror") {
                st.success();
                return;
            } else {
                if (settings.error)
                    settings.error(err, ee);
                if (window.console)
                    window.console.log(err);
            }
         
        }
        $.ajax(st)
    }
}(jQuery));