
function changePage(tableName, pageNumber) {
    reaload(tableName, pageNumber,$('#'+tableName +'-paging-items-per-page').val(),"");
}

function prev(tableName) {
    reaload(tableName, ControlModel[tableName].PAGE_INDEX - 1, $('#' + tableName + '-paging-items-per-page').val(),"");
}


function prevDots(tableName, pageNumber) {
    reaload(tableName, pageNumber, $('#' + tableName + '-paging-items-per-page').val(),"");

}

function next(tableName) {  
    reaload(tableName, ControlModel[tableName].PAGE_INDEX + 1, $('#' + tableName + '-paging-items-per-page').val(), "");
}

function nextDots(tableName, pageNumber) {
    reaload(tableName, pageNumber, $('#' + tableName + '-paging-items-per-page').val(),"");

}

function jumpPage(tableName, pageNumber) {
    reaload(tableName, pageNumber, $('#' + tableName + '-paging-items-per-page').val(),"");
}

function changeItemsPerpage(tableName) {
    reaload(tableName, 1, $('#' + tableName + '-paging-items-per-page').val(),"");

}
function reaload(tableName, pageNumber, pageSize, filterString) {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: ControlModel[tableName].TABLE_URL,
        data: {
            tableData: ControlModel[tableName],
            param: {
                PAGE_NUMBER:pageNumber,
                PAGE_SIZE: pageSize,
                STRING_FILTER:filterString==null || filterString==""?ControlModel[tableName].STRING_FILTER : filterString,
                VALUE: ControlModel[tableName].VALUE_DYNAMIC
            }
        },
        dataType: "json",
        success: function (response) {
            $("#table-" + tableName).html(response);
            loadingBody.Hide();

        },
        error: function (response, status, error) {
            alert("Error try again");
            loadingBody.Hide();
        }
    });
}

function clickCheckAll(This) {
    var data = $(".row-checkbox");
    if ($(This).is(":checked")) {
        $('#au-checkmark-all').removeClass('mix-state');
        if (data != null && data.length>0) {
            for (var i = 0 ; i < data.length;i++){
                $($(".row-checkbox")[i]).prop("checked", true)
            }
        }
    }
    else {
        $('#au-checkmark-all').removeClass('mix-state');
        if (data != null && data.length > 0) {
            for (var i = 0 ; i < data.length; i++) {
                $($(".row-checkbox")[i]).prop("checked", false)
            }
        }
    }
}

function clickRowCheck(id) {
    var checkAll = true;
    var noCheck = true;
    var hasCheck = false;

    var rowCheckId = "#checkbox_" + id;
    var data = $(".row-checkbox");
    if (data != null && data.length > 0) {
        for (var i = 0 ; i < data.length; i++) {
            if (!$(data[i]).is(":checked")) {
                checkAll = false;
            }
            else  {
                hasCheck = true;
            }
            if ($(data[i]).is(":checked")) {
                noCheck = false;
            }
        }
    }
    if (!checkAll && hasCheck) {
        $($('#checked-all')).prop("checked", false);
        $('#au-checkmark-all').addClass('mix-state');

    }
    else if (!checkAll && !hasCheck) {
        $($("#checked-all")).prop("checked", false);
        $('#au-checkmark-all').removeClass('mix-state');

    }
    else if (checkAll) {
        $($("#checked-all")).prop("checked", true);
        $('#au-checkmark-all').removeClass('mix-state');

    }
   
}

function exportExcel(tableName) {
    loadingBody.Show();
    $.ajax({
        type: "POST",
        url: ControlModel[tableName].TABLE_SESION_EXPORT_URL,
        data: {
            tableData: ControlModel[tableName],
            paramType: {
                STRING_FILTER: ControlModel[tableName].STRING_FILTER,
                VALUE: ControlModel[tableName].VALUE_DYNAMIC
            }
        },
        dataType: "json",
        success: function (response) {
            if (response.result) {
                window.location = ControlModel[tableName].TABLE_EXPORT_URL + "?TABLE_NAME=" + tableName;
            }
            loadingBody.Hide();

        },
        error: function (response, status, error) {
            alert("Error try again");
            loadingBody.Hide();
        }
    });

    loadingBody.Hide();

    //$.ajax({
    //    type: "POST",
    //    url: ControlModel[tableName].TABLE_EXPORT_URL,
    //    data: {
    //        tableData: ControlModel[tableName],
    //        param: {
    //            PAGE_NUMBER: 1,
    //            PAGE_SIZE: 99999,
    //            STRING_FILTER: filterString == null || filterString == "" ? ControlModel[tableName].STRING_FILTER : filterString,
    //            VALUE: ControlModel[tableName].VALUE_DYNAMIC
    //        }
    //    },
    //    dataType: "json",
    //    success: function (response) {
    //        $("#table-" + tableName).html(response);
    //        loadingBody.Hide();

    //    },
    //    error: function (response, status, error) {
    //        alert("Error try again");
    //        loadingBody.Hide();
    //    }
    //});
}