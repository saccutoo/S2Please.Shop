

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


function saveDelete(id) {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: "/ADMIN/Product/Delete",
        data: {
            id: id
        },
        dataType: "json",
        success: function (response) {
            if (response.result.Success) {
                toastr["success"](response.result.Message, response.result.CacheName);
                $("#modal-center").modal("hide");
                reaload("Product", ControlModel["Product"].PAGE_INDEX, $('#Product-paging-items-per-page').val(), "");
            }
            loadingBody.Hide();

        },
        error: function (response, status, error) {
            alert("Error try again");
        }
    });
}


function clickAdd() {
    window.location.href = "/admin/product-save/0";
}

function reloadTableColorAndSize() {
    //listImg = [];
    debugger
    var color = $("#color").val();
    var size = $("#size").val();
    if (color == null || size == null || color == "" || size == "") {
        return false;
    }
    else {
        var data = $('#form-product').serializeArray();
        data.push({ name: "COLOR", value: color });
        data.push({ name: "SIZE", value: size })
        for (var i = 0; i < data.length; i++) {
            if (data[i].name.indexOf('.PRICE') > -1) {
                data[i].value = data[i].value.split(',').join('');
            }
        }
        $.ajax({
            type: "POST",
            url: "/ADMIN/Product/ReloadTableColorAndSize",
            data: data,
            dataType: "json",
            success: function (response) {
                $("#body-color-size-map").html(response);
                var rows = $("#body-color-size-map tr ");
                if (rows != null && rows.length > 0) {
                    reloadIndex(rows);
                }
            },
            error: function (response, status, error) {
                alert("Error try again");
            }
        });
    }

}


