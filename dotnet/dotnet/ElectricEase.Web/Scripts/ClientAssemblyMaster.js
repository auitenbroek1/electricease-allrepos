//Purpose: To do the script operations of Client's Assembly Master Page
//Created By: T. Arun Kumar
//Created On: 10-Oct-19

var partsList;
var ClientAssemblyMasterDataTable;

$(document).ready(function () {
    InitiateColumnSearch();
    loadGrid();
    loadPartsPopupGrid();
    //FillPartsPopup();

    $("#AddParts").click(function () {
        saveparts();//To continue existing function
    });

    //row click operation
    $('#ClientPartsTable tbody').on('click', 'tr', function (event) {
        if (event.target.type !== 'checkbox') {
            $(':checkbox', this).trigger('click');
        }
    });

    $('#ClientPartsTable tbody').on('click', 'input[type="checkbox"]', function (e) {
        var checkBoxId = $(this).val();
        var rowIndex;

        //Assemblyparts Operations
        rowIndex = $.inArray(checkBoxId, selectedpart); //Checking if the Element is in the array.
        if (this.checked && rowIndex === -1) {
            selectedpart.push(checkBoxId); // If checkbox selected and element is not in the list->Then push it in array.
        }
        else if (!this.checked && rowIndex !== -1) {
            selectedpart.splice(rowIndex, 1); // Remove it from the array.
        }
    });

    //To check re check check boxes when the page loads
    $("#ClientPartsTable").on('draw.dt', function () {
        $('#ClientPartsTable tbody input[type=checkbox]').each(function () {
            if ($.inArray($(this).val(), selectedpart) === -1) {
                if ($(this).is(':checked')) {
                    $(this).click();
                }
            }
            else {
                if (!$(this).is(':checked')) {
                    $(this).click();
                }
            }
        });
    });
});

function loadGrid() {
    ClientAssemblyMasterDataTable = $('#ClientAssemblyMasterDataTable').DataTable({
        "deferRender": true,//to speedup the data loading
        "aLengthMenu": [[5, 10, 15, 25, -1], [5, 10, 15, 25, "All"]],
        "sDom": '<"top"flp>rt<"bottom"pri>',
        "order": [[5, "desc"]],
        "ajax": {
            url: "../Assemblies_Master/GetAssembliesList",
            type: 'GET',
            datatype: "json"
        },
        "filterDropDown": {
            columns: [
                {
                    idx: 1
                }
            ],
            bootstrap: true
        },
        "columns": [
            { width: "20%", data: "Assemblies_Name" },
            { width: "20%", data: "Assemblies_Category" },
            { width: "30%", data: "Assemblies_Description" },
            { width: "7%", data: "assemblypartsCount" },
            { width: "10%", data: "Type" },
            {
                data: "Updated_Date",
                visible: false
            },
            {
                width: "8%",
                mRender: function (data, type, row) {
                    var name = encodeURI(row.Assemblies_Name);
                    var actionControls = '<a href="javascript:void(0);" onclick="editassembly(\'' + name + '\')">'
                        + '<span class="glyphicon glyphicon-pencil"></span></a>';
                    if (row.IsActive === true) {
                        actionControls = actionControls.concat('<a href="javascript:void(0);" onclick="deleteAssembly(\'' + name + '\')">'
                            + '<span class="glyphicon glyphicon-trash"></span></a>');
                    }
                    else {
                        $(this).closest('tr').addClass("deletedparts");
                    }
                    return actionControls;
                }
            }
        ],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            if (aData["assemblypartsCount"] === 0) {
                $(nRow).addClass('deletedparts');
            }
        }
    });
}

function InitiateColumnSearch() {
    $('#ClientPartsTable thead tr').clone(true).appendTo('#ClientPartsTable thead');
    $('#ClientPartsTable thead tr:eq(1) th').each(function (i) {
        var title = $(this).text();
        if (title !== "Select") {
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');

            //search by "oninput" method
            $('input', this).on('input', function () {
                if (ClientPartsTable.column(i).search() !== this.value) {
                    ClientPartsTable
                        .column(i)
                        .search(this.value)
                        .draw();
                }
            });
        }
        else {
            $(this).html('');
        }
    });
}

function loadPartsPopupGrid() {
    ClientPartsTable = $('#ClientPartsTable').DataTable({
        "deferRender": true,//to speedup the data loading
        "aLengthMenu": [[5, 10, 15, 25], [5, 10, 15, 25]],
        "sDom": '<"top"flp>rt<"bottom"pri>',
        "processing": true,
        "language": {
            loadingRecords: '&nbsp;',
            processing: '<div class="dtspinner"></div>'
        },
        "orderCellsTop": true,
        "ajax": {
            url: "../Assemblies_Master/GetPartsListByClientID",
            type: 'GET',
            datatype: "json"
        },
        "columns": [
            {
                width: "2%",
                orderable: false,
                mRender: function (data, type, row) {
                    var partNumber = row.Part_Number.replace(/ /g, "_");
                    var actionControls = '<input id="chkPart_' + partNumber + '" type="checkbox" value="' + row.Part_Number + '"/>';
                    return actionControls;
                }
            },
            { width: "20%", data: "Part_Category" },
            { width: "30%", data: "Description" },
            { width: "20%", data: "Part_Number" },
            { width: "14%", data: "Cost" },
            { width: "14%", data: "Resale_Cost" }
        ],
        "filterDropDown": {
            columns: [
                {
                    idx: 1
                }
            ],
            bootstrap: true
        },
        "order": [[3, "asc"]]
    });
}

function OpenPartsPopup() {
    if (validateBeforePopup()) {
        selectedpart = [];

        KeyPopupStatus = "Open";
        KeyCurrentOperation = "JobParts";
        $('#PartsLabortblInfo tr ').each(function () {
            var partNumber = $(this).find("#partnumber").text().trim();//.replace(/ /g, "_");
            selectedpart.push(partNumber);
        });

        //To clear the individual search text boxes in datatable
        $('#ClientPartsTable thead tr:eq(1) th input').each(function (i) {
            $(this).val('');
        });

        $('#ClientPartsTable_filterSelect1').val('');
        ClientPartsTable.columns().search('').draw();

        $("#OpenPartsPopUp").click();
    }
}

function validateBeforePopup() {
    if ($("#Assemblies_Name").val().trim() === "" || $("#Assemblies_Category").val() === "" || $("#Assemblies_Description").val().trim() === "") {
        DisplayNotification("Please fill all the mandatory fields.", "error");
        return false;
    }
    return true;
}

//To set the regex validaitons for the text boxes on the key press event
function setRegexValidations() {
    $("[name = 'txtCost']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'txtResaleCost']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'txtEstimatedQty']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'txtLaborUnit']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,4})?$/ });
    $("#assmeblymasterinfo_labor_cost").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("#assmeblymasterinfo_Lobor_Resale").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
}



