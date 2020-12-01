$(document).on("change", "#role", function (e) {
    $.ajax({
        type: "POST",
        url: "/ADMIN/Permission/ShowFormSavePermissonRole",
        data: {
            id: $("#role").val()
        },
        dataType: "json",
        success: function (response) {
            $("#permission-role").html(response.html);
            loadingBody.Hide();
        },
        error: function (response, status, error) {
            alert("Error try again");
            loadingBody.Hide();
        }
    });
});