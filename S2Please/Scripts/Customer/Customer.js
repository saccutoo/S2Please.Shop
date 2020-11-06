
function updateCustomer() {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/WEB_SHOP/User/UpdateCustomer",
        data: $('#form-customer').serializeArray(),
        success: function (response) {
            if (response.result.Success) {
                toastr["success"](response.result.CacheName, response.result.Message);
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
            }
            else {
                toastr["error"](response.result.CacheName, response.result.Message);
            }
        }
    });
}

function viewOrderDetail(id) {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/WEB_SHOP/User/ViewOrderDetail",
        data: {
            id:id
        },
        success: function (response) {
            $("#my-modal-content-center").html(response.html)
            $("#my-modal-center").modal("show");
            loadingBody.Hide();
        }
    });
}

// model remove
function viewModalRemove(id, link) {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/Base/Remove",
        data: {
            id: id,
            linkRemove: link
        },
        success: function (response) {
            $("#my-modal-content-center").html(response.html)
            $("#my-modal-center").modal("show");
            loadingBody.Hide();
        }
    });
}

function remove(id, link) {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: link,
        data: {
            id: id
        },
        success: function (response) {
            if (response.result.Success) {
                $("#my-modal-center").modal("hide");
                toastr["success"](response.result.CacheName, response.result.Message);
                setTimeout(function () {
                    window.location.reload();
                }, 1000);

            }
            else {
                toastr["error"](response.result.CacheName, response.result.Message);
            }

        }
    });
}
//function viewModalUpdate(id, link) {
//    loadingBody.Show();
//    $.ajax({
//        type: "POST",
//        url: link,
//        data: {
//            id: id
//        },
//        success: function (response) {
//            $("#my-modal-content-center").html(response.html)
//            $("#my-modal-center").modal("show");
//            loadingBody.Hide();
//        }
//    });
//}

function comeBack() {
    window.location.href = "/customer";
}


function updateOrder() {
    loadingBody.Show();
    $("#form-update-order label[name*='message-error-']").text("");
    $.ajax({
        type: "POST",
        url: "/WEB_SHOP/User/UpdateOrder",
        data: $('#form-update-order').serializeArray(),
        success: function (response) {
            if (response.Invalid) {
                var validations = response.Result;
                renderError(validations, "form-update-order");
                loadingBody.Hide();
            }
            else if (response.result.Success) {
                toastr["success"](response.result.CacheName, response.result.Message);
                loadingBody.Hide();
                setTimeout(function () { window.location.href = "/customer"; }, 2000);

            }
            else {
                toastr["error"](response.result.CacheName, response.result.Message);
                loadingBody.Hide();

            }
        }
    });

}