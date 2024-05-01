
$("#ClearHistorySearch").click(function () {
    var id = 1
    if (localStorage.getItem("currentLog") == 2) {
        id = 2
    }
    $(".admin-data-container").load('/AdminPartials/EmailLogsIndex?type=' + id, function () {
    });
});
$("#EmailLogHistory").on("submit", function (e) {
    e.preventDefault();
    var id = 1;
    if (localStorage.getItem("currentLog") == 2) {
        id = 2;
    }
    var formdatas = $("#EmailLogHistory").serialize();
    $.ajax({
        url: '/AdminPartials/EmailLogRecords?type=' + id,
        method: 'POST',
        data: formdatas,
        success: function (res) {
            $("#EmailLogs").html(res);
        }
    })
})

function PaginatedData(PageNumber) {
    var id = 1;
    if (localStorage.getItem("currentLog") == 2) {
        id = 2;
    }
    var formdatas = $("#EmailLogHistory").serialize();
    $.ajax({
        url: '/AdminPartials/EmailLogRecords?type=' + id + '&PageNumber='+PageNumber,
        method: 'POST',
        data: formdatas,
        success: function (res) {
            $("#EmailLogs").html(res);
        }
    })
}

function lastPage() {
    let lastpage = 999999;
    PaginatedData(lastpage);
};
function FirstPage() {
    if ($("#PageNumber").val() > 1) {
        PaginatedData();
    }
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