$("#Admin-create-drop-Down").click(function () {
    $(".admin-data-container").load('/AdminPartials/CreateAdmin', function () {
        $.ajax({
            url: '/AdminPartials/LoadRegion',
            method: 'POST',
            success: function (response) {
                $('.FetchRegion').empty();
                $('.FetchRegion').append('<option value="" Selected disabled>Select Region</option>')
                response.region.forEach(function (regionname) {
                    $('.FetchRegion').append('<option value="' + regionname.regionid + '">' + regionname.name + '</option>')
                })
            }
        });
        $.ajax({
            url: '/AdminPartials/AccountaccessFetch',
            method: 'POST',
            success: function (response) {
                $('.loadRole').empty();
                $('.loadRole').append('<option value="" Selected disabled>Select Role</option>')
                response.roles.forEach(function (statusname) {
                    $('.loadRole').append('<option value="' + statusname.roleid + '">' + statusname.name + '</option>')
                })
            }
        });
    });
});


$("#LoadRole").on("change", function () {
    var roleid = $(this).val();
    if (roleid == -1) {
        roleid = null;
    }
    $(".admin-data-container").load('/AdminPartials/UserAccess?roletype=' + roleid, function () {
        $("#" + roleid).prop("selected", true);

        $.ajax({
            url: '/AdminPartials/AccountaccessFetch',
            method: 'POST',
            success: function (response) {
                $('#LoadRole').empty();
                $('#LoadRole').append('<option value="-1">All Roles</option>')
                response.roles.forEach(function (statusname) {

                    if (statusname.roleid == roleid) {
                        $("#LoadRole").attr("data-account-type", statusname.accounttype)
                        $('#LoadRole').append('<option value="' + statusname.roleid + '" id=" ' + statusname.roleid + '" selected>' + statusname.name + '</option>')
                    }
                    else {
                        $('#LoadRole').append('<option value="' + statusname.roleid + '" id=" ' + statusname.roleid + '">' + statusname.name + '</option>')
                    }
                })

                if ($("#LoadRole").attr("data-account-type") == "2") {
                    $("#Admin-create-drop-Down").prop("hidden", false);
                    $("#CreatePhysician").prop("hidden", true);
                }
                else if ($("#LoadRole").attr("data-account-type") == "1") {
                    $("#Admin-create-drop-Down").prop("hidden", true);
                    $("#CreatePhysician").prop("hidden", false);
                }
            }
        });
    });
});

