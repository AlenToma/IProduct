function formatDate(date, incTime) {
    if (date === null)
        return "";

    if (typeof date == "string")
        date = new Date(date);
    var day = (date.getDate() <= 9 ? "0" + date.getDate() :
        date.getDate());
    var month = (date.getMonth() + 1 <= 9 ? "0" +
        (date.getMonth() + 1) : (date.getMonth() + 1));
    var dateString = day + "-" + month + "-" + date.getFullYear() + " " + (incTime ? date.getHours() + ":" + date.getMinutes() : "");

    return dateString;
}

function isNullOrEmpty(val) {
    if (!val || val === null || val === "")
        return true;
    else return false;
}


function Sort(data, column, direction) {
    if (direction == "none")
        return data;
    return data.sort(function (row, rowb) {
        var isInt = !(typeof row[column] == "string")
        var textA = row[column];
        var textB = rowb[column];
        if (!isInt) {
            if (direction == "desc")
                return textA < textB ? -1 : 1;
            else return textA < textB ? 1 : -1;
        } else {
            if (direction == "desc")
                return textB - textA;
            else return textA - textB;
        }
    });
};

function bindHtmlEditor(selector, browserUrl, change) {
    tinyMCE.remove();
    $(".mce-tinymce").remove();
    $(selector).show();
    selector = 'textarea' + selector;
    tinymce.init({
        selector: selector,
        //textareas: selector,
        height: 400,
        theme: 'modern',
        plugins: [
            'advlist autolink lists link image charmap print preview anchor',
            'searchreplace visualblocks fullscreen',
            'insertdatetime media table contextmenu',
            'textcolor colorpicker image imagetools code'
        ],
        toolbar1: 'formatselect | bold italic strikethrough forecolor backcolor |' +
            ' link | alignleft aligncenter alignright alignjustify  |' +
            ' numlist bullist outdent indent  |' +
            ' forecolor backcolor image |' +
            ' removeformat fullscreen code',
        image_advtab: true,
        templates: [
            { title: 'Test template 1', content: 'Test 1' },
            { title: 'Test template 2', content: 'Test 2' }

        ], init_instance_callback: function (editor) {
            editor.on('Change', function (e) {
                if (change)
                    change(editor);
              
            });
        },
        file_browser_callback: function (field_name, url, type, win) {
            var filebrowser = browserUrl;
            filebrowser += ((filebrowser.indexOf("?") < 0) ? "?type=" + type : "&type=" + type) + "&t=" + new Date().toString();
            tinymce.activeEditor.windowManager.open({
                title: "File Manager",
                width: 600,
                height: 600,
                url: filebrowser
            }, {
                    window: win,
                    input: field_name
                });
            return false;
        },
        content_css: [
            '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
            '//www.tinymce.com/css/codepen.min.css'
        ]
    });

    $(selector).each(function () {
        var id = $(this).attr("id");
        var html = $(this).val();
        if (!isNullOrEmpty(id))
            tinyMCE.DOM.setHTML(id, html );
    })
}