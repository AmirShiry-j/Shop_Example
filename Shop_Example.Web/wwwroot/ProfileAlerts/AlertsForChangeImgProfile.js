
$('.img-profile').click(function () {

    if ($(this).hasClass("have")) {


        Swal.fire({
            title: 'مایل به چه انجام کاری در تصویر پروفایل خود هستید؟',
            showDenyButton: true,
            showConfirmButton: true,
            showCancelButton: true,
            confirmButtonText: 'تغییر تصویر',
            denyButtonText: 'حذف تصویر',
            cancelButtonText: 'هیچکدوم'

        }).then((result) => {

            if (result.isConfirmed) {

                UploadImage();

            } else if (result.isDenied) {

                DeleteImage();

            }
        })

    }
    else {

        UploadImage();

    }
});


function UploadImage() {

    Swal.fire({
        title: 'تنظیم تصویر پروفایل',
        showCancelButton: true,
        confirmButtonText: 'آپلود',
        howCancelButton: true,
        cancelButtonText: 'لغو',
        input: 'file',
        onBeforeOpen: () => {
            $(".swal2-file").change(function () {
                var reader = new FileReader();
                reader.readAsDataURL(this.files[0]);
            });
        }
    }).then((file) => {
        if (file.value) {

            var formData = new FormData();
            var file = $('.swal2-file')[0].files[0];
            formData.append("fileToUpload", file);
            $.ajax({
                headers: {
                    'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                },
                method: 'post',
                url: '/Profile/UploadImage',
                data: formData,
                processData: false,
                contentType: false,
                success: function (model) {

                    if (model.isSuccess == true) {

                        Swal.fire('Uploaded', 'تصویر پروفایل با موفقیت آپدیت شد', 'success');

                        $(".img-profile").attr('src', '/ProfileImages/' + model.imageSrc).addClass("have");

                        $(".avator").attr('src', '/ProfileImages/' + model.imageSrc);

                        
                    }
                    else {
                        Swal.fire({
                            type: 'error',
                            title: 'Oops...',
                            text: 'ست شدن پروفایل به مشکل برخورد'
                        })
                    }
                },
                error: function () {

                    window.location.replace("/Error");

                }
            })
        }
    })

}

function DeleteImage() {

    $.ajax({

        url: "/Profile/DeleteImage",
        method: "Get",
        success: function (result) {

            if (result == true) {

                $(".img-profile").attr('src', '/assets/images/man.png').removeClass("have");

                $(".avator").attr('src', '/assets/images/man.png');

                Swal.fire('Uploaded', 'تصویر پروفایل با موفقیت حذف شد', 'success');

            }
            else {

                Swal.fire({
                    type: 'error',
                    title: 'Oops...',
                    text: 'تصویر پروفایل حذف نشد'
                })
            }
        },
        error: function () {

            window.location.replace("/Error");
        }

    });

}
