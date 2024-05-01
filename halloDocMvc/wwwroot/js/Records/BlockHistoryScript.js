$(document).ready(function () {
    $("#BlocKHistoryTable").load("/AdminPartials/BlockHistoryRecords");
});
$("#ClearBlockSearch").click(function () {
    $(".admin-data-container").load('/AdminPartials/BlockHistoryIndex');
});
$("#BlockHistory").on("submit", function (e) {
    e.preventDefault();
    var formdatas = $("#BlockHistory").serialize();
    $.ajax({
        url: '/AdminPartials/BlockHistoryRecords',
        method: 'POST',
        data: formdatas,
        success: function (res) {
            $("#BlocKHistoryTable").html(res);
        }
    })
})
function ManipulateData(PageNumber) {

    var formdatas = $("#BlockHistory").serialize();
    $.ajax({
        url: '/AdminPartials/BlockHistoryRecords?PageNumber=' + PageNumber,
        method: 'POST',
        data: formdatas,
        success: function (res) {
            $("#BlocKHistoryTable").html(res);
        }
    })

}

function UnBlock(reqid, blockid) {
    $.ajax({
        url: '/AdminPartials/UnBlockPatient',
        method: 'POST',
        data: {
            reqid: reqid,
            blockid: blockid
        },
        success: function (res) {
            //chagne check
            if (res) {
                $("#" + reqid).attr("checked", false);
                $("#" + reqid + blockid).attr("hidden", true);
                toastr.success("Patient UnBlocked")
            }
            else {
                toastr.error("Patient Not UnBlocked")
            }
        }
    })
};

function lastPage() {
    let lastpage = 999999;
    ManipulateData(lastpage);
};
function FirstPage() {
    if ($("#PageNumber").val() > 1) {
        ManipulateData();
    }
}
function Previous() {
    var CurrentPage = localStorage.getItem("RequestState");
    if ($("#PageNumber").val() > 1) {
        var pageNumbner = +$("#PageNumber").val() - 1;
        ManipulateData(pageNumbner);
    }
    else {
        return true;
    }
}
function Next(maxpage) {
    if ($("#PageNumber").val() != maxpage) {
        var pageNumbner = +$("#PageNumber").val() + 1;
        ManipulateData(pageNumbner);
    }
    else {
        return true;
    }
}
