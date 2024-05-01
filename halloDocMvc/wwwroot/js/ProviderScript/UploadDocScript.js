function DeletedAll() {
    let count = 0;
    let checkboxes = document.querySelectorAll(".checkbtn");
    var filesna = document.querySelectorAll(".filesna");
    checkboxes.forEach(function (checkbox) {
        if (checkbox.checked) {
            DeleteSeperateFile(filesna[count].innerHTML);
        }
        count++;
    });
}

$('.UploadFile').on('submit', function (e) {
    e.preventDefault();
    var formData = new FormData($(this)[0]);
    $.ajax({
        url: '/ProviderAccount/Upload_View_Document',
        data: formData,
        method: 'POST',
        processData: false,
        contentType: false,
        success: function (req_id) {
            toastr.success("File Uploaded Successfully");
            $(".Provider-data-container").load('/ProviderAccount/View_Document?id=' + req_id);
        }
    });
});

function DeleteSeperateFile(fileName) {
    $.ajax({
        url: '/ProviderAccount/DeleteSeperateFile',
        data: { fileName: fileName },
        method: 'POST',
        success: function (req_id) {
            if (!req_id) {
                toastr.error("File Not Deleted");
                $(".Provider-data-container").load('/ProviderAccount/View_Document?id=' + req_id);
            }
            else {
                toastr.success("File Deleted Successfully");
                $(".Provider-data-container").load('/ProviderAccount/View_Document?id=' + req_id);
            }
        }
    });
}
