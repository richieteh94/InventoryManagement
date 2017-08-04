var formCount = 0;

function addForm() {
    var num = document.getElementById("refillStockNumber").value;

    if (isNaN(num)) {
        alert("Please enter number into the stock refill form.")
    } else {
        var repeats = document.getElementById("repeatForm").innerHTML = "";
        for (i = 0; i < num; i++) {
            addRepeatForm(i);
        }
        formCount = num;
    }
}

function addRepeatForm(num) {
    var repeat = document.getElementById("repeatForm");

    var panel = document.createElement("div");
    panel.className = "panel panel-default";

    var panelBody = document.createElement("div");
    panelBody.className = "panel-body";

    var formGroupBarcode = document.createElement("div");
    formGroupBarcode.className = "form-group";

    var labelBarcode = document.createElement("label");
    labelBarcode.htmlFor = "refillBarcode" + num;
    var node1 = document.createTextNode("Barcode:");
    labelBarcode.appendChild(node1);

    var inputBarcode = document.createElement("input");
    inputBarcode.className = "form-control";
    inputBarcode.placeholder = "eg: 3000212302132";
    inputBarcode.addEventListener("blur", function (event) {
        blurSearch(num);
        event.preventDefault();
    });
    inputBarcode.id = "refillBarcode" + num;
    inputBarcode.type = "text";

    var formGroupName = document.createElement("div");
    formGroupName.className = "form-group";

    var labelName = document.createElement("label");
    labelName.htmlFor = "refillName" + num;
    var node2 = document.createTextNode("Name:");
    labelName.appendChild(node2);

    var inputName = document.createElement("input");
    inputName.className = "form-control";
    inputName.placeholder = "eg: NutriPlus Egg";
    inputName.readOnly = "true";
    inputName.id = "refillName" + num;
    inputName.type = "text";

    var formGroupAmount = document.createElement("div");
    formGroupAmount.className = "form-group";

    var labelAmount = document.createElement("label");
    labelAmount.htmlFor = "refillAmount" + num;
    var node3 = document.createTextNode("Quantity:");
    labelAmount.appendChild(node3);

    var inputAmount = document.createElement("input");
    inputAmount.className = "form-control";
    inputAmount.id = "refillAmount" + num;
    inputAmount.type = "number";

    formGroupBarcode.appendChild(labelBarcode);
    formGroupBarcode.appendChild(inputBarcode);
    formGroupName.appendChild(labelName);
    formGroupName.appendChild(inputName);
    formGroupAmount.appendChild(labelAmount);
    formGroupAmount.appendChild(inputAmount);

    panelBody.appendChild(formGroupBarcode);
    panelBody.appendChild(formGroupName);
    panelBody.appendChild(formGroupAmount);

    panel.appendChild(panelBody);

    repeat.appendChild(panel);
}

function blurSearch(num) {
    $("refillBarcode" + num).ready(function () {

        var searchInput = document.getElementById("refillBarcode" + num).value;
        document.getElementById("refillName" + num).value = "";

        if (searchInput != "" || searchInput != null) {
            $.ajax({
                type: "POST",
                url: "HomePage.aspx/searchItemInModal",

                contentType: "application/json",
                dataType: "json",
                data: "{Num:" + num + ",Input: " + searchInput + "}",
                success: OnSuccess,
                failure: function (response) {
                    alert("Fail :" + response.responseText);
                },
                error: function (response) {
                    alert("Error: " + response.responseText);
                }
            });
        }
        
    });
}

function OnSuccess(result) {
    var searchedResult = result.d;
    var obj = JSON.parse(searchedResult);
    //alert("Success:" + obj["Name"]);
    if (obj["Name"] == "") {
        alert(obj["SearchInput"] + " does not exist.");
    } else {
        document.getElementById("refillName" + obj["Row"]).value = obj["Name"];
    }
}

function refillStock() {
    var exist = false; var allExist = true;
    for (i = 0; i < formCount; i++) {
        var name = document.getElementById("refillName"+i).value;
        var quantity = document.getElementById("refillAmount"+i).value;
        if (name != "" && quantity != "") {
            exist = true;
        } else {
            allExist = false;
        }
    }

    if (exist && allExist) {
        var arrayData = new Array();
        for (i = 0; i < formCount; i++) {
            var product = new Object();
            product.barcode = document.getElementById("refillBarcode" + i).value;
            product.name = document.getElementById("refillName" + i).value;
            product.quantity = document.getElementById("refillAmount" + i).value;
            arrayData.push(product);
        }
        //var jsonArray = JSON.stringify(arrayData);
        //alert(jsonArray);

        $.ajax({
            type: "POST",
            url: "HomePage.aspx/refillItems",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ products: arrayData }),
            success: function (response) {
                
                alert(response.d);
                $('#refillStock').modal('toggle');
                window.location.href = "HomePage.aspx";
                  
            },
            failure: function (response) {
                alert("Fail :" + response.d);
            },
            error: function (response) {
                alert("Error: " + response.responseText);
            }
        });
        
    } else {
        alert("There is/are blank value(s).");
    }
}