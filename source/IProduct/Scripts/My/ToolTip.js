(function ($) {
    $.toolTipIni = function () {

        function build() {
            var items = $("span[title]:not([title='']):visible,a[title]:not([title='']):visible,div[title]:not([title='']):visible,input[title]:not([title='']):visible,li[title]:not([title='']):visible");
            $(items).each(function () {
                var o = $(this);
                var title = o.attr("title");
                if (title && title != "" && !o.is(":hidden")) {
                    var offset = this.getBoundingClientRect();
                    o.attr("title", "");
                    o.addClass("tooltips");
                    var toolSpan = $("<span class='toolSpan'>" + title + "</span>");
                    toolSpan.css({
                        top: (offset.top + offset.height) + 10,
                        left: offset.left + (offset.width / 2)
                    });

                    o.append(toolSpan);
                    toolSpan.hide();
                    var timeOut = undefined;
                    o.mouseover(function (e) {
                        var offset = o[0].getBoundingClientRect();
                        clearTimeout(timeOut);
                        timeOut = setTimeout(function () {
                            toolSpan.css({
                                left: e.clientX,
                                top: e.clientY + 10
                            });
                            toolSpan.show();
                        }, 800);
                    }).mouseout(function () {
                        clearTimeout(timeOut);
                        toolSpan.hide();
                    });

                }
            });

            setTimeout(build, 100);
        }
        build();
    }

}(jQuery));