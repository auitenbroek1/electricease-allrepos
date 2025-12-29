$(document).ready(function () {
    $("#Loading").hide();
    Cookies.set('N_partstpageno', "");
    Cookies.set('N_Partsdtpagelen', "");
    Cookies.set('N_Partsearch', "");
    $('li').removeClass('active');
    $("#Nparts").addClass('active');
    $(window).unload(function () {
        Cookies.set('N_Partsearch', '');
        Cookies.set("partsfilter", '');
    });
    $(".content-title").text("Nationwide Parts Master");
    getpartslist("");
    //BindNationWidePartsDataTable(1);
    $("#Part_Category").select2();

    $("#bootomNationParts").focus();
    
    setRegexValidations();
    $("#LaborUnit").keyup(function () {
        handleDecimalDot($(this));
    });
});
function ShowOtherCategory(value) {
    if (value == "other") {
        $("[name=OtherPart_Category]").removeAttr("disabled");
    }
    else {
        $("#OtherPart_Category").val('');
        $("[name=OtherPart_Category]").attr("disabled", "disabled");
    }
}

function checknum(element) {
    var values = $(element).val();
    var rgx = /^[0-9]*\.?[0-9]*$/;
    if (values.match(rgx)) {}
    else {
        $(element).val(0);
        //$(element).val($(element).val().replace(/\D/, ''));
    }
}
function PartcatgoryValidate() {
    var LaborCategory = $('#Part_Category').val();
    if (LaborCategory != "") {
        var otherCategory = $("#OtherPart_Category").val();
        othercatgoryMsg = "\"Other Category\" field is required, If you had selected \"Other\" in category field.";
        if (LaborCategory == "other" && otherCategory == '') {
            DisplayNotification(othercatgoryMsg, "error");
            return false;

        }
        else {
            return true;
        }
    }
    else {
        DisplayNotification("\"Part Category\" is required.", "error");
        return false;

    }
}
function CostValidate() {
    if ($("#Cost").val() != "") {
        //var intRegex = /^\d+(?:\.\d\d?)?$/;
        var intRegex = /^(\d*\.?\d+|\d+\.\d*)$/;
        if (!$("#Cost").val().match(intRegex)) {
            DisplayNotification("Cost should be a “Number”.", "error");
            return false;
        }
        else {
            return true;
        }
    }
    else {
        DisplayNotification("“Cost” is required.", "error");
        return false;
    }

}
function ResaleValidate() {
    if ($("#Rcost").val() != "") {
        // var intRegex = /^\d+(?:\.\d\d?)?$/;
        var intRegex = /^(\d*\.?\d+|\d+\.\d*)$/;
        if (!$("#Rcost").val().match(intRegex)) {
            DisplayNotification("Resale should be a “Number”.", "error");
            return false;
        }
        else {
            var Cost = $("#Cost").val();
            var ResaleCost = $("#Rcost").val();
            costMsg = "Resale price should be higher than “My Cost” price.";
            if (parseFloat(ResaleCost) <= parseFloat(Cost)) {
                if (!confirm("“Resale price” is lesser than “My Cost” price. Press OK to continue.")) {
                    DisplayNotification("User cancelled the operation!", "error");
                    $("#Rcost").focus();
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return true;
            }

        }
    }
    else {
        // DisplayNotification("Resale is Required", "error");
        return true;
    }

}
function nameValidate(val) {
    var partname = val;
    //var characterReg = /^[A-Za-z\d=#$%...-]+$/;
    var characterReg = /^[-"/'_ a-zA-Z0-9]+$/;
    if (!characterReg.test(partname)) {
        $("#partnumsg").text("special characters are not allowed");
        $('#Part_Number').focus();
        return false;
    }
    else {
        $("#partnumsg").text("");
        return true;
    }

}
function PartNumberValidate() {
    var PartNumber = $('#Part_Number').val();
    if (PartNumber != "") {
        return true;
    }
    else {
        DisplayNotification("\"Part Number\" is required.", "error");
        return false;
    }
}

function SuperAdminParts() {
    if (validatePartDetails()) {
        var Model = {
            "Client_ID": $("#Client_ID").val(),
            "Part_Category": $('#Part_Category').val(),
            "OtherPart_Category": $('#OtherPart_Category').val(),
            "Part_Number": $('#Part_Number').val(),
            "Cost": $('#Cost').val(),
            "Resale_Cost": $('#Rcost').val(),
            "Purchased_From": $('#Purchased_From').val(),
            "Description": $('#Description').val(),
            "Client_Description": '',
            "UOM": $('#UOM').val(),
            "LaborUnit": $('#LaborUnit').val(),
        }
        $.ajax({
            url: '../NationwideParts/CheckIsExistPartNumber',
            data: Model,
            type: 'POST',
            success: function (data) {
                if (data == "Already Exists") {
                    msg = "Part already exists. Would you like to update it?"
                    if (confirm(msg)) {
                        Model = {
                            "Client_ID": $("#Client_ID").val(),
                            "Part_Category": $('#Part_Category').val(),
                            "OtherPart_Category": $('#OtherPart_Category').val(),
                            "Part_Number": $('#Part_Number').val(),
                            "Cost": $('#Cost').val(),
                            "Resale_Cost": $('#Rcost').val(),
                            "Purchased_From": $('#Purchased_From').val(),
                            "Description": $('#Description').val(),
                            "Client_Description": '',
                            "UOM": $('#UOM').val(),
                            "LaborUnit": $('#LaborUnit').val()
                        }
                        $.ajax({
                            url: '../NationwideParts/AddNationwidePartsDetails',
                            data: Model,
                            type: 'POST',
                            success: function (data) {
                                if (data == "“New Parts” has been added successfully.") {
                                    DisplayNotification("“New Parts” have been added.", "success");
                                    Cookies.set('N_partstpageno', "");
                                    Cookies.set('N_Partsdtpagelen', "");
                                    Cookies.set('N_Partsearch', "");
                                    $("#title-cont").text("Add Parts Details");
                                    $("#saveb").show();
                                    $("#updateb").hide();
                                    $("#Cancel").hide();
                                    $("#Reset").show();
                                    $("#Part_Category").val('').trigger('change');
                                    $("#Part_Number").val('');
                                    $("#Cost").val('');
                                    $("#UOM").val('');
                                    $("#Rcost").val('');
                                    $("#Purchased_From").val('');
                                    $('#failedbtn').hide();
                                    $("#Description").val('');
                                    $("#Client_Description").val('');
                                    $("#LaborUnit").val('');
                                    editParts('0');
                                    if (Cookies.get('partsfilter') != null && Cookies.get('partsfilter') != "")
                                        getpartslist(Cookies.get('partsfilter'), 1);
                                    else
                                        getpartslist("", 1);
                                }
                                else if (data == "“Parts” are updated successfully.") {
                                    DisplayNotification("“Part” has been updated successfully.", "success");
                                    $("#title-cont").text("Add Parts Details");
                                    //$("#Part_Category").val('');
                                    $("#saveb").show();
                                    $("#updateb").hide();
                                    $("#Cancel").hide();
                                    $("#Reset").show();
                                    $("#Part_Category").val('').trigger('change');
                                    $("#Part_Number").val('');
                                    $("#Cost").val('');
                                    $("#UOM").val('');
                                    $("#Rcost").val('');
                                    $("#Purchased_From").val('');
                                    $("#Description").val('');
                                    $("#Client_Description").val('');
                                    $('#failedbtn').hide();
                                    $("#LaborUnit").val('');
                                    editParts('0');
                                    if (Cookies.get('partsfilter') != null && Cookies.get('partsfilter') != "")
                                        getpartslist(Cookies.get('partsfilter'), 1);
                                    else
                                        getpartslist("", 1);

                                }
                                else {
                                    DisplayNotification(data, "error");
                                }
                            },
                            error: function (err) {
                            }
                        });
                    }
                    else {
                        $('#Part_Number').focus();
                    }
                }
                else {
                    msg = "Would you like to add this part?"
                    if (confirm(msg)) {
                        Model = {
                            "Client_ID": $("#Client_ID").val(),
                            "Part_Category": $('#Part_Category').val(),
                            "OtherPart_Category": $('#OtherPart_Category').val(),
                            "Part_Number": $('#Part_Number').val(),
                            "Cost": $('#Cost').val(),
                            "Resale_Cost": $('#Rcost').val(),
                            "Purchased_From": $('#Purchased_From').val(),
                            "Description": $('#Description').val(),
                            "Client_Description": '',
                            "UOM": $('#UOM').val(),
                            "LaborUnit": $('#LaborUnit').val()
                        }
                        $.ajax({
                            url: '../NationwideParts/AddNationwidePartsDetails',
                            data: Model,
                            type: 'POST',
                            success: function (data) {
                                if (data == "“New Parts” has been added successfully.") {
                                    DisplayNotification("“New Parts” have been added.", "success");
                                    Cookies.set('N_partstpageno', "");
                                    Cookies.set('N_Partsdtpagelen', "");
                                    Cookies.set('N_Partsearch', "");
                                    $("#saveb").show();
                                    $("#updateb").hide();
                                    $("#Cancel").hide();
                                    $("#Reset").show();
                                    $("#Part_Category").val('').trigger('change');
                                    $("#Part_Number").val('');
                                    $("#Cost").val('');
                                    $("#UOM").val('');
                                    $("#Rcost").val('');
                                    $("#Purchased_From").val('');
                                    $("#Description").val('');
                                    $("#Client_Description").val('');
                                    $('#failedbtn').hide();
                                    $("#LaborUnit").val('');
                                    editParts('0');
                                    if (Cookies.get('partsfilter') != null && Cookies.get('partsfilter') != "")
                                        getpartslist(Cookies.get('partsfilter'), 1);
                                    else
                                        getpartslist("", 1);
                                    $("#bootomNationParts").focus();
                                }
                                else if (data == "“Parts” are updated successfully.") {
                                    DisplayNotification("“Part” has been updated successfully.", "success");
                                    $("#saveb").show();
                                    $("#updateb").hide();
                                    $("#Cancel").hide();
                                    $("#Reset").show();
                                    $("#Part_Category").val('').trigger('change');
                                    $("#Part_Number").val('');
                                    $("#Cost").val('');
                                    $("#UOM").val('');
                                    $("#Rcost").val('');
                                    $("#Purchased_From").val('');
                                    $("#Description").val('');
                                    $("#Client_Description").val('');
                                    $('#failedbtn').hide();
                                    $("#LaborUnit").val('');
                                    editParts('0');
                                    if (Cookies.get('partsfilter') != null && Cookies.get('partsfilter') != "")
                                        getpartslist(Cookies.get('partsfilter'), 1);
                                    else
                                        getpartslist("", 1);
                                    $("#bootomNationParts").focus();
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
            }
        })
    }
}
function validatePartDetails() {
    var valid = true;
    $("#spancategoryerror").hide();
    $("#spanothercategoryerror").hide();
    $("#spanpartnumbererror").hide();
    $("#spanmycosterror").hide();
    $("#spanresaleerror").hide();
    $("#spaninvalidothercategoryerror").hide();
    if ($("#Part_Category").val() == null || $("#Part_Category").val() == "") {
        $("#spancategoryerror").show();
        valid = false;
    }
    if ($("#Part_Category").val() == "0" || $("#Part_Category").val() == "other") {
        if ($("#OtherPart_Category").val() == null || $("#OtherPart_Category").val() == "") {
            $("#spanothercategoryerror").show();
            valid = false;
        }
        if ($("#OtherPart_Category").val().toLowerCase() == "other") {
            $("#spaninvalidothercategoryerror").show();
            valid = false;
        }
    }
    if ($("#Part_Number").val() == null || $("#Part_Number").val() == "") {
        $("#spanpartnumbererror").show();
        valid = false;
    }
    if ($("#Cost").val() == null || $("#Cost").val() == "") {
        $("#spanmycosterror").show();
        valid = false;
    }
    if (parseFloat($("#Rcost").val()) <= parseFloat($("#Cost").val())) {
        if (!confirm("“Resale price” is lesser than “My Cost” price. Press OK to continue.")) {
            DisplayNotification("User cancelled the operation!", "error");
            $("#Rcost").focus();
            valid = false;
        }
    }    
    if (!valid) {
        DisplayNotification("Please fill “All” required (*) field.", "error");
    }
    return valid;
}
function getpartslist(val, showBootom) {
    if (val == "ALL Parts Category")
        val = "";
    Cookies.set("partsfilter", val);
    $("#N_ptexample").DataTable().ajax.reload(null, false);
    if (showBootom === 1) {
        $("#bootomNationParts").focus();
    }
    //$.ajax({
    //    url: '../NationwideParts/GetNationalPartsList?PartCatgory=' + val,
    //    type: 'Get',
    //    success: function (data) {
    //        $('#SAdminpartslist').html(data);
    //        if (val == "") {
    //            val = "ALL Parts Category";
    //            $('#Part_CategoryList').val(val);
    //        }
    //        else {
    //            $('#Part_CategoryList').val(val);
    //        }
    //        if (showBootom === 1) {
    //            alert();
    //            $("#bootomNationParts").focus();
    //        }
    //    },
    //    error: function (err) {
    //        DisplayNotification("\"Network Error\" Please try later!", "error");
    //    }
    //});
}
function editParts(val1) {
    $.ajax({
        url: '../NationwideParts/EditPartsDetails?PartsNumber=' + encodeURIComponent(val1),
        type: 'Get',
        success: function (data) {
            $("#Part_Category").focus();
            $("#title-cont").text("Edit Parts Details")
            //$("#Part_Category").val(data.Part_Category).trigger('change');
            $("#Part_Number").val(data.Part_Number);
            $("#Cost").val(data.Cost);
            $("#UOM").val(data.UOM);
            $("#LaborUnit").val(data.LaborUnit);
            $("#Rcost").val(data.Resale_Cost);
            $("#Purchased_From").val(data.Purchased_From);
            $("#Description").val(data.Description);
            //$("#Client_Description").val(data.Client_Description);
            $("#saveb").hide();
            $("#updateb").show();
            $("#Cancel").show();
            $("#Reset").hide();
            $("#Part_Category").empty();
            $("#Part_Category").append('<option value="">' + "Select" + '</option>');
            $.each(data.partcatgoryList, function (i, value) {
                $("#Part_Category").append('<option value="' + value.Part_Category + '">' + value.Part_Category + '</option>');
            });
            $("#Part_Category").val(data.Part_Category).trigger('change');
            //if (val1 === '0') {
            //    $("#Part_Category").val('').trigger('change');
            //}
            //$("#Part_Category").select2()
            //$('#divaddparts').html(data); 
        },
        error: function (err) {
            //DisplayNotification("\"Network Error\" Please try later!", "error");
        }
    });
}

function deleteParts(val1) {
    msg = "Would you like to delete this part?"
    if (confirm(msg)) {
        $.ajax({
            url: '../NationwideParts/DeletePartsDetails',
            data: { PartsNumber: val1 },
            type: 'POST',
            success: function (data) {
                if (data != "“Deleted” successfully.") {
                    DisplayNotification(data, "error");
                }
                editParts('0', '0');

                if (Cookies.get('partsfilter') != null && Cookies.get('partsfilter') != "")
                    getpartslist(Cookies.get('partsfilter'));
                else
                    getpartslist("");
                DisplayNotification("“Parts details” are deleted successfully.", "success");
                spinner.stop();
            },
            error: function (err) { }

        });
    }
}

function confirmImport() {
    $("#importfile").val("").clone(true);
    swal({
        title: "Are you sure, want to \"Import\" parts?",
        showCancelButton: true,
        confirmButtonClass: "btn btn-success",
        confirmButtonText: "Yes",
        cancelButtonText: "No"
    }, function () {
        $('#importfile').click();
    });
    //if (confirm('Are you sure, do you want to \"Import\" parts?')) {
    //    $('#importfile').click();
    //}
}
function ResetAll() {
    msg = "Are you sure, do you want to “Clear” all fields?"
    if (confirm(msg)) {
        $("#Client_ID").val("");
        $("#Part_Category").val('').trigger('change');
        $("#OtherPart_Category").val("");
        $("#Part_Number").val("");
        $("#Cost").val("");
        $("#Rcost").val("");
        $("#LaborUnit").val("");
        $("#Description").val("");
        $("#UOM").val("");
        $("#Client_Description").val("");
        $("#Purchased_From").val("");
        $('#failedbtn').hide();
        if ($("#IsAdmin").val() == "Admin") {
            $("#Part_Category").attr('disabled', 'disabled');
        }
        $("#OtherPart_Category").attr('disabled', 'disabled');
        return true;
    }
    else {
        return false;
    }
}

function ImportParts() {
    $("#failedbtn").hide();
    var formdata = new FormData();
    var doc = document.getElementById('importfile');
    if (doc.files.length > 0) {
        formdata.append('partsheet', doc.files[0]);
        var target = $("body");
        $("#Loading").show();
        $.ajax({
            //url: '../NationwideParts/Importparts',
            url: '../NationwideParts/ImportParts',
            data: formdata,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (data) {
                $("#Loading").hide();
                if (data.includes("Error")) {
                    $("#importfile").val("").clone(true);
                    DisplayNotification(data, 'error');
                }
                else {
                    $('#lblreport').html(data);
                    $('#failedbtn').show();
                    DisplayNotification("\"Import Parts\" completed. Please check the report for status.", 'success');
                    $("#importfile").val("").clone(true);
                    editParts('0');
                    if (Cookies.get('partsfilter') != null && Cookies.get('partsfilter') != "") {
                        getpartslist(Cookies.get('partsfilter'));
                    }
                    else {
                        getpartslist("");
                    }
                }

                //if (data != 'In excel \"No\" records are available!' && data != "Please import the valid template, the file should be \"xlsx\" format only!" && data != "Invalid parts template!") {
                //    $("#Loading").hide();
                //    DisplayNotification("\"Import Parts\" completed. Please check the report for status.", 'success');
                //    //DisplayNotification("Kindly check the failed uploads report", 'success');
                //    $('#lblreport').html(data);
                //    $('#failedbtn').show();
                //    $("#importfile").val("").clone(true);
                //    editParts('0');
                //    if (Cookies.get('partsfilter') != null && Cookies.get('partsfilter') != "")
                //        getpartslist(Cookies.get('partsfilter'));
                //    else
                //        getpartslist("");
                //}
                //else {
                //    $("#Loading").hide();
                //    DisplayNotification(data, 'error');
                //    $("#importfile").val("").clone(true);

                //}
            },
            error: function (err) { }
        });
    }
}

function Cancel() {
    msg = "Are you sure, do you want to cancel?"
    if (confirm(msg)) {
        //editParts('0');
        //getpartslist(" ");
        $("#title-cont").text("Add Parts Details")
        // window.location.href = '../NationwideParts/Index';
        $("#Client_ID").val("");
        $("#Part_Category").val('').trigger('change');
        $("#OtherPart_Category").val("");
        $("#Part_Number").val("");
        $("#Cost").val("");
        $("#Rcost").val("");
        $("#LaborUnit").val("");
        $("#Description").val("");
        $("#UOM").val("");
        $("#Client_Description").val("");
        $("#Purchased_From").val("");
        $('#failedbtn').hide();
        if ($("#IsAdmin").val() == "Admin") {
            $("#Part_Category").attr('disabled', 'disabled');
        }
        $("#OtherPart_Category").attr('disabled', 'disabled');
        editParts('0');
        if (Cookies.get('partsfilter') != null && Cookies.get('partsfilter') != "")
            getpartslist(Cookies.get('partsfilter'), 1);
        else
            getpartslist("", 1);
        $('#failedbtn').hide();
        return true;
    }
    else {
        return false;
    }
}

function setRegexValidations() {
    $("#Cost").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("#Rcost").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,2})?$/ });
    $("#LaborUnit").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,4})?$/ });
}