function reloadIndex(rows) {
    for (var i = 0; i < rows.length; i++) {
        $(rows[i]).attr("id", "tr-" + i);
        var childrenTD = $(rows[i]).children();
        if (childrenTD != null && childrenTD.length > 0) {
            for (var j = 0; j < childrenTD.length; j++) {
                var rowsTD = $(childrenTD[j]).children();
                if (rowsTD != null && rowsTD.length > 0) {
                    for (var x = 0; x < rowsTD.length; x++) {
                        if (rowsTD[x].localName == "input" && rowsTD[x].getAttribute("name").indexOf('INDEX') > -1) {
                            $(rowsTD[x]).attr("name", "ColorSizeMap[" + i + "].INDEX");
                            $(rowsTD[x]).val(i);
                            continue;
                        }
                        if (rowsTD[x].localName == "input" && rowsTD[x].getAttribute("name").indexOf('IS_MAIN') > -1) {
                            $(rowsTD[x]).attr("name", "ColorSizeMap[" + i + "].IS_MAIN");
                            continue;
                        }

                        if (rowsTD[x].localName == "span" && rowsTD[x].getAttribute("class").indexOf('zmdi-delete') > -1) {
                            $(rowsTD[x]).attr("onclick", "removeRow(" + i + ")");
                            continue;
                        }
                        if (rowsTD[x].localName == "div" && rowsTD[x].getAttribute('id') != null && rowsTD[x].getAttribute("id").indexOf('upload-img') > -1) {
                            $(rowsTD[x]).attr("id", "upload-img-" + i);
                            var childrenDiv = $(rowsTD[x]).children();
                            if (childrenDiv != null && childrenDiv.length > 0) {
                                for (var z = 0; z < childrenDiv.length; z++) {


                                    if (childrenDiv[z].localName == "input" && childrenDiv[z].getAttribute("name").indexOf('IMG') > -1) {
                                        $(childrenDiv[z]).attr("name", "ColorSizeMap[" + i + "].IMG");
                                        $(childrenDiv[z]).attr("id", "file-" + i);
                                        $(childrenDiv[z]).attr("onchange", "changeFile(this," + i + ")");
                                        continue;
                                    }
                                    if (childrenDiv[z].localName == "input" && childrenDiv[z].getAttribute("name").indexOf('FILE_NAME') > -1) {
                                        $(childrenDiv[z]).attr("name", "ColorSizeMap[" + i + "].FILE_NAME");
                                        $(childrenDiv[z]).attr("id", "file-name-" + i);
                                        continue;
                                    }
                                    if (childrenDiv[z].localName == "span") {
                                        $(childrenDiv[z]).attr("onclick", "clickFile(" + i + ")");
                                        continue;
                                    }


                                }
                            }
                            continue;
                        }
                        if (rowsTD[x].localName == "div" && rowsTD[x].getAttribute('id') != null && rowsTD[x].getAttribute("id").indexOf('show-img') > -1) {
                            $(rowsTD[x]).attr("id", "show-img-" + i);
                            var childrenDiv = $(rowsTD[x]).children();
                            if (childrenDiv != null && childrenDiv.length > 0) {
                                for (var z = 0; z < childrenDiv.length; z++) {
                                    if (childrenDiv[z].localName == "img") {
                                        $(childrenDiv[z]).attr("id", "img-" + i);
                                        continue;
                                    }
                                }
                            }
                            continue;
                        }

                        if (rowsTD[x].localName == "div" && rowsTD[x].getAttribute('id') != null && rowsTD[x].getAttribute("id").indexOf('remove-img-') > -1) {
                            $(rowsTD[x]).attr("id", "remove-img-" + i);
                            $(rowsTD[x]).attr("onclick", "removeImg(" + i + ")");
                            continue;
                        }

                        if (rowsTD[x].localName == "div" && rowsTD[x].getAttribute("class").indexOf('padding-0') > -1) {

                            var childrenDiv = $(rowsTD[x]).children();
                            if (childrenDiv != null && childrenDiv.length > 0) {
                                for (var z = 0; z < childrenDiv.length; z++) {
                                    if (childrenDiv[z].localName == "input" && childrenDiv[z].getAttribute("name").indexOf('COLOR') > -1) {
                                        $(childrenDiv[z]).attr("name", "ColorSizeMap[" + i + "].COLOR");
                                        $(childrenDiv[z]).attr("id", "color_" + i);
                                        continue;
                                    }
                                    if (childrenDiv[z].localName == "input" && childrenDiv[z].getAttribute("name").indexOf('SIZE') > -1) {
                                        $(childrenDiv[z]).attr("name", "ColorSizeMap[" + i + "].SIZE");
                                        $(childrenDiv[z]).attr("id", "size_" + i);
                                        continue;
                                    }
                                    if (childrenDiv[z].localName == "input" && childrenDiv[z].getAttribute("name").indexOf('AMOUNT') > -1) {
                                        $(childrenDiv[z]).attr("name", "ColorSizeMap[" + i + "].AMOUNT");
                                        $(childrenDiv[z]).attr("id", "amount_" + i);

                                        $("#" + childrenDiv[z].getAttribute("id")).inputmask({
                                            alias: childrenDiv[z].getAttribute("input-mask"),
                                            prefix: '',
                                            digits: '0'
                                        });
                                        continue;
                                    }
                                    if (childrenDiv[z].localName == "input" && childrenDiv[z].getAttribute("name").indexOf('PRICE') > -1) {
                                        $(childrenDiv[z]).attr("name", "ColorSizeMap[" + i + "].PRICE");
                                        $(childrenDiv[z]).attr("id", "price_" + i);
                                        $("#" + childrenDiv[z].getAttribute("id")).inputmask({
                                            alias: childrenDiv[z].getAttribute("input-mask"),
                                            prefix: '',
                                            digits: '0'
                                        });
                                        continue;
                                    }
                                    if (childrenDiv[z].localName == "input" && childrenDiv[z].getAttribute("name").indexOf('RATE') > -1) {
                                        $(childrenDiv[z]).attr("name", "ColorSizeMap[" + i + "].RATE");
                                        $(childrenDiv[z]).attr("id", "rate_" + i);

                                        $("#" + childrenDiv[z].getAttribute("id")).inputmask({
                                            alias: childrenDiv[z].getAttribute("input-mask"),
                                            prefix: '',
                                            digits: '0'
                                        });
                                        continue;
                                    }


                                    if (childrenDiv[z].localName == "label" && childrenDiv[z].getAttribute("name").indexOf('COLOR') > -1) {
                                        $(childrenDiv[z]).attr("name", "message-error-ColorSizeMap[" + i + "].COLOR");
                                        $(childrenDiv[z]).attr("id", "message-error-color-" + i);
                                        continue;
                                    }
                                    if (childrenDiv[z].localName == "label" && childrenDiv[z].getAttribute("name").indexOf('SIZE') > -1) {
                                        $(childrenDiv[z]).attr("name", "message-error-ColorSizeMap[" + i + "].SIZE");
                                        $(childrenDiv[z]).attr("id", "message-error-size-" + i);
                                        continue;
                                    }
                                    if (childrenDiv[z].localName == "label" && childrenDiv[z].getAttribute("name").indexOf('AMOUNT') > -1) {
                                        $(childrenDiv[z]).attr("name", "message-error-ColorSizeMap[" + i + "].AMOUNT");
                                        $(childrenDiv[z]).attr("id", "message-error-amount-" + i);
                                        continue;
                                    }
                                    if (childrenDiv[z].localName == "label" && childrenDiv[z].getAttribute("name").indexOf('PRICE') > -1) {
                                        $(childrenDiv[z]).attr("name", "message-error-ColorSizeMap[" + i + "].PRICE");
                                        $(childrenDiv[z]).attr("id", "message-error-price-" + i);
                                        continue;
                                    }


                                }
                            }
                            continue;
                        }
                    }
                }
            }
        }
    }

}

