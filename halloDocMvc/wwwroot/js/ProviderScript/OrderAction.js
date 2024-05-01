function OrderCheckOutPage(id) {
    $(".Provider-data-container").load('/ProviderAccount/OrderCheckOutPage?ReqId=' + id);
};

$('#Profession').on("change", function () {
    var value = $(this).val();
    $.ajax({
        url: '/ProviderAccount/GetBusinessByProfession',
        data: { ProfessionId: value },
        method: 'POST',
        success: function (response) {
            $('#BusinessName').empty();
            $('#BusinessName').append('<option value="" Selected disabled>--Business--</option>')
            response.businessData.forEach(function (BusinessValue) {
                $('#BusinessName').append('<option value="' + BusinessValue.vendorid + '">' + BusinessValue.vendorname + '</option>')
            })
            $('#BusinessContact').val("");
            $('#BusinessEmail').val("");
            $('#FaxNumber').val("");
        }
    });
});
$('#BusinessName').on("change", function () {
    var value = $(this).val();
    $.ajax({
        url: '/ProviderAccount/GetBusinessData',
        data: { VendorId: value },
        method: 'POST',
        success: function (response) {
            $('#BusinessContact').val(response.businesscontact);
            $('#BusinessEmail').val(response.email);
            $('#FaxNumber').val(response.faxnumber);
        }
    });

});
