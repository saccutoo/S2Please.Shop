
function viewDetail(id) {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/ADMIN/Order/Detail",
        data: {
            id: id
        },
        dataType: "json",
        success: function (response) {
            $("#modal-content-center").html(response.html);
            $("#modal-center").modal("show");
            loadingBody.Hide();
        },
        error: function (response, status, error) {
            debugger
            alert("Error try again");
            loadingBody.Hide();

        }
    });
}


function deleteProduct(id) {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/ADMIN/Product/showFormDelete",
        data: {
            id: id
        },
        dataType: "json",
        success: function (response) {
            $("#modal-content-center").html(response);
            $("#modal-center").modal("show");
            loadingBody.Hide();
        },
        error: function (response, status, error) {
            alert("Error try again");
        }
    });
}


