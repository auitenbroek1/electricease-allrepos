
var DistAssemblyExportTbl;

$(document).ready(function () {
    $("#Loading").hide();
    loadDistAssemblyExportGrid();
    loadClients();

    //row click operation
    $('#DistAssemblyExportTbl tbody').on('click', 'tr', function (event) {
        if (event.target.type !== 'checkbox') {
            $(':checkbox', this).trigger('click');
        }
    });

    //Check box operation
    $('#DistAssemblyExportTbl tbody').on('click', 'input[type="checkbox"]', function (e) {
        var checkBoxId = $(this).val();
        var rowIndex = -1;

        //Checking if the Element is in the array.
        $.each(selectedAssembiles, function (index, obj) {
            $.each(obj, function (key, value) {
                if (value === checkBoxId) {
                    rowIndex = index;
                }
            });
        });

        //rowIndex = $.inArray(checkBoxId, selectedAssembiles); //Checking if the Element is in the array.
        if (this.checked && rowIndex === -1) {
            selectedAssembiles.push({ "Name": checkBoxId }); // If checkbox selected and element is not in the list->Then push it in array.
        }
        else if (!this.checked && rowIndex !== -1) {
            selectedAssembiles.splice(rowIndex, 1); // Remove it from the array.
        }
    });
});

function loadAssemblyAndClients() {
    loadClients();
    loadDistAssemblyExportGrid();
}

function loadDistAssemblyExportGrid() {
    var disID = $('#DistributorID').val();
    if (disID === "") disID = 0;
    $('#DistAssemblyExportTbl').DataTable().destroy();
    DistAssemblyExportTbl = $('#DistAssemblyExportTbl').DataTable({
        //"deferRender": true,//to speedup the data loading
        "aLengthMenu": [[5, 10, 15, 25, -1], [5, 10, 15, 25, "All"]],
        "sDom": '<"top"flp>rt<"bottom"pri>',
        "processing": true,
        "language": {
            loadingRecords: 'Loading...',
            processing: '<div class="dtspinner"></div>'
        },
        "orderCellsTop": true,
        "ajax": {

            url: "../DistributorAssembly/GetAssembliesByDistributorID",
            data: { distributorID: disID },
            type: 'GET',
            datatype: "json"
        },
        "filterDropDown": {
            columns: [
                {
                    idx: 2
                }
            ],
            bootstrap: true
        },
        "columns": [
            {
                width: "10%",
                orderable: false,
                mRender: function (data, type, row) {
                    var assemblyName = row.Assemblies_Name.replace(/ /g, "a_a");
                    var actionControls = '<input id="Check_' + assemblyName + '" type="checkbox" value="' + row.Assemblies_Name + '" />';
                    return actionControls;
                }
            },
            { width: "20%", data: "Assemblies_Name" },
            { width: "15%", data: "Assemblies_Category" },
            { width: "25%", data: "Assemblies_Description" },
            { width: "10%", data: "assemblypartsCount" },
            { width: "10%", data: "Type" }
        ],
        "order": [[0, "asc"]]
    });
}

function loadClients() {
    var disID = $('#DistributorID').val();
    if (disID === "") disID = 0;
    $.ajax({
        url: '../DistributorAssembly/GetClientsByDistributorID',
        type: 'GET',
        data: { distributorID: disID },
        success: function (data) {
            //followed the existing code structure to retrive the multi select dropdown style. Its worst case. should recover in revamp
            $("#drpDownClientDiv").html(data);
            //$("#drpDownClient").find('option[value!=""]').remove();
            //var $clientlist = $("#drpDownClient");

            //$.each(data.data.ListData, function (i, clientList) {
            //    $('<option>', {
            //        value: clientList.value
            //    }).html(clientList.Name).appendTo($clientlist);
            //});

            $('#Client_ID').multiselect({
                includeSelectAllOption: true
            });
        }
    });
}

function ValidateLaborUnit() {
    $("#Loading").show();
    var Did = $("#DistributorID").val();

    if (Did !== 0 && Did !== "" && Did !== null) {
        if (ClienIds.length > 0) {
            if (selectedAssembiles.length > 0) {

                var model = {
                    "DisbutorId": Did,
                    "ClienId": ClienIds,
                    "selectedAssembiles": selectedAssembiles
                };

                $.ajax({
                    url: '../DistributorAssembly/ValidateDistributorClientsLaborUnit',
                    type: 'POST',
                    data: JSON.stringify(model),
                    cache: false,
                    contentType: 'application/json',
                    success: function (data) {
                        if (data === "Success") {
                            $("#Loading").hide();
                            alert("Labor units are validated successfully!");
                            window.location.href = "../DistributorAssembly/DistributorAssemblyExport";
                        }
                        else {
                            $("#Loading").hide();
                            DisplayNotification(data, "error");
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $("#Loading").hide();
                        DisplayNotification(xhr.status, "error");
                    }
                });
            }
            else {
                $("#Loading").hide();
                DisplayNotification("Please select Assemblies!", "error");
            }
        }
        else {
            $("#Loading").hide();
            DisplayNotification("Please select Distributor clients!", "error");
        }
    }
    else {
        $("#Loading").hide();
        DisplayNotification("Please select the Distributor!", "error");
    }
}

function ExportSelectAll(val) {

    var table = $('#DistAssemblyExportTbl').DataTable();

    var allPages = table.rows().nodes();

    if (val.checked) {
        $("input:checkbox", allPages).each(function () {
            var alreadyAdded = false;
            var value = this.value;
            if (selectedAssembiles.indexOf(value) === -1)
                $.map(selectedAssembiles, function (elementOfArray, indexInArray) {
                    if (elementOfArray.Name === value) {
                        alreadyAdded = true;
                    }
                });
            if (!alreadyAdded) {
                if (value !== "selectall") {
                    selectedAssembiles.push({ "Name": value });
                }

            }
            this.checked = true;
        });
        if (table.data().count()) {
            DisplayNotification("\"Select All\" check box has \"Selected all the Entries\"", "success");
        }
    }
    else {
        $(allPages).find("input:checkbox").each(function () {
            selectedAssembiles = [];
            this.checked = false;
        });
        if (table.data().count()) {
            DisplayNotification("Deselected all the Entries", "success");
        }
    }
}

