$("ul.gallery-actions li .add-product-wishes").on("click", function (e) {

    e.preventDefault();

    var element = $(this);

    if (!(element.hasClass("disable"))) {
        //for delay
        element.addClass("disable");
        element.css("cursor", "not-allowed");

        var productId = element.attr("ProductId");
        var hasFavorite = element.hasClass("active");

        if (hasFavorite == false) {
            $.ajax(
                {
                    url: "/Favorite/Add/" + productId,
                    method: "Get",
                    success: function (result) {

                        if (result == true) {

                            setTimeout(function () {

                                element.addClass("active");

                                element.css("cursor", "pointer");
                                element.removeClass("disable");

                            }, 500);
                        }
                    },
                    error: function () {
                        location.replace("/Account/Login");
                    }
                });
        }
        else {
            $.ajax(
                {
                    url: "/Favorite/Remove/" + productId,
                    method: "Get",
                    success: function (result) {

                        if (result == true) {

                            setTimeout(function () {

                                element.removeClass("active");

                                element.css("cursor", "pointer");
                                element.removeClass("disable");

                            }, 500);
                        }
                    },
                    error: function () {
                        location.replace("/Account/Login");
                    }
                });
        }

    }

});