
$(document).ready(function () {
    //To logout all the tabs if we click the logout button on one tab
    if (lsTest()) {
        $('#btnSignOut').on('click', function () {
            // change logout-event and therefore send an event
            localStorage.setItem('EE-logout-event', 'logout' + Math.random());
            return true;
        });
    } 
});

//This function will multipy input1 and input2. Result will be in 'result'
//Note: Will return 2 decimal points only
//val - Pass the series of 3 text boxes from grid
//decimalCount - number of decimalCount needed as output
//a*b=c
function MultiplyOnSeries(input1, input2, result, decimalCount) {

    for (var i = 0; i < input1.length; i++) {

        var a = Number($(input1[i]).val());
        if (isNaN(a)) {
            a = 0;
        }

        //To Handle dot(.)
        if ($(input1[i]).val().trim() === '.') {
            $(input1[i]).val('0.');
        }

        var b = Number($(input2[i]).val());
        if (isNaN(b)) {
            b = 0;
        }

        //To Handle dot(.)
        if ($(input2[i]).val().trim() === '.') {
            $(input2[i]).val('0.');
        }

        //to make the decimal count default as 2
        if (decimalCount === undefined) {
            decimalCount = 2;
        }

        //To generate the tenSeries to roundup the value
        var tenSeries = '1';
        for (var sNo = 0; sNo < decimalCount; sNo++) {
            tenSeries = tenSeries.concat('0');
        }

        //Multiplication
        $(result[i]).val(Math.round((a * b) * tenSeries) / tenSeries);//to roundup to specified decimals
    }
}

//This function will add all the rows of a column in the grid
//val - Pass the series of text boxes from grid
function AdditionOnSeries(val, decimalCount) {
    var total = 0;
    for (var i = 0; i < val.length; i++) {
        var value = Number($(val[i]).val());
        if (isNaN(value)) {
            value = 0;
        }

        //to make the decimal count default as 2
        if (decimalCount === undefined) {
            decimalCount = 2;
        }

        //To generate the tenSeries to roundup the value
        var tenSeries = '1';
        for (var sNo = 0; sNo < decimalCount; sNo++) {
            tenSeries = tenSeries.concat('0');
        }

        total = (parseFloat((total * tenSeries).toFixed()) + parseFloat((value * tenSeries).toFixed()))/tenSeries;
    }
    return total;
}

//To Handle dot(.)
function handleDecimalDot(element) {
    if ($(element).val().trim() === '.') {
        $(element).val('0.');
    }
}

//To check the given string input is zero or Not a Number
function isZeroOrNaN(input) {
    try {
        var value = parseFloat(input) || 0;
        if (value === 0) {
            return true;
        }
        else {
            return false;
        }
    }
    catch
    {
        return true;
    }
}

//Check local storage present in the browser or not (to check old browser)
function lsTest() {
    var test = 'test';
    try {
        localStorage.setItem(test, test);
        localStorage.removeItem(test);
        return true;
    } catch (e) {
        return false;
    }
}

// listen to storage event
window.addEventListener('storage', function (event) {
    if (event.key === 'EE-logout-event') {
        window.location = "../Login/Login";
    }
}, false);

//Redirect to the session timeout page for ajax call
function validateSessionTimeout(data) {
    var expired = data.includes("/Login/SessionTimout");
    if (expired) {
        window.location.href = "../Login/SessionTimout";
    }
}





