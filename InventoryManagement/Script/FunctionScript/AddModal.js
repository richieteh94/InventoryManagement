function addNewStock() {
    var barcode = document.getElementById("addBarcode").value;
    var name = document.getElementById("addName").value;
    var price = document.getElementById("addPrice").value;
    var quantity = document.getElementById("addAmount").value;
    var e = document.getElementById("addStatus");
    var status = e.options[e.selectedIndex].text;

    if (barcode == "" || name == "" || price == "" || quantity == "" || status == "") {
        alert("Please fill in the field.");
    }  else {
        var product = new Object();
        product.barcode = barcode;
        product.name = name;
        product.price = price;
        product.quantity = quantity;
        product.status = status;

        $.ajax({
            type: "POST",
            url: "HomePage.aspx/addModalConfirm",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{product:" + JSON.stringify(product) + "}",
            success: function (response) {

                alert(response.d);
                $('#addNewStock').modal('toggle');
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