(function ($)
{

	$.fn.tabs = function (options)
	{

		// This is the easiest way to have default options.
		var settings = $.extend({
			// These are the defaults.
			data: [],
			onSelect: function () { },
			selectedTab: undefined,
			wizard: false,
			autoHeight: false,
			type: "tabs" // tabs Or collapse
		}, options);

		var container = $("<div/>", { "class": "tabContainer tab-collapse" });
		$(this).append(container);

		container.selectedItem = function ()
		{
			return settings.selectedTab;
		};

		container.findItem = function (tabId)
		{
			var item = undefined;
			$.each(settings.data, function ()
			{
				if(this.id === tabId)
					item = this;

			});
			return item;
		};


		container.select = function (tabId)
		{
			if(settings.selectedTab && settings.selectedTab.id === tabId && settings.type !== "collapse")
				return;
			var slideDown = settings.selectedTab && settings.selectedTab.id === tabId;
			var item = container.findItem(tabId);
			item.enable();
			if(container.find(".tabs").length > 0)
			{
				container.find(".tabs").find(".selected").removeClass("selected");
				container.find(".tabs").find("li[tab='" + tabId + "']").addClass("selected");
			}
			if(settings.type !== "collapse")
			{
				container.children("div").hide();
				container.children("div[tab='" + tabId + "']").css("display", "block");
			} else
			{
				if(container.children("div").slideUp !== undefined)
				{
					container.children("div").slideUp("fast");
					if(!slideDown)
						container.children("div[tab='" + tabId + "']").slideDown("fast");
				} else container.children("div[tab='" + tabId + "']").show();


			}



			container.find("h4 > i").removeClass("fa-caret-up").addClass("fa-caret-down");
			if(!slideDown)
				container.find("h4[tab='" + tabId + "'] >i").addClass("fa-caret-up");
			if(!slideDown)
			{
				settings.selectedTab = item;
				if(settings.onSelect)
					settings.onSelect(item);
				if(settings.wizard)
					item.content.css({ "border-radius": "5px" });
			} else
			{
				settings.selectedTab = undefined;
			}
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
			if(settings.type !== "collapse")
			{
				container.html("<ul class='tabs'></ul>");
				if(settings.wizard)
					container.children(".tabs").addClass("wizard");
			}
		};

		container.add = function (content, title, tabId)
		{
			var item = {
				id: tabId,
				content: $("<div  class='tabContent' tab='" + tabId + "'></div>"),
				handler: $("<li tab='" + tabId + "'><span>" + container.firstUpperCase(title) + "</span></li>"),
				select: function () { return container.select(tabId); },
				disable: function () { container.disable(tabId); return container.findItem(tabId); },
				enable: function () { container.enable(tabId); return container.findItem(tabId); }
			};

			if(settings.type === "collapse")
			{
				item.handler = $("<h4 tab='" + tabId + "' >" + container.firstUpperCase(title) + " </h4>").append("<i class='fa fa-caret-down'></i>");
			}

			if(settings.autoHeight)
				item.content.css("max-height", "100%");
			if(container.findItem(tabId))
			{
				item = container.findItem(tabId);
				return item;
			}
			settings.data.push(item);
			item.content.append(content);
			if(settings.type !== "collapse")
			{
				container.append(item.content.hide()).find(".tabs").append(item.handler);
			} else
			{
				container.append(item.handler).append(item.content.hide());
			}

			item.handler.click(function ()
			{
				if(!item.handler.hasClass("disabled"))
					container.select(tabId);
			});
			return item;
		};

		container.Build();
		return container;
	};
}(jQuery));