$(document).ready(function () {
    $('#color').tagsInput({
        onAddTag: () => {
            reloadTableColorAndSize();
            reloadImgByColor('',0);
        },
        onRemoveTag: (e, element) => {
            reloadImgByColor(element,0);
            showButtonPlus();
            reloadTableColorAndSize();

        },
    });

    $('#size').tagsInput({
        onAddTag: () => {
            reloadTableColorAndSize();
            reloadImgByColor('',0);
        },
        onRemoveTag: (e, element) => {
            reloadImgByColor('',0);
            showButtonPlus();
            reloadTableColorAndSize();

        },
    });

});

function reloadImgByColor(colorRemove,removeImg) {
    var color = $("#color").val();
    if (color == null || color =='') {
        color = colorRemove.split(' ').join('-');
    }
    if (color.indexOf(',')>-1) {
        color = color.split(',');
        if (color.length > 0) {
            var dynamic = "";
            for (var i = 0; i < color.length; i++) {
                var fileName = "";
                var id = "0";
                var isRemove = '0';
                var colorName = color[i].split(' ').join('-');
                if ($('#file-name-' + colorName).length > 0) {
                    fileName = $('#file-name-' + color).val();
                }
                if ($('#id-' + colorName).length > 0) {
                    id = $('#id-' + colorName).val();
                }
                if (colorRemove != null && colorRemove != '' && colorRemove == colorName) {
                    isRemove = '1';
                }
                if (dynamic == null || dynamic == "") {
                    dynamic = "{ COLOR:'" + colorName + "',FILE_NAME:'" + fileName + "',ID:'" + id + "',IS_REMOVE:'" + isRemove + "',IS_REMOVE_IMG:'" + removeImg + "'}";
                }
                else {
                    dynamic += ",{ COLOR:'" + colorName + "',FILE_NAME:'" + fileName + "',ID:'" + id + "',IS_REMOVE:'" + isRemove + "',IS_REMOVE_IMG:'" + removeImg + "'}";
                }
            }
            dynamic = "[" + dynamic + "]";
            var formData = new FormData();
            formData.append('dynamic', dynamic);

            $.ajax({
                type: 'POST',
                url: "/ADMIN/Product/UploadImgByColor",
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.Success) {
                        $('#list-img-by-color').html(response.Html);
                    }
                },
            });
        }
    }
    else if (color != null && color !='') {
        var fileName = "";
        var id = "0";
        var color = color.split(' ').join('-');
        var isRemove = '0';
        var isRemoveImg = '0';

        if ($('#file-name-' + color).length > 0) {
            fileName = $('#file-name-' + color).val();
        }
        if ($('#id-' + color).length > 0) {
            id = $('#id-' + color).val();
        }
        if (colorRemove != null && colorRemove != '' && colorRemove == color) {
            isRemove = "1";            
        }
        var dynamic = "{ COLOR:'" + color + "',FILE_NAME:'" + fileName + "',ID:'" + id + "',IS_REMOVE:'" + isRemove + "',IS_REMOVE_IMG:'" + removeImg + "'}";
        dynamic = "[" + dynamic + "]";

        var formData = new FormData();
        formData.append('dynamic', dynamic);

        $.ajax({
            type: 'POST',
            url: "/ADMIN/Product/UploadImgByColor",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.Success) {
                    $('#list-img-by-color').html(response.Html);
                }
            },
        });
    }
}



