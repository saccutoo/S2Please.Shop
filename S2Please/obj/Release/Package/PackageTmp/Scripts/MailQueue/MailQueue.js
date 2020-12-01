function saveMailQueue() {
    $("#form-mail-queue label[name*='message-error-']").text("");
    $.ajax({
        type: "POST",
        url: "/ADMIN/MailQueue/SaveMailQueue",
        data: $('#form-mail-queue').serializeArray(),
        dataType: "json",
        success: function (response) {
            if (response.Invalid) {
                var validations = response.Result;
                renderError(validations, "form-mail-queue");               
            }
            else if (response.result.Success) {
                toastr["success"](response.result.Message, response.result.CacheName);
                $("#modal-center").modal("hide");
                reaload("MailQueue", ControlModel["Menu"].PAGE_INDEX, $('#MailQueue-paging-items-per-page').val(), "");
            }           
            else if (!response.result.IsPermission) {
                window.location.href = response.result.Url;
            }
            else if (!response.result.Success && response.result.IsPermission) {
                toastr["error"](response.result.Message, response.result.CacheName);
            }
            loadingBody.Hide();
        },
        error: function (response, status, error) {
            alert("Error try again");
        }
    });
}

function Update(id) {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/ADMIN/MailQueue/ShowFormSaveMailQueue",
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
            alert("Error try again");
            loadingBody.Hide();
        }
    });
}

function deleteMenu(id) {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/Base/ShowFormDelete",
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

function saveDelete(id) {
    $.ajax({
        type: "POST",
        url: "/ADMIN/MailQueue/Delete",
        data: {id:id},
        dataType: "json",
        success: function (response) {
             if (response.result.Success) {
                toastr["success"](response.result.Message, response.result.CacheName);
                $("#modal-center").modal("hide");
                 reaload("MailQueue", ControlModel["MailQueue"].PAGE_INDEX, $('#MailQueue-paging-items-per-page').val(), "");
            }
            else if (!response.result.IsPermission) {
                window.location.href = response.result.Url;
            }
            else if (!response.result.Success && response.result.IsPermission) {
                toastr["error"](response.result.Message, response.result.CacheName);
            }
            loadingBody.Hide();
        },
        error: function (response, status, error) {
            alert("Error try again");
        }
    });
}

function viewDetailMailQueue(id,dataId,dataType) {
    loadingBody.Show();
    var url = "";
    if (dataType='Order') {
        url = "/ADMIN/Order/Detail";
    }
    $.ajax({
        type: "POST",
        url: url,
        data: {
            id: dataId,
            isUpdate: false
        },
        dataType: "json",
        success: function (response) {
            $("#modal-content-center").html(response.html);
            $("#modal-center").modal("show");
            loadingBody.Hide();
        },
        error: function (response, status, error) {
            alert("Error try again");
            loadingBody.Hide();
        }
    });
}