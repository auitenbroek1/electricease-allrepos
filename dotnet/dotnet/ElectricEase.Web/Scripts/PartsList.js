//Purpose: To do the script operations of Client's Parts Master Page
//Created By: T. Arun Kumar
//Created On: 14-Oct-19

var ClientPartsMasterDataTable;

function InitiateColumnSearch() {
    $('#ClientPartsMasterDataTable thead tr').clone(true).appendTo('#ClientPartsMasterDataTable thead');
    $('#ClientPartsMasterDataTable thead tr:eq(1) th').each(function (i) {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="Search ' + title + '" />');

        //search by "oninput" method
        $('input', this).on('input', function () {
            if (ClientPartsMasterDataTable.column(i).search() !== this.value) {
                ClientPartsMasterDataTable
                    .column(i)
                    .search(this.value)
                    .draw();
            }
        });
    });
}

function loadGrid() {
    ClientPartsMasterDataTable = $('#ClientPartsMasterDataTable').DataTable({
        "deferRender": true,//to speedup the data loading
        "aLengthMenu": [[5, 10, 15, 25], [5, 10, 15, 25]],
        "sDom": '<"top"flp>rt<"bottom"pri>',
        "processing": true,
        "language": {
            loadingRecords: 'Loading...',
            processing: '<div class="dtspinner"></div>' 
        },
        "orderCellsTop": true,
        "order": [[8, "desc"]],
        "ajax": {
            url: "../Parts_Master/GetPartsListByClientID",
            type: 'GET',
            datatype: "json"
        },
        "columns": [
            { width: "10%", data: "Part_Category" },
            { width: "20%", data: "Description" },
            { width: "15%", data: "Part_Number" },
            { width: "5%", data: "Cost" },
            { width: "5%", data: "Resale_Cost" },
            { width: "5%", data: "UOM" },
            { width: "15%", data: "Purchased_From" },
            { width: "12%", data: "Type" },
            {
                data: "Updated_Date",
                visible: false
            },
            {
                width: "7%",
                mRender: function (data, type, row) {
                    var actionControls = '<a href="javascript:void(0);" onclick="editParts(\'' + row.Part_Number.trim().replace(/\\/g, "\\\\").replace("'", "\'") + '\',\'' + row.Client_ID + '\');">'
                        + '<span class="glyphicon glyphicon-pencil"></span></a>';

                    actionControls = actionControls.concat('<a href="javascript:void(0);" onclick="deleteParts(\'' + row.Part_Number.trim().replace(/\\/g, "\\\\").replace("'", "\'") + '\',\'' + row.Client_ID + '\')">'
                        + '<span class="glyphicon glyphicon-trash"></span></a>');

                    return actionControls;
                }
            }
        ],
        "filterDropDown": {
            columns: [
                {
                    idx: 0
                }
            ],
            bootstrap: true
        }
    });
}

function setRegexValidations() {
    $("#Cost").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("#Rcost").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("#LaborUnit").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,4})?$/ });
}

$("#LaborUnit").keyup(function () {
    handleDecimalDot($(this));
});

