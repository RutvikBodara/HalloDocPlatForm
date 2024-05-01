$(document).ready(function () {
    $("#loadPartnerTable").load("/AdminPartials/SearchRecords");
});
$("#ClearSearch").click(function (e) {
    e.preventDefault();
    $("#Search-Records").click();
});

$("#StartDate").on("change", function () {
    var fromDate = $("#StartDate").val();
    var toDate = $("#EndDate").val();
    if (toDate == "") {
        $(".StartDateValidation").html("");
        $(".EndDateValidation").html("");
        return true;
    }
    else if (toDate < fromDate) {
        $(".StartDateValidation").html("FromDate Must Be Less Then ToDate");
        return false;
    }
    else
    {
        $(".EndDateValidation").html("");
        $(".StartDateValidation").html("");
    }
});
$("#EndDate").on("change", function () {
    var fromDate = $("#StartDate").val();
    var toDate = $("#EndDate").val();
    if (fromDate=="")
    {
        $(".StartDateValidation").html("");
        $(".EndDateValidation").html("");
    }
    if (toDate < fromDate) {
        $(".EndDateValidation").html("EndDate Must Be Greater Then FromDate");
        return false;
    }
    else {
        $(".StartDateValidation").html("");
        $(".EndDateValidation").html("");
    }
});
$("#SearchForm").on("submit", function (e) {
    e.preventDefault();
    var fromDate = $("#StartDate").val();
    var toDate = $("#EndDate").val();
    if (fromDate== null || toDate == null) {
        console.log("Valid Range")
    }
    else {
    if (toDate < fromDate) {
        $(".StartDateValidation").html("FromDate Must Be Less Then ToDate");
        return false;
    }
    }
    
    var formdatas = $("#SearchForm").serialize();
    $.ajax({
        url: '/AdminPartials/SearchRecords',
        method: 'POST',
        data: formdatas,
        success: function (res) {
            $("#loadPartnerTable").html(res);
        }
    })
});
$("#ExportPatHistory").click(function (e) {
    e.preventDefault();
    var datas = $("#Patient-table-Excel");
    TableToExcel.convert(datas[0], {
        name: `PatientData.xlsx`,
        sheet: {
            name: `PatientData`
        }
    });

});
function DeleteRecords(reqid) {
    $.ajax({
        url: '/AdminPartials/DeleteRecords',
        method: 'POST',
        data: { reqid: reqid },
        success: function (response) {
            if (response) {
                toastr.success("deleted");
            }
            else {
                toastr.error("Not deleted");
            }
            $(".admin-data-container").load('/AdminPartials/SearchRecordsIndex', function () {
            });
        }
    });
}
function PaginatedData(PageNumber) {
    var formdatas = $("#SearchForm").serialize();
    $.ajax({
        url: '/AdminPartials/SearchRecords?PageNumber='+PageNumber,
        method: 'POST',
        data: formdatas,
        success: function (res) {
            $("#loadPartnerTable").html(res);
        }
    })
}
function lastPage() {
    let lastpage = 999999;
    PaginatedData(lastpage);
};
function FirstPage() {
    PaginatedData()
    //$("#loadPartnerTable").load("/AdminPartials/SearchRecords");
}
function Previous() {
    if ($("#PageNumber").val() > 1) {
        var pageNumbner = +$("#PageNumber").val() - 1;
        PaginatedData(pageNumbner);
    }
    else {
        return true;
    }
}
function Next(maxpage) {
    if ($("#PageNumber").val() != maxpage) {
        var pageNumbner = +$("#PageNumber").val() + 1;
        PaginatedData(pageNumbner);
    }
    else {
        return true;
    }
}