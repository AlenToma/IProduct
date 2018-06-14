(function ($) {

    $.fn.DataTableTree = function (options) {

        // This is the easiest way to have default options.
        var settings = $.extend({
            // These are the defaults.
            columns: [{ data: "name", child: "sureName" }, { data: "mail", child: "sureName" }],
            data: [{ name: "alen Toma", mail: "alen@htomail.com", children: [{ sureName: "jhshshshs" }] }, { name: "blen Toma", mail: "blen@htomail.com", children: [{ sureName: "sdf" }] }],
            width: 300,
            sort: "none",
            sortColumn: "",
            pageSize: 20,
            totalPages: undefined,
            selectedPage: undefined,
            searchText: "",
            childrenField: "children",
        }, options);

        var container = $(this);
        container.addClass("datatabletree")

        var columnWidth;


        container.firstUpperCase = function (input) {
            if (input && input.length > 1)
                return input[0].toUpperCase() + input.substr(1);
            return "	&nbsp;";
        }

        container.Sort = function (data, column, direction) {
            if (direction == "none")
                return data;
            return data.sort(function (row, rowb) {
                var textA = row[column];
                var textB = rowb[column];
                if (direction == "desc")
                    return textA < textB ? -1 : 1;
                else return textA < textB ? 1 : -1;
            });
        };

        container.BuildThead = function () {


            container.html("");
            //-------------Searchable Fields
            var ulSearchable = $("<ul><li><input type='text' placeholder='Search' /><input type='button' value='OK' /><li></ul>");
            ulSearchable.find("li").width(settings.width);
            container.append(ulSearchable);
            ulSearchable.find("input").last().click(function () {
                settings.searchText = ulSearchable.find("input").first().val();
                container.BuildData();
            });

            ulSearchable.find("input").first().keyup(function (e) {
                if (e.keyCode == 13) {
                    settings.searchText = ulSearchable.find("input").first().val();
                    container.BuildData();
                    return false;

                }
            });

            var ul = $("<ul><ul></ul></ul>");
            container.append(ul);
            $.each(settings.columns, function () {
                var value = this.data && this.data != "" ? this.data : "";
                if (!this.sort)
                    this.sort = "none"
                if (!settings.sortColumn && this.sortable != false)
                    settings.sortColumn = this.data;
                else if (settings.sortColumn == this.data)
                    this.sort = settings.sort;
                else this.sort = "none";
                ul.find("ul").first().append("<li column='" + value + "' class='th " + (this.data && this.data.length > 1 && (this.sortable != false) ? this.sort : "notsortiable") + "'>" + container.firstUpperCase(value) + "</li>")
            });

            ul.find("ul").first().find("li").css({ width: columnWidth + "%" });
            ul.find("ul").first().find("li:not(.notsortiable)").click(function () {
                if (settings.sort == "none" || settings.sort == "asc")
                    settings.sort = "desc";
                else settings.sort = "asc";
                settings.sortColumn = $(this).attr("column");
                container.BuildThead();
            });
            container.BuildData();
        }

        function GetValue(column, item) {
            var value;
            var columnName = column.value ? column.value : column.data;
            if (!$.isFunction(columnName)) {
                if (columnName && columnName != "") {
                    if (columnName.indexOf(".") != -1) {
                        if (item[columnName.split(".")[0]])
                            value= item[columnName.split(".")[0]][columnName.split(".")[1]];
                        else value= " ";
                    }
                    else value =item[columnName];
                } else value= " ";
            } else value = columnName(item);
            if (value && value !== null)
                return value;
            return " ";
        }

        function SetWidth() {
            var w = container.width();
            var colLength = settings.columns.length;
            if (w > 100) {
                var colWidth = (w / colLength) - (colLength >2 ? 11: 50);
                container.children("ul:eq(1)").find("li").width(colWidth);
                container.find(".data").find("li > ul").children("li").width(colWidth)
            } else {
                setTimeout(SetWidth, 100);
            }
        }

        container.BuildData = function () {

            container.find("ul.data").remove();
            container.append("<ul class='data'></ul>");
            if (!settings.totalPages)
                settings.totalPages = Math.ceil(settings.data.length / settings.pageSize);
            var dataArray = settings.data;
            if ($.isFunction(settings.data))
                dataArray = settings.data(settings);
            else dataArray = dataArray.slice(((settings.selectedPage - 1) * settings.pageSize), dataArray.length);


            var data = $.grep(container.Sort(dataArray, settings.sortColumn, settings.sort), function (a, i) {
                var item = undefined;
                $.each(settings.columns, function () {
                    var column = this;
                    var value = GetValue(column, a);
                    if (value && value.toString().toLowerCase().indexOf(settings.searchText.toLowerCase()) != -1)
                        item = this;
                });
                return item != undefined;

            });

            data = $.grep(data, function (a, i) {
                return i <= settings.pageSize
            });

            $.each(data, function (i,a) {
                var item = this;
                var row = $("<li><ul></ul></li>");
                $.each(settings.columns, function () {
                    var column = this;
                    var value = GetValue(column, item);
                    var col = $("<li title='" + value + "'><span>" + value + "</span></li>");
                    row.find("ul").append(col);
                    if (column.class)
                        col.find("span").addClass(column.class);

                    if (column.click)
                        col.click(function () {
                            column.click(item);
                        });

                    if (column.edit) {
                        col.find("span").addClass("edit")
                        col.find("span").click(function () {
                            column.edit(item, i);
                        });

                    }

                    if (column.delete) {
                        col.append("<span class='delete'></span>");
                        col.find("span").last().click(function () {
                            column.delete(item);
                        });
                    }

                    if (column.child && item[settings.childrenField] && item[settings.childrenField].length > 0) {
                        var value;

                        if (!row.hasClass("hasChild")) {
                            row.addClass("hasChild")
                            row.prepend("<span></span>");
                        }
                        var table = $("<table><thead><tr><th>" + container.firstUpperCase(column.child.data) + "</th></tr></thead><tbody></tbody></table>");
                        if (!column.child.data || column.child.data.length <= 0)
                            table.find("thead").css("visibility", "hidden");
                        $.each(item[settings.childrenField], function () {
                            var tr = $("<tr><td><span>" + GetValue(column.child, this) + "</span></td></tr>");
                            var tm = this;
                            if (column.child.edit) {
                                tr.find("td").find("span").addClass("edit")
                                tr.find("td").find("span").click(function () {
                                    column.child.edit(tm);
                                });

                            }

                            if (column.child.delete) {
                                tr.find("td").append("<span class='delete'></span>");
                                tr.find("td").find("span").last().click(function () {
                                    column.child.delete(tm);
                                });
                            }
                            table.find("tbody").append(tr);
                        })
                        col.append(table);

                    }

                });

                if (row.hasClass("hasChild"))
                    row.children("span").click(function (e) {
                        if (!row.hasClass("show")) {
                            row.addClass("show");
                        } else {
                            row.removeClass("show");
                        }

                    });

                container.find(".data").append(row);
                //row.find("ul li").css({ width: columnWidth + "%" });
            });

            container.BuildPager();
     
            SetWidth();
        }

        container.BuildPager = function () {
            container.find(".pager").remove();
            if (settings.totalPages <= 0) // dont build
                return;

            if (!settings.selectedPage || settings.selectedPage > settings.totalPages)
                settings.selectedPage = 1;
            container.append("<ul class='pager'></ul>");

            var start = settings.selectedPage;
            var end = settings.selectedPage;
            var counter = 3;
            //If the page cannot be devised by 5 enter the loop
            if ((start % counter != 0) && (end % counter != 0)) {
                //Get the next nearest page that is divisible by 5
                while ((end % counter) != 0) {
                    end++;
                }
                //Get the previous nearest page that is divisible by 5
                while ((start % counter) != 0) {
                    start--;
                }

            }
            //The page is divisible by 5 so get the next 5 pages in the pagination
            else {
                end += counter;
            }
            //We are on the first page
            if (start == 0) {
                start++;
                end++;
            }



            //We are on the last page
            if (end == settings.totalPages || end > settings.totalPages) {
                end = settings.totalPages;
            }

            if (start == settings.selectedPage && start > 1)
                start--;

            while (end - start < counter && end - start > 0)
                start--;


            if (start <= 0)
                start = 1;

            while (end - start > counter)
                end--;

            for (var i = start; i <= end; i++)
                container.find(".pager").append("<li class='" + (settings.selectedPage == i ? "selected" : "") + "' pageNumber='" + i + "'>" + i + "</li>");


            container.find(".pager").prepend("<li class='prev' pageNumber='" + (settings.selectedPage - 1) + "' >Prev</li>");

            container.find(".pager").append("<li class='next' pageNumber='" + (settings.selectedPage + 1) + "' >Next</li>");

            if (settings.selectedPage <= 1)
                container.find(".pager").find(".prev").addClass("disabled");

            if (settings.selectedPage >= settings.totalPages)
                container.find(".pager").find(".next").addClass("disabled");

            container.find(".pager").find("li:not('.selected'):not('disabled')").click(function () {
                settings.selectedPage = parseInt($(this).attr("pageNumber"));
                container.BuildData();
            });

        }
        $(window).bind("resize", function () { SetWidth() });
        container.BuildThead();

        return container;
    };

}(jQuery));