function showButtonPlus() {

    var color = $("#color").val();
    var size = $("#size").val();
    var countColor = 0;
    var countSize = 0;
    if (color == null || size == null || color == "" || size == "") {
        $("#button-plus-row").css("display", "none")
        return false;
    }
    else {
        if (color.indexOf(',') > -1) {
            var listColor = color.split(",");
            countColor = listColor.length;
        }
        else {
            countColor = 1;
        }

        if (size.indexOf(',') > -1) {
            var listSize = size.split(",");
            countSize = listSize.length;
        }
        else {
            countSize = 1;
        }

        var rows = $("#body-color-size-map tr ");
        var count = countSize * countColor;
        if (rows.length < count) {
            $("#button-plus-row").css("display", "block")
        }
        else {
            $("#button-plus-row").css("display", "none")
        }
    }
}

function clickFile(color) {
    //var value = $(element)[0].getAttribute('value');
    $("#file-" + color).click();
    return;

}

function changeFile(element,color) {
    var file = element.files;
    if (file[0].type.indexOf('image') == -1) {
        $("label[name='message-error-headingTwo']").text(errorImg3);
        $("#card-headingTwo").animate({
            'background-color': 'red',
        });
        $("#card-headingTwo").animate({
            'background-color': 'rgba(0,0,0,.03)',
        });
        return;
    }

    var id = "0";
    if ($('#id-' + color).length > 0) {
        id = $('#id-' + color).val();
    }
    var dynamic = "{ COLOR:'" + color + "',FILE_NAME:'" + file[0].name + "',ID:'" + id + "'}";
    dynamic = "[" + dynamic + "]";
    var formData = new FormData();
    formData.append('files', file[0]);
    formData.append('dynamic', dynamic);

    $.ajax({
        type: 'POST',
        url: "/ADMIN/Product/UploadImgByColor",
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.Success) {
                $('#list-img-by-color').html(response.Html);
            }
        },
    });
    //readURL(element,value);
}

function readURL(input,value) {
    if (input.files && input.files[0]) {
     

        var reader = new FileReader();
        reader.onload = function (e) {
            var divBao = document.getElementById('row-img-by-color-' + value);

            img = document.createElement("img");
            img.src = e.target.result;
            $(img).attr('class', 'img-thumbnail img-by-color');
            $(img).attr('id', 'img-'+value);


            var parts = e.target.result.split(";base64,");

            stringBase64 = document.createElement("input");
            $(stringBase64).attr('hidden', 'hidden');
            //$(stringBase64).attr('name', 'ListImgByColor[' + i + '].IMG');
            $(stringBase64).attr('value', parts[1]);
            $(stringBase64).attr('type', 'text');
            $(stringBase64).attr('id', 'input-' + value);
            $(stringBase64).attr('name', 'input-img');

            colorInput = document.createElement("input");
            $(colorInput).attr('hidden', 'hidden');
            $(colorInput).attr('value',value);
            $(colorInput).attr('type', 'text');
            $(colorInput).attr('id', 'color-input-' + value);
            $(stringBase64).attr('name', 'input-color');

            console.log(test(e.target.result))

            htmla = document.createElement("p");
            $(htmla).attr('class', 'fa fa-times row-img-slide');
            $(htmla).attr('onclick', 'removeRowImgByColor(this)');
            $(htmla).attr('id', 'p-' + value);

            divBao.append(img);
            divBao.append(stringBase64);
            divBao.append(colorInput);
            divBao.append(htmla);

            $('#span-' + value).css('display', 'none');
            $(divBao).css('border', 'none');

        }
        reader.readAsDataURL(input.files[0]);


    }
}


