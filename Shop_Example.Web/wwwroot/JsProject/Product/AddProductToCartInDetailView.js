function AddToCard(productId) {

    $.ajax({
        url: "/Cart/AddToCart/" + productId,
        success: function (result) {
            if (result == true) {

                Swal.fire('سبد خرید', 'محصول با موفقیت به سبد خرید اضافه شد', 'success');

            }
            else {

                Swal.fire('سبد خرید', 'محصول به سبد خرید اضافه نشد', 'error');

            }
        }
    });

}