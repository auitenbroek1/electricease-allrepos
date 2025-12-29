$(document).ready(function () {
    $("#Loading").hide();
    Cookies.set('partstpageno', "");
    Cookies.set('Partsdtpagelen', "");
    Cookies.set('Partsearch', "");
    $('li').removeClass('active');
    $("#Dparts").addClass('active');
    $(".content-title").text("Distributor Parts Master");
    $(window).unload(function () {
        Cookies.set('Partsearch', '');
        Cookies.set("distpartsfilter", '');
    });
    $("#DistributorID").select2();
    $("#Part_Category").select2();

    getpartslist("");
    $("#submit").click(function () {
        var intRegex = /^\d+(?:\.\d\d?)?$/;
        var ClientName = $("#Client_ID option:selected").val();
        message = "Client Field is Required";
        if (ClientName == 0) {
            $("#ClietName_Errmsg").html("Required").show();
            return false;
        }

        var value = $("#Part_Category option:selected").val();
        if (value == 0) {
            $("#Category_errmsg").html("Required").show();
            return false;
        }


        if ($("#Part_Number").val() == "") {
            $("#PartNumber_errmsg").html("Required").show();
            return false;
        }

        var value = $("#Part_Category option:selected").val();

        var otherCategory = $("#OtherPart_Category").val();
        othercatgoryMsg = "\"Other Category\" field is required, If you had selected \"Other\" in category field.";
        if (value == 1 && otherCategory == '') {

            if (confirm(othercatgoryMsg)) {
                $("[name=OtherPart_Category]").focus();
                return false;

            }
        }



        if (!$("#Cost").val().match(intRegex)) {
            $("#errmsg").html("Number only").show();
            return false;
        }


        if (!$("#Rcost").val().match(intRegex)) {
            $("#RCosterrmsg").html("Number only").show();
            return false;
        }


        var Cost = $("#Cost").val();
        var ResaleCost = $("#Rcost").val();
        costMsg = "Resale price should be higher than “My Cost” price.";
        if (parseFloat(ResaleCost) <= parseFloat(Cost)) {
            if (confirm(costMsg)) {
                return false;
                $("#Rcost").focus();
            }
            else {
                return false;
            }
        }

        var Cost = $("#Cost").val();
        var ResaleCost = $("#Resale_Cost").val();
        costMsg = "Resale should be  Higher than My Cost";
        if (parseFloat(ResaleCost) <= parseFloat(Cost)) {
            if (confirm(costMsg)) {
                return false;
                $("#Resale_Cost").focus();
            }
            else {
                return false;

            }
        }
        if (parseFloat(Cost) == 0) {
            CostZero = "Cost should be Higher than 0";
            var result = confirm(CostZero)
            if (result == true) {
                return false;
            }
            else {
                return false;
            }
        }
    });

    //To validate the Labor Unit format by regex
    $("#LaborUnit").limitkeypress({ rexp: /^(\d{1,4})?(\.)?(\.\d{1,4})?$/ });
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
function partnameValidate(val) {
    var partname = $(val).val();
    //var characterReg = /^[A-Za-z\d=#$%...-]+$/;
    var characterReg = /^[-"/'_ a-zA-Z0-9]+$/;
    if (!characterReg.test(partname)) {
        $("#partnumsg").text("special characters are not allowed except(Slash,Minus,Underscore)");
        $('#Part_Number').focus();
        return false;
    }
    else {
        $("#partnumsg").text("");
        return true;
    }

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
                DisplayNotification(costMsg, 'error');
                return false;
            }
            else {
                return true;
            }

        }
    }
    else {
        DisplayNotification("“Resale” is required.", "error");
        return false;
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
function editParts(val1, val2) {
    $.ajax({
        url: '../DistributorParts/EditPartsDetails?PartsNumber=' + encodeURIComponent(val1) + '&D_Id=' + val2,
        type: 'Get',
        success: function (data) {
            $("#title-cont").text("Edit Parts Details");

            //Note: These distributor, category dropdown changes are just work around. The way of retrieving is totally wrong. We need to rewamp the add and edit facility of the page.
            //To refill the distributor data
            $("#DistributorID").find('option[value!=""]').remove();
            var $distributorlist = $("#DistributorID");

            $.each(data.distributorList, function (i, distributorList) {
                //To remove the "STANDARD" option from the list
                if (distributorList.value !== -1) {
                    $('<option>', {
                        value: distributorList.value
                    }).html(distributorList.Name).appendTo($distributorlist);
                }
            });

            $("#DistributorID").val(data.DistributorID);
            $("#DistributorID").focus();

            //To refill the category data
            $("#Part_Category").find('option[value!=""]').remove();
            var $partcatgorylist = $("#Part_Category");

            $.each(data.partcatgoryList, function (i, partcatgoryList) {
                $('<option>', {
                    value: partcatgoryList.Part_Category.trim()
                }).html(partcatgoryList.Part_Category).appendTo($partcatgorylist);
            });
            //$("#DistributorID").val(data.DistributorID);
            //$("#DistributorID").find('option[value="' + data.DistributorID + '"]').attr('selected', 'selected');
            $("#Part_Category").val(data.Part_Category).trigger('change');


            //$("#DistributorID").val(data.DistributorID).trigger('change');
            //setTimeout(function () {
            //    $("#Part_Category").val(data.Part_Category).trigger('change');
            //}, 500);

            
            //$("#DistributorID").val(data.DistributorID);
            $("#Part_Number").val(data.Part_Number);
            $("#Cost").val(data.Cost);
            $("#UOM").val(data.UOM);
            $("#Rcost").val(data.Resale_Cost);
            $("#Purchased_From").val(data.Purchased_From);
            $("#Description").val(data.Description);
            $("#LaborUnit").val(data.LaborUnit);
            $("#Part_Category").val(data.Part_Category);

            // $("#Client_Description").val(data.Client_Description);
            $("#saveb").hide();
            $("#updateb").show();
            $("#Cancel").show();
            $("#Reset").hide();


        },
        error: function (err) {
            DisplayNotification("\"Network Error\" Please try later!", "error");
        }
    });
}
function Cancel() {
    msg = "Are you sure, do you want to cancel?"
    if (confirm(msg)) {
        $('#failedbtn').hide();
        //editParts('0');
        //getpartslist(" ");
        //$("#title-cont").text("Add Parts Details")
        window.location.href = '../DistributorParts/Index';
        return true;
    }
    else {
        return false;
    }
}
function GetMyPartsCatgory(val) {
    var clientid = $(val).val();
    var partnumber = $("#Part_Number").val();
    //if (clientid == "") {
        //$("#Part_Category").attr('disabled', 'disabled');
    //}
    //else {
        $.ajax({
            url: '../DistributorParts/GetMyDistributorPartsCatgory?Did=' + clientid + '&partnumber=' + partnumber,
            type: 'Get',
            success: function (data) {
                $("#Part_Category").removeAttr('disabled')
                dropdownElement = $("#Part_Category");
                dropdownElement.find('option[value!=""]').remove();
                var $partcatgorylist = $("#Part_Category");
                //if (partnumber != "" && partnumber != null) {
                //    $.each(data.partcatgoryList, function (i, catgory) {
                //        $('<option>', {
                //            value: catgory.Part_Category
                //        }).html(catgory.Part_Category).appendTo($partcatgorylist);
                //    });
                //    $("#Part_Category").val(data.Part_Category);
                //}
                //else {
                    $.each(data, function (i, catgory) {
                        $('<option>', {
                            value: catgory.Part_Category
                        }).html(catgory.Part_Category).appendTo($partcatgorylist);

                    });
                //}
            },
            error: function (err) { }
        });
    //}
}
function saveParts() {
    if (validatePartDetails()) {
        var Model = {
            "Client_ID": $("#Client_ID").val(),
            "DistributorID": $("#DistributorID").val(),
            "Part_Category": $('#Part_Category').val(),
            "OtherPart_Category": $('#OtherPart_Category').val(),
            "Part_Number": $('#Part_Number').val(),
            "Cost": $('#Cost').val(),
            "Resale_Cost": $('#Rcost').val(),
            "Purchased_From": $('#Purchased_From').val(),
            "Description": $('#Description').val(),
            "Client_Description": "",
            "UOM": $('#UOM').val(),
            "LaborUnit": $('#LaborUnit').val()
        }
        $.ajax({
            url: '../DistributorParts/CheckIsExistPartNumber',
            data: Model,
            type: 'POST',
            success: function (data) {
                if (data == "Already Exists") {
                    msg = "Part already exists. Would you like to update it?"
                    if (confirm(msg)) {
                        SavePartsDetails(Model)
                        $("#title-cont").text("Add Parts Details");
                    }
                    else {
                        $('#Part_Number').focus();
                    }
                }
                else {
                    msg = "Would you like to add this part?"
                    if (confirm(msg)) {

                        SavePartsDetails(Model)
                    }
                }
            }
        })
    }
}
function validatePartDetails() {
    var valid = true;
    $("#spandistributorerror").hide();
    $("#spancategoryerror").hide();
    $("#spanothercategoryerror").hide();
    $("#spanpartnumbererror").hide();
    //$("#spanmycosterror").hide();
    //$("#spanresaleerror").hide();
    $("#spaninvalidothercategoryerror").hide();
    if ($("#DistributorID").val() == null || $("#DistributorID").val() == "") {
        $("#spandistributorerror").show();
        valid = false;
    }
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
    if (!valid) {
        DisplayNotification("Please fill “All” required (*) field.", "error");
    }
    return valid;
}
function SavePartsDetails(Model) {
    $.ajax({
        url: '../DistributorParts/AddPartsDetails',
        data: Model,
        type: 'POST',
        success: function (data) {
            if (data == "“New Parts” has been added successfully.") {
                DisplayNotification("“New Parts” have been added.", "success");
                Cookies.set('partstpageno', "");
                Cookies.set('Partsdtpagelen', "");
                Cookies.set('Partsearch', "");
                $("#saveb").show();
                $("#updateb").hide();
                $("#Cancel").hide();
                $("#Reset").show();
                $("#Part_Category").val('').trigger('change');
                $("#DistributorID").val('').trigger('change');
                $("#Part_Number").val('');
                $("#Cost").val('');
                $("#UOM").val('');
                $("#Rcost").val('');
                $("#Purchased_From").val('');
                $("#Description").val('');
                $("#Client_Description").val('');
                $("#LaborUnit").val('');
                $('#failedbtn').hide();
                if (Cookies.get('distpartsfilter') != null && Cookies.get('distpartsfilter') != "")
                    getpartslist(Cookies.get('distpartsfilter'), 1);
                else
                    getpartslist("", 1);
            }
            else if (data == "“Parts” are updated successfully.") {
                DisplayNotification("“Part” has been updated successfully.", "success");
                $("#saveb").show();
                $("#updateb").hide();
                $("#Cancel").hide();
                $("#Reset").show();
                $("#Part_Category").val('').trigger('change');
                $("#DistributorID").val('').trigger('change');
                $("#Part_Number").val('');
                $("#Cost").val('');
                $("#UOM").val('');
                $("#LaborUnit").val('');
                $("#Rcost").val('');
                $("#Purchased_From").val('');
                $("#Description").val('');
                $("#Client_Description").val('');
                $('#failedbtn').hide();
                if (Cookies.get('distpartsfilter') != null && Cookies.get('distpartsfilter') != "")
                    getpartslist(Cookies.get('distpartsfilter'), 1);
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
function getpartslist(val, showBootom) {
    if (val == "ALL Parts Category")
        val = "";
    Cookies.set("distpartsfilter", val);
    $("#ptexample").DataTable().ajax.reload(null, false);
    if (showBootom === 1) {
        $("#bootomDistributorParts").focus();
    }
    //$.ajax({
    //    url: '../DistributorParts/PartsList?PartCatgory=' + val,
    //    type: 'Get',
    //    success: function (data) {
    //        $('#divpartslist').html(data);
    //        if (val == "") {
    //            val = "ALL Parts Category";
    //            $('#Part_CategoryList').val(val);
    //        }
    //        else {
    //            $('#Part_CategoryList').val(val);
    //        }
    //        if (showBootom === 1) {
    //            $("#bootomDistributorParts").focus();
    //        }
    //    },
    //    error: function (err) {
    //        DisplayNotification("\"Network Error\" Please try later!", "error");
    //    }
    //});
}

function deleteParts(val1, val2) {
    msg = "Would you like to delete this part?"
    if (confirm(msg)) {
        $.ajax({
            url: '../DistributorParts/DeletePartsDetails',
            data: { PartsNumber: val1, DistributorID: val2 },
            type: 'POST',
            success: function (data) {
                if (data != "“Deleted” successfully.") {
                    DisplayNotification(data, "error");
                }
                //getpartslist();
                if (Cookies.get('distpartsfilter') != null && Cookies.get('distpartsfilter') != "")
                    getpartslist(Cookies.get('distpartsfilter'));
                else
                    getpartslist("");
                DisplayNotification("“Parts details” are deleted successfully.", "success");
                // spinner.stop();
            },
            error: function (err) { }

        });
    }
    else {
        return false;
    }
}

function ResetAll() {
    msg = "Are you sure, do you want to “Clear” all fields?"
    if (confirm(msg)) {
        $("#Client_ID").val("");
        $("#Part_Category").val('').trigger('change');
        $("#DistributorID").val('').trigger('change');
        $("#OtherPart_Category").val("");
        $("#Part_Number").val("");
        $("#Cost").val("");
        $("#Rcost").val("");
        $("#Description").val("");
        $("#Client_Description").val("");
        $("#Purchased_From,#UOM").val("");
        $('#failedbtn').hide();
        $("#UOM").val("");
        $("#LaborUnit").val('');
        //if ($("#IsAdmin").val() == "Admin") {
        //    $("#Part_Category").attr('disabled', 'disabled');
        //}
        $("#Part_Category").removeAttr('disabled', 'disabled');
        $("#OtherPart_Category").attr('disabled', 'disabled');
        return true;
    }
    else {
        return false;
    }
}
function confirmImport() {
    if ($("#DistributorID").val() != 0) {
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
    } else {
        DisplayNotification("Please select \"Distributor\"", 'error');
        $("#importfile").val("").clone(true);
    }
}
function ImportParts() {
    $("#Loading").show();
    $("#failedbtn").hide();
    var formdata = new FormData();
    var doc = document.getElementById('importfile');
    if (doc.files.length > 0) {
        formdata.append('partsheet', doc.files[0]);
        var target = $("body");
        $.ajax({
            //url: '../DistributorParts/Importparts?Did=' + $("#DistributorID").val(),
            url: '../DistributorParts/ImportParts?distributorID=' + $("#DistributorID").val(),
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
                    if (Cookies.get('distpartsfilter') != null && Cookies.get('distpartsfilter') != "") {
                        getpartslist(Cookies.get('distpartsfilter'));
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
                //    //editParts('0',0);
                //    if (Cookies.get('distpartsfilter') != null && Cookies.get('distpartsfilter') != "")
                //        getpartslist(Cookies.get('distpartsfilter'));
                //    else
                //        getpartslist("");
                //}
                //else {
                //    //editParts('0', '0');
                //    $("#Loading").hide();
                //    DisplayNotification(data, 'error');
                //    $("#importfile").val("").clone(true);
                //}
            },
            error: function (err) { }
        });
    }
}