$("#CreatePhysician").click(function () {

    localStorage.setItem("providerfrom", "user");
    $(".admin-data-container").load('/AdminPartials/CreateProvider', function () {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(showPosition);
        }
        function showPosition(position) {
            $("#Lattitude").val(position.coords.latitude);
            $('#Longtitude').val(position.coords.longitude);

        }
        $.ajax({
            url: '/AdminPartials/LoadRegion',
            method: 'POST',
            success: function (response) {
                $('.FetchRegion').empty();
                $('.FetchRegion').append('<option value="" Selected disabled>Select Region</option>')
                response.region.forEach(function (regionname) {
                    $('.FetchRegion').append('<option value="' + regionname.regionid + '">' + regionname.name + '</option>')
                })
            }
        });
        $.ajax({
            url: '/AdminPartials/AccountaccessFetch',
            method: 'POST',
            success: function (response) {
                $('.loadRole').empty();
                $('.loadRole').append('<option value="" Selected disabled>Select Role</option>')
                response.roles.forEach(function (statusname) {
                    if (statusname.accounttype == 1) {
                        $('.loadRole').append('<option value="' + statusname.roleid + '">' + statusname.name + '</option>')
                    }
                })
            }
        });
    });
});
function EditPhysicianProfile(phyid) {
    localStorage.setItem("providerfrom", "user");
    localStorage.setItem("profileEdit", "Provider");
    localStorage.setItem("phyid", phyid)
    $(".admin-data-container").load('/AdminPartials/EditPhysicianProfile?phyid=' + phyid, function () {
        // load status
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(showPosition);
        }
        function showPosition(position) {
            $("#Lattitude").val(position.coords.latitude);
            $('#Longtitude').val(position.coords.longitude);

        }
        $.ajax({
            url: '/AdminPartials/LoadStatus',
            method: 'POST',
            success: function (response) {
                response.forEach(function (status) {
                    if (($('.loadstatus').val()) == status.statusId) {
                    }
                    else {
                        $('.loadstatus').append('<option value="' + status.statusId + '">' + status.statusname + '</option>')
                    }
                });
            }
        })
        // load role
        $.ajax({
            url: '/AdminPartials/AccountaccessFetch',
            method: 'POST',
            success: function (response) {
                response.roles.forEach(function (role) {
                    if (($('.loadRole').val()) == role.roleid || role.accounttype != 1) {
                    }
                    else {
                        $('.loadRole').append('<option value="' + role.roleid + '">' + role.name + '</option>')
                    }
                });
            }
        });
        // load region
        $.ajax({
            url: '/AdminPartials/LoadRegion',
            method: 'POST',
            success: function (response) {
                response.region.forEach(function (regionname) {
                    if ($('.loadRegion').val() == regionname.regionid) {
                    } else {
                        $('.loadRegion').append('<option value="' + regionname.regionid + '">' + regionname.name + '</option>')
                    }
                });
            }
        });
    });
}
function EditAdmin(adminid) {
    localStorage.setItem("profileEdit", "custadmin");
    localStorage.setItem("adminid", adminid);
    $(".admin-data-container").load('/AdminPartials/AdminProfile?curadminid=' + adminid, function () {
        // load status
        $.ajax({
            url: '/AdminPartials/LoadStatus',
            method: 'POST',
            success: function (response) {
                response.forEach(function (status) {
                    if (($('.loadstatus').val()) == status.statusId) {
                    }
                    else {
                        $('.loadstatus').append('<option value="' + status.statusId + '">' + status.statusname + '</option>')
                    }
                });
            }
        })
        // load role
        $.ajax({
            url: '/AdminPartials/LoadRole',
            method: 'POST',
            success: function (response) {
                response.forEach(function (role) {
                    if (($('.loadRole').val()) == role.roleid) {
                    }
                    else {
                        $('.loadRole').append('<option value="' + role.roleid + '">' + role.name + '</option>')
                    }
                });
            }
        });
        // load region
        $.ajax({
            url: '/AdminPartials/LoadRegion',
            method: 'POST',
            success: function (response) {
                response.region.forEach(function (regionname) {
                    if ($('.loadRegion').val() == regionname.regionid) {

                    } else {
                        $('.loadRegion').append('<option value="' + regionname.regionid + '">' + regionname.name + '</option>')
                    }
                });
            }
        });
    });
};

function ManipulateData(PageNumber) {
    var roleid = $("#LoadRole").val();
    if (roleid == -1) {
        roleid = null;
    }
    $(".admin-data-container").load('/AdminPartials/UserAccess?roletype=' + roleid + '&PageNumber=' + PageNumber  , function () {
        $("#" + roleid).prop("selected", true);

        $.ajax({
            url: '/AdminPartials/AccountaccessFetch',
            method: 'POST',
            success: function (response) {
                $('#LoadRole').empty();
                $('#LoadRole').append('<option value="-1">All Roles</option>')
                response.roles.forEach(function (statusname) {

                    if (statusname.roleid == roleid) {
                        $("#LoadRole").attr("data-account-type", statusname.accounttype)
                        $('#LoadRole').append('<option value="' + statusname.roleid + '" id=" ' + statusname.roleid + '" selected>' + statusname.name + '</option>')
                    }
                    else {
                        $('#LoadRole').append('<option value="' + statusname.roleid + '" id=" ' + statusname.roleid + '">' + statusname.name + '</option>')
                    }
                })

                if ($("#LoadRole").attr("data-account-type") == "2") {
                    $("#Admin-create-drop-Down").prop("hidden", false);
                    $("#CreatePhysician").prop("hidden", true);
                }
                else if ($("#LoadRole").attr("data-account-type") == "1") {
                    $("#Admin-create-drop-Down").prop("hidden", true);
                    $("#CreatePhysician").prop("hidden", false);
                }
            }
        });
    });
}

function lastPage() {
    let lastpage = 999999;
    ManipulateData(lastpage);
};

function FirstPage() {
    ManipulateData();
}
function Previous() {
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