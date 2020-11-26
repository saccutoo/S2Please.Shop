function clickAdd() {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/ADMIN/ProductType/ShowFormSaveProductType",
        data: {
            id: 0
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

function saveProductType() {
    $("#product-type label[name*='message-error-']").text("");
    $.ajax({
        type: "POST",
        url: "/ADMIN/ProductType/SaveProductType",
        data: $('#product-type').serializeArray(),
        dataType: "json",
        success: function (response) {
            if (response.Invalid) {
                var validations = response.Result;
                renderError(validations, "product-type");
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
                $("#modal-center").modal("hide");
                reaload("ProductType", ControlModel["ProductType"].PAGE_INDEX, $('#ProductType-paging-items-per-page').val(), "");
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
        url: "/ADMIN/ProductType/ShowFormSaveProductType",
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

function deleteProductType(id) {
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
        url: "/ADMIN/ProductType/Delete",
        data: {id:id},
        dataType: "json",
        success: function (response) {
             if (response.result.Success) {
                toastr["success"](response.result.Message, response.result.CacheName);
                $("#modal-center").modal("hide");
                reaload("ProductType", ControlModel["ProductType"].PAGE_INDEX, $('#ProductType-paging-items-per-page').val(), "");
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

function viewDetailProductType(id) {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/ADMIN/ProductType/ShowFormSaveProductType",
        data: {
            id: id,
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