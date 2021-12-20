function RemoveToCart(ProductId) {
    $.ajax({
        url: "/Cart/RemoveToCart/" + ProductId,
        success: function (result) {
            $(".widget-shopping-cart .mini-cart-item[ProductId-cart-row=" + ProductId + "]").fadeOut().remove();
        }
    })
}