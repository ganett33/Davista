function loadDataTable() {
    dataTable = $('#tblData_house').DataTable({
        "ajax": {
            "url": "/Webmanager/House/GetAll"
        },
        "columns": [
            { "data": "projectType.projectTypeName", "width": "15%" },
            { "data": "houseName", "width": "15%" },
            { "data": "houseDescription", "width": "40%" },
            {
                "data": "houseImage",
                "render": function (data) {
                    return `
                    <img src="${data}" alt="${data.houseName}" style="max-width: 200px; max-height: 200px;" />
                    `;
                },
                "width": "20%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="d-flex justify-content-center">
                            <a href="/Webmanager/House/Upsert?id=${data}" 
                                class="btn btn-primary text-white m-1 p-2 rounded-1 edit-link">
                                <i class="fa-regular fa-pen-to-square "></i>
                            </a>
                            <div class="delete-section">
                                <a href="#" data-id="${data}"
                                    class="btn btn-danger text-white m-1 p-2 rounded-1 delete-link">
                                    <i class="fa-solid fa-trash-can "></i>
                                </a>
                                <div class="btn btn-outline-danger delete-confirm" style="display:none" data-delete-id="${data}">Confirm</div>
                            </div>
                        </div>
                    `
                },
                "width": "10%"
            }
        ]
    });
}

// Event delegation for delete links
$('#tblData_house').on('click', '.delete-link', function () {
    var deleteLink = $(this);
    var confirmButton = deleteLink.siblings(".delete-confirm");


    deleteLink.hide();
    confirmButton.show();

    var cancelDelete = function () {
        removeEvents();
        showDeleteLink();
    };

    var deleteItem = function () {
        removeEvents();
        confirmButton.hide();

        var itemId = deleteLink.data('id');

        $.post(
            '/Webmanager/House/Delete/' + itemId,
            AddAntiForgeryToken({ id: itemId })
        ).done(function () {
            dataTable.row(deleteLink.closest("tr")).remove().draw();
        }).fail(function () {
            alert("Error deleting the item.");
        });

        return false;
    };

    var removeEvents = function () {
        confirmButton.off("click", deleteItem);
        $(document).off("click", cancelDelete);
        $(document).off("keypress", onKeyPress);
    };

    var showDeleteLink = function () {
        confirmButton.hide();
        deleteLink.show();
        editLink.show();
    };

    var onKeyPress = function (e) {
        // Cancel if the escape key is pressed
        if (e.which == 27) {
            cancelDelete();
        }
    };

    confirmButton.on("click", deleteItem);
    $(document).on("click", cancelDelete);
    $(document).on("keypress", onKeyPress);

    return false;
});

// AddAntiForgeryToken function remains the same
AddAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

// Initialize the DataTable
loadDataTable();