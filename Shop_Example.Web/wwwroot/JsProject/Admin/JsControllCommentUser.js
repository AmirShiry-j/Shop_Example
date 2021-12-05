function BlockUser(UserId, Id) {

    if (confirm("از بلاک کردن (برای کامنت گذاشتن) این کاربر مطمئن هستید؟") == true) {

        $.ajax({
            url: "/Admin/Comment/BlockUser/" + UserId,
            method: "Get",
        }).done(function (result) {

            if (result == true) {

                alert("کاربر مورد نظر بلاک شد");

                window.location.reload();
            }
        });

    }
}

function UnBlockUser(UserId, Id) {


    if (confirm("آیا میخواید این کاربر را از حالت بلاک دربیارید؟") == true) {

        $.ajax({
            url: "/Admin/Comment/UnBlockUser/" + UserId,
            method: "Get"
        }).done(function (result) {

            if (result == true) {

                alert("کاربر از حالت بلاک در اومد");

                window.location.reload();

            }
        });

    }
}