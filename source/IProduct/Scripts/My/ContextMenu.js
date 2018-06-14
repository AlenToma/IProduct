(function ($) {

    $.fn.contextMenu = function (options) {
        var settings = $.extend({
            dataSource: [],
            click: function (item) { },
            onLoad: function (settings) { },
            action: "right"
        }, options);
        var container = $(this);
        var ini = false;
        container.attr("title", "Right click to edit");
        function buildContext(e) {
            $(".contextMenu").remove();
            if (settings.onLoad)
                settings.onLoad(settings);
            var context = $("<ul class='contextMenu'></ul>");
            $.each(settings.dataSource, function () {
                var x = this;
                var div = $("<li class='contextItem'></li>");
                if (typeof x.text === "string")
                    div.html($("<span>" + x.text + "</span>"));
                else
                    div.html($("<span></span>").append(x.text));

                if (x.click)
                    div.click(function () { x.click(); })
                div.click(function () {
                    if (settings.click)
                        settings.click(x);
                    context.remove();
                });

                function loadItem(item, parent) {
                    var li = $("<li class='contextItem'></li>");
                    if (typeof item.text === "string")
                        li.html($("<span>" + item.text + "</span>"));
                    else
                        li.html($("<span></span>").append(item.text));

                    li.click(function () {
                        if (settings.click)
                            settings.click(item);
                        context.remove();
                    });

                    if (item.children && item.children.length > 0) {
                        parent.addClass("hasChildren");
                        li.addClass("hasChildren");
                        var y = $("<ul class='contextsubItem'></ul>");
                        $.each(item.children, function () {
                            loadItem(this, y);
                        });
                        li.append(y);
                    }

                    parent.append(li);
                }


                if (x.children && x.children.length > 0)
                    div.addClass("hasChildren");
                var ul = $("<ul class='contextsubItem'></ul>");
                $.each(x.children, function () {
                    loadItem(this, ul);
                });
                if (ul.children("li").length > 0)
                    div.append(ul);
                context.append(div);
            });
            var iFrame = undefined;
            try {
                iFrame = container.closest("html").parent();
                if (iFrame.length <= 0)
                    iFrame = undefined;
            } catch (ee) {

            }
            context.css({
                left: e.pageX,
                top: e.pageY
            });
            if (!iFrame)
                $("body").append(context);
            else {
                iFrame.find("body").find(".contextMenu").remove();
                iFrame.find("body").append(context);
                iFrame.find("body").mousedown(function (e) {
                    var target = $(e.target);
                    if (!(target.parent().hasClass("contextItem") || target.hasClass("contextItem") || target.hasClass("contextMenu"))) {
                        context.remove();
                        iFrame.find("body").find(".contextMenu").remove();

                    }

                });
            }

            context.slideDown("slow");
            context.width(Math.max.apply(Math,
                $.map(context.find("div"),
                    function (o) {
                        return o.getBoundingClientRect().width;
                    })));
            context.children(".contextItem").css("max-width", context.width() - (context.children(".contextItem").outerWidth(true) - context.width()));
            ini = true;
        }

        $("body").mousedown(function (e) {
            var target = $(e.target);
            if (!(target.parent().hasClass("contextItem") || target.hasClass("contextItem") || target.hasClass("contextMenu")))
                $(".contextMenu").remove();

        });
        if (settings.action == "right") {
            container.bind("contextmenu", function (e) {
                buildContext(e);
                return false;
            });
        } else {
            container.bind("click", function (e) {
                buildContext(e);
                return false;
            });
        }

        return container;
    }

}(jQuery));