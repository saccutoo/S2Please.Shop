
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

