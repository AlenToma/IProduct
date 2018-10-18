(function ($)
{

	$.fn.autofill = function (options)
	{
		// This is the easiest way to have default options.
		var settings = $.extend({
			// These are the defaults.
			data: [{ name: "alen Toma", mail: "alen@htomail.com", children: [{ sureName: "jhshshshs" }] }, { name: "blen Toma", mail: "blen@htomail.com", children: [{ sureName: "sdf" }] }],
			textField: "",
			childTextField: undefined,
			childrenProperty: "children",
			valueField: "",
			childValueField: "",
			itemBinder: undefined,
			ajaxUrl: undefined,
			width: undefined,
			onselect: undefined,
			selectedValue: "",
			items: [],
			disabledItems: []
		}, options);


		var container = $(this);
		var added = container.parent().parent().hasClass("autofill");
		var ul = added ? container.closest(".autofill") : $("<ul class='autofill'><li><span class='arrow'>&nbsp;</span></li><li><ul></ul></li></ul>");
		if(!added)
		{
			ul.insertAfter(container);
			ul.find("li").first().prepend(container);
		}

		container = ul; // ul

		container.firstUpperCase = function (input)
		{
			if(input && input.length > 1)
				return input[0].toUpperCase() + input.substr(1);
			return "&nbsp;";
		};

		container.Show = function (o, arg)
		{
			if(o.slideDown)
				o.slideDown(arg);
			else o.show(arg);
		};

		container.Hide = function (o, arg)
		{
			if(o.slideUp)
				o.slideUp(arg);
			else o.hide(arg);
		};

		container.Sort = function (data, column, direction)
		{
			if(direction === "none")
				return data;
			return data.sort(function (row, rowb)
			{
				var textA = row[column];
				var textB = rowb[column];
				if(direction === "desc")
					return textA < textB ? -1 : 1;
				else return textA < textB ? 1 : -1;
			});
		};

		function SetSelectedValue()
		{
			if(settings.selectedValue && settings.selectedValue !== null && settings.selectedValue !== "")
			{
				var found = $.grep(settings.items, function (a) { return a[settings.valueField] === settings.selectedValue });
				if(found.length <= 0 && settings.data && settings.data.length > 0)
					found = $.grep(settings.data, function (a) { return a[settings.valueField] === settings.selectedValue });
				if(found.length > 0)
				{
					container.find("input").val(found[0][settings.textField]);
				} else
					$.ajax({
						url: settings.ajaxUrl,
						data: JSON.stringify({ value: settings.selectedValue }),
						type: "POST",
						dataType: "json",
						contentType: "application/json; charset=utf-8",
						success: function (data)
						{
							if(data && data.success && data.success === true)
								data = data.data;
							if(data.length > 0)
								container.find("input").val(data[0][settings.textField]);
						},
						error: function ()
						{

						}
					});
			}
		}

		container.getData = function (callback, focus)
		{
			if(!settings.ajaxUrl)
				return callback($.grep(settings.data, function (a) { return focus === true || a[settings.textField].toLowerCase().indexOf(container.find("input").val().toLowerCase()) !== -1 }));

			$.ajax({
				url: settings.ajaxUrl,
				data: JSON.stringify({ value: focus ? "" : container.find("input").val() }),
				type: "POST",
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (data)
				{
					if(data && data.success && data.success === true)
						data = data.data;
					callback(data);
				},
				error: function ()
				{

				}
			});
		};

		container.select = function ()
		{
			var dta = container.children("li:last").last().find("ul");
			var selectedLi = dta.find(".selected");
			if(selectedLi.children(".disabled").length > 0)
				return;
			if(selectedLi.length > 0)
			{
				container.find("input").val(selectedLi.children("span").text());
				container.children("li:last").last().hide();
			}
			if(settings.onselect && selectedLi.length)
			{

				settings.onselect(JSON.parse(selectedLi.attr("data")));

			}
		};

		container.ajust = function ()
		{
			var rect = container[0].getBoundingClientRect();
			var inputcontainer = container.children("li:eq(0)");
			var li = container.children("li:eq(1)");
			li.css({ top: inputcontainer.outerHeight(true) + rect.top });
			if(!inputcontainer.find(".arrow").is(":hidden"))
				inputcontainer.find(".arrow").height(inputcontainer.find("input").height());
		};

		var timeout = undefined;
		container.Build = function (focus)
		{
			if(timeout)
				clearTimeout(timeout);
			var dta = container.children("li:last").last().find("ul");
			settings.width = container.find("input").outerWidth(true);
			ul.find("li").last().width(settings.width);
			dta.html("");
			container.getData(function (data)
			{
				timeout = setTimeout(function ()
				{
					data = container.Sort(data, settings.textField, "desc");
					data = $.grep(data, function (i, a) { return a <= 300; });
					if(settings.items)
						$.each(settings.items, function () { data.unshift(this); });
					function isDisabled(li, item)
					{
						$.each(settings.disabledItems, function ()
						{
							if(this[settings.valueField] == item[settings.valueField])
								li.find("span").addClass("disabled");

						});
					}
					function buildItem(parent, item)
					{
						if(settings.childrenProperty && item[settings.childrenProperty] && item[settings.childrenProperty].length > 0 && settings.childTextField)
						{
							parent.addClass("hasChildren");
							var ul = $("<ul></ul>");

							parent.find("handler").click(function ()
							{
								parent.toggleClass("show");
							});

							$.each(item[settings.childrenProperty], function ()
							{
								var li = $("<li data='" + JSON.stringify(this) + "'><span>" + container.firstUpperCase(this[settings.childTextField]) + "</span></li>");
								if(settings.itemBinder)
									settings.itemBinder(li.find("span"), this);

								buildItem(li, this);
								isDisabled(li, this);
								ul.append(li);
								if(settings.childValue && this[settings.childValueField] === settings.selectedValue)
								{
									li.addClass("selected");
								}

							});
							parent.append(ul);
						}
					}

					$.each(data, function ()
					{
						var li = $("<li data='" + JSON.stringify(this) + "'><span>" + container.firstUpperCase(this[settings.textField]) + "</span></li>");
						if(settings.itemBinder)
							settings.itemBinder(li.find("span"), this);
						buildItem(li, this);
						isDisabled(li, this);
						dta.append(li);
						if(settings.childValue && this[settings.valueField] === settings.selectedValue)
							li.addClass("selected");
					});


					dta.find("li> span:not(.disabled)").mouseover(function ()
					{
						$(this).parent().addClass("selected");
					}).mouseout(function ()
					{
						$(this).parent().removeClass("selected");
					}).click(function ()
					{
						$(this).parent().addClass("selected");
						container.select();
					});
					if(data.length > 0)
						container.Show(dta.parent(), focus ? "slow" : "fast");
					else container.Hide(dta.parent(), focus ? "slow" : "fast");


					container.ajust();
					if(dta.find(".selected").length > 0)
						dta.parent().animate({ scrollTop: dta.find(".selected").offset().top }, 0);
				}, 100);
			}, focus);


		};

		if(!added)
			container.find("input").bind("focus", function ()
			{
				container.Build(true);
			});

		if(!added)
			container.find("input").bind("keyup", function (e)
			{
				var x = event.keyCode;
				if(x != 13 && x != 40 && x != 38 && x != 27)
					container.Build();
				else if(x == 13)
					container.select();
				else if(x == 27)
					container.children("li:last").last().hide();
			});

		// keydown
		if(!added)
			container.find("input").bind("keydown", function (e)
			{
				var dta = container.children("li:last").last().find("ul");
				var selectedIndex = -1;
				dta.find("li").each(function (i, a)
				{
					if($(this).hasClass("selected"))
						selectedIndex = i;
				});
				var x = event.keyCode;
				if(x == 40 || x == 38)
				{
					dta.find(".selected").removeClass("selected");

					if(container.children("li:last").is(":hidden"))
						container.Build();
					if(x === 40)
					{
						if(selectedIndex < 0 || selectedIndex >= dta.find("li").length)
						{
							$(dta.find("li")[0]).addClass("selected");
							dta.parent().scrollTop(0);
						}
						else
						{
							dta.find("li").each(function (i, a) { if(i == selectedIndex + 1) $(this).addClass("selected") });

							dta.parent().scrollTop(dta.parent().scrollTop() + dta.children("li").find("span:first").outerHeight());
						}
					} else if(x === 38)
					{
						if(selectedIndex < 0 || selectedIndex >= dta.find("li").length)
						{
							$(dta.find("li")[dta.find("li").length - 1]).addClass("selected");
							dta.parent().scrollTop(dta.parent()[0].scrollHeight);
						}
						else
						{
							dta.find("li").each(function (i, a) { if(i == selectedIndex - 1) $(this).addClass("selected") });
							dta.parent().scrollTop(dta.parent().scrollTop() - dta.children("li").find("span:first").outerHeight());
						}
					}

				}
			});
		if(!added)
			$("body,html").bind("mousedown", function (e)
			{
				var target = $(e.target);
				if(!target.hasClass("autofill") && !target.hasClass("arrow") && target.closest(".autofill").length <= 0)
					container.Hide(container.children("li").last(), "fast");
			});
		if(!added)
			container.find(".arrow").bind("click", function ()
			{
				//container.Build(true);
				container.find("input").focus();
			});

		SetSelectedValue();
		setTimeout(container.ajust, 100);
		return container;
	};
}(jQuery));