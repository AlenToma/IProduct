(function ($)
{
	$.fn.checkBox = function (options)
	{
		var settings = $.extend({
			label: function () { },
			prop: undefined,
			items: undefined
		}, options);
		settings.items = this;
		var setCounter = settings.items === undefined;

		function validateBox(o)
		{
			var container = o.parent();
			if(!container.hasClass("lightDataCheckBox"))
				return;
			var checkType = o.attr("checkType");
			if(o.is(":disabled"))
				container.find("span").attr("disabled", "disabled");
			
			if(!checkType || checkType === "")
			{
				if(o.is(":checked"))
				{
					container.find(".b").addClass("checked");
				} else
				{
					container.find(".b").removeClass("checked");
				}
			} else
			{

				container.find(".b").find(".val").removeClass("checked");
				if(o.is(":checked"))
				{
					container.find(".b").find(".val").first().addClass("checked");
				} else
				{
					container.find(".b").find(".val").last().addClass("checked");
				}
			}
		}

		settings.prop = function (checked)
		{
			$.each(settings.items, function ()
			{
				$(this).prop("checked", checked);
				validateBox($(this));
			});
		};

		function bind(items)
		{
			items = items ? items : $("input[type='checkbox']:not(.iniLightDataTableCheckBox)");
			$.each(items, function ()
			{

				var o = $(this);
				var label = o.attr("label");
				var boxClass = o.attr("boxClass");
				if(!o.hasClass("iniLightDataTableCheckBox"))
				{
					if(o.attr("v") === "false" || o.attr("v") === "true")
						o.prop("checked", eval(o.attr("v")));

					var checkType = o.attr("checkType");

					o.addClass("iniLightDataTableCheckBox");
					var container = $("<div class='lightDataCheckBox'><span class='box b'></span></div>");
					if(label && label !== "")
						container.append("<span class='text'>" + label + "</span>");
					if(boxClass && boxClass !== "")
						container.addClass(boxClass);

					container.insertAfter(o);
					container.append(o);
					o.hide();
					if(checkType && checkType !== "")
					{
						container.find(".box").removeClass("box").addClass("customBox").append("<span class='val'>" +
							checkType.split(",")[0] + "</span><span class='val'>" +
							checkType.split(",")[1] +
							"</span>");

						container.css("min-width",
							container.find(".val").first().outerWidth(true) +
							container.find(".val").last().outerWidth(true) +
							2);
						container.css("margin-top", "0px");
						container.css("margin-bottom", "26px");
					}

					o.change(function ()
					{
						validateBox(o);
					});

					if(!checkType || checkType === "")
					{
						container.find(".b").click(function ()
						{
							o.click();
							o.prop("checked", o.is(":checked"));
							validateBox(o);
						});
					} else
					{
						container.find(".b").find(".val").first().click(function ()
						{
							o.prop("checked", true);
							o.change();
							validateBox(o);
						});

						container.find(".b").find(".val").last().click(function ()
						{
							o.prop("checked", false);
							o.change();
							validateBox(o);
						});
					}

					validateBox(o);
				}

			});
			if(setCounter)
				setTimeout(bind, 100);
		}

		bind(settings.items);
		return settings;
	};

}(jQuery));