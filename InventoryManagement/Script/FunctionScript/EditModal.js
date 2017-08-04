function searchEditModal() {
    var searchInput = document.getElementById("editSearchInput").value;

    $.ajax({
        type: "POST",
        url: "HomePage.aspx/searchItemEditModal",
        contentType: "application/json",
        dataType: "json",
        data: "{searchInput:"+searchInput+"}",
        success: function (response) {
            
            //$('#edit').modal('toggle');
            //window.location.href = "HomePage.aspx"
            //alert("Barcode : " + response.d);
            var searchedResult = response.d;
            
            var obj = JSON.parse(searchedResult);
            //alert("Success:" + obj["Name"]);
            if (obj["Name"] == "") {
                alert(obj["SearchInput"] + " does not exist.");
            } else {
                document.getElementById("editBarcode").value = obj["Barcode"];
                document.getElementById("editName").value = obj["Name"];
                document.getElementById("editPrice").value = obj["Price"];
                if (obj["Status"]=="Available") {
                    document.getElementById("editStatus").value = 1;
                } else {
                    document.getElementById("editStatus").value = 0;
                }
            }

        },
        failure: function (response) {
            alert("Fail :" + response.d);
        },
        error: function (response) {
            alert("Error: " + response.responseText);
        }
    });
}

function editStock() {
    var barcode = document.getElementById("editBarcode").value;
    var name = document.getElementById("editName").value;
    var price = document.getElementById("editPrice").value;
    var e = document.getElementById("editStatus");
    var status = e.options[e.selectedIndex].text;

    if (barcode == ""){
        alert("Please search an item before edit.");
    } else if(name == "" || price == ""){
        alert("Please fill in the fill.");
    } else {
        var product = new Object();
        product.barcode = barcode;
        product.name = name;
        product.price = price;
        product.status = status;

        $.ajax({
           type: "POST",
           url: "HomePage.aspx/searchEditModalConfirm",
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           data: "{product:" + JSON.stringify(product) + "}",
           success: function (response) {

               alert(response.d);
               $('#editStock').modal('toggle');
               window.location.href = "HomePage.aspx";
           },
           failure: function (response) {
                alert("Fail :" + response.d);
           },
           error: function (response) {
                alert("Error: " + response.responseText);
           }
        });
    }
}