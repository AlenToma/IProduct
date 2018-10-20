(function ($)
{

	$.fn.shoppingCart = function (options)
	{
		// This is the easiest way to have default options.
		var settings = $.extend({
			get: undefined,
			save: undefined,
			field: undefined,
			image: undefined,
			connector: undefined, // its a top menu shopping cart,
			display: "block",
			itemCount: 0,
		}, options);

		var container = $("<div></div>").addClass("shoppingCart");
		if(settings.display !== "none")
			$(this).append(container);
		var table = $("<table/>", { id: "cart", "class": "table table-hover table-condensed" })
			.append($("<thead/>").append($("<tr/>")
				.append($("<th/>", { style: "50%", html: "Product" }))
				.append($("<th/>", { style: "50%", html: "Price" }))
				.append($("<th/>", { style: "50%", html: "Quantity" }))
				.append($("<th/>", { style: "50%", "class": "text-center", html: "Subtotal" }))
				.append($("<th/>", { style: "50%", html: "" }))))
			.append($("<tbody/>"))
			.append($("<tfoot/>"));
		container.append(table);
		container.load = function ()
		{
			table.find("tbody").html("");
			container.ax({
				url: settings.get,
				async: false,
				success: function (data)
				{
					if(data === null)
						return;
					var total = 0;
					var totalSum = 0;
					$.each(data.productTotalInformations, function ()
					{
						total += this.v;
					});
					if(settings.connector && data.products.length > 0)
						$(settings.connector).html("(" + total + ")");
					settings.count = total;
					if(settings.display !== "none")
						$.each(data.products, function ()
						{
							var x = this;
							var info = $.grep(data.productTotalInformations, function (a) { return a.k === x.id })[0];
							var input = '<div class="row">' +
								'<div class="col-sm-3">' +
								'<div class="input-group">' +
								'<span class="input-group-btn">' +
								'<button type="button" class="btn btn-default btn-number" data-type="minus">' +
								'<span class="glyphicon glyphicon-minus"></span>' +
								'</button>' +
								'</span>' +
								'<input type="number" readonly style="width: 52px;" class="form-control input-number" value="' + info.v + '" />' +
								'<span class="input-group-btn">' +
								'<button type="button" class="btn btn-default btn-number" data-type="plus">' +
								'<span class="glyphicon glyphicon-plus"></span>' +
								'</button>' +
								'</span>' +
								'</div>' +
								'</div > ';


							var filethumpfullpath = "";
							if(this.images !== null)
								filethumpfullpath = this.images[0].images.fileThumpFullPath;
							src = settings.image + filethumpfullpath;
							var url = settings.image + "/Home/Product?id=" + x.id;

							var tr = $("<tr/>")
								.append($("<td/>", { "data-the": "Product" })
									.append($("<div/>", { "class": "row" })
										.append($("<div/>", { "class": "col-sm-2" })
											.append($("<img style='width: 92%;' href='" + url + "' src='" + src + "' class='img-responsiv' />")))
										.append($("<div/>", { "class": "col-sm-10 text-content" })
											.append($("<h2/>", { "class": "nomargin", href: url, html: x.name }))
											.append($("<p/>", { href: url, html: (isNullOrEmpty(this.description) ? "" : this.description) })))));
							tr.append($("<td/>", { "data-th": "Price", html: this.price.formatMoney() + ":-" }));
							tr.append($("<td/>", { "data-th": "Quantity" })
								.append(input));
							tr.append($("<td/>", { "data-th": "Subtotal", html: (this.price * info.v).formatMoney() + ":-" }));
							tr.append($("<td/>", { "class": "action", "data-th": "" })
								.append($('<button class="btn btn-danger btn-sm"><i class="fa fa-trash-o"></i></button>')));


							tr.find('.btn[data-type="minus"]').click(function ()
							{
								container.add(x.id, -1);
								container.load();
							});

							tr.find(".fa-trash-o").click(function ()
							{
								$("body").dialog({
									title: "Please Confirm",
									data: $("<span class='info'></span>").html("Are you sure?"),
									onConfirm: function ()
									{
										container.add(x.id, -1000);
										container.load();
									}
								}).Show();
							});

							tr.find('.btn[data-type="plus"]').click(function ()
							{
								container.add(x.id, 1);
								container.load();
							});
							table.find("tbody").append(tr);

							totalSum += this.price * info.v;
						});
					table.find("tfoot").html("");
					table.find("tfoot")
						.append($("<tr/>", { "class": "visible-xs" })
							.append($("<td/>", { "class": "text-center", html: "<strong>Total " + totalSum.formatMoney() + ":-</strong>" })));
					table.find("tfoot")
						.append($("<tr/>")
							.append($("<td/>").append($("<a/>", { "class": "btn btn-warning", html: "Continue Shopping" }).prepend('<i class="fa fa-angle-left"></i>')))
							.append($("<td/>", { "class": "hidden-xs" }))
							.append($("<td/>", { "colspan": "2", style: "text-align: right;", "class": "hidden-xs text-center" }).append("<strong>Total " + totalSum.formatMoney() + ":-</strong>"))
							.append($("<td/>", { html: '<a href="#" class="btn btn-success btn-block">Checkout <i class="fa fa-angle-right"></i></a>' })));
					toLocation($(":not([href=''])"));
				}
			});
		};

		container.count = function ()
		{
			container.load();
			return settings.count;
		};

		container.invoice = function ()
		{
			var result = undefined;
			container.ax({
				url: settings.get,
				async: false,
				success: function (data)
				{
					result = data;
				}
			});

			return result;
		};

		container.update = function (field, value)
		{
			container.ax({
				url: settings.field,
				data: JSON.stringify({ field: field, value: value }),
				async: false,
				success: function (data)
				{
				}
			});

			return result;
		};

		container.add = function (productId, count)
		{
			if(!count)
				count = 1;
			$("body").ax({
				url: settings.save,
				data: JSON.stringify({ productId: productId, count: count }),
				async: false,
				success: function (data)
				{
					container.load();
				}

			});
		};
		container.load();
		return container;
	};
}(jQuery));