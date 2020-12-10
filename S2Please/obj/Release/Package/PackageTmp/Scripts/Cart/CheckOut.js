
var messengerNotificationFromCart = $.connection.messengerHub;


function login() {
    $(".message-error-username").css("display", "none");
    $(".message-error-password").css("display", "none");
    $(".message-error-password1").css("display", "none");
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/WEB_SHOP/Authen/LoginInCheckOut",
        data: {
            userName: $("#username").val(),
            passWord: $("#password").val()
        },
        success: function (response) {
            if (!response.Success) {
                if (response.Is_Error_User_Name_Required) {
                    $(".message-error-username").css("display", "block");
                }
                if (response.IS_Error_Password_Required) {
                    if (response.Message !=null) {
                        $(".message-error-password1").css("display", "block");
                    }
                    else {
                        $(".message-error-password").css("display", "block");

                    }
                }
            }
            else {
                window.location.reload();
            }
            loadingBody.Hide();

        }
    });
}

function order() {
    loadingBody.Show();
    $("#form-order label[name*='message-error-']").text("");
    var checkData = $("input:radio[name='model.METHOD_PAY']");
    var valueCheck;
    for (var i = 0; i < checkData.length; i++) {
        if ($("input:radio[name='model.METHOD_PAY']")[i].checked ) {
            valueCheck = $("input:radio[name='model.METHOD_PAY']")[i].value;
            break;
        }      
    }
    if (valueCheck=="1") {
        loadingBody.Hide();

    }
    else {
        $.ajax({
            type: "POST",
            url: "/WEB_SHOP/Cart/Order",
            data: $('#form-order').serializeArray(),
            success: function (response) {           
                if (response.Invalid) {
                    var validations = response.Result;
                    renderError(validations, "form-order");

                    $("#group-account-billing-details").animate({
                        'background-color': 'red',
                    });
                    $("#group-account-billing-details").animate({
                        'background-color': '#eee',
                    });
                    loadingBody.Hide();
                }
                else if (response.result.Success) {
                    toastr["success"](response.result.CacheName, response.result.Message);
                    $.connection.hub.start().done(function () {                     
                        messengerNotificationFromCart.server.reloadNotification();
                    });

                    window.location.href = "/order_information/" + response.data.ToDecrypt
                }
                else {
                    toastr["error"](response.result.CacheName, response.result.Message);
                    loadingBody.Hide();

                }
            }
        });
    }

}

function backToCart() {
    window.location.href = "/cart";
}

$(document).ready(function () {
    $('#city').on('select2:selecting', function (e) {
        $.ajax(
         {
             type: "POST",
             url: "/WEB_SHOP/Cart/ReloadDistrict",
             data: {
                 codeCity: e.params.args.data.id,
             },
             success: function (response) {
                 $("#select2-district").html(response[0]);
                 $("#select2-community").html(response[1]);
                 if (shipFees!=null && shipFees.length > 0) {
                     var data = $.grep(shipFees, function (element, index) {
                         return element.CODE_CITY != 0;
                     });
                 }
                
                 if (data != null && data.length>0) {
                     if (e.params.args.data.id == data[0].CODE_CITY) {

                         $("#fee-ship").text(parseFloat(data[0].PRICE, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                         var total = $("#input-total").val();
                         total = parseInt(total) + data[0].PRICE;
                         $("#fee_ship").val(data[0].PRICE);
                         $("#total").text(parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                         $("#fee_ship").val(shipFees[0].ID);

                     }
                     else {
                         $("#fee-ship").text(parseFloat(data[0].PRICE, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                         var total = $("#input-total").val();
                         total = parseInt(total) + data[0].PRICE;
                         $("#fee_ship").val(data[0].PRICE);
                         $("#total").text(parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                         $("#fee_ship").val(shipFees[1].ID);

                     }
                 }
                
             }
         });
    });

    $(document).on("change","#district", function (e) {
        $.ajax(
         {
             type: "POST",
             url: "/WEB_SHOP/Cart/ReloadCommunity",
             data: {
                 codeDistrict: $(e.currentTarget).val(),
             },
             success: function (response) {
                 $("#select2-community").html(response);
             }
         });
    });
})
