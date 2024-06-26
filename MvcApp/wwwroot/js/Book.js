﻿var dataTable;

$(document).ready(function () {
    LoadDataTable();
});

function LoadDataTable() {
    dataTable = $('#bookTable').DataTable({
        "ajax": { url: '/admin/book/getall' },
        "columns": [
            {
                data:"imageUrl",
                "render": function (data) {
                    return `<img src="${data}" alt="Book Image" style="width:100%">`;
                },
                "width": "15%"
            },
            { data: 'title', "width": "15%" },
            { data: 'author', "width": "15%" },
            { data: 'price', "width": "15%" },
            { data: 'category.name', "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group " role="group">
                     <a href="/admin/book/upsert?id=${data}" class="btn btn-primary text-white "> <i class="bi bi-pencil-square"></i> Edit</a>               
                     <a href="/customer/home/details/${data}" class="btn btn-primary  text-white" ><i class="bi bi-info-circle-fill"></i>Info</a>
                     <a onClick=Delete('/admin/book/delete/${data}') class="btn btn-danger  text-white"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`;
                },
                "width": "15%"
            }
        ],
        
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure you wan't to delete this book?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
        else {
            console.log("Oooop! something went wrong");
        }
    });
}
