(function ($)
{

	$.fn.dialog = function (options)
	{

		// This is the easiest way to have default options.
		var settings = $.extend({
			// These are the defaults.
			data: undefined,
			buttons: [],
			closeText: "X",
			onCancel: function () { },
			zIndex: 10,
			title: "",
			removable: true,
			screan: false,
			cancelText: "NO",
			confirmText: "YES",
			onConfirm: undefined,
			draggable: true,
			width: undefined,
			height: undefined,
			onShow: undefined
		}, options);
		var item = {};
		var dialogDim = undefined;
		var container = $(this);
		if(!settings.title || settings.title === "")
			settings.title = "	&#160;";
		var div = $("<div class='dialog'><h1>" +settings.title + "<span class='close'>" + settings.closeText + "</span></h1><div><table><tr> <td></td>  </tr><tr><td style='height:11px'></td></tr></table></div></div>");
		$.each(settings.buttons, function ()
		{
			var x = this;
			var span = $("<span></span>").html(this.text).click(function ()
			{
				var result = x.click();
				if(result == undefined || result == true)
					item.Hide();
			});

			div.find("h1").append(span);
		})

		item.getDilogDim = function ()
		{
			if(dialogDim)
				return dialogDim;
			dialogDim = $("<div class='dialogDim'></div>");
			settings.zIndex += $(".dialog").length * 2;
			dialogDim.css("z-index", settings.zIndex);
			settings.zIndex++;
		}

		item.Hide = function ()
		{
			if(!settings.removable)
			{
				if(div.slideUp)
				{
					dialogDim.slideUp("slow");
					div.slideUp("slow");
				}
				else
				{
					dialogDim.hide("slow");
					div.hide("slow");
				}

			} else
			{
				dialogDim.remove();
				div.remove();
			}
			return item;
		};

		item.Show = function ()
		{

			if(div.slideDown)
			{
				dialogDim.slideDown("fast");
				div.slideDown("fast");
			}
			else
			{
				dialogDim.show("fast");
				div.show("fast");
			}


			if(div.draggable && settings.draggable)
				div.draggable({ scroll: true, handle: div.find("h1:first-child") });
			div.center(container);

			$(window).bind("resize", function ()
			{
				div.center(container);
			});

			if(settings.width)
				div.children("div").width(settings.width);
			if(settings.height)
				div.children("div").height(settings.height);
			if(settings.onShow)
				settings.onShow(settings);
			return item;
		}

		item.Build = function ()
		{
			item.getDilogDim();
			div.css("z-index", settings.zIndex);
			div.hide();
			dialogDim.hide();
			container.prepend(dialogDim).prepend(div);
			div.find("tr:first-child").find("td").append(settings.data);

			if(settings.onConfirm)
			{
				div.find("tr").last().find("td").first().append("<span>" + settings.cancelText + "</span>");
				div.find("tr").last().find("td").first().find("span").click(function ()
				{
					var res = settings.onCancel(settings);
					if(res == undefined || res == true)
						item.Hide();
				});

				div.find("tr").last().find("td").first().append("<span>" + settings.confirmText + "</span>");

				div.find("tr").last().find("td").first().find("span:last-child").click(function ()
				{
					var res = settings.onConfirm(settings);
					if(res == undefined || res == true)
						item.Hide();
				});
			} /*else div.children("div").children("table").children("tbody").children("tr").last().remove();*/

			if(settings.screan)
			{
				div.css({ width: "90%", height: "90%" });

			}

			div.find(".close").click(function ()
			{
				var res = settings.onCancel(settings);
				if(res == undefined || res == true)
					item.Hide();

			});
		}

		item.find = function (filter)
		{
			return div.find(filter);
		}
		item.Build();
		return item;
	}
}(jQuery));