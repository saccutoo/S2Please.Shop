function updateCart() {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/WEB_SHOP/Cart/Update",
        data: $('#cart-form').serializeArray(),
        success: function (response) {
            if (response.Success) {
                toastr["success"](response.Message, response.CacheName)
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
            }
            else {
                toastr["error"](response.Message, response.CacheName);
                loadingBody.Hide();

            }
        }
    });
}

function continueShopping() {
    window.location.href = "/home-page";
}

$('#checkout').click(function () {
    window.location.href = "/checkout";

})