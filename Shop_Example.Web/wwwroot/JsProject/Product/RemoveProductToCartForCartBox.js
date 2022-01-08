function RemoveFavoriteResponseBig(ProductId) {

    $.ajax({

        url: "/Favorite/Remove/" + ProductId,
        method: "Get"

    }).done(function (result) {

        if (result == true) {


            $(".table-favorites table tbody tr[product-id='" + ProductId + "']").remove();


        }

    });

}

function RemoveFavoriteResponseSmall(ProductId) {

    $.ajax({

        url: "/Favorite/Remove/" + ProductId,
        method: "Get"

    }).done(function (result) {

        if (result == true) {


            $("div[product-id='" + ProductId + "']").remove();


        }

    });

}