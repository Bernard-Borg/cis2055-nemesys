window.onload = function () {

    //Adds click event handler to make an AJAX request to star when the star is clicked
    $(".clickable").click(function () {
        let star = $(this);

        //Might be undefined if user deletes the attribute
        if (star.attr("reportId") != undefined) {
            let id = parseInt(star.attr("reportId"));

            //NaN might happen if user changes reportId attribute to something which isn't a number
            if (!isNaN(id)) {
                let dataToSend = {
                    reportId: id
                }

                //Sends ajax request
                $.ajax({
                    type: 'POST',
                    url: '/Home/Star',
                    data: dataToSend,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            //If StarReport finished successfully, update the UI
                            if (star.hasClass("starred")) {
                                star.removeClass("starred").addClass("unstarred");
                            } else {
                                star.removeClass("unstarred").addClass("starred");
                            }
                        }
                    }
                });
            } else {
                alert("Don't mess with the code!");
            }
        } else {
            alert("Don't mess with the code!");
        }
    });
}