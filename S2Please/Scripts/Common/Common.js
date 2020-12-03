
//thay đổi ngon ngữ hiển thị
function changeLanguage() {
    $.ajax({
        type: "POST",
        url: "/Base/ChangeLanguage",
        data: {
            languageId: $('#language').val()
        },
        dataType: "json",
        success: function (msg) {
            if (msg) {
                window.location.reload();
            }
        },
        error: function (req, status, error) {
            alert("Error try again");
        }
    });
}

//prev slide sản phẩm
function clickPrev(productTypeId, url) {
    var myCarousel = "#myCarousel_" + productTypeId;
    var prevId = "#prev_" + productTypeId;
    var nextId = "#next_" + productTypeId;

}

//next slide sản phẩm
function clickNext(productTypeId, url) {
    var myCarousel = "#myCarousel_" + productTypeId;
    var prevId = "#prev_" + productTypeId;
    var nextId = "#next_" + productTypeId;
    var nextIndex = $(myCarousel)[0].getAttribute('next-index');
    var pageSlide = parseInt($(myCarousel + ' .item.active')[0].getAttribute('silde-index'));
    if (nextIndex == pageSlide) {
        var model = {
            PAGE_NUMBER: parseInt($(myCarousel)[0].getAttribute('page-number')) + 1,
            PAGE_SIZE: parseInt($(myCarousel)[0].getAttribute('number-product-get')),
            VALUE: productTypeId,
            STRING_FILTER: pageSlide
        }
        $.ajax({
            type: "POST",
            url: url,
            data: model,
            dataType: "json",
            success: function (response) {
                var carouselInner = "#carousel-inner-" + productTypeId;
                $(carouselInner).append(response);
                $(myCarousel).attr('page-number', (parseInt($(myCarousel)[0].getAttribute('page-number')) + 1).toString());
                $(myCarousel).attr('next-index', (parseInt($(myCarousel)[0].getAttribute('next-index')) + 3).toString());

            },
            error: function (response, status, error) {
                alert("Error try again");
            }
        });
    }
}

//khởi tạo loading body html
var loadingBody = {};

loadingBody.Show=function(){
    $("#body").LoadingOverlay("show", {
        //background: "rgba(165, 190, 100, 0.5)",
        //image: "/Image/Áo.jpg",
        image: "",
        fontawesome: "fas fa-biohazard fa-spin",
        //text: "Loading...",
        maxSize: 50
    }) ;
}


loadingBody.Hide = function () {
    $("#body").LoadingOverlay("hide");
}

function viewDetail(id) {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/ADMIN/Product/Detail",
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
//search sản phẩm
//$("#autocomplete-search").select2({
//    ajax: {
//        url: "/WEB_SHOP/Search/SearchProduct",
//        type: "POST",
//        data: function (params) {

//            var queryParameters = {
//                searchString: params.term
//            }
//            return queryParameters;
//        },
//        processResults: function (data) {
//            return {
//                results: $.map(data.data, function (item) {
//                    return {
//                        text: item.NAME,
//                        id: item.PRODUCT_KEY
//                    }
//                })
//            };
//        }
//    }
//});

//$('#autocomplete-search').on('select2:selecting', function (e) {
//    window.location.href = "/WEB_SHOP/Product/Detail?toDecrypt=" + e.params.args.data.id;
//})

var input = document.getElementById("autocomplete-search");

autocomplete({
    input: input,
    fetch: function (text, update) {
        text = text.toLowerCase();
        $.ajax({
            type: "POST",
            url: "/WEB_SHOP/Search/SearchProduct",
            data: {
                searchString: text
            },
            dataType: "json",
            success: function (response) {
                var suggestions = [];
                if (response != null && response.data != null && response.data.length>0) {
                    for(var i=0;i< response.data.length;i++){
                        suggestions.push({ label: response.data[i].NAME, value: response.data[i].PRODUCT_KEY })
                    }
                }
                update(suggestions);

            },
            error: function (response, status, error) {
                alert("Error try again");
            }
        });
       
    },
    onSelect: function (item) {
        window.location.href = "/Product-Detail/" + item.value;
    }
});


function searchProduct(searchKey) {
    if (event.which == 13) {
        window.location.href = "/WEB_SHOP/Product/Products?textSearch=" + searchKey;
    }
}


//khởi tại toastr js

toastr.options = {
    "closeButton": true,
    "debug": true,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": true,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

//render error

function renderError(validations, formName) {
    for (var i = 0; i < validations.length; i++) {
        var elementName = validations[i].ColumnName;
        if (elementName != "" && elementName != undefined && elementName != null) {
            var element = $("#" + formName + " label[name='message-error-" + elementName + "']");
            $(element).text(validations[i].ErrorMessage);
        }
    }
}


function showChat() {
    $("#page-content").show();
    $('.font-lato').hide();
}

function hideChat() {
    $("#page-content").hide();
    $('.font-lato').show();
}

function startChat() {
    $("#form-information-chat label[name*='message-error-']").text("");

    $.ajax({
        type: "POST",
        url: "/Base/StartChat",
        data: $('#form-information-chat').serializeArray(),
        dataType: "json",
        success: function (response) {
            if (response.Invalid) {
                var validations = response.Result;
                renderError(validations, "form-information-chat");
            }
            if (response.result.Success) {
                $('#chat-content').append(response.result.Html);
                $('#chat-content').show();
                $('#chat-publisher').show();
                $('#information-chat').hide();
            }
        },
        error: function (req, status, error) {
            alert("Error try again");
        }
    });
}