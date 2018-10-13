(function ($) {

    $.fn.shoppingCart = function (options) {
        // This is the easiest way to have default options.
        var settings = $.extend({
            get: undefined,
            save: undefined,
            field: undefined,
            image: undefined,
            connector: undefined, // its a top menu shopping cart,
            display: "block",
            itemCount: 0
        }, options);

        var container = $("<div></div>").addClass("shoppingCart");
        if (settings.display !== "none")
            $(this).append(container);
        var table = $("<table><thead><tr></tr></thead><tbody></tbody><tfoot><tr><td colspan='5' style='text-align: right;'>Total:</td><td><span class='totalSum'> </span></td> </tr></tfoot></table>");
        table.find("thead > tr")
            .append("<th style='width:16px'></th>")
            .append("<th style='width:16px'><span></span></th>")
            .append("<th>Product</th>")
            .append("<th>Quantity</th>")
            .append("<th>Price</th>")
            .append("<th>Subtotal</th>");
        container.append(table);
        container.load = function () {
            table.find("tbody").html("");
            container.ax({
                url: settings.get,
                async: false,
                success: function (data) {
                    if (data === null)
                        return;
                    var total = 0;
                    var totalSum = 0;
                    $.each(data.producttotalinformations, function () {
                        total += this.v;
                    });
                    if (settings.connector && data.products.length > 0)
                        $(settings.connector).html("(" + total + ")");
                    settings.count = total;
                    if (settings.display !== "none")
                        $.each(data.products, function () {
                            var x = this;
                            var filethumpfullpath = "";
                            if (this.images !== null)
                                filethumpfullpath = this.images[0].images.filethumpfullpath;
                            var info = $.grep(data.producttotalinformations, function (a) { return a.k === x.id })[0];
                            src = settings.image + filethumpfullpath;
                            var tr = $("<tr></tr>")
                                .append("<td style='width:16px'><span class='delete'></span></td>")
                                .append("<td style='width:100'><img src='" + src + "' /></td>")
                                .append("<td><span> " + this.name + " </span><span class='description'>" + (isNullOrEmpty(this.description) ? "" : this.description) + " </span></td>")
                                .append("<td><span class='btn minus'>-</span><input type='text' disabled class='quantity' value='" + info.v + "' /> <span class='btn plus'>+</span> </td>")
                                .append("<td><span class='price'> " + this.price.formatMoney() + ":- </span></td>")
                                .append("<td><span class='price'> " + (this.price * info.v).formatMoney() + ":- </span></td>");
                            tr.find(".minus").click(function () {
                                container.add(x.id, -1);
                                container.load();
                            });

                            tr.find(".delete").click(function () {
                                $("body").dialog({
                                    title: "Please Confirm",
                                    data: $("<span class='info'></span>").html("Are you sure?"),
                                    onConfirm: function () {
                                        container.add(x.id, -1000);
                                        container.load();
                                    }
                                }).Show();

                            });

                            tr.find(".plus").click(function () {
                                container.add(x.id, 1);
                                container.load();
                            });
                            table.find("tbody").append(tr);

                            totalSum += this.price * info.v;
                        });
                    table.find(".totalSum").html(totalSum.formatMoney() + ":-");
                }
            });
        };

        container.count = function () {
            container.load();
            return settings.count;
        };

        container.invoice = function () {
            var result = undefined;
            container.ax({
                url: settings.get,
                async: false,
                success: function (data) {
                    result = data;
                }
            });

            return result;
        };

        container.update = function (field, value) {
            container.ax({
                url: settings.field,
                data: JSON.stringify({ field: field, value: value }),
                async: false,
                success: function (data) {
                }
            });

            return result;
        };

        container.add = function (productId, count) {
            if (!count)
                count = 1;
            $("body").ax({
                url: settings.save,
                data: JSON.stringify({ productId: productId, count: count }),
                async: false,
                success: function (data) {
                    container.load();
                }

            });
        };
        container.load();
        return container;
    };
}(jQuery));