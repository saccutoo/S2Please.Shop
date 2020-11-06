
function changePage(name, pageNumber) {
    var param = {
        PAGE_NUMBER: pageNumber,
        PAGE_SIZE: 9,
    }
    var productTypes = [];
    var dataProductType = document.getElementsByName('checkbox');
    for (var i = 0; i < dataProductType.length; i++) {
        if (dataProductType[i].checked == true) {
            productTypes.push(dataProductType[i].value);
        }
    }
    var fromMoney = $('#from-money').val();
    var toMoney = $('#to-money').val();
    reaload(ControlModel[name].Url, param, productTypes, fromMoney, toMoney);
    topFunction();
}

function prev(name) {
    var param = {
        PAGE_NUMBER: (ControlModel[name].Parram.PAGE_NUMBER - 1),
        PAGE_SIZE: 9,
    }
    var productTypes = [];
    var dataProductType = document.getElementsByName('checkbox');
    for (var i = 0; i < dataProductType.length; i++) {
        if (dataProductType[i].checked == true) {
            productTypes.push(dataProductType[i].value);
        }
    }
    var fromMoney = $('#from-money').val();
    var toMoney = $('#to-money').val();
    reaload(ControlModel[name].Url, param, productTypes, fromMoney, toMoney);
    topFunction();
}

function prevDots(name, pageNumber) {
    var param = {
        PAGE_NUMBER: pageNumber,
        PAGE_SIZE: 9,
    }
    var productTypes = [];
    var dataProductType = document.getElementsByName('checkbox');
    for (var i = 0; i < dataProductType.length; i++) {
        if (dataProductType[i].checked == true) {
            productTypes.push(dataProductType[i].value);
        }
    }
    var fromMoney = $('#from-money').val();
    var toMoney = $('#to-money').val();
    reaload(ControlModel[name].Url, param, productTypes, fromMoney, toMoney);
    topFunction();
}

function next(name) {
    var param = {
        PAGE_NUMBER: (ControlModel[name].Parram.PAGE_NUMBER + 1),
        PAGE_SIZE: 9,
    }
    var productTypes = [];
    var dataProductType = document.getElementsByName('checkbox');
    for (var i = 0; i < dataProductType.length; i++) {
        if (dataProductType[i].checked == true) {
            productTypes.push(dataProductType[i].value);
        }
    }
    var fromMoney = $('#from-money').val();
    var toMoney = $('#to-money').val();
    reaload(ControlModel[name].Url, param, productTypes, fromMoney, toMoney);
    topFunction();
}

function nextDots(name, pageNumber) {
    var param = {
        PAGE_NUMBER: pageNumber,
        PAGE_SIZE: 9,
    }
    var productTypes = [];
    var dataProductType = document.getElementsByName('checkbox');
    for (var i = 0; i < dataProductType.length; i++) {
        if (dataProductType[i].checked == true) {
            productTypes.push(dataProductType[i].value);
        }
    }
    var fromMoney = $('#from-money').val();
    var toMoney = $('#to-money').val();
    reaload(ControlModel[name].Url, param, productTypes, fromMoney, toMoney);
    topFunction();
}

function jumpPage(name, pageNumber) {
    var param = {
        PAGE_NUMBER: pageNumber,
        PAGE_SIZE: 9,
    }
    var productTypes = [];
    var dataProductType = document.getElementsByName('checkbox');
    for (var i = 0; i < dataProductType.length; i++) {
        if (dataProductType[i].checked == true) {
            productTypes.push(dataProductType[i].value);
        }
    }
    var fromMoney = $('#from-money').val();
    var toMoney = $('#to-money').val();
    reaload(ControlModel[name].Url, param, productTypes, fromMoney, toMoney);
    topFunction();
}

function reaload(url, param, productTypes, fromMoney, toMoney) {
    $.ajax({
        type: "POST",
        url: url,
        data: {
            paramModel: param,
            productTypes: productTypes,
            fromMoney: fromMoney,
            toMoney: toMoney
        },
        dataType: "json",
        success: function (response) {
            $("#list-product").html(response);
        },
        error: function (response, status, error) {
            alert("Error try again");
        }
    });
}

function topFunction() {
    $("html, body").animate({ scrollTop: "100" });
}

function clickProductType(name, id) {
    var productTypes = [];
    var dataProductType = document.getElementsByName('checkbox');
    for (var i = 0; i < dataProductType.length; i++) {
        if (dataProductType[i].checked==true) {
            productTypes.push(dataProductType[i].value);
        }  
    }
    var param = {
        PAGE_NUMBER: 1,
        PAGE_SIZE: 9,
    }
    var fromMoney = $('#from-money').val();
    var toMoney = $('#to-money').val();
    reaload(ControlModel[name].Url, param, productTypes, fromMoney, toMoney)
}

function clickSearchProduct(name) {
    var productTypes = [];
    var dataProductType = document.getElementsByName('checkbox');
    for (var i = 0; i < dataProductType.length; i++) {
        if (dataProductType[i].checked == true) {
            productTypes.push(dataProductType[i].value);
        }
    }
    var param = {
        PAGE_NUMBER: 1,
        PAGE_SIZE: 9,
    }
    var fromMoney = $('#from-money').val();
    var toMoney = $('#to-money').val();

    reaload(ControlModel[name].Url, param, productTypes, fromMoney, toMoney)
}

var method = {};

method.ActiveMenu = function (name) {
    var menu = $('.menu-li');
    for (var i = 0; i < menu.length; i++) {
        if ($(menu)[i].getAttribute('value') == ControlModel[name].ProductGroups[0]) {
            $($(menu)[i]).attr("class", 'menu-li active');
            break;
        }
    }
}