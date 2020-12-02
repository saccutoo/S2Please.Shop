

$(document).on("change", "#role", function (e) {
    loadingBody.Show();
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

$(document).on("change", "#user", function (e) {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/ADMIN/Permission/ShowFormSavePermissonUser",
        data: {
            id: $("#user").val()
        },
        dataType: "json",
        success: function (response) {
            $("#permission-user").html(response.html);
            loadingBody.Hide();
        },
        error: function (response, status, error) {
            alert("Error try again");
            loadingBody.Hide();
        }
    });
});

function savePermissionRole() {
    var data = [];
    var checkbox = $('.check-box-is-save');
    if (checkbox != null && checkbox.length > 0) {
        for (var i = 0; i < checkbox.length; i++) {
            if ($(checkbox[i]).is(":checked")) {
                var obj = {
                    EMPLOYEE_ROLE_PERMISSON_ID: $(checkbox[i])[0].getAttribute("employes-role-permission-id"),
                    MENU_ID: $(checkbox[i])[0].getAttribute("menu-id"),
                    MENU_PERMISSION_ID: $(checkbox[i])[0].getAttribute("menu-permission-id")
                }
                data.push(obj);
            }
        }
    }
    $.ajax({
        type: "POST",
        url: "/ADMIN/Permission/SavePermissionRole",
        data: {
            datas: data,
            roleId: $("#role").val()
        },
        dataType: "json",
        success: function (response) {
           if (response.result.Success) {
                toastr["success"](response.result.Message, response.result.CacheName);
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
            loadingBody.Hide();
        }
    });
}


function savePermissionUser() {
    var data = [];
    var checkbox = $('.check-box-is-save');
    if (checkbox != null && checkbox.length > 0) {
        for (var i = 0; i < checkbox.length; i++) {
            if ($(checkbox[i]).is(":checked")) {
                var obj = {
                    EMPLOYEE_ROLE_PERMISSON_ID: $(checkbox[i])[0].getAttribute("employes-role-permission-id"),
                    MENU_ID: $(checkbox[i])[0].getAttribute("menu-id"),
                    MENU_PERMISSION_ID: $(checkbox[i])[0].getAttribute("menu-permission-id")
                }
                data.push(obj);
            }
        }
    }
    $.ajax({
        type: "POST",
        url: "/ADMIN/Permission/SavePermissionUser",
        data: {
            datas: data,
            userId: $("#user").val()
        },
        dataType: "json",
        success: function (response) {
            if (response.result.Success) {
                toastr["success"](response.result.Message, response.result.CacheName);
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
            loadingBody.Hide();
        }
    });
}