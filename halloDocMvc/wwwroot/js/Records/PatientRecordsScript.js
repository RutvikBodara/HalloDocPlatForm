$(document).ready(function () {
    $("#PatientHitory").load("/AdminPartials/PatientRecords");
});
$("#ClearHistorySearch").click(function () {
    $(".admin-data-container").load('/AdminPartials/PatientRecordsIndex', function () {
    });
});
$("#SearchPatHistory").on("submit", function (e) {
    e.preventDefault();
    var formdatas = $("#SearchPatHistory").serialize();
    $.ajax({
        url: '/AdminPartials/PatientRecords',
        method: 'POST',
        data: formdatas,
        success: function (res) {
            $("#PatientHitory").html(res);
        }
    })
})
function Explore(userid) {
    $.ajax({
        url: '/AdminPartials/PatientHistoryIndex',
        method: 'POST',
        data: { userid: userid },
        success: function (res) {
            localStorage.setItem("BackDoc", "PatientHistoryIndex")
            localStorage.setItem("userid", userid);
            $(".admin-data-container").html(res);
        }
    })
}