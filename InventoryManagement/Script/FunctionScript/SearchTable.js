function searchTable() {
    $("searchTable").ready(function () {

        var searchInput = document.getElementById("searchInput").value;
        var tableElm = document.getElementById("searchTable");
        var tableChilds = tableElm.getElementsByTagName("tr");

        if (searchInput != "") {
            if (isNaN(searchInput)) {
                for (i = 1; i < tableElm.rows.length; i++) {
                    var filter = searchInput.toUpperCase();
                    var name = tableElm.rows.item(i).cells.item(1).innerHTML;
                    if (name.toUpperCase().indexOf(filter) > -1) {
                        var row = tableChilds[i];
                        row.style.display = "";
                        //row.style.visibility = "hidden";
                    } else {
                        var row = tableChilds[i];
                        row.style.display = "none";
                    }
                }
            } else {
                for (i = 1; i < tableElm.rows.length; i++) {
                    var barcode = tableElm.rows.item(i).cells.item(0).innerHTML;
                    
                    if (searchInput != barcode) {
                        var row = tableChilds[i];
                        row.style.display = "none";
                        //row.style.visibility = "hidden";
                    } else {
                        var row = tableChilds[i];
                        row.style.display = "";
                    }
                }
            }   
        } else {
            for (i = 0; i < tableElm.rows.length; i++) {
                
                var row = tableChilds[i];
                row.style.display = "";
                //row.style.visibility = "visible";
            }
        }
    });
}
