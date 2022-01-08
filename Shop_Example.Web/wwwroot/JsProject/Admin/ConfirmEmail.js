function ConfirmEmail(UserId) {

    if (confirm("از تایید ایمیل کاربر به صورت دستی مطمئن هستید؟") == true) {

        $.ajax({
            url: "/Admin/User/ConfirmEmailUser/" + UserId,
            method: "Get",
        }).done(function (result) {

            if (result == true) {

                alert("ایمیل کاربر به صورت دستی از طرف شما تایید شد");

                window.location.reload();
            }
        });

    }
}

function UnConfirmEmail(UserId) {

    if (confirm("آیا میخواهید ایمیل کاربر رو به صورت دستی رد تایید کنید؟ (کاربر میتونه باز ایمیل خودشو از طریق سرویس سایت به تایید برسونه)") == true) {

        $.ajax({
            url: "/Admin/User/UnConfirmEmailUser/" + UserId,
            method: "Get"
        }).done(function (result) {

            if (result == true) {

                alert("ایمیل کاربر به صورت دستی از طرف شما رد تایید شد ");

                window.location.reload();

            }
        });

       
    }
}