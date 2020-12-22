
function viewDetailOrder(id) {
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

//function clickAdd() {
//    loadingBody.Show();
//    $.ajax({
//        type: "POST",
//        url: "/ADMIN/Product/ShowFormAddOrderCustomer",
//        dataType: "json",
//        success: function (response) {
//            $("#modal-content-center").html(response);
//            $("#modal-center").modal("show");
//            loadingBody.Hide();
//        },
//        error: function (response, status, error) {
//            alert("Error try again");
//        }
//    });
//}

function clickAdd() {
    loadingBody.Show();
    window.location.href = "/admin/order-save/0";
}


function AddProduct(id) {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/ADMIN/Order/ShowFormAddProductToCart",
        dataType: "json",
        data: {
            productId: id
        },
        success: function (response) {
            $("#modal-content-center").html(response.html);
            $("#modal-center").modal("show");
            loadingBody.Hide();
        },
        error: function (response, status, error) {
            alert("Error try again");
        }
    });
}

function clickImgColor(id, color, img,element) {
    $('#color-name').text(color);
    $('.img-color').removeClass('active');
    $(element).addClass('active');
    var sizeId=$('.thumbnail-size.active')[0].getAttribute("size-id");
    ViewPriceDetail(id, sizeId, $('#product-id').val());
}

function clickSize(id,sizeName,element) {
    $('#size-name').text(sizeName);
    $('.thumbnail-size').removeClass('active');
    $(element).addClass('active');
    var colorId = $('.img-color.active')[0].getAttribute("color-id");
    ViewPriceDetail(colorId, id, $('#product-id').val());
}

function ViewPriceDetail(colorId,sizeId,productId) {
    $.ajax({
        type: "POST",
        url: "/ADMIN/Order/ViewPriceDetail",
        data: {
            colorId: colorId,
            sizeId: sizeId,
            productId: productId
        },
        dataType: "json",
        success: function (response) {
            $('#price-detail').html(response.html);
            $('#product-amount').text(response.AMOUNT)
        },
        error: function (response, status, error) {
            alert("Error try again");
        }
    });
}

function addToCart() {
    var colorId = $('.img-color.active')[0].getAttribute("color-id");
    var sizeId = $('.thumbnail-size.active')[0].getAttribute("size-id");
    var productId = $('#product-id').val()
    $.ajax({
        type: "POST",
        url: "/ADMIN/Order/AddProductToCart",
        data: {
            colorId: colorId,
            sizeId: sizeId,
            productId: productId
        },
        dataType: "json",
        success: function (response) {
            if (!response.result.Success) {
                toastr["error"](response.result.Message, response.result.CacheName);
            }
            else {
                $('#body-cart').html(response.result.Html);
                $("#modal-center").modal("hide");
            }
        },
        error: function (response, status, error) {
            alert("Error try again");
        }
    });
}

function updateCartAll() {
    $.ajax({
        type: "POST",
        url: "/ADMIN/Order/UpdateCartAll",
        data: $('#form-order').serializeArray(),
        dataType: "json",
        success: function (response) {
            if (!response.result.Success) {
                toastr["error"](response.result.Message, response.result.CacheName);
                $('#body-cart').html(response.result.Html);

            }
            else {
                toastr["success"](response.result.Message, response.result.CacheName);
                $('#body-cart').html(response.result.Html);
            }
        },
        error: function (response, status, error) {
            alert("Error try again");
        }
    });
}

function saveOrder() {
    loadingBody.Show();
    $("#form-order label[name*='message-error-']").text("");

    $.ajax({
        type: "POST",
        url: "/ADMIN/Order/Order",
        data: $('#form-order').serializeArray(),
        dataType: 'json',
        success: function (response) {
            if (response.Invalid) {
                var validations = response.Result;
                renderError(validations, "form-order");
                var checkCustomer = false;
                var checkCart= false;
                var statusOrder= false;

                for (var i = 0; i < validations.length; i++) {
                    if (validations[i].ColumnName.indexOf('.FULL_NAME') > -1 ||
                        validations[i].ColumnName.indexOf('.PHONE') > -1) {
                        checkCustomer = true;
                    }
                    else if (validations[i].ColumnName.indexOf('.STATUS') > -1 ||
                        validations[i].ColumnName.indexOf('.STATUS_PAY') > -1 ||
                        validations[i].ColumnName.indexOf('.METHOD_PAY') > -1 || 
                        validations[i].ColumnName.indexOf('.FEE_SHIP') > -1) {
                        statusOrder = true;
                    }
                    else if (validations[i].ColumnName.indexOf('listCard') > -1) {
                        checkCart = true;
                    }
                }

                if (checkCustomer) {
                    $("#cardOne").animate({
                        'background-color': 'red',
                    });
                    $("#cardOne").animate({
                        'background-color': 'rgba(0,0,0,.03)',
                    });
                } 

                if (checkCart) {
                    $("#cardTwo").animate({
                        'background-color': 'red',
                    });
                    $("#cardTwo").animate({
                        'background-color': 'rgba(0,0,0,.03)',
                    });
                }

                if (statusOrder) {
                    $("#cardThree").animate({
                        'background-color': 'red',
                    });
                    $("#cardThree").animate({
                        'background-color': 'rgba(0,0,0,.03)',
                    });
                }
               loadingBody.Hide();
                return;
            }
            else if (!response.result.IsPermission) {
                window.location.href = response.result.Url;
            }
            else if (response.result.Success) {
                loadingBody.Hide();
                toastr["success"](response.result.Message, response.result.CacheName);
                setTimeout(function () { window.location.href = "/admin/order" }, 2000);
            }
           
        }
    });
}

