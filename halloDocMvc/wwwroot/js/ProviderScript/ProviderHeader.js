$(document).ready(function () {
    localStorage.setItem("CurrentAccount", "Provider");
    $('.Provider-btn').css({ "border": "none" })
    $('#ProviderDash').css({ "border-bottom": "solid 1px", "border-color": "#60cef3", "color": "#60cef3" })

    if (localStorage.getItem("RequestState") != null) {
        var reqType = localStorage.getItem("RequestState");
        showsvg(reqType);
        $(".table-container").load('/ProviderAccount/' + reqType);
    }
    else {
        showsvg('NewRequests');
    }
});
$('#ProviderDash').click(function () {
    $('.Provider-btn').css({ "border-bottom": "none", "color": "" })
    $('#ProviderDash').css({ "border-bottom": "solid 1px", "border-color": "#60cef3", "color": "#60cef3" })
});
$('#MySchedule').click(function () {
    $('.Provider-btn').css({ "border-bottom": "none", "color": "" })
    $('#MySchedule').css({ "border-bottom": "solid 1px", "border-color": "#60cef3", "color": "#60cef3" })
});
$('#ProviderMyProfile').click(function () {
    $('.Provider-btn').css({ "border-bottom": "none", "color": ""})
    $('#ProviderMyProfile').css({ "border-bottom": "solid 1px", "border-color": "#60cef3", "color": "#60cef3" })
});
$("#ProviderInvoicing").click(function () {
    $('.Provider-btn').css({ "border-bottom": "none", "color": "" })
    $('#ProviderInvoicing').css({ "border-bottom": "solid 1px", "border-color": "#60cef3", "color": "#60cef3" })
});

$('#MySchedule').click(function () {
    $(".Provider-data-container").load('/ProviderAccount/MySchedule', function () {
    });
});

$("#ProviderMyProfile").click(function () {
    localStorage.setItem("providerfrom", "Provider");
    localStorage.setItem("profileEdit", "Provider");
    $(".Provider-data-container").load('/ProviderAccount/EditPhysicianProfile', function () {
    });
})
$("#ProviderInvoicing").click(function () {
    $(".Provider-data-container").load('/ProviderAccount/LoadInvoiceIndex', function () {
    });
});