function removeRowImgByColor(color,id) {
    reloadImgByColor(color,1);
}

function reloadIndexImgByColor() {
    var rows = $('.row-img-by-color');
    if (rows.length > 0) {
        for (var i = 0; i < rows.length; i++) {
            //childrenDiv[z].getAttribute("name").indexOf('COLOR') > -1
            var value = rows[i].getAttribute('value');
            $('#input-' + value).attr('name', 'ListImgByColor[' + i + '].IMG');
            $('#color-input-' + value).attr('name', 'ListImgByColor[' + i + '].COLOR');

        }
    }
}

function removeRow(index) {
    var color = $("#color_" + index).val();
    var size = $("#size_" + index).val();
    var data = $.grep(listImg, function (element, index) {
        return element.color == color && element.size == size;
    });
    if (data != null && data.length > 0) {
        listImg = $.grep(listImg, function (element, index) {
            return element.color != color && element.size != size;
        });

        $.ajax({
            type: "POST",
            url: "/ADMIN/Product/removeFile",
            data: {
                id: $("#product_id").val(),
                fileName: data[0].file.name

            },
            dataType: "json",
            success: function (response) {

            },
            error: function (response, status, error) {
                alert("Error try again");
            }
        });
    }
    var tr = $("#tr-" + index);
    $(tr).remove();
    var rows = $("#body-color-size-map tr ");
    if (rows != null && rows.length > 0) {
        reloadIndex(rows);
    }
    showButtonPlus();
}

function saveProduct() {
    loadingBody.Show();
    $("#form-product label[name*='message-error-']").text("");
    reloadIndexSlideImg();
    reloadIndexImgByColor();

    $.ajax({
        type: "POST",
        url: "/ADMIN/Product/CheckValidation",
        data: $('#form-product').serializeArray(),
        dataType: 'json',
        success: function (response) {
            if (response.Invalid) {
                var validations = response.Result;
                renderError(validations, "form-product");
                var checkProduct = false;
                var colorAndSize = false;
                var cartDetail = false;
                var checkFileNameColor = false;

                for (var i = 0; i < validations.length; i++) {
                    if (validations[i].ColumnName.indexOf('.COLOR') > -1 || validations[i].ColumnName.indexOf('.SIZE') > -1) {

                        colorAndSize = true;
                    }
                    else if (validations[i].ColumnName.indexOf('.NAME') > -1 ||
                             validations[i].ColumnName.indexOf('.PRODUCT_TYPE_ID') > -1 ||
                             validations[i].ColumnName.indexOf('.PRODUCT_GROUP_ID') > -1) {
                        checkProduct = true;
                    }
                    else if (validations[i].ColumnName.indexOf('Card.ColorSizeMap') > -1) {

                        cartDetail = true;
                    }
                }

                if (colorAndSize) {
                    $("#card-headingTwo").animate({
                        'background-color': 'red',
                    });
                    $("#card-headingTwo").animate({
                        'background-color': 'rgba(0,0,0,.03)',
                    });
                }

                if (checkProduct) {
                    $("#card-headingOne").animate({
                        'background-color': 'red',
                    });
                    $("#card-headingOne").animate({
                        'background-color': 'rgba(0,0,0,.03)',
                    });
                }

                if (cartDetail) {
                    $("#card-headingThree").animate({
                        'background-color': 'red',
                    });
                    $("#card-headingThree").animate({
                        'background-color': 'rgba(0,0,0,.03)',
                    });
                }
                loadingBody.Hide();
                return;
            }
            
            var rows = $('.div-input-image-row');

            //var element = $("#file-slide-img")[0].files;
            if (rows == null || rows == "" || rows.length == 0) {
                $("#message-error-slide-image").text(errorImg1);
                $("#card-headingFour").animate({
                    'background-color': 'red',
                });
                $("#card-headingFour").animate({
                    'background-color': 'rgba(0,0,0,.03)',
                });
                loadingBody.Hide();
                return;
            }
            
            var rows = $('.row-img-by-color');
            if (rows.length > 0) {
                for (var i = 0; i < rows.length; i++) {
                    var value = rows[i].getAttribute('value');
                    if ($('#img-' + value).val() == null || $('#img-' + value).val()=='') {
                        $("label[name='message-error-headingTwo']").text(errorImg4);
                        $("#card-headingTwo").animate({
                            'background-color': 'red',
                        });
                        $("#card-headingTwo").animate({
                            'background-color': 'rgba(0,0,0,.03)',
                        });
                        loadingBody.Hide();
                        return;
                    }

                }
            }

            var data = $('#form-product').serializeArray();
            for (var i = 0; i < data.length; i++) {
                if (data[i].name.indexOf('PRICE') > -1) {
                    if (data[i].value.indexOf(',') > -1) {
                        data[i].value = data[i].value.split(',').join('');
                    }
                }
            }

            $.ajax({
                type: "POST",
                url: "/ADMIN/Product/SaveProduct",
                data: data,
                dataType: 'json',
                success: function (response) {
                    if (response.result !=null && !response.result.IsPermission) {
                        window.location.href = response.result.Url;
                    }
                    else if (response.Success) {
                        toastr["success"](response.CacheName, response.Message);
                        loadingBody.Hide();
                        setTimeout(function () { window.location.href ="/admin/product" }, 2000);

                    }
                    else {
                        toastr["false"](response.CacheName, response.Message);
                        loadingBody.Hide();

                    }
                },
                error: function (response, status, error) {
                    alert("Error try again");
                    loadingBody.Hide();

                }
            });
        }
    });
}

