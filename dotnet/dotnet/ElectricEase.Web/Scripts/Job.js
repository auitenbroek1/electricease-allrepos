//Job operations - After starts to re-engineering by Arun

var AsName;
var rownum;
var ClientPartsTable;
var KeyCurrentOperation;//This should be populate before opening parts popup 
var KeyPopupStatus;//This is to know whether parts popup closed with out saving it.
var BackupPartsList;//To backup the parts list getting from the database.
var BackupSelectedParts;//To backup the parts list selected from the popup
var CurrentAssemblyName;

$(document).ready(function () {
    InitiateColumnSearch();
    loadPartsPopupGrid();
    setRegexValidations();

    $("#AddParts").click(function () {
        KeyPopupStatus = "Closed";

        $('#ClosePartsPopUp').click();

        if (KeyCurrentOperation === "JobAssemblyParts") {
            saveassemblyparts(CurrentAssemblyName);//To continue existing function
        }
        else if (KeyCurrentOperation === "JobParts") {
            saveparts('selected');//To continue existing function
        }
    });

    //row click operation
    $('#ClientPartsTable tbody').on('click', 'tr', function (event) {
        if (event.target.type !== 'checkbox') {
            $(':checkbox', this).trigger('click');
        }
    });

    //Check box operation
    $('#ClientPartsTable tbody').on('click', 'input[type="checkbox"]', function (e) {
        var checkBoxId = $(this).val();
        var rowIndex;

        if (KeyCurrentOperation === "JobAssemblyParts") {
            //Assemblyparts Operations
            rowIndex = $.inArray(checkBoxId, assemblySelectedParts); //Checking if the Element is in the array.
            if (this.checked && rowIndex === -1) {
                assemblySelectedParts.push(checkBoxId); // If checkbox selected and element is not in the list->Then push it in array.
            }
            else if (!this.checked && rowIndex !== -1) {
                assemblySelectedParts.splice(rowIndex, 1); // Remove it from the array.
            }
        }
        else if (KeyCurrentOperation === "JobParts") {
            //Job Parts Operations
            rowIndex = $.inArray(checkBoxId, partsselectedpart); //Checking if the Element is in the array.
            if (this.checked && rowIndex === -1) {
                partsselectedpart.push(checkBoxId); // If checkbox selected and element is not in the list->Then push it in array.
            }
            else if (!this.checked && rowIndex !== -1) {
                partsselectedpart.splice(rowIndex, 1); // Remove it from the array.
            }
        }
    });

    //To check re check check boxes when the page loads
    $("#ClientPartsTable").on('draw.dt', function () {

        var processArray;
        if (KeyCurrentOperation === "JobAssemblyParts") {
            processArray = assemblySelectedParts.slice();
        }
        else if (KeyCurrentOperation === "JobParts") {
            processArray = partsselectedpart.slice();
        }
        $('#ClientPartsTable tbody input[type=checkbox]').each(function () {
            if ($.inArray($(this).val(), processArray) === -1) {
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

    //Delete Operation in Job-Assembly-Parts
    $(document).on('click', '.remove-part', function () {
        if (!confirm('Do you want to delete this part? Press OK to continue.')) {
            DisplayNotification('User cancelled the operation!', "error");
        }
        else {

            //To reset the calculation
            $(this).closest('tr').find('[name="TxtEstQty"]').val(0);
            var TxtEstQty = $(this).closest('tr').find('[name="TxtEstQty"]');

            //Old method to get AsName and Rowno
            GetAsNameRowNum(TxtEstQty[0]);

            //Remove this in assemblySelectedParts list
            var partNumber = $(this).closest('tr').find("#partnumber").text();
            assemblySelectedParts.splice(assemblySelectedParts.indexOf(partNumber), 1);

            //To get the current table
            var currentPartsTable = $(this).closest('div').find('[name="AssemblyPartsTable"]');

            //Removing the row
            $(this).closest("tr").remove();

            //To Calculate the parts - version: 2
            Calculate(currentPartsTable, AsName, rownum);
        }
    });
});

function OpenPartsPopup() {
    partsselectedpart = [];

    KeyPopupStatus = "Open";
    KeyCurrentOperation = "JobParts";
    $('#partstbody tr ').each(function () {
        var partNumber = $(this).find("#partnumber").text().trim();//.replace(/ /g, "_").replace("+", "");
        partsselectedpart.push(partNumber);
    });

    //To clear the individual search text boxes in datatable
    $('#ClientPartsTable thead tr:eq(1) th input').each(function (i) {
        $(this).val('');
    });

    $('#ClientPartsTable_filterSelect1').val('');
    ClientPartsTable.columns().search('').draw();
    $("#OpenPartsPopUp").click();
}

function ClosePartsPopup() {
    if (KeyPopupStatus === "Open") {
        if (KeyCurrentOperation === "JobAssemblyParts") {
            assemblySelectedParts = BackupSelectedParts;
        }
        else if (KeyCurrentOperation === "JobParts") {
            partsselectedpart = BackupSelectedParts;
        }
    }
}

function loadPartsPopupGrid() {
    ClientPartsTable = $('#ClientPartsTable').DataTable({
        "deferRender": true,//to speedup the data loading
        "aLengthMenu": [[5, 10, 15, 25], [5, 10, 15, 25]],
        "sDom": '<"top"flp>rt<"bottom"pri>',
        "orderCellsTop": true,
        "processing": true,
        "language": {
            loadingRecords: '&nbsp;',
            processing: '<div class="dtspinner"></div>'
        }, 
        "ajax": {
            url: "../Job/GetPartsListByClientID",
            type: 'GET',
            datatype: "json"
        },
        "columns": [
            {
                width: "2%",
                orderable: false,
                mRender: function (data, type, row) {
                    var partNumber = row.Part_Number.replace(/ /g, "_").replace("+", "");
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

//Version - 2 to calculate the current parts
function CalculateV2(val) {
    GetAsNameRowNum(val);

     //To get the current assembly name
    if (AsName === "") {
        AsName = $(val).closest('tr').find('[name="hdnAssemblyName"]').val();
    }

    //To get the current row number
    var tr = $(val).closest('table').closest('tr');
    rownum = tr.prev().find('input[name="AssemblyIndex"]').val();


    var currentPartsTable =  $(val).closest('div').find('[name="AssemblyPartsTable"]');
    Calculate(currentPartsTable, AsName, rownum);
}

//old method - to be remove in future
function GetAsNameRowNum(val) {
    //Old Calculations - starts
    AsName = val;
    var asnname = val;
    if (typeof val.id != 'undefined') {
        var idsplit = val.id.split('_');
        AsName = idsplit[0];
        for (var i = 1; i < idsplit.length - 2; i++) {
            AsName += "_" + idsplit[i];
        }
        asnname = $(val).parent().parent().parent().parent().parent().parent();
        asnname = asnname[0].id;
        asnname = asnname.substr(6, 1);
    }
    if (AsName.indexOf(' ') > -1) {
        AsName = AsName.replace(/ /g, "_");
    }

    rownum = $('#' + AsName + '-' + asnname).attr('data-row');

    //Old Calculations - Ends
}

function Calculate(table, AsName, rownum) {
    PartsRowCalculation(table);
    LaborRowCalculation(table);
    PartsCalculation(table);
    CalcLaborTotal(table);
    CalcPartsGrandTotal(table);
    ContinueV1Calculation(AsName, rownum);
    bid_summary_update_labor_row_1();
}

//To Calculate the individual rows
function PartsRowCalculation(table) {

    var costTxtBoxes = table.find('[name = "txtPartsCost"]');
    var estQtyTxtBoxes = table.find('[name = "TxtEstQty"]');
    var estCostTotalTxtBoxes = table.find('[name = "EstCost_Total"]');
    MultiplyOnSeries(costTxtBoxes, estQtyTxtBoxes, estCostTotalTxtBoxes);

    var resalecostTxtBoxes = table.find('[name = "txtPartsResaleCost"]');
    estQtyTxtBoxes = table.find('[name = "TxtEstQty"]');
    var estResaleTotalTxtBoxes = table.find('[name = "EstResaleCost"]');
    MultiplyOnSeries(resalecostTxtBoxes, estQtyTxtBoxes, estResaleTotalTxtBoxes);

    //Labor unit calculation
    var LaborUnitTxtBoxes = table.find('[name = "txtLaborUnit"]');
    estQtyTxtBoxes = table.find('[name = "TxtEstQty"]');
    var estLaborUnitTxtBoxes = table.find('[name = "txtCalcLaborUnit"]');
    MultiplyOnSeries(LaborUnitTxtBoxes, estQtyTxtBoxes, estLaborUnitTxtBoxes, 4);
    LaborValue = AdditionOnSeries(estLaborUnitTxtBoxes, 4);
    table.find('[name="assmeblymasterinfo.Estimated_Hour"]').val(LaborValue); 

    // update the assembly item
    var $assembly_parts = table.parents('[data-assembly-parts]');
    var $assembly_item = $assembly_parts.prev();
    $assembly_item.attr('data-labor-hours', LaborValue);

    var actualQtyTxtBoxes = table.find('[name = "txtActualQty"]');
    var actualTotalTxtBoxes = table.find('[name = "txtActualTotal"]');
    MultiplyOnSeries(resalecostTxtBoxes, actualQtyTxtBoxes, actualTotalTxtBoxes);
}

//To Calculate the Labor rows
function LaborRowCalculation(table) {

    var costTxtBoxes = table.find('[name = "assmeblymasterinfo.labor_cost"]');
    var estQtyTxtBoxes = table.find('[name = "assmeblymasterinfo.Estimated_Hour"]');
    var estCostTotalTxtBoxes = table.find('[name = "assmeblymasterinfo.LaborEst_CostTotal"]');
    MultiplyOnSeries(costTxtBoxes, estQtyTxtBoxes, estCostTotalTxtBoxes);

    var resalecostTxtBoxes = table.find('[name = "assmeblymasterinfo.Lobor_Resale"]');
    var estResaleTotalTxtBoxes = table.find('[name = "assmeblymasterinfo.LaborEst_ResaleTotal"]');
    MultiplyOnSeries(resalecostTxtBoxes, estQtyTxtBoxes, estResaleTotalTxtBoxes);
}

//To Calculate values for parts
function PartsCalculation(table) {

    //To Calculate the total of Estimated cost of all parts
    var currentEstCostTotalTxtBoxes = table.find('[name="EstCost_Total"]');
    var EstCostTotal = AdditionOnSeries(currentEstCostTotalTxtBoxes);
    table.find('[name="assmeblymasterinfo.PartCostTotal"]').val(EstCostTotal.toFixed(2));

    //To Calculate the total of Resale cost of all parts
    var currentEstResaleCostTotalTxtBoxes = table.find('[name="EstResaleCost"]');
    var EstResaleCostTotal = AdditionOnSeries(currentEstResaleCostTotalTxtBoxes);
    table.find('[name="assmeblymasterinfo.PartResaleTotal"]').val(EstResaleCostTotal.toFixed(2));

    //To Calculate the total of Resale cost of all parts
    var currentActualTotalTxtBoxes = $(table).find('[name="txtActualTotal"]');
    var ActualCostTotal = AdditionOnSeries(currentActualTotalTxtBoxes);
    table.find('[name="assmeblymasterinfo.PartsActualTotal"]').val(ActualCostTotal.toFixed(2));
}

//To Calculate the values for labor
function CalcLaborTotal(table) {

    //To Fill Actual Quantity
    var estimatedHours = table.find('[name="assmeblymasterinfo.Estimated_Hour"]').val();
    table.find('[name="assmeblymasterinfo.labour_actual_hours"]').val(estimatedHours);

    //To Calculate Est.Cost of Labor
    var cost = table.find('[name="assmeblymasterinfo.labor_cost"]').val();
    table.find('[name="assmeblymasterinfo.LaborEst_CostTotal"]').val((cost * estimatedHours).toFixed(2));

    //To Calculate Est.Resale of Labor
    var ResaleCost = table.find('[name="assmeblymasterinfo.Lobor_Resale"]').val();
    table.find('[name="assmeblymasterinfo.LaborEst_ResaleTotal"]').val((ResaleCost * estimatedHours).toFixed(2));

    //To Calculate Actual Total
    var actualQuantity = table.find('[name="assmeblymasterinfo.labour_actual_hours"]').val();
    table.find('[name="assmeblymasterinfo.labour_actual_total"]').val((ResaleCost * actualQuantity).toFixed(2));
}

//To Calculate the values grand total of labor
function CalcPartsGrandTotal(table) {

    //To Calculate Grand Est.Cost Total
    var totalPartsEstCost = Number(table.find('[name="assmeblymasterinfo.PartCostTotal"]').val());
    var totalLaborEstCost = Number(table.find('[name="assmeblymasterinfo.LaborEst_CostTotal"]').val());
    table.find('[name="assmeblymasterinfo.GrandCostTotal"]').val((totalPartsEstCost + totalLaborEstCost).toFixed(2));

    //To Calculate Grand Resale Total
    var totalPartsResaleCost = Number(table.find('[name="assmeblymasterinfo.PartResaleTotal"]').val());
    var totalLaborResaleCost = Number(table.find('[name="assmeblymasterinfo.LaborEst_ResaleTotal"]').val());
    table.find('[name="assmeblymasterinfo.GrandResaleTotal"]').val((totalPartsResaleCost + totalLaborResaleCost).toFixed(2));

    //To Calculate Grand Actual Total
    var totalGrandParts = Number(table.find('[name="assmeblymasterinfo.PartsActualTotal"]').val());
    var totalGrandLabor = Number(table.find('[name="assmeblymasterinfo.labour_actual_total"]').val());
    table.find('[name="assmeblymasterinfo.GrandActualTotal"]').val((totalGrandParts + totalGrandLabor).toFixed(2));
}

function ChangeActualV2(val) {

    //To fill the estimated quantity box
    var TxtEstQty = $(val).closest('tr').find('[name="TxtEstQty"]').val();
    var value = Number(TxtEstQty);
    if (isNaN(value)) {
        value = 0;
    }
    $(val).closest('tr').find('[name="txtActualQty"]').val(value.toFixed(0));

    //To fill the actual total box
    var EstResaleCost = $(val).closest('tr').find('[name="EstResaleCost"]').val();
    value = Number(EstResaleCost);
    if (isNaN(value)) {
        value = 0;
    }
    $(val).closest('tr').find('[name="txtActualTotal"]').val(value.toFixed(2));
}

//To be modified in future
function ContinueV1Calculation(AsName, rownum) {
    calculatemultiplier(AsName, rownum);
    estimatevalues();
    footertotal();
}

//To set the regex validaitons for the text boxes on the key press event
function setRegexValidations() {

    //Job Assembly
    $("[name = 'txtAssemblyMultiplier']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'txtAssmEstQty']").limitkeypress({ rexp: /^(\d{1,4})?$/ });
    $("[name = 'txtAssmActualQty']").limitkeypress({ rexp: /^(\d{1,4})?$/ });

    //Job Assembly Parts
    $("[name = 'txtPartsCost']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'txtPartsResaleCost']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'TxtEstQty']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'txtLaborUnit']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,4})?$/ });
    $("[name = 'assmeblymasterinfo.labor_cost']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'assmeblymasterinfo.Lobor_Resale']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });

    //Job Parts
    $("[name = 'txtJobPartsCost']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'txtJobPartsResaleCost']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'txtJobPartsEstQty']").limitkeypress({ rexp: /^(\d{1,4})?$/ });
    $("[name = 'txtJobPartsActualQty']").limitkeypress({ rexp: /^(\d{1,4})?$/ });

    //Job Labor
    $("[name = 'txtLaborCost']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'txtLaborResaleCost']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'txtLaborEstimatedHours']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'txtLaborActualHours']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });

    //Job DJE/VQ
    $("[name = 'txtDJECost']").limitkeypress({ rexp: /^(\d{1,6})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'txtDJEProfit']").limitkeypress({ rexp: /^(\d{1,3})?(\.)?(\.\d{1,2})?$/ });

    //Job Estimate - Estimate 1
    $("#Estimate1_AssemblyTax").limitkeypress({ rexp: /^(\d{1,3})?(\.)?(\.\d{1,2})?$/ });
    $("#Estimate1_LaborTax").limitkeypress({ rexp: /^(\d{1,3})?(\.)?(\.\d{1,2})?$/ });
    $("#Estimate1_PartsTax").limitkeypress({ rexp: /^(\d{1,3})?(\.)?(\.\d{1,2})?$/ });
    $("#Estimate1_DJETax").limitkeypress({ rexp: /^(\d{1,3})?(\.)?(\.\d{1,2})?$/ });
    $("#Estimate1_VQTax").limitkeypress({ rexp: /^(\d{1,3})?(\.)?(\.\d{1,2})?$/ });

    //Job Estimate - Estimate 2
    $("#Estimate2_AssemblyProfit").limitkeypress({ rexp: /^(\d{1,3})?(\.)?(\.\d{1,2})?$/ });
    $("#Estimate2_AssemblyTax").limitkeypress({ rexp: /^(\d{1,3})?(\.)?(\.\d{1,2})?$/ });
    $("#Estimate2_LaborProfit").limitkeypress({ rexp: /^(\d{1,3})?(\.)?(\.\d{1,2})?$/ });
    $("#Estimate2_LaborTax").limitkeypress({ rexp: /^(\d{1,3})?(\.)?(\.\d{1,2})?$/ });
    $("#Estimate2_PartsProfit").limitkeypress({ rexp: /^(\d{1,3})?(\.)?(\.\d{1,2})?$/ });
    $("#Estimate2_PartsTax").limitkeypress({ rexp: /^(\d{1,3})?(\.)?(\.\d{1,2})?$/ });
    $("#Estimate2_DJEProfit").limitkeypress({ rexp: /^(\d{1,3})?(\.)?(\.\d{1,2})?$/ });
    $("#Estimate2_DJETax").limitkeypress({ rexp: /^(\d{1,3})?(\.)?(\.\d{1,2})?$/ });
    $("#Estimate2_VQProfit").limitkeypress({ rexp: /^(\d{1,3})?(\.)?(\.\d{1,2})?$/ });
    $("#Estimate2_VQTax").limitkeypress({ rexp: /^(\d{1,3})?(\.)?(\.\d{1,2})?$/ });

    //Job Estimate - Estimate 2
    $("#Estimation3_Override").limitkeypress({ rexp: /^(\d{1,10})?(\.)?(\.\d{1,2})?$/ });
}