function clickAdd() {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/ADMIN/MenuAdmin/ShowFormSaveMenuAdmin",
        data: {
            id: 0
        },
        dataType: "json",
        success: function (response) {
            $("#modal-content-right").html(response.html);
            $("#modal-right").modal("show");
            loadingBody.Hide();
        },
        error: function (response, status, error) {
            alert("Error try again");
            loadingBody.Hide();
        }
    });
}

function saveMenuAdmin() {
    $("#form-menu-admin label[name*='message-error-']").text("");
    $.ajax({
        type: "POST",
        url: "/ADMIN/MenuAdmin/SaveMenuAdmin",
        data: $('#form-menu-admin').serializeArray(),
        dataType: "json",
        success: function (response) {
            if (response.Invalid) {
                var validations = response.Result;
                renderError(validations, "form-menu-admin");
                var check = false;
                for (var i = 0; i < validations.length; i++) {
                    if (validations[i].ColumnName.indexOf('.PROPERTY_VALUE') > -1) {
                        check = true;
                        break;
                    }                   
                }
                if (check) {
                    $("#cardThree").animate({
                        'background-color': 'red',
                    });
                    $("#cardThree").animate({
                        'background-color': 'rgba(0,0,0,.03)',
                    });
                }
            }
            else if (response.result.Success) {
                toastr["success"](response.result.Message, response.result.CacheName);
                $("#modal-right").modal("hide");
                window.location.reload();
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
        url: "/ADMIN/MenuAdmin/ShowFormSaveMenuAdmin",
        data: {
            id: id
        },
        dataType: "json",
        success: function (response) {
            $("#modal-content-right").html(response.html);
            $("#modal-right").modal("show");
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
        url: "/ADMIN/MenuAdmin/Delete",
        data: {id:id},
        dataType: "json",
        success: function (response) {
             if (response.result.Success) {
                toastr["success"](response.result.Message, response.result.CacheName);
                $("#modal-center").modal("hide");
                 window.location.reload();
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

function viewDetailMenuAdmin(id) {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/ADMIN/MenuAdmin/ShowFormSaveMenuAdmin",
        data: {
            id: id,
            isUpdate: false
        },
        dataType: "json",
        success: function (response) {
            $("#modal-content-right").html(response.html);
            $("#modal-right").modal("show");
            loadingBody.Hide();
        },
        error: function (response, status, error) {
            alert("Error try again");
            loadingBody.Hide();
        }
    });
}