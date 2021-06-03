$(document).ready(function ($) {
    $.each($('.textarea-with-counter'), function (index, element) {
        let textarea = $(element).children('textarea');
        let maxLength = textarea.attr("data-val-maxlength-max");
        let textareaSpan = $(element).children('.textarea-counter');

        textareaSpan.html(maxLength);

        textarea.keyup(function () {
            textareaSpan.html(maxLength - $(textarea).val().length);
        });
    });
});