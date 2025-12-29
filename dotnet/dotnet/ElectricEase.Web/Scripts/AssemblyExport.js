function ValidateLaborUnit() {
    $("#Loading").show();
    if (DistributorId.length > 0 || ClienId.length > 0) {
        if (AssemblyIds.length > 0) {

            var model = {
                "DisbutorId": DistributorId,
                "ClienId": ClienId,
                "AssemblyId": AssemblyIds
            };
            
            $.ajax({
                url: '../NationwideAssembly/ValidateLaborUnit',
                type: 'POST',
                data: JSON.stringify(model),
                cache: false,
                contentType: 'application/json',
                success: function (data) {
                    if (data === "Success") {
                        $("#Loading").hide();
                        alert("Labor units are validated successfully!");
                        window.location.href = "../NationwideAssembly/AssemblyExport";
                        //DisplayNotification("Labor units are validated successfully.", "success");
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
            DisplayNotification("Please select “Assemblies”!", "error");
        }
    }
    else {
        $("#Loading").hide();
        DisplayNotification("Please select “Distributors” or “Standard” names!", "error");
    }
}

