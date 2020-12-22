
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


//Get value language
function getValueLanguage(key) {
    $.ajax({
        type: "POST",
        url: "/Base/getValueLanguage",
        data: {
            key: key
        },
        dataType: "json",
        success: function (response ) {
            return response.key
        },
        error: function (req, status, error) {
            return key
        }
    });
}

//decodeEntities string
function decodeEntities(encodedString) {
    var textArea = document.createElement('textarea');
    textArea.innerHTML = encodedString;
    return textArea.value;
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


// model remove
function viewModalRemove(id,link) {
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

//khởi tạo loading body html
var loadingBody = {};

loadingBody.Show=function(){
    $("#body").LoadingOverlay("show", {
        //background: "rgba(165, 190, 100, 0.5)",
        //image: "/Image/LogoS2.png",
        image: "",
        fontawesome: "fas fa-sun-o fa-spin",
        //text: "Loading...",
        maxSize: 50
    }) ;
}


loadingBody.Hide = function () {
    $("#body").LoadingOverlay("hide");
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

function showPopupColumn(type, tableName) {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/ADMIN/Filter/ShowModalColumn",
        data: {
            type: type,
            tableName: tableName
        },
        dataType: "json",
        success: function (response) {
            $("#modal-content-right").html(response);
            $("#modal-right").modal("show");
            loadingBody.Hide();

        },
        error: function (req, status, error) {
            loadingBody.Hide();
            alert("Error try again");
        }
    });
}

function columnCheck(columnID) {
    if ($("#cb-filter-column-" + columnID).is(":checked")) {
        $("#filter-column-config-" + columnID).removeAttr("hidden");
    }
    else {
        $("#filter-column-config-" + columnID).attr("hidden", "hidden");
    }
}

$(document).ready(function () {

    $(document).on("change", "#city", function (e) {
        $.ajax(
            {
                type: "POST",
                url: "/ADMIN/System/ReloadDistrict",
                data: {
                    codeCity: $('#city').val(),
                },
                success: function (response) {
                    $("#select2-district").html(response[0]);
                    $("#select2-community").html(response[1]);
                }
            });
    });

    $(document).on("change", "#district", function (e) {
        $.ajax(
            {
                type: "POST",
                url: "/ADMIN/System/ReloadCommunity",
                data: {
                    codeDistrict: $('#district').val(),
                },
                success: function (response) {
                    $("#select2-community").html(response);
                }
            });
    });

    $("#GlobalSearchInput").keyup(function (e) {
        var div = $('#showSearchDiv');
        if ($("#GlobalSearchInput").val().length > 0) {
            div.slideDown("slow");
            $.ajax(
                {
                    type: "POST",
                    url: "/ADMIN/System/SearchCommonFromAdmin",
                    data: {
                        filterString: $("#GlobalSearchInput").val(),
                    },
                    success: function (response) {
                        $("#showSearchDiv").html(response);
                    }
                });
        }
        else {
            div.slideUp("slow")
        }
    });

})