function plusRow() {
    var color = $("#color").val();
    var size = $("#size").val();
    var data = $('#form-product').serializeArray();
    data.push({ name: "COLOR", value: color });
    data.push({ name: "SIZE", value: size })

    $.ajax({
        type: "POST",
        url: "/ADMIN/Product/ReloadTableColorAndSize",
        data: data,
        dataType: "json",
        success: function (response) {
            $("#body-color-size-map").append(response);
            var rows = $("#body-color-size-map tr ");
            if (rows != null && rows.length > 0) {
                reloadIndex(rows);
                showButtonPlus();
            }
        },
        error: function (response, status, error) {
            alert("Error try again");
        }
    });
}

function clickRadioIsMain(element) {

    var inputRadio = $('.input-radio');
    if (inputRadio != null && inputRadio.length > 0) {
        for (var i = 0; i < inputRadio.length; i++) {
            $(inputRadio[i]).prop("checked", false);

        }
    }
    $(element).prop("checked", true);
}


function clickListFileImage() {
    $("#message-error-slide-image").text("");
    $("#file-slide-img").click();
}

function changeListFileImage(element) {
    if (element != null && element.files != null && element.files.length > 8) {
        $("#message-error-slide-image").text(errorImg2);
        $("#card-headingFour").animate({
            'background-color': 'red',
        });
        $("#card-headingFour").animate({
            'background-color': 'rgba(0,0,0,.03)',
        });
        $('#file-slide-img').val("");
        return false;
    }

    else {

        var formData = new FormData();
        for (var i = 0; i < element.files.length; i++) {
            formData.append('files', element.files[i]);
        }
        formData.append('attachment', element.files[i]);
        $.ajax({
            type: 'POST',
            url: "/ADMIN/Product/UploadSlideImg",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.Success) {
                    $('#slide-image-id').html(response.Html);
                    $("#remove-list-image").css("display", "block");
                }
            },
        });
        //readListImg(element);
    }

}


function comeBackProduct() {
    window.location.href = "/admin/product";
}

function removeListImage() {
    var formData = new FormData();
    formData.append('removeAll', true);

    $.ajax({
        type: 'POST',
        url: "/ADMIN/Product/RemoveFileSlide",
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.Success) {
                $('#slide-image-id').html(response.Html);
                $("#remove-list-image").css("display", "none");

            }
        },
    });
}

