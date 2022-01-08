function ConfirmComment(Id) {

    if (confirm("آیا از تایید کردن این کامنت مطمئن هستید؟") == true) {

        $.ajax({
            url: "/Admin/Comment/Confirm/" + Id,
            method: "Get"
        }).done(function (result) {

            if (result == true) {

                $("tr[comment-id='" + Id + "']").fadeOut().remove();

                alert("کامنت مورد نظر با موفقیت تایید شد");
            }
        });

    }
}