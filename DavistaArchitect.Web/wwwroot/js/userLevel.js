var dataTable;

$(document).ready(function () {
	loadDataTable();
});

function loadDataTable() {
	dataTable = $('#tblData_userLevel').DataTable({
		"ajax": {
			"url": "/Webmanager/UserLevel/GetAll"
		},
		"columns": [
			{ "data": "name", "width": "15%" },
			{ "data": "description", "width": "20%" },
			{ "data": "level", "width": "5%" },
			{ "data": "authorityLevel", "width": "50%" },
			{
				"data": "id",
				"render": function (data) {
					return `
						<div class="btn-group-vertical " role="group">
							<a href="/Webmanager/UserLevel/Upsert?id=${data}" class="btn btn-primary text-white m-1 p-1 rounded-1"><i class="fa-regular fa-pen-to-square "></i></a>
							<a onClick=Delete('/Webmanager/UserLevel/Delete/${data}') class="btn btn-danger text-white m-1 p-1 rounded-1"> <i class="fa-solid fa-trash-can "></i></a>
						</div>
						`
				},
				"width": "10%"
			}
		]

	});
}




function Delete(url) {
	Swal.fire({
		title: 'Are you sure?',
		text: "You won't be able to revert this!",
		icon: 'warning',
		showCancelButton: true,
		confirmButtonColor: '#3085d6',
		cancelButtonColor: '#d33',
		confirmButtonText: 'Yes, delete it!'
	}).then((result) => {
		if (result.isConfirmed) {
			$.ajax({
				url: url,
				type: 'DELETE',
				success: function (data) {
					if (data.success) {
						dataTable.ajax.reload();
						toasts.success(data.message);
					} else {
						toasts.error(data.message);
					}
				}
			})
		}
	})
}