function changeVideo(element) {
    $("#message-error-video").text("");
    if (element.files[0].type.indexOf('video') == -1) {
        $("#message-error-video").text(errorVideoFormat);
        $("#card-headingFive").animate({
            'background-color': 'red',
        });
        $("#card-headingFive").animate({
            'background-color': 'rgba(0,0,0,.03)',
        });
        $("#file-video").val("");
        return
    }

    else {

        if (formatBytes(element.files[0].size) > 200) {
            $("#message-error-video").text(errorVideo);
            $("#card-headingFive").animate({
                'background-color': 'red',
            });
            $("#card-headingFive").animate({
                'background-color': 'rgba(0,0,0,.03)',
            });
            $("#file-video").val("");
            return
        }

        var reader = new FileReader();

        // Onload Function will run after video has loaded
        reader.onload = function (file) {

            var fileContent = file.target.result;
            //$('.video-upload').append('<video src="' + fileContent + '"  controls></video>');
        }

        // Get the selected video from Dialog
        reader.readAsDataURL(element.files[0]);

    }
}

function formatBytes(size) {
    return ((parseInt(size) / 1024) / 1024) //MB
}


function Update(id) {
    loadingBody.Show();
    window.location.href = "/admin/product-save/" + id;
}

function removeImg(index) {
    $("#show-img-" + index).css("display", "none");
    $("#upload-img-" + index).css("display", "block");
    $("#file-" + index).val("");
    $("#file-name-" + index).val("");
}

function removeRowImgslide(fileName, Id) {
    var formData = new FormData();
    formData.append('fileName', fileName);
    formData.append('id', Id);

    $.ajax({
        type: 'POST',
        url: "/ADMIN/Product/RemoveFileSlide",
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.Success) {
                $('#slide-image-id').html(response.Html);
            }
        },
    });
    //element.parentElement.remove();
    //var row = $('.div-input-image-row');
    //if (row.length ==0) {
    //    $(".image-times").remove();
    //    $(".div-input-image-row").remove();
    //    $("#div-input-multi-image").css("display", "block");
    //    $("#remove-list-image").css("display", "none");
    //    $("#file-slide-img").val("");
    //    $("#is_check_save_list_image").val("true");
    //}
    //else {
    //    reloadIndexSlideImg();

    //}
}

function reloadIndexSlideImg() {
    var rows = $('.div-input-image-row');
    if (rows.length > 0) {
        for (var i = 0; i < rows.length; i++) {
            $(rows[i]).attr("id", "div-input-image-" + i);
            var children = $(rows[i]).children();
            if (children != null && children.length > 0) {
                for (var x = 0; x < children.length; x++) {

                    if (children[x].localName == "input") {
                        $(children[x]).attr("name", "ListSlide[" + i + "].IMG");
                        continue;
                    }
                }
            }
        }
    }
}


function test(base64StringFromURL)
{
    var parts = base64StringFromURL.split(";base64,");
    var contentType = parts[0].replace("data:", "");
    var base64 = parts[1];
    return base64ToByteArray(base64);
}

function base64ToByteArray(base64String) {
    try {            
        var sliceSize = 1024;
        var byteCharacters = atob(base64String);
        var bytesLength = byteCharacters.length;
        var slicesCount = Math.ceil(bytesLength / sliceSize);
        var byteArrays = new Array(slicesCount);

        for (var sliceIndex = 0; sliceIndex < slicesCount; ++sliceIndex) {
            var begin = sliceIndex * sliceSize;
            var end = Math.min(begin + sliceSize, bytesLength);

            var bytes = new Array(end - begin);
            for (var offset = begin, i = 0; offset < end; ++i, ++offset) {
                bytes[i] = byteCharacters[offset].charCodeAt(0);
            }
            byteArrays[sliceIndex] = new Uint8Array(bytes);
        }
        return byteArrays;
    } catch (e) {
        console.log("Couldn't convert to byte array: " + e);
        return undefined;
    }
}