function comeBackOrder() {
    loadingBody.Show();
    window.location.href = "/admin/order";
}

function Update(id) {
    $.ajax({
        type: "POST",
        url: "/ADMIN/Order/CheckStatus",
        data: {
            id: id
        },
        dataType: 'json',
        success: function (response) {
            if (response.result.Success) {
                loadingBody.Show();
                window.location.href = "/admin/update-order/" + id;
            }
            else if (!response.result.Success) {
                toastr["error"](response.result.Message, response.result.CacheName);
            }

        }
    });
}

function updateOrder() {
    loadingBody.Show();
    $("#form-order label[name*='message-error-']").text("");

    $.ajax({
        type: "POST",
        url: "/ADMIN/Order/SaveUpdateOrder",
        data: $('#form-order').serializeArray(),
        dataType: 'json',
        success: function (response) {
            if (response.Invalid) {
                var validations = response.Result;
                renderError(validations, "form-order");
                var checkCustomer = false;
                var checkOrderDetail = false;
                var statusOrder = false;

                for (var i = 0; i < validations.length; i++) {
                    if (validations[i].ColumnName.indexOf('.FULL_NAME') > -1 ||
                        validations[i].ColumnName.indexOf('.PHONE') > -1) {
                        checkCustomer = true;
                    }
                    else if (validations[i].ColumnName.indexOf('.STATUS') > -1 ||
                        validations[i].ColumnName.indexOf('.STATUS_PAY') > -1 ||
                        validations[i].ColumnName.indexOf('.METHOD_PAY') > -1 ||
                        validations[i].ColumnName.indexOf('.FEE_SHIP') > -1) {
                        statusOrder = true;
                    }
                    else if (validations[i].ColumnName.indexOf('orderDetails') > -1) {
                        checkOrderDetail = true;
                    }
                }

                if (checkCustomer) {
                    $("#cardOne").animate({
                        'background-color': 'red',
                    });
                    $("#cardOne").animate({
                        'background-color': 'rgba(0,0,0,.03)',
                    });
                }

                if (checkOrderDetail) {
                    $("#cardTwo").animate({
                        'background-color': 'red',
                    });
                    $("#cardTwo").animate({
                        'background-color': 'rgba(0,0,0,.03)',
                    });
                }

                if (statusOrder) {
                    $("#cardThree").animate({
                        'background-color': 'red',
                    });
                    $("#cardThree").animate({
                        'background-color': 'rgba(0,0,0,.03)',
                    });
                }
                loadingBody.Hide();
                return;
            }
            else if (!response.result.IsPermission){
                window.location.href = response.result.Url;
            }
            else if (response.result.Success) {
                loadingBody.Hide();
                toastr["success"](response.result.Message, response.result.CacheName);
                setTimeout(function () { window.location.href = "/admin/order" }, 2000);
            }

        }
    });
}

function updateStatusOrder(status) {
    var listDatas = [];
    var row = $('.row-checkbox');
    if (row != null && row.length > 0) {
        for (var i = 0; i < row.length; i++) {
            if ($(row[i]).is(":checked") == true) {
                listDatas.push({ "ID": $(row[i]).val()})
            }
        }
    }
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/ADMIN/Order/UpdateStatusOrder",
        data: {
            listDatas: listDatas,
            status: status
        },
        dataType: 'json',
        success: function (response) {
            if (!response.result.IsPermission) {
                window.location.href = response.result.Url;
            }
            else if (response.result.Success) {
                reaload("Order", 1, $("#Order-paging-items-per-page").val(), "");
                toastr["success"](response.result.Message, response.result.CacheName);
            }
            else if (!response.result.Success) {
                toastr["error"](response.result.Message, response.result.CacheName);
            }
            loadingBody.Hide();
        }
    });
}

function deleteOrder(id) {  
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
        url: "/ADMIN/Order/Delete",
        data: {
            id: id
        },
        dataType: 'json',
        success: function (response) {
            loadingBody.Hide();
            if (!response.result.IsPermission) {
                window.location.href = response.result.Url;
            }
            else if (response.result.Success) {
                toastr["success"](response.result.Message, response.result.CacheName);
                $("#modal-center").modal("hide");
                reaload("Order", ControlModel["Order"].PAGE_INDEX, $('#Order-paging-items-per-page').val(), "");
            }
            else if (!response.result.Success) {
                toastr["error"](response.result.Message, response.result.CacheName);
            }

        }
    });
}

function updateStatusPay(statusPay) {
    var listDatas = [];
    var row = $('.row-checkbox');
    if (row != null && row.length > 0) {
        for (var i = 0; i < row.length; i++) {
            if ($(row[i]).is(":checked") == true) {
                listDatas.push({ "ID": $(row[i]).val() })
            }
        }
    }
    $.ajax({
        type: "POST",
        url: "/ADMIN/Order/UpdateStatusPay",
        data: {
            listDatas: listDatas,
            statusPay: statusPay
        },
        dataType: 'json',
        success: function (response) {
            loadingBody.Hide();
            if (!response.result.IsPermission) {
                window.location.href = response.result.Url;
            }
            else if (response.result.Success) {
                reaload("Order", 1, $("#Order-paging-items-per-page").val(), "");
                toastr["success"](response.result.Message, response.result.CacheName);
            }
            else if (!response.result.Success) {
                toastr["error"](response.result.Message, response.result.CacheName);
            }

        }
    });
}