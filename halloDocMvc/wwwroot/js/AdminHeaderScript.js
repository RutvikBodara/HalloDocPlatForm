var CurrentColor ="#000"
$(document).ready(function () {
    if (localStorage.getItem("PageTheme") == "dark") {
        CurrentColor ="#fff"
    }
    localStorage.setItem("CurrentAccount", "Admin");
    $('#AdminDash').css({ "border-bottom": "solid 1px", "border-color": "#60cef3", "color": "#60cef3" })
});
$('#AdminDash').click(function () {

    $('.admin-btn').css({ "border-bottom": "none", "color": "" })
    $('#AdminDash').css({ "border-bottom": "solid 1px", "border-color": "#60cef3", "color": "#60cef3" })
});
$('#ProviderLocation').click(function () {
    $('.admin-btn').css({ "border-bottom": "none", "color": "" })
    $('#ProviderLocation').css({ "border-bottom": "solid 1px", "border-color": "#60cef3", "color": "#60cef3" })
});
$('#AdminProfileEdit').click(function () {
    $('.admin-btn').css({ "border-bottom": "none", "color": "" })
    $('#AdminProfileEdit').css({ "border-bottom": "solid 1px", "border-color": "#60cef3", "color": "#60cef3" })
});
$('#Provider').click(function () {
    $('.admin-btn').css({ "border-bottom": "none", "color": "" })
    $('#Provider').css({ "border-bottom": "solid 1px", "border-color": "#60cef3", "color": "#60cef3" })
});
$('#Partner').click(function () {
    $('.admin-btn').css({ "border-bottom": "none", "color": "" })
    $('#Partner').css({ "border-bottom": "solid 1px", "border-color": "#60cef3", "color": "#60cef3" })
});
$('#Access').click(function () {
    $('.admin-btn').css({ "border-bottom": "none", "color": "" })
    $('#Access').css({ "border-bottom": "solid 1px", "border-color": "#60cef3", "color": "#60cef3" })
});
$('#Records').click(function () {
    $('.admin-btn').css({ "border-bottom": "none", "color": "" })
    $('#Records').css({ "border-bottom": "solid 1px", "border-color": "#60cef3", "color": "#60cef3" })
});
$("#ProviderLocation").click(function () {
    $(".admin-data-container").load('/AdminPartials/LoadProviderLocationPartial')
})
$("#Provider-drop-down").click(function () {
    $(".admin-data-container").load('/AdminPartials/ProviderPage', function () {
        localStorage.setItem("lastregion", "");
    });
});
$("#Scheduling-drop-Down,#Scheduling-drop-Down-mobile").click(function () {
    $(".admin-data-container").load('/Schedule/SchedulingIndexPage');
});
$("#Account-access-drop-down").click(function () {
    $(".admin-data-container").load('/AdminPartials/Accountaccess');
});
$("#User-Access").click(function (e) {
    $(".admin-data-container").load('/AdminPartials/UserAccess', function () {
        $.ajax({
            url: '/AdminPartials/AccountaccessFetch',
            method: 'POST',
            success: function (response) {
                $('#LoadRole').empty();
                $('#LoadRole').append('<option value="-1">All Roles</option>')
                response.roles.forEach(function (statusname) {
                    $('#LoadRole').append('<option value="' + statusname.roleid + '" id=" ' + statusname.roleid + '" data-account-type="' + statusname.accounttype + '">' + statusname.name + '</option>')

                })
            }
        });

        //// check if the html5 history api is available in the browser first
        //if (window.history && window.history.pushState) {
        //    // push the state to the url in the address bar
        //    history.pushState({}, e.target.textContent, e.target.href);
        //}

    });
});
$("#Partner").click(function () {
    $(".admin-data-container").load('/AdminPartials/PartnerIndex', function () {
    });
});
$("#Search-Records").click(function () {
    $(".admin-data-container").load('/AdminPartials/SearchRecordsIndex', function () {
    });
});
$("#Patient-Records").click(function () {
    $(".admin-data-container").load('/AdminPartials/PatientRecordsIndex', function () {
    });
});
$("#Email-Logs,#SMS-Logs").click(function () {
    var id = 1;
    if ($(this).attr("id") == "SMS-Logs") {
        id = 2;
    }

    $(".admin-data-container").load('/AdminPartials/EmailLogsIndex?type=' + id, function () {
        $("#EmailLogs").load("/AdminPartials/EmailLogRecords?type=" + id);
        localStorage.setItem("currentLog", id);

    });
});

$("#Blocked-History").click(function () {
    $(".admin-data-container").load('/AdminPartials/BlockHistoryIndex', function () {
    });
});