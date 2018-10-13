(function ($) {

    jQuery.fn.center = function (parent) {
        var w = !parent || $(parent).prop("tagName") == "BODY" ? $(window) : $(parent)
        this.css({
            'position': 'absolute',
            'top': Math.abs(((w.height() - $(this).outerHeight()) / 2) + w.scrollTop()),
            'left': Math.abs(((w.width() - $(this).outerWidth()) / 2) + w.scrollLeft())
        });
        return this;
    };

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
        var timeout = setTimeout(function () { if (loader) loader.show(); }, 800);
        var st = $.extend({}, settings);
        st.success = function (data) {
            clearTimeout(timeout);
            if (data === undefined || data === null || data.success === undefined || data.success !== false) {
                if (data && data.success && data.success === true)
                    settings.success(data.data);
                else
                    settings.success(data);

            }
            else st.error(data.error);
            $("." + settings.timestamp).remove();
        };
        st.error = function (jqXHR, textStatus, errorThrown) {
            clearTimeout(timeout);
            $("." + settings.timestamp).remove();
            if (textStatus === "parsererror" || jqXHR.status === 0) {
                st.success();
                return;
            } else {
                if (settings.error)
                    settings.error(jqXHR, textStatus, errorThrown);
                if (window.console) {
                    window.console.log('jqXHR:');
                    window.console.log(jqXHR);
                    window.console.log('textStatus:');
                    window.console.log(textStatus);
                    window.console.log('errorThrown:');
                    window.console.log(errorThrown);
                    if ($("body").dialog === undefined)
                        alert('status code: ' + jqXHR.status + '\n errorThrown: ' + errorThrown + '\n jqXHR.responseText:' + jqXHR.responseText);
                    else
                        $("body").dialog({
                            title: "Error has occurred. Please contact the administrator. ",
                            data: $("<div></div>").html('<p>status code: ' + jqXHR.status + '</p><p>errorThrown: ' + errorThrown + '</p><p>jqXHR.responseText:</p><div>' + jqXHR.responseText + '</div>')
                        }).Show();
                }
            }

        };
        $.ajax(st);
    };
}(jQuery));