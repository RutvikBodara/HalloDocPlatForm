$(document).ready(function () {
    $(".PhysicianNotessubmit").on("submit", function (e) {
        e.preventDefault();

        var formData = new FormData($(this)[0]);
        $.ajax({
            url: '/ProviderAccount/AddProviderNotes',
            method: 'POST', // Explicitly set method as POST
            data: formData,
            processData: false, // Don't process the data
            contentType: false, // Don't set contentType
            success: function (response) {
                var id = response.requestid;
                toastr.success('Notes Uploaded Successfully!');
                $(".Provider-data-container").load('/ProviderAccount/ShowViewNotes?requestid=' + id);
            },
            error: function (xhr, status, error) {
            }
        });
    });
});