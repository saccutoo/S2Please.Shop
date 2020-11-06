$(document).on('ready', function () {
    $(".regular").slick({
        dots: false,
        infinite: true,
        centerMode: false,
        slidesToShow: 4,
        slidesToScroll: 1
    });

    
});

$('.horver-img-slide').hover(function () {
    var url = this.getAttribute('href');
    if (url.indexOf(".mp4") > -1) {
        return;
    }
    else {
        $('#image-main').attr("src", url);

    }
})   

$(document).on('click', '.img-color', function () {
    $('.img-color').removeClass('active');
    $(this).addClass('active');
});

$(document).on('click', '.thumbnail-size', function () {
    $('.thumbnail-size').removeClass('active');
    $(this).addClass('active');
});

function clickImgColor(id, color, img) {
    $('#color-id').val(id);
    $('#image-main').attr("src", "/Image/"+img);
    $('#text-color').text(color);
    $('#product-color').text(color);
   
    var model = {
        colorID: id,
        sizeID: $('#size-id').val(),
        productId: $('#product-id').val(),
    }
    $.ajax({
        type: "POST",
        url: "/WEB_SHOP/Product/GetPrice",
        data: model,
        dataType: "json",
        success: function (response) {
            $('#product-price').html(response.Html);
            $('#product-amount').text(response.ProductMapper.AMOUNT);
            $('#text-color').text(color);
        },
        error: function (response, status, error) {
            alert("Error try again");
        }
    });
}


function clickSize(id) {
    $('#size-id').val(id);
    var model = {
        colorID:  $('#color-id').val(),
        sizeID: id,
        productId: $('#product-id').val(),
    }
    $.ajax({
        type: "POST",
        url: "/WEB_SHOP/Product/GetPrice",
        data: model,
        dataType: "json",
        success: function (response) {
            $('#product-price').html(response.Html);
            $('#product-amount').text(response.ProductMapper.AMOUNT);

        },
        error: function (response, status, error) {
            alert("Error try again");
        }
    });
}


//thêm vào giỏ hàng
$('#btn-add-cart').click(function () {
    loadingBody.Show();
    var cart = {
        PRODUCT_ID: $('#product-id').val(),
        COLOR_ID: $('#color-id').val(),
        SIZE_ID: $('#size-id').val(),
        AMOUNT: $('#value-input-amount').val(),
    }
    $.ajax({
        type: "POST",
        url: "/WEB_SHOP/Cart/AddCart",
        data: cart,
        dataType: "json",
        success: function (response) {
            if (response.Success) {
                toastr["success"](response.Message, response.CacheName)
                $("#total-cart").text("(" + response.Total+")");
            }
            else {
                toastr["error"](response.Message, response.CacheName)
            }
            loadingBody.Hide();

        },
        error: function (response, status, error) {
            alert("Error try again");
            loadingBody.Hide();
        }
    });
})

