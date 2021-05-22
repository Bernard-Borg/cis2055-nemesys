window.onload = function () {
    $(".clickable").click(function () {
        if ($(this).hasClass("starred")) {
            $(this).removeClass("starred");
            $(this).addClass("unstarred");
        } else {
            $(this).removeClass("unstarred");
            $(this).addClass("starred");
        }
    });
}