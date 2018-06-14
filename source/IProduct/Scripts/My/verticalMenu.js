(function ($) {

    $.fn.verticlaMenu = function (options) {

        // This is the easiest way to have default options.
        var settings = $.extend({
            // These are the defaults.
            data: [{ name: "alen Toma", mail: "alen@htomail.com", children: [{ sureName: "jhshshshs" }] }, { name: "blen Toma", mail: "blen@htomail.com", children: [{ sureName: "sdf" }] }],
            textName: "",
            childName: undefined,
            childrenProperty: "children",
            itemBinder: undefined
        }, options);

        var container = $(this);
        container.firstUpperCase = function (input) {
            if (input && input.length > 1)
                return input[0].toUpperCase() + input.substr(1);
            return "	&nbsp;";
        }

        container.Build = function () {
            container.html("");
            function buildItem(parent, item, addline) {
                if (settings.childrenProperty && item[settings.childrenProperty] && item[settings.childrenProperty].length > 0 && settings.childName) {
                    parent.addClass("hasChildren");
                    var ul = $("<ul></ul>");

                    parent.prepend("<line></line><handler>&nbsp;</handler>");
                    if (!addline)
                        parent.find("line:first").remove();
                    parent.find("handler").click(function () {
                        parent.toggleClass("show")
                    });

                    $.each(item[settings.childrenProperty], function () {
                        var li = $("<li><span>" + container.firstUpperCase(this[settings.childName]) + "</span></li>");
                        if (settings.itemBinder)
                            settings.itemBinder(li.find("span"), this);
                        buildItem(li, this);
                        ul.append(li);
                    });
                    parent.append(ul);
                }
            }
            var ul = $("<ul class='verticlaMenu'></ul>");
            $.each(settings.data, function () {
                var li = $("<li><span>" + container.firstUpperCase(this[settings.textName]) + "</span></li>");
                if (settings.itemBinder)
                    settings.itemBinder(li.find("span"), this);
                buildItem(li, this, true);
                ul.append(li);
            });
            container.append(ul);
        }

        container.Build();
        return container;
    }
}(jQuery));