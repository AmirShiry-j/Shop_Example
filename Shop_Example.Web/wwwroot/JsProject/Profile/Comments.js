function DeleteComment(CommentId) {

    Swal.fire({

        title: 'توجه',
        text: "آیا از حذف کردن کامنت خود برای این محصول مورد نظر مطمئن هستید؟",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: 'خیر',
        confirmButtonText: 'بله, حذف شه'

    }).then((result) => {


        if (result.value == true) {

            $.ajax({
                url: "/Comment/Remove/" + CommentId,
                method: "Get"
            }).done(function (res) {
                if (res) {


                    Swal.fire(
                        'Deleted!',
                        'کامنت شما با موفقیت حذف شد',
                        'success'
                    ).then(function () {

                        location.reload();

                    });

                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'عملیات ناموق بود و ظاهرا مشکلی وجود داره',
                        footer: '<a href="">Why do I have this issue?</a>'
                    })
                }
            });
        }
        else if (result.isDenied) {

        }
    });


}
