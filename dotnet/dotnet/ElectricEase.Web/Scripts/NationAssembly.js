var isloaded = false;
var isediting = false;
var isDuplicating = false;
var category = "";
var isassembly = 0;
var selectedpart = [];
var updatestatus = 1;
var partsList;
var NationwideAssemblyMasterDataTable;

$(document).ready(function () {
    Cookies.set('NAssemdtpageno', "");
    Cookies.set('NAssemdtpagelen', "");
    Cookies.set('NAssemsearch', "");
    Cookies.set('NAssemdtsorting', "");
    Cookies.set('nationpartsearch', "");
    $('li').removeClass('active');
    $("#Nassemblies").addClass('active');
    $(".content-title").text("Nationwide Assemblies Master");

    $(window).unload(function () {
        Cookies.set('NAssemsearch', '');
        Cookies.set("assemfilter", '');
    });
    $("button").click(function () {
        $("p").slideToggle();
    });
    getassemblyInfo('');
    //getassemblyList();

    loadGrid();
    InitiateColumnSearch();
    //FillPartsPopup();
    loadPartsPopupGrid();
    $("#AddParts").click(function () {
        saveparts();//To continue existing function
    });

    //row click operation
    $('#ClientPartsTable').on('click', 'tbody tr', function (event) {
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

    $("#Assemblies_Category").select2();

    setRegexValidations();

});

function loadGrid() {
    NationwideAssemblyMasterDataTable = $('#NationwideAssemblyMasterDataTable').DataTable({
        "deferRender": true,//to speedup the data loading
        "aLengthMenu": [[5, 10, 15, 25, -1], [5, 10, 15, 25, "All"]],
        "sDom": '<"top"flp>rt<"bottom"pri>',
        "language": {
            loadingRecords: 'Loading...'
        },
        "ajax": {
            url: "../NationwideAssembly/GetNationWideAssembliesGrid",
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
            { width: "6%", data: "assemblypartsCount" },
            {
                data: "Updated_Date",
                visible: false
            },
            {
                width: "9%",
                mRender: function (data, type, row) {
                    var actionControls = '<a href="javascript:void(0);" onclick="editassembly(\'' + row.Assemblies_Name + '\',\'' + row.Assemblies_Id +'\')">'
                        + '<span class="glyphicon glyphicon-pencil"></span></a>';

                    actionControls = actionControls.concat('<a href="javascript:void(0);" onclick="duplicateAssembly(\'' + row.Assemblies_Name + '\',\'' + row.Assemblies_Id + '\')">'
                        + '<span class="glyphicon glyphicon-file"></span></a>');

                    if (row.IsActive === true) {
                        actionControls = actionControls.concat('<a href="javascript:void(0);" onclick="deleteAssembly(\'' + row.Assemblies_Name + '\',\'' + row.Assemblies_Id + '\')">'
                            + '<span class="glyphicon glyphicon-trash"></span></a>');
                    }
                    
                    return actionControls;
                }
            }
        ],
        "order": [[4, "desc"]],
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
            url: "../NationwideParts/GetNationwidePartsList",
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
            if (partNumber !== "") {
                selectedpart.push(partNumber);
            }
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

function toggleChevron(e) {
    $(e.target)
        .prev('.panel-heading')
        .find("i.indicator")
        .toggleClass('glyphicon-chevron-down glyphicon-chevron-up');
}
$('#accordion').on('hidden.bs.collapse', toggleChevron);
$('#accordion').on('shown.bs.collapse', toggleChevron);
$("#pdfbtn").on("click", function () {

    window.open("../App/ShowMasterPDF?PdfPath=" + "~/HelpPdf/AssemblyMaster.pdf");
})
function getassemblyInfo(val, val1) {
    $.ajax({
        url: '../NationwideAssembly/AssemblyInfo?Name=' + encodeURIComponent(val) + '&AssebliesId=' + val1,
        type: 'GET',
        success: function (data) {
            $('#divasseminfo').html(data);
            $("#Assemblies_Category").select2();
            $('#partstable').find("input:checkbox").each(function () {
                if (this.checked == true) {
                    this.checked = false;
                }
            });

            if (val != "0" && val != "") {

                isassembly = 1;
                updatestatus = 0;
                saveparts();

            }
            else {
                $("#Isactive").bootstrapToggle('on');
            }
            $('#Addbtn').text("Update");

            if (isDuplicating) {
                $("#Assemblies_Name").prop("disabled", false);
                $("#Assemblies_Name").val($("#Assemblies_Name").val() + ' - copy');
                isDuplicating = false;
            }
        },
        error: function (err) {

        }
    });
}
function getassemblyList(val, showBot) {
    if (val == null || typeof val === 'undefined')
        val = "";
    $.ajax({
        url: '../NationwideAssembly/getallAssembliesList?assembName=' + val,
        type: 'GET',
        //dataType: "json",
        success: function (data) {
            $('#divassemblylist').html(data);
            setTimeout(function () {
                NationwideAssemblylistcategory('');
                if (val != "")
                    $('#lstassembly_cat').val(val);
            }, 2000);
            if (showBot === 1) {
                $("#divNAssBot").focus();
            }
        },
        error: function (err) {
        }
    });
}
//function getparts(val) { 
//    $.ajax({
//        url: '../NationwideAssembly/AssemblyPartsLaborInfo?Name=' + val,
//        type: 'GET',
//        success: function (data) {
//            $('#divpartsinfo').html(data);

//            if ($('#partstbody tr').length > 0) {
//                $('#PartsLabortblInfo').show();
//            }
//        },
//        error: function (err) {
//        }
//    });
//}

function editassembly(val, val1) {
    $("#Assemblies_Name").focus();
    $("#updatebtn").show();
    $("#Cancel").show();
    $("#addbtn").hide();
    $("#Reset").hide();
    //$('#Addbtn').text("Update");
    $('#partstbody').html("");
    selectedpart = [];
    isloaded = false;
    isediting = true;
    getassemblyInfo(val, val1);
}

function duplicateAssembly(name, id) {

    $("#Assemblies_Name").focus();
    $("#addbtn").show();
    $("#Cancel").show();
    $("#updatebtn").hide();
    $("#Reset").hide();
    $('#partstbody').html("");
    selectedpart = [];
    isloaded = false;
    isDuplicating = true;
    getassemblyInfo(name, id);
}

function modalAssignToRM() {

    selectedpart = [];
    var assemblyname = $('#Assemblies_Name').val();
    var lstpartnum = "";
    var tablRows = $('#partstbody tr');
    // var tablRows = $('#partstbody tr')
    i = 0;
    if (tablRows.length > 0) {
        $('#partstbody tr ').each(function () {
            var customerId = $(this).find("#partnumber").html();
            lstpartnum += customerId.trim() + ",";
            selectedpart.push(customerId.trim());
        });
    }
    if ($('#laborDatatbl tr').length > 0) {
        updatestatus = 1;
    }
    $.ajax({
        url: '../NationwideAssembly/GetallAssemblyParts',
        data: { name: assemblyname, lstparts: lstpartnum },
        type: 'POST',
        success: function (data) {

            $('#divpartsdata').html(data);
            $('#openpopup').click();
            //if (selectedpart.length == 0) {

            //}
            //$('#partstable').find("input:checkbox").each(function () {
            //    if (this.checked == true) {
            //        i++
            //    }
            //    var tablerowlength = $("#partsbody tr").length;
            //    if (tablerowlength == i) {
            //        document.getElementById("selectall").checked = true;
            //    }
            //});

            //if (i == 0) {
            //    $("#Npsubmit").attr('disabled', 'disabled');
            //}
            //$('#Part_Category').val("ALL");
        },
        error: function (err) {
        }
    });
}
function ShowOtherCategory(val) {
    var lstpartnum = "";
    if (val == "") {
        val = "ALL";
    }
    var tablRows = $('#partstbody tr')
    if (tablRows.length > 0) {
        $('#partstbody tr ').each(function () {
            var customerId = $(this).find("#partnumber").html();
            lstpartnum += customerId.trim() + ",";
        });
    }
    var AssID = $("#AsId").val();
    $.ajax({
        url: '../NationwideAssembly/GetallAssemblyParts?PartCatgory=' + val + "&lstparts=" + lstpartnum + "&AssebliesId=" + AssID,
        type: 'Get',
        success: function (data) {
            $('#divpartsdata').html(data);
            if (val == "") {
                val = "ALL";
                $('#Part_Category').val(val);
            }
            else {
                $('#Part_Category').val(val);
            }
        },
        Error: function (err) { }

    });
}
function checknum(element) {

    var values = $(element).val();

    var rgx = /^[0-9]*\.?[0-9]*$/;
    if (values.match(rgx)) {

    }
    else {
        $(element).val(0);
        //$(element).val($(element).val().replace(/\D/, ''));
    }
}

function saveparts() {

    var lstpart = "";
    //  var assemblyname = $('#Assemblies_Name').val();
    var AsName = $('#Assemblies_Name').val().replace(/\s\s+/g, ' ');
    var AsID = $("#AsId").val();
    var PartstblRows = $('#partstbody tr');
    var oldlabour = null;
    var oldPartsData = new Array();
    var oldLaborData = new Array();
    var newPartsData = new Array();
    if (PartstblRows.length > 0) {
        var txtLaborUnit = $(PartstblRows).find("[name='txtLaborUnit']");
        //var LabortablRows = $('#LabortblInfo tr')
        for (var i = 0; i < PartstblRows.length; i++) {
            var activevalue = $(PartstblRows[i]).find('#activeparts').text().trim();
            if (activevalue == "true") {
                oldPartsData.push({
                    'IsActivePartsDetails': $(PartstblRows[i]).find('#activeparts').text().trim(),
                    'Part_Number': $(PartstblRows[i]).find('#partnumber').text(),
                    'Part_Category': $(PartstblRows[i]).find('#Part_Category').text().trim(),
                    'Parts_Description': $(PartstblRows[i]).find('#Parts_Description').text().trim(),
                    'Part_Cost': $(PartstblRows).find('#' + "P_" + i + "_0").val(),
                    'Resale_Cost': $(PartstblRows).find('#' + "P_" + i + "_1").val(),
                    'Estimated_Qty': $(PartstblRows).find('#' + "P_" + i + "_2").val(),
                    'EstCost_Total': $(PartstblRows).find('#' + "P_" + i + "_3").val(),
                    'EstResaleCost_Total': $(PartstblRows).find('#' + "P_" + i + "_4").val(),
                    'LaborUnit': $(txtLaborUnit[i]).val()
                });
            }
        }
    }
    if (updatestatus === 1) {

        if ($('#laborDatatbl tr').length > 0) {
            var llabour = $('#laborDatatbl tr');
            oldlabour = '<tr><th colspan="3">Labor</th><th>';
            oldlabour += '<input type="text" id="assmeblymasterinfo_labor_cost" value="' + $('#laborDatatbl tr')[1].cells[1].children[0].value.trim() + '" class = "form-control"  onkeyup="checknum(this);OnKeypressEvent();"  style="text-align:right"/>';

            oldlabour += '</th><th>';
            oldlabour += '<input type="text"  id="assmeblymasterinfo_Lobor_Resale" value="' + $('#laborDatatbl tr')[1].cells[2].children[0].value.trim() + '" class = "form-control" onkeyup="OnKeypressEvent();"  style="text-align:right"/>';
            oldlabour += '</th><th>';
            oldlabour += '<input type="text"  id="assmeblymasterinfo_Estimated_Hour" value="' + $('#laborDatatbl tr')[1].cells[3].children[0].value.trim() + '" readonly class = "form-control"  onkeyup="OnKeypressEvent();"  style="text-align:right"/>';
            oldlabour += '</th><th>';
            oldlabour += '</th><th>';
            oldlabour += '<input type="text"  id="assmeblymasterinfo_LaborEst_CostTotal" value="' + $('#laborDatatbl tr')[1].cells[5].children[0].value.trim() + '" class = "form-control" readonly = "readonly"onkeyup="OnKeypressEvent();"  style="text-align:right"/>';
            oldlabour += '</th><th>';
            oldlabour += '<input type="text"  id="assmeblymasterinfo_LaborEst_ResaleTotal" value="' + $('#laborDatatbl tr')[1].cells[6].children[0].value.trim() + '" class = "form-control" readonly = "readonly"onkeyup="OnKeypressEvent();"  style="text-align:right"/>';
            oldlabour += '</th>';
            oldlabour += '</tr>';
        }

    }

    //if (document.getElementById("selectall") != null) {
    //    if (document.getElementById("selectall").checked === true) {
    //        lstpart = "selectall";
    //    }
    //    else {
            if (selectedpart !== null && selectedpart.length > 0) {
                for (var p = 0; p < selectedpart.length; p++) {
                    if (lstpart === "")
                        lstpart += selectedpart[p];
                    else
                        lstpart += "," + selectedpart[p];
                }
            }
    //    }
    //}

    //$('#popupclose').click();
    $("#ClosePartsPopUp").click();

    $.ajax({
        url: '../NationwideAssembly/SelectedAssemblyPartsLaborInfo',
        data: { lstparts: lstpart, AssebliesId: AsID, SearchStr: Cookies.get('nationpartsearch') },
        type: 'POST',
        success: function (data) {

            //var totlength=Number(oldPartsData.length)+Number(data.length);
            var trow = '';
            var estimatedqty = 0.00;
            var estimatedcost = 0.00;
            var estimatedresalecost = 0.00;
            var GrandCostTot = 0.00;
            var GrandCostResaleTot = 0.00;
            var disabled = "";

            for (var i = 0; i < data.partslist.length; i++) {

                var isavailable = 0;
                for (var j = 0; j < oldPartsData.length; j++) {

                    if (data.partslist[i].Part_Number == oldPartsData[j].Part_Number) {

                        trow += '<tr>';
                        trow += '<td id="activeparts" style="display:none">' + oldPartsData[j].IsActivePartsDetails + '</td>';
                        trow += '<td id="partnumber">' + oldPartsData[j].Part_Number + '</td>';
                        trow += '<td id="Part_Category">' + oldPartsData[j].Part_Category + '</td>';
                        if (oldPartsData[j].Parts_Description == null || oldPartsData[j].Parts_Description == "null") {
                            oldPartsData[j].Parts_Description = "";
                        }
                        trow += '<td id="Parts_Description">' + oldPartsData[j].Parts_Description + '</td><td>';

                        //Cost
                        var Partscostid = "P_" + i + "_0";
                        trow += '<input name="txtCost" type="text" id="' + Partscostid + '" class="form-control" value="' + oldPartsData[j].Part_Cost + '" onkeyup="checknum(this);calculatecost(this);" style="text-align:right" />';
                        trow += '</td><td>';

                        //Resale Cost
                        var Rcostid = "P_" + i + "_1";
                        trow += '<input name="txtResaleCost" type="text" id="' + Rcostid + '" class="form-control" value="' + oldPartsData[j].Resale_Cost + '" onkeyup="checknum(this);calculatecost(this);" style="text-align:right" />';
                        trow += '</td><td>';

                        //Est.Qty
                        estimatedqty = Number(estimatedqty) + Number(oldPartsData[j].Estimated_Qty);
                        var EstQtyid = "P_" + i + "_2";
                        var EstQty = oldPartsData[j].Estimated_Qty == 0 ? "0" : oldPartsData[j].Estimated_Qty;
                        EstQty = Number(EstQty);
                        trow += '<input type="text" name="txtEstimatedQty" id="' + EstQtyid + '" class="form-control" value="' + EstQty + '" onkeyup="checknum(this);calculatecost(this);" style="text-align:right" />';
                        trow += '</td><td>';

                        //Labor Unit
                        trow += '<input type="text" name="txtLaborUnit" class="form-control" value="' + oldPartsData[j].LaborUnit + '" onkeyup="checknum(this);calculatecost(this);" style="text-align:right" />';
                        trow += '</td><td>';

                        estimatedcost = Number(estimatedcost) + Number(oldPartsData[j].EstCost_Total);
                        var EstCostTotid = "P_" + i + "_3";
                        trow += '<input type="text" id="' + EstCostTotid + '" class="form-control" disabled value="' + oldPartsData[j].EstCost_Total + '" onkeyup="checknum(this);calculatecost(this);" style="text-align:right" />';

                        //Calc Labor Unit 
                        trow += '<input type="hidden" name="txtCalcLaborUnit" class="form-control" />';
                        trow += '</td><td>';

                        estimatedresalecost = Number(estimatedresalecost) + Number(oldPartsData[j].EstResaleCost_Total);
                        var EstRCostTotid = "P_" + i + "_4";
                        trow += '<input type="text" id="' + EstRCostTotid + '" class="form-control" disabled value="' + oldPartsData[j].EstResaleCost_Total + '" onkeyup="checknum(this);calculatecost(this);" style="text-align:right" />';
                        trow += '</td>';
                        trow += '</tr>';
                        isavailable = Number(isavailable) + Number(1);

                    }
                }
                if (isavailable == 0) {

                    var disabled = "";
                    //if (data.assmeblymasterinfo != null && data.assmeblymasterinfo.assemblypartsCount == 0 || data.partslist[i].IsActivePartsDetails == false) {
                    //    trow += '<tr style="background-color:#e74c3c" class="deletedparts">';
                    //    disabled = "disabled";
                    //}
                    //else {
                        trow += '<tr>';
                    //}
                    trow += '<td id="activeparts" style="display:none">' + data.partslist[i].IsActivePartsDetails + '</td>';
                    trow += '<td id="partnumber">' + data.partslist[i].Part_Number + '</td>';
                    trow += '<td id="Part_Category">' + data.partslist[i].Part_Category + '</td>';
                    if (data.partslist[i].Parts_Description == null || data.partslist[i].Parts_Description == "null") {
                        data.partslist[i].Parts_Description = "";
                    }
                    trow += '<td id="Parts_Description">' + data.partslist[i].Parts_Description + '</td><td>';

                    //Cost
                    var Partscostid = "P_" + i + "_0";
                    trow += '<input type="text" name="txtCost" ' + disabled + ' id="' + Partscostid + '" class="form-control" value="' + data.partslist[i].Part_Cost + '" onkeyup="checknum(this);calculatecost(this);" style="text-align:right" />';
                    trow += '</td><td>';

                    //Resale Cost
                    var Rcostid = "P_" + i + "_1";
                    trow += '<input type="text" name="txtResaleCost"  ' + disabled + ' id="' + Rcostid + '" class="form-control" value="' + data.partslist[i].Resale_Cost + '" onkeyup="checknum(this);calculatecost(this);" style="text-align:right" />';
                    trow += '</td><td>';
                    if (data.assmeblymasterinfo != null && data.assmeblymasterinfo.assemblypartsCount == 0) {
                        estimatedqty = "0";
                    }
                    else {
                        if (data.partslist[i].IsActivePartsDetails == true) {
                            estimatedqty = Number(estimatedqty) + Number(data.partslist[i].Estimated_Qty);
                        }
                    }

                    //Est.Qty
                    var EstQtyid = "P_" + i + "_2";
                    var EstQty = data.partslist[i].Estimated_Qty == 0 ? "0" : data.partslist[i].Estimated_Qty;
                    EstQty = Number(EstQty);
                    trow += '<input type="text" name="txtEstimatedQty" ' + disabled + ' id="' + EstQtyid + '" class="form-control" value="' + EstQty + '"onkeyup="checknum(this);calculatecost(this);" style="text-align:right" />';
                    trow += '</td><td>';

                    //Labor Unit
                    trow += '<input type="text" name="txtLaborUnit" class="form-control" value="' + data.partslist[i].LaborUnit + '" onkeyup="checknum(this);calculatecost(this);" style="text-align:right" />';
                    trow += '</td><td>';

                    //Here calculate Estimate cost total
                    if (data.assmeblymasterinfo != null && data.assmeblymasterinfo.assemblypartsCount == 0) {
                        estimatedcost = "0";
                    }
                    else {
                        if (data.partslist[i].IsActivePartsDetails == true) {
                            estimatedcost = Number(estimatedcost) + Number(data.partslist[i].EstCost_Total);
                        }
                    }
                    //estimatedcost = Number(estimatedcost) + Number(data.partslist[i].EstCost_Total);
                    var EstCostTotid = "P_" + i + "_3";
                    var EstCostTot = data.partslist[i].EstCost_Total == 0 ? "0" : data.partslist[i].EstCost_Total;
                    trow += '<input type="text" id="' + EstCostTotid + '" class="form-control" disabled value="' + EstCostTot + '" onkeyup="checknum(this);calculatecost(this);" style="text-align:right" />';
                    //Calc Labor Unit 
                    trow += '<input type="hidden" name="txtCalcLaborUnit" class="form-control" />';
                    trow += '</td><td>';
                    //Here calculate Estimate resalecost total
                    if (data.assmeblymasterinfo != null && data.assmeblymasterinfo.assemblypartsCount == 0) {
                        estimatedresalecost = "0";
                    }
                    else {
                        if (data.partslist[i].IsActivePartsDetails == true) {
                            estimatedresalecost = Number(estimatedresalecost) + Number(data.partslist[i].EstResaleCost_Total);
                        }
                    }
                    //estimatedresalecost = Number(estimatedresalecost) + Number(data.partslist[i].EstResaleCost_Total);
                    var EstRCostTotid = "P_" + i + "_4";
                    var EstRCostTot = data.partslist[i].EstResaleCost_Total == 0 ? "0" : data.partslist[i].EstResaleCost_Total;
                    trow += '<input type="text" id="' + EstRCostTotid + '" class="form-control" disabled value="' + EstRCostTot + '" onkeyup="checknum(this);calculatecost(this);" style="text-align:right" />';
                    trow += '</td>';
                    trow += '</tr>';
                }
            }


            $('#partstbody').html(trow);
            var footerrow = '<tr><th colspan="5">Total</th><th>';

            var EstQtyTot = estimatedqty == 0 ? "0" : estimatedqty;
            footerrow += '<input type="text" id="assmeblymasterinfo_Estimated_Qty_Total" value="' + Number(EstQtyTot) + '" class = "form-control Estimate-FooterValue" readonly = "readonly" style="text-align:right"  />';
            footerrow += '</th><th>';
            footerrow += '</th><th>';
            var estimatedcostTot = estimatedcost == 0 ? "0" : estimatedcost;
            footerrow += '<input type="text"  id="assmeblymasterinfo_PartCostTotal" value="' + Number(estimatedcostTot).toFixed(2) + '" class = "form-control Estimate-FooterValue" readonly = "readonly"  style="text-align:right"/>';
            footerrow += '</th><th>';
            var estimatedresalecostTot = estimatedresalecost == 0 ? "0" : estimatedresalecost;
            footerrow += '<input type="text"  id="assmeblymasterinfo_PartResaleTotal" value="' + Number(estimatedresalecostTot).toFixed(2) + '" class = "form-control Estimate-FooterValue" readonly = "readonly" style="text-align:right"/>';
            footerrow += '</th>';
            footerrow += '</tr>';

            var footerlabourrow = "";

            try {
                if (isassembly == 1) {

                    if (oldlabour == null || oldlabour == "") {

                        footerlabourrow = '<tr><th colspan="3">Labor</th><th>';
                        footerlabourrow += '<input type="text" id="assmeblymasterinfo_labor_cost" value="' + data.assmeblymasterinfo.labor_cost + '" class = "form-control"  onkeyup="checknum(this);OnKeypressEvent();" style="text-align:right"/>';
                        //GrandCostTot = Number(estimatedcost) + Number(data.assmeblymasterinfo.labor_cost);
                        footerlabourrow += '</th><th>';
                        footerlabourrow += '<input type="text"  id="assmeblymasterinfo_Lobor_Resale" value="' + data.assmeblymasterinfo.Lobor_Resale + '" class = "form-control" onkeyup="checknum(this);OnKeypressEvent();" style="text-align:right"/>';
                        //GrandCostResaleTot = Number(estimatedresalecost) + Number(data.assmeblymasterinfo.Lobor_Resale);
                        footerlabourrow += '</th><th>';
                        var EstHr = data.assmeblymasterinfo.Estimated_Hour == 0 ? "0" : data.assmeblymasterinfo.Estimated_Hour;
                        // if (EstHr == "" || EstHr == "0")
                        //    EstHr = ""
                        //else
                        //    EstHr = Number(EstHr).toFixed(0);
                        footerlabourrow += '<input type="text"  id="assmeblymasterinfo_Estimated_Hour" value="' + EstHr + '" readonly class = "form-control"  onkeyup="checknum(this);OnKeypressEvent();" style="text-align:right"/>';
                        footerlabourrow += '</th><th>';
                        footerlabourrow += '</th><th>';
                        footerlabourrow += '<input type="text"  id="assmeblymasterinfo_LaborEst_CostTotal" value="' + data.assmeblymasterinfo.LaborEst_CostTotal + '" class = "form-control" readonly = "readonly"onkeyup="checknum(this);OnKeypressEvent();" style="text-align:right"/>';
                        footerlabourrow += '</th><th>';
                        footerlabourrow += '<input type="text"  id="assmeblymasterinfo_LaborEst_ResaleTotal" value="' + data.assmeblymasterinfo.LaborEst_ResaleTotal + '" class = "form-control" readonly = "readonly"onkeyup="checknum(this);OnKeypressEvent();" style="text-align:right"/>';
                        footerlabourrow += '</th>';
                        footerlabourrow += '</tr>';
                    }
                    else {
                        footerlabourrow = oldlabour;

                    }
                }
                else {

                    if (oldlabour == null || oldlabour == "") {

                        footerlabourrow = '<tr><th colspan="3">Labor</th><th>';
                        footerlabourrow += '<input type="text" id="assmeblymasterinfo_labor_cost" value="" class = "form-control"  onkeyup="checknum(this);OnKeypressEvent();" style="text-align:right"/>';
                        footerlabourrow += '</th><th>';
                        footerlabourrow += '<input type="text"  id="assmeblymasterinfo_Lobor_Resale" value="" class = "form-control" onkeyup="checknum(this);OnKeypressEvent();" style="text-align:right"/>';
                        footerlabourrow += '</th><th>';
                        footerlabourrow += '<input type="text"  id="assmeblymasterinfo_Estimated_Hour" value="" readonly class = "form-control" onkeyup="checknum(this);OnKeypressEvent();"style="text-align:right"/>';
                        footerlabourrow += '</th><th>';
                        footerlabourrow += '</th><th>';
                        footerlabourrow += '<input type="text"  id="assmeblymasterinfo_LaborEst_CostTotal" value="" class = "form-control" readonly = "readonly"onkeyup="checknum(this);OnKeypressEvent();" style="text-align:right"/>';
                        footerlabourrow += '</th><th>';
                        footerlabourrow += '<input type="text"  id="assmeblymasterinfo_LaborEst_ResaleTotal" value="" class = "form-control" readonly = "readonly"onkeyup="checknum(this);OnKeypressEvent();" style="text-align:right"/>';
                        footerlabourrow += '</th>';
                        footerlabourrow += '</tr>';
                    }
                    else {

                        footerlabourrow = oldlabour;
                    }
                }
            }
            catch
            {
                setRegexValidations();
            }


            $('#PartsLabortblInfo').show();
            $('#laborDatatbl').html('');
            $('#laborDatatbl').append(footerrow);
            $('#laborDatatbl').append(footerlabourrow);
            if (!isNaN($('#assmeblymasterinfo_labor_cost').val())) {
                //alert(estimatedcost);
                GrandCostTot = Number(estimatedcost) + Number($('#assmeblymasterinfo_LaborEst_CostTotal').val());
            }
            if (!isNaN($('#assmeblymasterinfo_Lobor_Resale').val())) {
                GrandCostResaleTot = Number(estimatedresalecost) + Number($('#assmeblymasterinfo_LaborEst_ResaleTotal').val());
            }

            var footergrandrow = '<tr class="Totalfooter-bgcolor"><th colspan="7" > Grand Total</th><th>';
            var GrandCostTotval = GrandCostTot == 0 ? "" : GrandCostTot;
            footergrandrow += '<input type="text" id="assmeblymasterinfo_GrandCostTotal" value="' + Number(GrandCostTotval).toFixed(2) + '" class = "form-control Estimate-FooterValue" readonly = "readonly"  style="text-align:right; border: none;font-weight: bold;"/>';
            footergrandrow += '</th><th>';
            var GrandCostResaleTotval = GrandCostResaleTot == 0 ? "" : GrandCostResaleTot;
            footergrandrow += '<input type="text"  id="assmeblymasterinfo_GrandResaleTotal" value="' + Number(GrandCostResaleTotval).toFixed(2) + '" class = "form-control Estimate-FooterValue" readonly = "readonly" style="text-align:right;border: none; font-weight: bold;"/>';
            footergrandrow += '</th>';
            footergrandrow += '</tr>';


            $('#laborDatatbl').append(footergrandrow);

            setRegexValidations();
            calculatecost();
        },
        error: function (err) {
            //alert("Error4");
        }
    });
}

function calculatecost(val) {
    PartsCost_Total = 0;
    PartsResaleCost_Total = 0;
    EstQty_Total = 0;
    var partsrow = $('#partstbody tr');
    if (partsrow.length > 0) {
        for (var i = 0; i < partsrow.length; i++) {
            //var activevalue = $(partsrow[i]).find('#activeparts').text().trim();
            //if (activevalue == "true")
            //{
            var cost = partsrow[i].cells[4].children[0].value;
            var resalecost = partsrow[i].cells[5].children[0].value;
            var qty = partsrow[i].cells[6].children[0].value;
            qty = Number(qty);
            EstQty_Total = Number(EstQty_Total) + Number(qty);
            if (isNaN(EstQty_Total)) {
                EstQty_Total = 0;
            }
            var estcosttotal = Number(Number(cost) * Number(qty));
            if (isNaN(estcosttotal)) {
                estcosttotal = 0;
            }
            partsrow[i].cells[8].children[0].value = estcosttotal.toFixed(2);

            //Est.Resale Total calculation
            var estresaletotal = Number(Number(resalecost) * Number(qty));
            if (isNaN(estresaletotal)) {
                estresaletotal = 0;
            }
            partsrow[i].cells[9].children[0].value = estresaletotal.toFixed(2);

            PartsCost_Total = Number(PartsCost_Total) + Number(partsrow[i].cells[8].children[0].value);
            if (isNaN(PartsCost_Total)) {
                PartsCost_Total = 0;
            }
            PartsResaleCost_Total = Number(PartsResaleCost_Total) + Number(partsrow[i].cells[9].children[0].value);
            if (isNaN(PartsResaleCost_Total)) {
                PartsResaleCost_Total = 0;
            }
        }
    }

    //Labor unit calculation
    var LaborUnitTxtBoxes = partsrow.find('[name = "txtLaborUnit"]');
    var estQtyTxtBoxes = partsrow.find('[name = "txtEstimatedQty"]');
    var estLaborUnitTxtBoxes = partsrow.find('[name = "txtCalcLaborUnit"]');
    MultiplyOnSeries(LaborUnitTxtBoxes, estQtyTxtBoxes, estLaborUnitTxtBoxes, 4);
    LaborValue = AdditionOnSeries(estLaborUnitTxtBoxes, 4);
    $('#assmeblymasterinfo_Estimated_Hour').val(LaborValue);


    $('#assmeblymasterinfo_Estimated_Qty_Total').val(Number(EstQty_Total).toFixed(4));
    $('#assmeblymasterinfo_PartCostTotal').val(Number(PartsCost_Total).toFixed(2));
    $('#assmeblymasterinfo_PartResaleTotal').val(Number(PartsResaleCost_Total).toFixed(2));
    OnKeypressEvent();
    var LaborEstCost = $("#assmeblymasterinfo_LaborEst_CostTotal").val();
    var LaborEstRcost = $("#assmeblymasterinfo_LaborEst_ResaleTotal").val();
    var GrandCostTotal = 0.00;
    var GrandRcostTotal = 0.00;
    GrandCostTotal = Number($('#assmeblymasterinfo_PartCostTotal').val()) + Number(LaborEstCost);
    if (isNaN(GrandCostTotal)) {
        GrandCostTotal = 0;
    }
    GrandRcostTotal = Number($('#assmeblymasterinfo_PartResaleTotal').val()) + Number(LaborEstRcost);
    if (isNaN(GrandRcostTotal)) {
        GrandRcostTotal = 0;
    }
    $("#assmeblymasterinfo_GrandCostTotal").val(Number(GrandCostTotal).toFixed(2));
    $("#assmeblymasterinfo_GrandResaleTotal").val(Number(GrandRcostTotal).toFixed(2));
}

function OnKeypressEvent() {

    var laborcost = $("#assmeblymasterinfo_labor_cost").val();
    var laborResale = $("#assmeblymasterinfo_Lobor_Resale").val();
    var laborEstHr = $("#assmeblymasterinfo_Estimated_Hour").val();
    EstLaborCostTot = 0;
    EstLaborResaleCostTot = 0;
    PartsResaleCost_Total = 0

    GrandCostTotal = 0;
    GrandResaleTotal = 0;

    PartsCost_Total = 0;

    if (laborcost != "" && laborResale != "" && laborEstHr != "") {

        EstLaborCostTot = parseFloat(laborcost) * parseFloat(laborEstHr);
        if (isNaN(EstLaborCostTot)) {
            EstLaborCostTot = 0;
        }
        //alert(EstLaborCostTot);
        $("#assmeblymasterinfo_LaborEst_CostTotal").val(Number(EstLaborCostTot).toFixed(2));

        EstLaborResaleCostTot = parseFloat(laborResale) * parseFloat(laborEstHr);
        if (isNaN(EstLaborResaleCostTot)) {
            EstLaborResaleCostTot = 0;
        }
        // alert(EstLaborResaleCostTot);
        $("#assmeblymasterinfo_LaborEst_ResaleTotal").val(Number(EstLaborResaleCostTot).toFixed(2));

        var tablRows = $('#partstbody tr')

        for (i = 0; i < tablRows.length; i++) {

            var estcost = $('#' + "P_" + i + "_3").val();
            //alert(estcost);
            PartsCost_Total += parseFloat(estcost);
            //alert(PartsCost_Total);

        }

        GrandCostTotal = PartsCost_Total + EstLaborCostTot;
        $("#assmeblymasterinfo_GrandCostTotal").val(Number(GrandCostTotal).toFixed(2));
        // alert(GrandCostTotal);

        var tablRows = $('#partstbody tr')

        for (i = 0; i < tablRows.length; i++) {
            var estresalecost = $('#' + "P_" + i + "_4").val();
            //alert(estresalecost);
            PartsResaleCost_Total += parseFloat(estresalecost);

        }
        GrandResaleTotal = PartsResaleCost_Total + EstLaborResaleCostTot;
        if (isNaN(GrandResaleTotal)) {
            GrandResaleTotal = 0;

        }
        //alert(GrandResaleTotal);
        $("#assmeblymasterinfo_GrandResaleTotal").val(Number(GrandResaleTotal).toFixed(2))

    }
    else {
        if (laborcost == "" || laborEstHr == "") {
            $("#assmeblymasterinfo_LaborEst_CostTotal").val("");
            $("#assmeblymasterinfo_GrandCostTotal").val("");
        }
        if (laborResale == "" || laborEstHr == "") {
            $("#assmeblymasterinfo_LaborEst_ResaleTotal").val("");
            $("#assmeblymasterinfo_GrandResaleTotal").val("")
        }
    }
}
//Here to validate assembly Category List
function AScatgoryValidate() {
    var ASCategory = $('#Assemblies_Category').val();
    if (ASCategory != "") {
        var otherCategory = $("#OtherAssemblies_Category").val();
        othercatgoryMsg = "\"Other Category\" field is required, If you had selected \"Other\" in category field.";
        if (ASCategory == "1" && otherCategory == '') {
            DisplayNotification(othercatgoryMsg, "error");
            return false;
        }
        else {
            return true;
        }
    }
    else {
        DisplayNotification("Assembly “Category” is required.", "error");
        return false;
    }
}

//function AsnameValidate(val) {
//    var asname = $(val).val();
//    //var characterReg = /^[A-Za-z\d=#$%...-]+$/;
//    var characterReg = /^[-_ a-zA-Z0-9]+$/;
//    if (!characterReg.test(asname)) {
//        $("#asnamemsg").text("Special characters are not allowed except (Minus, Underscore).");
//        $('#Assemblies_Name').focus();
//        return false;
//    }
//    else {
//        $("#asnamemsg").text("");
//        return true;
//    }
//}

function ValidateName(e) {
    //var asname = $("#Assemblies_Name").val();
    var characterReg = /^[-_ a-zA-Z0-9]+$/;
    if (!characterReg.test(e.value)) {
        return false;
    }
    ////var asname = val;
    //var asname = $("#Assemblies_Name").val();
    ////var characterReg = /^[A-Za-z\d=#$%...-]+$/;
    //var characterReg = /^[-_ a-zA-Z0-9]+$/;
    //if (!characterReg.test(asname)) {
    //    //$("#asnamemsg").text("special characters are not allowed");
    //    //$('#Assemblies_Name').focus();
    //    $("#Assemblies_Name").val("")
    //    return false;
    //}
    //else {
    //    //$("#asnamemsg").text("");
    //    return true;
    //}   
}
function saveassembly() {
    msg = "Would you like to add this assembly?"
    if (validateAssemblyInformation()) {
        var formData = new FormData();
        var model = new Array();
        var partslist = new Array();
        var LaborwithPartTotalList = new Array();
        var partsCostTotal = 0;
        var partsResaleTotal = 0;
        var laborCost = 0;
        var laboresale = 0;
        var EstHours = 0;
        var laborCostTotal = 0;
        var laborResaleTotal = 0;
        var grandCostTotal = 0;
        var grandResaleTotal = 0;
        var EstQtyTotal = 0;
        var isvalid = true;
        var errormsg = "";
        var tablRows = $('#partstbody tr')
        if (tablRows.length > 0) {
            var tablRows = $('#partstbody tr')
            var txtLaborUnitList = $(tablRows).find("[name='txtLaborUnit']");
            var txtEstimatedQty = $(tablRows).find("[name='txtEstimatedQty']");

            for (i = 0; i < tablRows.length; i++) {
                if ($(tablRows).find('#' + "P_" + i + "_0").val() == "0" || $(tablRows).find('#' + "P_" + i + "_0").val() == "") {
                    isvalid = false;
                    errormsg = "Please check the added Parts “Cost” is required.";
                }

                if (Number($(tablRows).find('#' + "P_" + i + "_0").val()) > Number($(tablRows).find('#' + "P_" + i + "_1").val())) {
                    if (!confirm('“Cost” is greater than the “Resale Cost” in the Parts - ' + $(tablRows[i]).find('#partnumber').text().trim() + '. Press OK to continue.')) {
                        DisplayNotification("User cancelled the operation!", "error");
                        isvalid = false;
                    }
                }

                if ($(txtEstimatedQty[i]).val().trim() == "") {
                    $(txtEstimatedQty[i]).val("0")
                    calculatecost();
                }


                if ($(txtEstimatedQty[i]).val() == "0" || $(txtEstimatedQty[i]).val() == "0.00") {
                    if (!confirm('“Est.Qty/Hrs” is 0 in the Part - ' + $(tablRows[i]).find('#partnumber').text().trim() + '. Press OK to continue.')) {
                        DisplayNotification("User cancelled the operation!", "error");
                        isvalid = false;
                    }
                }


                var estquenty;
                if ($(tablRows).find('#' + "P_" + i + "_2").val() == "") {
                    estquenty = 0
                }
                else {
                    estquenty = $(tablRows).find('#' + "P_" + i + "_2").val();
                }
                var estcosttotal;
                if ($(tablRows).find('#' + "P_" + i + "_3").val() == "") {
                    estcosttotal = 0
                }
                else {
                    estcosttotal = $(tablRows).find('#' + "P_" + i + "_3").val();
                }
                var estResalcosttotal;
                if ($(tablRows).find('#' + "P_" + i + "_4").val() == "") {
                    estResalcosttotal = 0
                }
                else {
                    estResalcosttotal = $(tablRows).find('#' + "P_" + i + "_4").val();
                }
                //var partcost;
                //if ($(tablRows).find('#' + "P_" + i + "_0").val() == "") {
                //    partcost = 0;
                //}
                //else {
                //    partcost = $(tablRows).find('#' + "P_" + i + "_0").val();
                //}
                var Resalepartcost;
                if ($(tablRows).find('#' + "P_" + i + "_1").val() == "") {
                    Resalepartcost = 0;
                }
                else {
                    Resalepartcost = $(tablRows).find('#' + "P_" + i + "_1").val();
                }
                partslist.push({
                    'Part_Number': $(tablRows[i]).find('#partnumber').text().trim(),
                    'Part_Category': $(tablRows[i]).find('#Part_Category').text().trim(),
                    'Part_Cost': $(tablRows).find('#' + "P_" + i + "_0").val(),
                    'Resale_Cost': Resalepartcost,
                    'Estimated_Qty': estquenty,
                    'EstCost_Total': estcosttotal,
                    'EstResaleCost_Total': estResalcosttotal,
                    'LaborUnit': $(txtLaborUnitList[i]).val()
                });

            }
        }
        if (isvalid == true) {
            var labortablRows = $('#laborDatatbl tr')
            if (labortablRows.length > 0) {
                $(labortablRows).each(function (index, row) {
                    if ($(labortablRows).find("#assmeblymasterinfo_labor_cost").val().trim() == "0" || $(labortablRows).find("#assmeblymasterinfo_labor_cost").val().trim() == "" || $(labortablRows).find("#assmeblymasterinfo_Lobor_Resale").val().trim() == "0" || $(labortablRows).find("#assmeblymasterinfo_Lobor_Resale").val().trim() == "" || $(labortablRows).find("#assmeblymasterinfo_Estimated_Hour").val().trim() == "0" || $(labortablRows).find("#assmeblymasterinfo_Estimated_Hour").val().trim() == "") {
                        if (!confirm('Labor value is 0. Press OK to continue.')) {
                            DisplayNotification("User cancelled the operation!", "error");
                            isvalid = false;
                        }
                    }
                    else {
                        if (Number($(labortablRows).find("#assmeblymasterinfo_labor_cost").val()) >= Number($(labortablRows).find("#assmeblymasterinfo_Lobor_Resale").val())) {
                            if (!confirm('Labor Cost is greater than resale cost!. Press OK to continue.')) {
                                DisplayNotification("User cancelled the operation!", "error");
                                isvalid = false;
                            }
                        }
                    }
                    EstQtyTotal = $(labortablRows).find('#assmeblymasterinfo_Est_Qty_Total').val();
                    partsCostTotal = $(labortablRows).find('#assmeblymasterinfo_PartCostTotal').val();
                    partsResaleTotal = $(labortablRows).find('#assmeblymasterinfo_PartResaleTotal').val();
                    laborCost = $(labortablRows).find("#assmeblymasterinfo_labor_cost").val();
                    laboresale = $(labortablRows).find("#assmeblymasterinfo_Lobor_Resale").val();
                    EstHours = $(labortablRows).find("#assmeblymasterinfo_Estimated_Hour").val();
                    laborCostTotal = $(labortablRows).find("#assmeblymasterinfo_LaborEst_CostTotal").val();
                    laborResaleTotal = $(labortablRows).find("#assmeblymasterinfo_LaborEst_ResaleTotal").val();
                    grandCostTotal = $(labortablRows).find("#assmeblymasterinfo_GrandCostTotal").val();
                    grandResaleTotal = $(labortablRows).find("#assmeblymasterinfo_GrandResaleTotal").val();
                });
                if (EstQtyTotal == undefined) {
                    EstQtyTotal = "0";
                }
                if (laborCost == "") {
                    laborCost = "0"
                }
                if (laboresale == "") {
                    laboresale = "0"
                }
                if (EstHours == "") {
                    EstHours = "0"
                }
                if (laborCostTotal == "") {
                    laborCostTotal = "0"
                }
                if (laborResaleTotal == "") {
                    laborResaleTotal = "0"
                }
                if (grandCostTotal == "") {
                    grandCostTotal = "0";
                }
                if (grandResaleTotal == "") {
                    grandResaleTotal = "0"
                }
            }
            if (isvalid == true) {
                //var assemName = string.replace(/\s\s+/g, ' ');
                var activestatus = $("#Isactive");
                var model = {
                    'Assemblies_Name': $('#Assemblies_Name').val().replace(/\s\s+/g, ' '),
                    'Assemblies_Description': $('#Assemblies_Description').val(),
                    'Assemblies_Category': $('#Assemblies_Category').val(),
                    'Active': true,
                    'severity': $('#severity').val(),
                    'OtherAssemblies_Category': $('#OtherAssemblies_Category').val(),
                    'PartsListData': partslist,
                    'Est_Qty_Total': EstQtyTotal,
                    'PartCostTotal': partsCostTotal,
                    'PartResaleTotal': partsResaleTotal,
                    'labor_cost': laborCost,
                    'Lobor_Resale': laboresale,
                    'Estimated_Hour': EstHours,
                    'LaborEst_CostTotal': laborCostTotal,
                    'LaborEst_ResaleTotal': laborResaleTotal,
                    'GrandCostTotal': grandCostTotal,
                    'GrandResaleTotal': grandResaleTotal
                };
                if (confirm(msg)) {
                    formData.append('model', JSON.stringify(model));
                    $.ajax({
                        url: '../NationwideAssembly/AddAssembliesDetails',
                        data: formData,
                        type: 'POST',
                        processData: false,
                        contentType: false,
                        success: function (data) {
                            if (data == "“Assemblies details” are added successfully.") {
                                DisplayNotification(data, "success");
                                getassemblyInfo('');
                                NationwideAssemblyMasterDataTable.ajax.reload(null, false);
                                //getassemblyList($('#lstassembly_cat').val(), 1);
                                isassembly = 0;
                                $('#partstbody').html('');
                                $('#laborDatatbl').html('');
                                $("#PartsLabortblInfo").hide();
                                updatestatus = 1;
                            }
                            else {
                                DisplayNotification(data, "error");
                            }
                        },
                        error: function (err) {
                        }
                    });
                }
            }
            else {
                DisplayNotification(errormsg, "error");
            }
        }
        else {
            DisplayNotification(errormsg, "error");
        }
    }
    //if (($("#Assemblies_Name").val() != "" && $("#Assemblies_Description").val() != "") && $('#PartsLabortblInfo >tbody >tr').length > 0 && nameValidate($("#Assemblies_Name").val()) == true) {

    //}
    //else {

    //}
}
function validateAssemblyInformation() {
    var valid = true;
    $("#spannameerror").hide();
    $("#spancategoryerror").hide();
    $("#spanothercategoryerror").hide();
    $("#spandescriptionerror").hide();
    if ($("#Assemblies_Name").val() == null || $("#Assemblies_Name").val() == "") {
        $("#spannameerror").show();
        valid = false;
    }
    if ($("#Assemblies_Category").val() == null || $("#Assemblies_Category").val() == "") {
        $("#spancategoryerror").show();
        valid = false;
    }
    if ($("#Assemblies_Category").val() == "1") {
        if ($("#OtherAssemblies_Category").val() == null || $("#OtherAssemblies_Category").val() == "") {
            $("#spanothercategoryerror").show();
            valid = false;
        }
    }
    if ($("#Assemblies_Description").val() == null || $("#Assemblies_Description").val() == "") {
        $("#spandescriptionerror").show();
        valid = false;
    }
    if ($('#PartsLabortblInfo >tbody >tr').length == 0) {
        valid = false;
    }
    if (!valid) {
        DisplayNotification("Please fill “All” required (*) field.", "error");
    }
    return valid;
}

function UpdateAssembly() {
    msg = "Would you like to update this assembly?";
    var formData = new FormData();
    if (validateAssemblyInformation()) {
        //if (($("#Assemblies_Name").val() != "" && $("#Assemblies_Description").val() != "") && ($("#Assemblies_Category").val() != 0 || ($("#Assemblies_Category").val() == 1 && $("#OtherAssemblies_Category").val() != "")) && $('#PartsLabortblInfo >tbody >tr').length > 0) {
        //if (AScatgoryValidate() == true) {
        var model = new Array();
        var partslist = new Array();
        var LaborwithPartTotalList = new Array();
        var partsCostTotal = 0;
        var partsResaleTotal = 0;
        var laborCost = 0;
        var laboresale = 0;
        var EstHours = 0;
        var laborCostTotal = 0;
        var laborResaleTotal = 0;
        var grandCostTotal = 0;
        var grandResaleTotal = 0;
        var EstQtyTotal = 0;
        var isvalid = true;
        var errormsg = "";
        var tablRows = $('#partstbody tr')
        if (tablRows.length > 0) {
            var tablRows = $('#partstbody tr')
            var txtLaborUnitList = $(tablRows).find("[name='txtLaborUnit']");
            var txtEstimatedQty = $(tablRows).find("[name='txtEstimatedQty']");
            for (i = 0; i < tablRows.length; i++) {
                if ($(tablRows).find('#' + "P_" + i + "_0").val() == "0" || $(tablRows).find('#' + "P_" + i + "_0").val() == "") {
                    isvalid = false;
                    errormsg = "Please check the added Parts “Cost” is required.";
                }

                if (Number($(tablRows).find('#' + "P_" + i + "_0").val()) > Number($(tablRows).find('#' + "P_" + i + "_1").val())) {
                    if (!confirm('“Cost” is greater than the “Resale Cost” in the Parts - ' + $(tablRows[i]).find('#partnumber').text().trim() + '. Press OK to continue.')) {
                        DisplayNotification("User cancelled the operation!", "error");
                        isvalid = false;
                    }
                }

                if ($(txtEstimatedQty[i]).val().trim() == "") {
                    $(txtEstimatedQty[i]).val("0")
                    calculatecost();
                }


                if ($(txtEstimatedQty[i]).val() == "0" || $(txtEstimatedQty[i]).val() == "0.00") {
                    if (!confirm('“Est.Qty/Hrs” is 0 in the Part - ' + $(tablRows[i]).find('#partnumber').text().trim() + '. Press OK to continue.')) {
                        DisplayNotification("User cancelled the operation!", "error");
                        isvalid = false;
                    }
                }

                var estquenty;
                if ($(tablRows).find('#' + "P_" + i + "_2").val() == "") {
                    estquenty = 0
                }
                else {
                    estquenty = $(tablRows).find('#' + "P_" + i + "_2").val();
                }
                var estcosttotal;
                if ($(tablRows).find('#' + "P_" + i + "_3").val() == "") {
                    estcosttotal = 0
                }
                else {
                    estcosttotal = $(tablRows).find('#' + "P_" + i + "_3").val();
                }
                var estResalcosttotal;
                if ($(tablRows).find('#' + "P_" + i + "_4").val() == "") {
                    estResalcosttotal = 0
                }
                else {
                    estResalcosttotal = $(tablRows).find('#' + "P_" + i + "_4").val();
                }
                //var partcost;
                //if ($(tablRows).find('#' + "P_" + i + "_0").val() == "") {
                //    partcost = 0;
                //}
                //else {
                //    partcost = $(tablRows).find('#' + "P_" + i + "_0").val();
                //}
                var Resalepartcost;
                if ($(tablRows).find('#' + "P_" + i + "_1").val() == "") {
                    Resalepartcost = 0;
                }
                else {
                    Resalepartcost = $(tablRows).find('#' + "P_" + i + "_1").val();
                }
                partslist.push({
                    'Part_Number': $(tablRows[i]).find('#partnumber').text().trim(),
                    'Part_Category': $(tablRows[i]).find('#Part_Category').text().trim(),
                    'Parts_Description': $(tablRows[i]).find('#Parts_Description').text().trim(),
                    'Part_Cost': $(tablRows).find('#' + "P_" + i + "_0").val(),
                    'Resale_Cost': Resalepartcost,
                    'Estimated_Qty': estquenty,
                    'EstCost_Total': estcosttotal,
                    'EstResaleCost_Total': estResalcosttotal,
                    'LaborUnit': $(txtLaborUnitList[i]).val()
                });
            }
        }
        if (isvalid == true) {
            var labortablRows = $('#laborDatatbl tr')
            if (labortablRows.length > 0) {

                if ($(labortablRows).find("#assmeblymasterinfo_labor_cost").val().trim() == "0" || $(labortablRows).find("#assmeblymasterinfo_labor_cost").val().trim() == "" || $(labortablRows).find("#assmeblymasterinfo_Lobor_Resale").val().trim() == "0" || $(labortablRows).find("#assmeblymasterinfo_Lobor_Resale").val().trim() == "" || $(labortablRows).find("#assmeblymasterinfo_Estimated_Hour").val().trim() == "0" || $(labortablRows).find("#assmeblymasterinfo_Estimated_Hour").val().trim() == "") {
                    if (!confirm('Labor value is 0. Press OK to continue.')) {
                        DisplayNotification("User cancelled the operation!", "error");
                        isvalid = false;
                    }
                }
                else {
                    if (Number($(labortablRows).find("#assmeblymasterinfo_labor_cost").val()) >= Number($(labortablRows).find("#assmeblymasterinfo_Lobor_Resale").val())) {
                        if (!confirm('Labor Cost is greater than resale cost!. Press OK to continue.')) {
                            DisplayNotification("User cancelled the operation!", "error");
                            isvalid = false;
                        }
                    }
                }
                $(labortablRows).each(function (index, row) {
                    EstQtyTotal = $(labortablRows).find('#assmeblymasterinfo_Estimated_Qty_Total').val();
                    partsCostTotal = $(labortablRows).find('#assmeblymasterinfo_PartCostTotal').val();
                    partsResaleTotal = $(labortablRows).find('#assmeblymasterinfo_PartResaleTotal').val();
                    laborCost = $(labortablRows).find("#assmeblymasterinfo_labor_cost").val();
                    laboresale = $(labortablRows).find("#assmeblymasterinfo_Lobor_Resale").val();
                    EstHours = $(labortablRows).find("#assmeblymasterinfo_Estimated_Hour").val();
                    laborCostTotal = $(labortablRows).find("#assmeblymasterinfo_LaborEst_CostTotal").val();
                    laborResaleTotal = $(labortablRows).find("#assmeblymasterinfo_LaborEst_ResaleTotal").val();
                    grandCostTotal = $(labortablRows).find("#assmeblymasterinfo_GrandCostTotal").val();
                    grandResaleTotal = $(labortablRows).find("#assmeblymasterinfo_GrandResaleTotal").val();
                });
                if (EstQtyTotal == undefined) {
                    EstQtyTotal = "0";
                }
                if (laborCost == "") {
                    laborCost = "0"
                }
                if (laboresale == "") {
                    laboresale = "0"
                }
                if (EstHours == "") {
                    EstHours = "0"
                }
                if (laborCostTotal == "") {
                    laborCostTotal = "0"
                }
                if (laborResaleTotal == "") {
                    laborResaleTotal = "0"
                }
                if (grandCostTotal == "") {
                    grandCostTotal = "0";
                }
                if (grandResaleTotal == "") {
                    grandResaleTotal = "0"
                }
            }
            if (isvalid == true) {
                var activestatus = $("#Isactive");

                var model = {
                    'Assemblies_Name': $('#Assemblies_Name').val().replace(/\s\s+/g, ' '),
                    'Assemblies_Description': $('#Assemblies_Description').val(),
                    'Assemblies_Category': $('#Assemblies_Category').val(),
                    'Active': activestatus[0].checked,
                    'severity': $('#severity').val(),
                    'OtherAssemblies_Category': $('#OtherAssemblies_Category').val(),
                    'PartsListData': partslist,
                    'Est_Qty_Total': EstQtyTotal,
                    'PartCostTotal': partsCostTotal,
                    'PartResaleTotal': partsResaleTotal,
                    'labor_cost': laborCost,
                    'labor_ResaleCost': laboresale,
                    'labor_EstimatedHours': EstHours,
                    'LaborEst_CostTotal': laborCostTotal,
                    'LaborEst_ResaleTotal': laborResaleTotal,
                    'GrandCostTotal': grandCostTotal,
                    'GrandResaleTotal': grandResaleTotal,
                    'AssemblyId': $("#AsId").val(),

                };
                if (confirm(msg)) {
                    formData.append('model', JSON.stringify(model));
                    $.ajax({
                        url: '../NationwideAssembly/UpdateAssembliesDetail',
                        data: formData,
                        type: 'POST',
                        processData: false,
                        contentType: false,
                        success: function (data) {
                            if (data == "“Assemblies details” has been updated successfully.") {
                                DisplayNotification("“Assemblies Details” are updated successfully.", "success");
                                $("#updatebtn").hide();
                                $("#Cancel").hide();
                                $("#addbtn").show();
                                $("#Reset").show();
                                selectedpart = [];
                                isassembly = 0;
                                isediting = false;
                                getassemblyInfo('');
                                NationwideAssemblyMasterDataTable.ajax.reload(null, false);
                                //getassemblyList($('#lstassembly_cat').val(), 1);
                                $('#partstbody').html('');
                                $('#laborDatatbl').html('');
                                $("#PartsLabortblInfo").css({ "display": "none" });
                                updatestatus = 1;

                            }
                            else {
                                DisplayNotification(data, "error");
                            }

                        },
                        error: function (err) {
                        }
                    });
                }

            }
            else {
                DisplayNotification(errormsg, "error");
            }
        }
        else {
            DisplayNotification(errormsg, "error");
        }
        //}
    }
    //else {
    //    DisplayNotification("Please fill the \"Name, Category, Description & Parts & Labor Details List\" fields!", "error");
    //}
}

function deleteAssembly(val, val1) {
    msg = "Would you like to delete this assembly?";
    if (confirm(msg)) {
        $.ajax({
            url: '../NationwideAssembly/DeleteAssembliesDetails?AssembliesName=' + encodeURIComponent(val) + '&AssemblieId=' + val1,
            type: 'Get',
            success: function (data) {
                if (data != "“Assemblies Details” deleted successfully.") {
                    DisplayNotification(data, "error");
                }
                else {

                    DisplayNotification(data, "success");
                    getassemblyInfo('');

                    $('#NationwideAssemblyMasterDataTable').DataTable().ajax.reload(null, false);
                    //getassemblyList($('#lstassembly_cat').val());
                    isassembly = 0;
                    $("#updatebtn").hide();
                    $("#Cancel").hide();
                    $("#addbtn").show();
                    $("#Reset").show();
                    $('#partstbody').html('');
                    $('#laborDatatbl').html('');
                    $("#PartsLabortblInfo").css({ "display": "none" });
                    var tablerow = $("#asbody tr");
                    selectedpart = [];
                    //for (i = 0; i < tablerow.length; i++) {
                    //    if (tablerow[i].cells[0].innerText == val) {
                    //        tablerow[i].closest('tr').remove();
                    //    }
                    //}
                }
            },
            error: function (err) { }
        });
    }
    else {
        return false;
    }
}

function selectrow(val) {

    var vauese = val;
    //var selected = $(val).attr('checked');
    //var table = $('#partstable').dataTable();
    //var allPages = table.fnGetNodes();
    //var checkbx = $(val).find("input[type=checkbox]");
    var vale = val;
    vale = vale.replace("/", "");
    var ischecked = $("#" + vale).is(":checked");
    var added = false;
    i = 0;
    if (ischecked == true) {
        //selectedpart = [];
        $.map(selectedpart, function (elementOfArray, indexInArray) {
            if (elementOfArray == vauese) {
                added = true;
            }
        });
        if (!added) {
            if (vauese != "selectall") {
                selectedpart.push(vauese);
            }
            var table = $('#partstable').dataTable();
            var allPages = table.fnGetNodes();
            $(allPages).find("input:checkbox").each(function () {
                if (this.checked == true) {
                    i++
                }
                var tablerowlength = $("#partsbody tr").length;
                if (tablerowlength == i) {
                    $("#selectall").prop("checked", true);
                }
            });
        }
    }
    else {
        //selectedpart = jQuery.grep(selectedpart, function (value) {
        //    return selectedpart[0] != vauese;
        //});
        selectedpart.splice(selectedpart.indexOf(vauese), 1);
        $("#selectall").prop("checked", false);
    }
    if (selectedpart.length == 0) {
        $("#Npsubmit").attr('disabled', 'disabled');
    }
    else {
        $("#Npsubmit").removeAttr('disabled', 'disabled');

    }

}

function selectrow1(val) {

    //var checkbx = $(val).find("input[type=checkbox]");
    //i = 0;
    //var check = checkbx[0];
    ////if (typeof check != 'undefined') {
    ////    if (checkbx[0].checked == false) {
    ////        checkbx[0].checked = true;
    ////        selectedpart.push(checkbx[0].value);
    ////    }
    ////    else {
    ////        document.getElementById("selectall").checked = false;
    ////        checkbx[0].checked = false;
    ////        if (selectedpart.indexOf(checkbx[0].value) > -1)
    ////            selectedpart.splice(selectedpart.indexOf(checkbx[0].value), 1);
    ////    }
    ////}
    ////else {
    //if (val.checked === true) {
    //    //  val.checked = true;
    //    selectedpart.push(val.id);
    //}
    //else {
    //    document.getElementById("selectall").checked = false;
    //    // val.checked = false;
    //    if (selectedpart.indexOf(val.id) > -1)
    //        selectedpart.splice(selectedpart.indexOf(val.id), 1);
    //}
    ////  }
    ////Here to check select all checkbox if all prts are Selected
    ////$('#partstable').find("input:checkbox").each(function () {
    ////    if (this.checked == true) {
    ////        i++
    ////    }
    ////    var tablerowlength = $("#partsbody tr").length;
    ////    if (tablerowlength == i) {

    ////        document.getElementById("selectall").checked = true;
    ////    }
    ////});

    //if (selectedpart.length == 0) {
    //    $("#Npsubmit").attr('disabled', 'disabled');
    //}
    //else {
    //    $("#Npsubmit").removeAttr('disabled', 'disabled');

    //}

    var checkbx = $(val).find("input[type=checkbox]");
    i = 0;
    var check = checkbx[0];
    if (typeof check != 'undefined') {
        if (checkbx[0].checked == false) {
            checkbx[0].checked = true;
            selectedpart.push(checkbx[0].value);
        }
        else {
            document.getElementById("selectall").checked = false;
            checkbx[0].checked = false;
            if (selectedpart.indexOf(checkbx[0].value) > -1)
                selectedpart.splice(selectedpart.indexOf(checkbx[0].value), 1);
        }
    }
    else {
        if (val.checked == false) {

            val.checked = true;
            //selectedpart.push(val.id);
        }
        else {
            document.getElementById("selectall").checked = false;
            val.checked = false;
            //if (selectedpart.indexOf(val.id) > -1)
            //    selectedpart.splice(selectedpart.indexOf(val.id), 1);
        }
    }
    //Here to check select all checkbox if all prts are Selected
    $('#partstable').find("input:checkbox").each(function () {
        if (this.checked == true) {
            i++
        }
        var tablerowlength = $("#partsbody tr").length;
        if (tablerowlength == i) {

            document.getElementById("selectall").checked = true;
        }
    });

    if (selectedpart.length == 0) {
        $("#Npsubmit").attr('disabled', 'disabled');
    }
    else {
        $("#Npsubmit").removeAttr('disabled', 'disabled');

    }

}


function resetall() {
    if (confirm("Are you sure, do you want to reset all fields?")) {
        clear();
    }
}

function clear() {
    $('#PartsLabortblInfo').hide();
    $('#Assemblies_Name').val('');
    $('#Assemblies_Category').val('').trigger('change');
    $('#Assemblies_Description').val('');
    $('#OtherAssemblies_Category').val('');
    $('#partstbody').html('');
    $('#laborDatatbl').html('');
    $("#AsId").val(0);
}

//function validateForm() {
//    if (document.myForm.Assemblies_Name.value == "") {
//        alert("Please enter your Assemblies Name");
//        document.myForm.Assemblies_Name.focus();
//        return false;
//    }
//    if (document.getElementById('Assemblies_Category').selectedIndex == 0) {
//        alert("Please select Assemblies Category");
//        document.myForm.Assemblies_Category.focus();
//        return false;
//    }
//    if (document.myForm.Assemblies_Description.value == "") {
//        alert("Please enter your Assemblies Description");
//        document.myForm.Assemblies_Description.focus();
//        return false;
//    }
//    return true;
//}
function PreASDescriptionInPopUp() {
    $.ajax({
        url: '../NationwideAssembly/GetPreviousDescriptionList',
        type: "Get",
        success: function (data) {
            $('#divDecriptiondata').html(data);
            $('#txtpreview').val($('#Assemblies_Description').val());
            $('#Descriptionopenpopup').click();

        },
        error: function (err) { }

    });
}

function PreviewPopUp() {
    var DescriptionData = $("#Assemblies_Description").val();
    $("#divPreviewdata").val(DescriptionData);
    $("#Previewopenpopup").click();

}
function PreviewKeyUp(val) {
    $("#Assemblies_Description").val($(val).val());

}
function SaveDescription() {

    var SelectedDescription = $("#txtpreview").val();
    $("#Assemblies_Description").val(SelectedDescription);
    $("#DecriptionInPopUpClose").click()

}
function Cancel() {
    msg = "Are you sure, do you want to cancel?"
    if (confirm(msg)) {
        clear();
        //window.location.href = '../NationwideAssembly/Index'
        return true;
    }
    else {
        return false;
    }
}

var isfirst = true;

function change() {

    if (isfirst) {
        isfirst = false;
        var activestatus = $("#Isactive")[0].checked;
        if (activestatus == true) {
            msg = "Would you like to activate this assembly details?";
        }
        else {
            msg = "Would you like to Deactivate this assembly details?";
        }
        if (confirm(msg) == true) {

            return false;
        }
        else {
            if (activestatus == true) {
                $("#Isactive").bootstrapToggle('off')

            }
            else {
                $("#Isactive").bootstrapToggle('on')

            }
            //$("#activateStatus").bootstrapToggle('off');
            // return false;
        }
        setTimeout(function () { isfirst = true; }, 500);
    }


}

function setRegexValidations() {
    $("[name = 'txtCost']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'txtResaleCost']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'txtEstimatedQty']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("[name = 'txtLaborUnit']").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,4})?$/ });
    $("#assmeblymasterinfo_labor_cost").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("#assmeblymasterinfo_Lobor_Resale").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
}