/// <reference path="bootstrap2.min.js" />
//function LoadClientIDDropdown()
//{
//    alert("ajxgggin");
//    var ddl = $("select[name=Client_ID]");
//    if ($(ddl).length > 0)
//    {
//        alert("ajx");
//        $.ajax({

//            url: "/Ajax/GetAllClientList",

//            contentType: "application/json;charset=utf-8",
//            datatype: "json",
//            beforeSend: function () {
//                $(ddl).prev().addClass("loading");
//            },

//            success: function (data) {

//                $(ddl).prev().removeClass("loading");
//                $(ddl).html("").append(GetDefaultOption($(ddl)));
//                $.each(data, function (i, item) {
//                    $(ddl).append("<option value='" + item.Client_ID + "'>" + item.User_ID + "</option>");

//                });
//                $(ddl).val(value);

//            },
//            error: function (result) {
//                $(ddl).prev().removeClass("loading");
//                //alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
//            }

//        });
//    }
//}
function LoadContent(element, url) {
    $.ajax({
        url: url,
        beforeSend: function () {
            $(element).prev().addClass("loading")
            $(element).html("");
        },
        success: function (result) {
            $(element).prev().addClass("loading")
            $(element).html(result);
        },
        error: function () {
            $(element).prev().addClass("loading")
            $(element).html("Error while loading content");
        }
    });
}
function LoadClientIDDropdown(value, callback) {
    // alert(value);
    var ddl = $("select[name=Client_ID]");
    if ($(ddl).length > 0) {
        //alert("if");
        $.ajax({
            url: "/Ajax/GetAllClientList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $(ddl).prev().addClass("loading");
            },
            success: function (data) {
                $(ddl).prev().removeClass("loading");

                //$(ddl).html("").append(GetDefaultOption($(ddl)));
                $.each(data, function (i, item) {
                    $(ddl).append("<option value='" + item.Client_ID + "'>" + item.Client_Company + "</option>");
                });
                $(ddl).val(value);

                if (callback != undefined)
                    callback();
            },
            error: function (result) {
                $(ddl).prev().removeClass("loading");

                if (callback != undefined)
                    callback();
                //alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }
}

//function LoadAssembliesNameListDropdown(value, callback) {
//     alert(value);
//    var ddl = $("select[name=Assemblies_Name]");
//    if ($(ddl).length > 0) {
//        //alert("if");
//        $.ajax({
//            url: "/Ajax/GetMyJobAssembliesList",
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            beforeSend: function () {
//                $(ddl).prev().addClass("loading");
//            },
//            success: function (data) {
//                $(ddl).prev().removeClass("loading");

//                //$(ddl).html("").append(GetDefaultOption($(ddl)));
//                $.each(data, function (i, item) {
//                    $(ddl).append("<option value='" + item.Assemblies_Name + "'>" + item.Assemblies_Name + "</option>");
//                });
//                $(ddl).val(value);

//                if (callback != undefined)
//                    callback();
//            },
//            error: function (result) {
//                $(ddl).prev().removeClass("loading");

//                if (callback != undefined)
//                    callback();
//                //alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
//            }
//        });
//    }
//}

function LoadPartsCategorydropdown(value, callback) {

    var ddl = $("select[name=Part_Category]");
    if ($(ddl).length > 0) {
        //alert("if");
        $.ajax({
            url: "/Ajax/GetPartsCategoryList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $(ddl).prev().addClass("loading");
            },
            success: function (data) {
                $(ddl).prev().removeClass("loading");

                //$(ddl).html("").append(GetDefaultOption($(ddl)));
                $(ddl).append("<option value='0'> Select  </option>")
                $(ddl).append("<option value='1'> other  </option>")
                $.each(data, function (i, item) {

                    $(ddl).append("<option value='" + item.Part_Category + "'>" + item.Part_Category + "</option>");
                });
                $(ddl).val(value);

                if (callback != undefined)
                    callback();
            },
            error: function (result) {
                $(ddl).prev().removeClass("loading");

                if (callback != undefined)
                    callback();
                //alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }


}

function LoadLabourCategorydropdown(value, callback) {

    var ddl = $("select[name=Labor_Category]");
    // alert("lab");
    if ($(ddl).length > 0) {
        //  alert("lab");
        $.ajax({
            url: "/Ajax/GetLabourCategoryList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $(ddl).prev().addClass("loading");
            },
            success: function (data) {
                $(ddl).prev().removeClass("loading");

                //$(ddl).html("").append(GetDefaultOption($(ddl)));
                $(ddl).append("<option value='0'> Select  </option>")
                $(ddl).append("<option value='1'> other  </option>")
                $.each(data, function (i, item) {
                    ;
                    $(ddl).append("<option value='" + item.Labor_Category + "'>" + item.Labor_Category + "</option>");
                });
                $(ddl).val(value);

                if (callback != undefined)
                    callback();
            },
            error: function (result) {
                $(ddl).prev().removeClass("loading");

                if (callback != undefined)
                    callback();
                //alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }


}
function LoadLegalCategorydropdown(value, callback) {

    var ddl = $("select[name=Legal_Category]");
    if ($(ddl).length > 0) {
        //alert("if");
        $.ajax({
            url: "/Ajax/GetLegalCategoryList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $(ddl).prev().addClass("loading");
            },
            success: function (data) {
                $(ddl).prev().removeClass("loading");

                //$(ddl).html("").append(GetDefaultOption($(ddl)));
                $(ddl).append("<option value='0'> Select or New Category </option>")
                $(ddl).append("<option value='1'> other  </option>")
                $.each(data, function (i, item) {

                    $(ddl).append("<option value='" + item.Legal_Category + "'>" + item.Legal_Category + "</option>");
                });
                $(ddl).val(value);

                if (callback != undefined)
                    callback();
            },
            error: function (result) {
                $(ddl).prev().removeClass("loading");

                if (callback != undefined)
                    callback();
                //alert('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }


}

function LoadAssebliesNamedropdown(value, callback) {

    var ddl = $("select[name=Assemblies_Name]");
    // alert("lab");
    if ($(ddl).length > 0) {
        //alert("lab");
        $.ajax({
            url: "/Ajax/GetMyAssemliesNameList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $(ddl).prev().addClass("loading");
            },
            success: function (data) {
                $(ddl).prev().removeClass("loading");

                //$(ddl).html("").append(GetDefaultOption($(ddl)));
                // $(ddl).append("<option value='0'> Select  </option>")
                //$(ddl).append("<option value='1'> other  </option>")
                $.each(data, function (i, item) {
                    ; //alert(i);
                    // alert(item.Assemblies_Name)
                    $(ddl).append("<option value='" + item.Assemblies_Name + "'>" + item.Assemblies_Name + "</option>");
                });
                $(ddl).val(value);

                if (callback != undefined)
                    callback();
            },
            error: function (result) {
                $(ddl).prev().removeClass("loading");

                if (callback != undefined)
                    callback();
                //alert                 ('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }


}


function LoadAssembliesCategorydropdown(value, callback) {
    //alert("lll");
    var ddl = $("select[name=Assemblies_Category]");
    // alert("ascat");
    if ($(ddl).length > 0) {
        //alert("lab");
        $.ajax({
            url: "/Ajax/GetAssemliesCategoryList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $(ddl).prev().addClass("loading");
            },
            success: function (data) {
                $(ddl).prev().removeClass("loading");

                //$(ddl).html("").append(GetDefaultOption($(ddl)));
                //$(ddl).append("<option value='0'> Select  </option>")
                $(ddl).append("<option value='1'> other  </option>")
                $.each(data, function (i, item) {
                    ; //alert(i);
                    // alert(item.Assemblies_Name)
                    $(ddl).append("<option value='" + item.Assemblies_Category + "'>" + item.Assemblies_Category + "</option>");
                });
                $(ddl).val(value);
                if (!isediting) {
                    setTimeout(function () {
                        var ddl = $("select[name=Assemblies_Category]");
                        var ddlopt = $("select[name=Assemblies_Category] option");
                        var opt = "";

                        for (var i = 0; i < ddlopt.length; i++) {
                            if ($(ddlopt[i]).val() != null && $(ddlopt[i]).val() != "" && $(ddlopt[i]).val() != "1") {
                                $('#lstassembly_cat').append("<option value='" + $(ddlopt[i]).val() + "'>" + $(ddlopt[i]).val() + "</options>");
                            }
                        }
                        $('#lstassembly_cat').val(category);
                    }, 2000)
                }
                if (callback != undefined)
                    callback();
            },
            error: function (result) {
                $(ddl).prev().removeClass("loading");

                if (callback != undefined)
                    callback();
                //alert                 ('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }


}

function LoadNationWideAssembliesCategorydropdown(value, callback) {
    //alert("lll");

    var ddl = $("select[name=Assemblies_Category]");
    // alert("ascat");
    if ($(ddl).length > 0) {
        //alert("lab");
        $.ajax({
            url: "/Ajax/GetNationWideAssemliesCategoryList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $(ddl).prev().addClass("loading");
            },
            success: function (data) {

                $(ddl).prev().removeClass("loading");

                //$(ddl).html("").append(GetDefaultOption($(ddl)));
                //$(ddl).append("<option value='0'> Select  </option>")
                $(ddl).append("<option value='1'> other  </option>")
                $.each(data, function (i, item) {
                    ; //alert(i);
                    // alert(item.Assemblies_Name)
                    $(ddl).append("<option value='" + item.Assemblies_Category + "'>" + item.Assemblies_Category + "</option>");
                });
                value = value.replaceAll('&amp;', '&');
                console.log('LoadNationWideAssembliesCategorydropdown', value);
                $(ddl).val(value);
                //if (!isediting) {
                //    setTimeout(function () {
                //        var ddl = $("select[name=Assemblies_Category]");
                //        var ddlopt = $("select[name=Assemblies_Category] option");
                //        var opt = "";

                //        for (var i = 0; i < ddlopt.length; i++) {
                //            if ($(ddlopt[i]).val() != null && $(ddlopt[i]).val() != "" && $(ddlopt[i]).val() != "1") {
                //                $('#lstassembly_cat').append("<option value='" + $(ddlopt[i]).val() + "'>" + $(ddlopt[i]).val() + "</options>");
                //            }
                //        }

                //        $('#lstassembly_cat').val(category);
                //    }, 2000)
                //}
                if (callback != undefined)
                    callback();
            },
            error: function (result) {
                $(ddl).prev().removeClass("loading");

                if (callback != undefined)
                    callback();
                //alert                 ('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }


}
function LoadNationWideAssembliesExportCategorydropdown(value, callback) {
    //alert("lll");

    var ddl = $("select[name=Assemblies_Category]");
    // alert("ascat");
    if ($(ddl).length > 0) {
        //alert("lab");
        $.ajax({
            url: "/Ajax/GetNationWideAssemliesCategoryList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $(ddl).prev().addClass("loading");
            },
            success: function (data) {
                $(ddl).prev().removeClass("loading");

                //$(ddl).html("").append(GetDefaultOption($(ddl)));
                //$(ddl).append("<option value='0'> Select  </option>")
                //$(ddl).append("<option value='1'> other  </option>")
                $.each(data, function (i, item) {
                    ; //alert(i);
                    // alert(item.Assemblies_Name)
                    $(ddl).append("<option value='" + item.Assemblies_Category + "'>" + item.Assemblies_Category + "</option>");
                });
                $(ddl).val(value);
                if (!isediting) {
                    setTimeout(function () {
                        var ddl = $("select[name=Assemblies_Category]");
                        var ddlopt = $("select[name=Assemblies_Category] option");
                        var opt = "";
                        //for (var i = 0; i < ddlopt.length; i++) {
                        //    if ($(ddlopt[i]).val() != null && $(ddlopt[i]).val() != "" && $(ddlopt[i]).val() != "1") {
                        //        $('#lstassembly_cat').append("<option value='" + $(ddlopt[i]).val() + "'>" + $(ddlopt[i]).val() + "</options>");
                        //    }
                        //}
                        $('#lstassembly_cat').val(category);
                    }, 2000)
                }
                if (callback != undefined)
                    callback();
            },
            error: function (result) {
                $(ddl).prev().removeClass("loading");

                if (callback != undefined)
                    callback();
                //alert                 ('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }


}

function LoadDistributorAssembliesCategorydropdown(value, callback) {
    var ddl = $("select[name=Assemblies_Category]");
    if ($(ddl).length > 0) {
        $.ajax({
            url: '/Ajax/GetDistributorAssemliesCategoryList?DistributorId=' + parseInt($("#DistributorID").val(), 0),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () { $(ddl).prev().addClass("loading"); },
            success: function (data) {
                $(ddl).prev().removeClass("loading");
                $(ddl).empty();
                $(ddl).append('<option value="">' + "Select" + '</option>');
                $.each(data, function (i, item) {
                    $(ddl).append("<option value='" + item.Assemblies_Category + "'>" + item.Assemblies_Category + "</option>");
                });
                if (value != "") {
                    $(ddl).val(value);
                }
                if (callback != undefined)
                    callback();
            },
            error: function (result) {
                $(ddl).prev().removeClass("loading");
                if (callback != undefined)
                    callback();
            }
        });
    }
}

function LoadDistributorAssembliesExportCategorydropdown(value, callback) {
    var ddl = $("select[name=Assemblies_Category]");
    if ($(ddl).length > 0) {
        $.ajax({
            url: "/Ajax/GetDistributorAssemliesCategoryList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () { $(ddl).prev().addClass("loading"); },
            success: function (data) {
                $(ddl).prev().removeClass("loading");
                $.each(data, function (i, item) {
                    $(ddl).append("<option value='" + item.Assemblies_Category + "'>" + item.Assemblies_Category + "</option>");
                });
                $(ddl).val(value);
                if (!isediting) {
                    setTimeout(function () {
                        var ddl = $("select[name=Assemblies_Category]");
                        var ddlopt = $("select[name=Assemblies_Category] option");
                        var opt = "";
                        $('#lstassembly_cat').val(category);
                    }, 2000)
                }
                if (callback != undefined)
                    callback();
            },
            error: function (result) {
                $(ddl).prev().removeClass("loading");
                if (callback != undefined)
                    callback();
            }
        });
    }
}


function NationwideAssemblylistcategory(value, callback) {

    var ddl = $("select[name=Assemblies_Category]");
    // alert("ascat");
    if ($(ddl).length > 0) {
        //alert("lab");
        $.ajax({
            url: "/Ajax/GetNationWideAssemliesCategoryList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $(ddl).prev().addClass("loading");
            },
            success: function (data) {

                if (!isediting) {
                    $('#lstassembly_cat').html("");
                    //var ddl = $("select[id=lstassembly_cat]");
                    //var ddlopt = $("select[id=lstassembly_cat] option");
                    //$(ddlopt).empty();
                    var opt = "";
                    $("#lstassembly_cat").append("<option value=''>All Assembly Category</option>");
                    $.each(data, function (i, item) {
                        $('#lstassembly_cat').append("<option value='" + item.Assemblies_Category + "'>" + item.Assemblies_Category + "</options>");
                    });
                    //for (var i = 0; i < data.length; i++) {
                    //    if ($(data[i]).val() != null && $(data[i]).val() != "" && $(data[i]).val() != "1") {
                    //        $('#lstassembly_cat').append("<option value='" + $(data[i]).val() + "'>" + $(data[i]).val() + "</options>");
                    //    }
                    //}

                    $('#lstassembly_cat').val(category);

                }
                if (callback != undefined)
                    callback();
            },
            error: function (result) {
                $(ddl).prev().removeClass("loading");

                if (callback != undefined)
                    callback();
                //alert                 ('Service call failed: ' + result.status + ' Type :' + result.statusText);
            }
        });
    }
}

function Distibutolistvalue(value, callback) {
    var ddl = $("select[name=Assemblies_Category]");
    if ($(ddl).length > 0) {
        $.ajax({
            url: "/Ajax/GetDistributorAssemliesCategoryList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () { $(ddl).prev().addClass("loading"); },
            success: function (data) {
                if (!isediting) {
                    $('#lstassembly_cat').html("");
                    var opt = "";
                    $("#lstassembly_cat").append("<option value=''>All Assembly Category</option>");
                    $.each(data, function (i, item) {
                        $('#lstassembly_cat').append("<option value='" + item.Assemblies_Category + "'>" + item.Assemblies_Category + "</options>");
                    });
                    $('#lstassembly_cat').val(category);
                }
                if (callback != undefined)
                    callback();
            },
            error: function (result) {
                $(ddl).prev().removeClass("loading");

                if (callback != undefined)
                    callback();
            }
        });
    }
}

