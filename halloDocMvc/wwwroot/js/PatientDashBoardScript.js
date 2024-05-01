
var Lastvisited = null;
var RequestId = null;
$(document).ready(function () {
    Lastvisited = "DashVisited";
    $('.nav-item1').click();
    $(".patient-data-container").load('/PatientDashboard/PatientDashBoardTable')
})
$("#ProfileEdit").click(function () {
    $(".patient-data-container").load('/PatientDashboard/PatientProfileEdit', function () {

        $.ajax({
            url: '/PatientDashboard/LoadRegion',
            method: 'POST',
            success: function (response) {
                $('.FetchRegion').empty();
                $('.FetchRegion').append('<option value="" Selected disabled>Select Region</option>')
                response.forEach(function (regionname) {
                    //get region of patient
                    if (regionname.regionid == $("#RegionID").val()) {
                        $('.FetchRegion').append('<option value="' + regionname.regionid + '"selected >' + regionname.name + '</option>')
                    }
                    else {
                    $('.FetchRegion').append('<option value="' + regionname.regionid + '">' + regionname.name + '</option>')
                    }
                });
            }
        })
    });
});
$("#PatientDash").click(function () {
    Lastvisited = "DashVisited";
    $(".patient-data-container").load('/PatientDashboard/PatientDashBoardTable')
})
function ReqDoc(id) {

    Lastvisited = "DocVisited";
    RequestId = id;
    $(".patient-data-container").load('/PatientDashboard/PatientDocumentRequestWise?id=' + id)

}

function ProfileBack() {

    if (Lastvisited == "DashVisited") {
        $('.nav-item1').click();
        $(".patient-data-container").load('/PatientDashboard/PatientDashBoardTable')
    }
    else if (Lastvisited == "Me") {
        $('.nav-item1').click();
        $(".patient-data-container").load('/PatientDashboard/MeRequest')
    }
    else if (Lastvisited == "SomeOneElse") {
        $('.nav-item1').click();
        $(".patient-data-container").load('/PatientDashboard/SomeoneRequest')
    }
    else if (Lastvisited == "DocVisited") {
        $('.nav-item1').click();
        $(".patient-data-container").load('/PatientDashboard/PatientDocumentRequestWise?id=' + RequestId)
    }
    else {
        $('.nav-item1').click();
        $(".patient-data-container").load('/PatientDashboard/PatientDashBoardTable')

    }
}

function MeBack() {
    $(".patient-data-container").load('/PatientDashboard/PatientDashBoardTable');
}

function SomeOneBack() {
    $(".patient-data-container").load('/PatientDashboard/PatientDashBoardTable')
}
function DocBack() {
    $(".patient-data-container").load('/PatientDashboard/PatientDashBoardTable')
}

var Mevar = document.getElementById("#Me");
var someonevar = document.getElementById("#SomeoneElse");

document.getElementById("continue").addEventListener("click", function () {
    var options = document.querySelector('input[name="option"]:checked').id;
    if (options === "Me") {
        Lastvisited = "Me";
        $(".patient-data-container").load('/PatientDashboard/MeRequest', function () {

            $.ajax({
                url: '/PatientDashboard/LoadRegion',
                method: 'POST',
                success: function (response) {
                    $('.FetchRegion').empty();
                    $('.FetchRegion').append('<option value="" Selected disabled>Select Region</option>')
                    response.forEach(function (regionname) {
                        $('.FetchRegion').append('<option value="' + regionname.regionid + '">' + regionname.name + '</option>')
                    });
                }
            })
        })
    }
    else if (options === "SomeoneElse") {
        Lastvisited = "SomeOneElse";
        $(".patient-data-container").load('/PatientDashboard/SomeoneRequest', function () {

            $.ajax({
                url: '/PatientDashboard/LoadRegion',
                method: 'POST',
                success: function (response) {
                    $('.FetchRegion').empty();
                    $('.FetchRegion').append('<option value="" Selected>Select Region</option>')
                    response.forEach(function (regionname) {
                        $('.FetchRegion').append('<option value="' + regionname.regionid + '">' + regionname.name + '</option>')
                    })


                }
            })

        })
    }
});