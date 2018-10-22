(function ($) {

	$.fn.treeView = function (options) {

		// This is the easiest way to have default options.
		var settings = $.extend({
			// These are the defaults.
			columns: [{ data: "name", child: "sureName", header: "Name" }, { data: "mail", child: "sureName", header: "Email" }],
			data: [{ name: "alen Toma", mail: "alen@htomail.com", children: [{ sureName: "jhshshshs" }] }, { name: "blen Toma", mail: "blen@htomail.com", children: [{ sureName: "sdf" }] }],
			width: 300,
			sort: "asc",
			sortColumn: "",
			pageSize: 20,
			totalPages: 1,
			selectedPage: 1,
			searchText: "",
			childrenField: "children",
			primaryKey: "id",
			onEdit: function () { },
			onDelete: function () { },
			buttons: [],// dynamic buttons,
			header: "",
			searchEnable: true
		}, options);
		var visibleColumns = 0;
		var lastColumn;
		class Dictionary {
			constructor() {
				this.items = {};
			}
			has(key) {
				return key in this.items;
			}
			set(key, value) {
				this.items[key] = value;
			}
			delete(key) {
				if (this.has(key)) {
					delete this.items[key];
					return true;
				}
				return false;
			}
		}
		var expandedItems = {};
		$(this).html("");
		var item = {};
		var table = $("<table class='tableView'><thead><tr></tr><tr></tr><tr></tr></thead><tbody></tbody><tfoot></tfoot></table>");
		$(this).append(table);


		item.sort = function (data, column, direction) {
			if (direction === "none" || !direction || direction === "" || !column || column.length <= 1)
				return data;
			return data.sort(function (row, rowb) {
				var textA = row[column];
				var textB = rowb[column];
				if (direction === "desc")
					return textA < textB ? -1 : 1;
				else return textA < textB ? 1 : -1;
			});
		};

		item.expanded = function (bg) {
			var keyString = bg.attr("key");
			var key = expandedItems[keyString];
			if (key === true) {
				bg.find("i").removeClass("desc").removeClass("asc").addClass("asc");
				table.find("table[key='" + keyString + "']").show();
			} else {
				table.find("table[key='" + keyString + "']").hide();
				bg.find("i").removeClass("desc").removeClass("asc").addClass("desc");

			}
		};


		item.databind = function () {
			table.children("tbody").find(".bg").each(function () {
				var td = $(this).parent();
				if (td.find("table > tbody> tr").length <= 0) {
					$(this).find("i").hide();
				} else {

					$(this).click(function () {
						var keystring = $(this).attr("key");
						var key = expandedItems[keystring];


						if (key)
							expandedItems[keystring] = false;
						else {
							expandedItems[keystring] = true;
						}
						item.expanded($(this));

					});
					item.expanded($(this));
				}

			});

			table.children("tbody").find("td> span").click(function () {
				table.children("tbody").find("span").attr("style", "");
				$(this).css("background-color", "rgba(204, 204, 204, 0.38)");
			});
		};

		item.pager = function () {
			table.children("tfoot").html("");
			if (settings.totalPages <= 0) // dont build
				return;

			if (!settings.selectedPage || settings.selectedPage > settings.totalPages)
				settings.selectedPage = 1;
			table.children("tfoot").append("<tr><td colspan='" + visibleColumns + "'></td></tr>").find("td").append("<ul class='pager'></ul>");

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
				table.children("tfoot").find(".pager").append("<li class='" + (settings.selectedPage == i ? "selected" : "") + "' pageNumber='" + i + "'>" + i + "</li>");


			table.children("tfoot").find(".pager").prepend("<li class='prev' pageNumber='" + (settings.selectedPage - 1) + "' >Prev</li>");

			table.children("tfoot").find(".pager").append("<li class='next' pageNumber='" + (settings.selectedPage + 1) + "' >Next</li>");

			if (settings.selectedPage <= 1)
				table.children("tfoot").find(".pager").find(".prev").addClass("disabled");

			if (settings.selectedPage >= settings.totalPages)
				table.children("tfoot").find(".pager").find(".next").addClass("disabled");

			table.children("tfoot").find(".pager").find("li:not('.selected'):not('.disabled')").click(function () {
				settings.selectedPage = parseInt($(this).attr("pageNumber"));
				item.body();
			});
		};

		item.ajest = function () {
			table.children("tbody").find(".action").each(function () {
				var height = 8;
				$(this).parent().children("td:not(.action)").each(function () { if ($(this).height() > height) height = $(this).height(); });
				$(this).css("min-height", height);
			});
		};

		item.build = function () {
			var timeout = undefined;
			item.body = function () {
				clearTimeout(timeout);
				timeout = setTimeout(function () {
					var tempSettings = {
						sort: settings.sort,
						sortColumn: settings.sortColumn,
						pageSize: settings.pageSize,
						totalPages: settings.totalPages,
						selectedPage: settings.selectedPage,
						searchText: settings.searchText
					};
					var data = settings.data(tempSettings);
					if (data && data.totalPages !== undefined) {
						settings.totalPages = data.totalPages;
						data = data.result;
					}

					data = item.sort(data, settings.sortColumn, settings.sort);
					table.children("tbody").html("");
					function getValue(col, object, asString) {
						var v = undefined;
						if (typeof col === "string") {
							var colSplitter = col.split(".");
							if (colSplitter.length <= 1) {
								v = object[col];
							} else {
								for (i = 0; i <= colSplitter.length - 1; i++) {
									if (v === undefined) {
										v = getValue(colSplitter[i], object);
										if (!v || v === null)
											break;
									} else {
										v = getValue(colSplitter[i], v);
									}
								}
							}

						}
						else v = col(object);

						return v === undefined || v === null || v === "" ? (asString ? "&#160;" : "") : v;
					}

					$.each(data, function () {
						var isFirstColumn = true;
						var value = this;
						var tr = $("<tr/>");
						tr.prepend("<td key='" + this[settings.primaryKey] + "' class='bg'><i></i></td>");
						$.each(settings.columns, function () {
							var column = this;
							var td = $("<td/>").append("<span>" + getValue(column.data, value, true) + "</span>");
							var getChildren = function (parent, data) {
								var children = undefined;
								var col = column.child ? column.child : column.data;
								if (col && settings.childrenField)
									children = getValue(settings.childrenField, data);

								if (children && children.length > 0) {
									var tb = $("<table><tbody></tbody></table>");
									var padding = 4;
									$.each(children, function () {
										var object = this;
										padding += 4;
										var subTr = $("<tr/>");
										var subTd = $("<td><span>" + getValue(col, this, true) + "</span></td>");
										subTr.append(subTd);
										var subChildren = getValue(settings.childrenField, this);
										if (subChildren && subChildren.length > 0) {
											subTd.append(tb);
											getChildren(tb, this);
											tb.attr("key", this[settings.primaryKey]);
										}
										if (isFirstColumn) {
											subTr.prepend("<td key='" + this[settings.primaryKey] + "' class='bg'><i></i></td>");
										} else subTr.find("td").css("padding-left", padding);
										parent.children("tbody").append(subTr);
										if ((settings.onEdit || settings.onDelete) && lastColumn.data === column.data) {
											var actions = $("<td class='action'><span class='edit'></span><span class='delete'></span></td>");
											subTr.append(actions);
											actions.find(".delete").click(function () {
												settings.onDelete(object);
											});
											actions.find(".edit").click(function () {
												settings.onEdit(object);
											});
										}
									});


								}
							};

							var childrenTable = $("<table key='" + value[settings.primaryKey] + "'><tbody></tbody></table>");
							getChildren(childrenTable, value);
							td.append(childrenTable);
							tr.append(td);
							if ((settings.onEdit || settings.onDelete) && lastColumn.data === column.data) {
								var actions = $("<td class='action'><span class='edit'></span><span class='delete'></span></td>");
								actions.find(".delete").click(function () {
									settings.onDelete(value);
								});
								actions.find(".edit").click(function () {
									settings.onEdit(value);
								});
								tr.append(actions);
							}

							isFirstColumn = false;
						});
						table.children("tbody").append(tr);
					});
					item.pager();
					item.databind();
					item.ajest();
				}, 50);
			};


			item.header = function () {
				table.find("thead>tr").html("");
				var body = table.find("tbody").html("");
				visibleColumns = 0;
				if (table.find("thead").find("th").length <= 0) {
					$.each(settings.columns, function () {
						lastColumn = this;
						visibleColumns++;
						var column = this;
						var th = $("<th></th>")
							.append($("<table><tbody><tr><td><span>" + this.header + "</span></td><td class='bg'><i></i></td></tr></tbody>"));
						if (column.sortable === false) {
							th.find(".bg i").remove();
						}
						if (this.data === settings.sortColumn) {
							if (settings.sort === "desc")
								th.find(".bg>i").addClass("desc");
							else th.find(".bg>i").addClass("asc");
						}

						th.find("i").click(function () {

							if ($(this).hasClass("desc")) {
								settings.sort = "asc";
							} else if ($(this).hasClass("asc")) {
								settings.sort = "desc";
							} else settings.sort = "asc";
							settings.selectedPage = 1;
							settings.sortColumn = column.data;
							item.header();

						});
						if (visibleColumns <= 1) {
							table.children("thead").find("tr:eq(1)").append("<th class='bg'><i></i></th>");
							visibleColumns++;
						}
						table.children("thead").find("tr:eq(1)").append(th);
					});
					if (settings.onEdit || settings.onDelete) {
						table.children("thead").find("tr:eq(1)").append("<th class='haction'></th>");
						visibleColumns++;
					}
					var searchView = $("<th colspan='" + (visibleColumns) + "'></th>").append("<input type='text' placeholder='Search' value='" + settings.searchText + "' />");
					table.children("thead").children("tr:last-child").append(searchView);

					if (!settings.searchEnable)
						table.children("thead").children("tr:last-child").hide();
					if (settings.header.length > 0 || settings.buttons.length > 0) {
						var tableHeader = $("<th colspan='" + (visibleColumns) + "'><h1>" + settings.header + "</h1></th>");
						$.each(settings.buttons, function () {
							var btn = $("<button class='tableBtn'>" + this.text + " </button>");
							btn.click(this.click);
							tableHeader.prepend(btn);
						});
						table.children("thead").children("tr:first-child").append(tableHeader);
					} else table.children("thead").children("tr:first-child").hide();
				}

				item.body();

				table.children("thead").find("input").keyup(function (e) {
					//See notes about 'which' and 'key'
					if (e.keyCode === 13) {
						settings.searchText = $(this).val();
						item.body();
						return false;
					}
				});
			};
			item.header();
		};
		item.build();
		return item;
	};

}(jQuery));