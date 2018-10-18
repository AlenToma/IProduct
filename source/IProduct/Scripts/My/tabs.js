(function ($) {

    $.fn.tabs = function (options) {

        // This is the easiest way to have default options.
        var settings = $.extend({
            // These are the defaults.
            data: [],
            onSelect: function () { },
            selectedTab: undefined,
            wizard: false,
            autoHeight: false
        }, options);

		var container = $("<div/>", { "class": "tabContainer" });
		$(this).append(container);

		container.selectedItem = function ()
		{
			return settings.selectedTab;
		};

        container.findItem = function (tabId) {
            var item = undefined;
            $.each(settings.data, function () {
                if (this.id === tabId)
                    item = this;

            });
            return item;
		};

        container.select = function (tabId) {
            if (settings.selectedTab && settings.selectedTab.id == tabId)
                return;
            var item = container.findItem(tabId);
            item.enable();
            container.find(".tabs").find(".selected").removeClass("selected");
            container.find(".tabs").find("li[tab='" + tabId + "']").addClass("selected");
            container.children("div").hide();
			container.children("div[tab='" + tabId + "']").css("display", "inline-table");
            settings.selectedTab = item;
            if (settings.onSelect)
                settings.onSelect(item);
            if (settings.wizard)
                item.content.css({ "border-radius": "5px" });
            return item;
		};

		container.firstUpperCase = function (input)
		{
			if(input && input.length > 1)
				return input[0].toUpperCase() + input.substr(1);
			return "	&nbsp;";
		};

		container.disable = function (tabId)
		{
			var item = container.findItem(tabId);
			item.handler.addClass("disabled");
		};

		container.enable = function (tabId)
		{
			var item = container.findItem(tabId);
			item.handler.removeClass("disabled");
		};

		container.Build = function ()
		{
			container.html("<ul class='tabs'></ul>");
			if(settings.wizard)
				container.children(".tabs").addClass("wizard");
		};

        container.add = function (content, title, tabId) {
            var item = {
                id: tabId,
                content: $("<div  class='tabContent' tab='" + tabId + "'></div>"),
                handler: $("<li tab='" + tabId + "'><span>" + container.firstUpperCase(title) + "</span></li>"),
                select: function () { return container.select(tabId) },
                disable: function () { container.disable(tabId); return container.findItem(tabId) },
                enable: function () { container.enable(tabId); return container.findItem(tabId) }
            };
            if (settings.autoHeight)
                item.content.css("max-height", "100%");
            if (container.findItem(tabId)) {
                item = container.findItem(tabId);
                return item;
            }
            settings.data.push(item);
            item.content.append(content);
            container.append(item.content.hide());
            container.find(".tabs").append(item.handler);
            item.handler.click(function () {
                if (!item.handler.hasClass("disabled"))
                    container.select(tabId);
            });
            return item;
		};

        container.Build();
        return container;
	};
}(jQuery));