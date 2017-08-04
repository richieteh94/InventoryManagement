<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="InventoryManagement.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="Content/bootstrap.min.css" />
    <script src="Scripts/jquery-1.10.2.min.js" ></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/FunctionScript/RefillModal.js"></script>
    <script src="Scripts/FunctionScript/EditModal.js"></script>
    <script src="Scripts/FunctionScript/AddModal.js"></script>
    <script src="Scripts/FunctionScript/SearchTable.js"></script>
    <link rel="stylesheet" href="Content/PageCSS/HomePage.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>

        <div id="bar1"> 
            <div class="dropdown">
               <button id="menu_image" class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
               <img src="Images/menu.png" /></button>
                    <ul class="dropdown-menu ">
                        <li><a href="#" runat="server" onserverclick="LogOut">LogOut</a></li>
                    </ul>
            </div>

            <div id="username" runat="server" >Username</div>

        </div>
        <div id="bar2">
            <button id="btnRefill" type="button" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#refillStock">Refill Stock</button>
            <button id="btnAdd" type="button" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#addNewStock" >Add New Stock</button>
            <button id="btnEdit" type="button" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#editStock">Edit Stock Details</button>
        </div>
        <div id="bar3">
            
            <div class="input-group col-xs-10">
              <input type="text" class="form-control" id="searchInput" placeholder="Search" name="search"/>
              <div class="input-group-btn">
                <button onclick="searchTable()" class="btn btn-default" type="button"><i class="glyphicon glyphicon-search"></i></button>
              </div>
            </div>

        </div>
        <div id="bar4">
            
            <asp:Table runat="server" ID="searchTable" class="table table-bordered">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>Barcode</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Price</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Quantity</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Status</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                    
            </asp:Table>

           <!-- <table class="table table-bordered">
                <thead>
                  <tr>
                    <th id="barcode">Barcode</th>
                    <th id="name">Name</th>
                    <th id="price">Price(RM)</th>
                    <th id="quantity">Quantity</th>
                  </tr>
                </thead>
                <tbody>
                    
                
                  <tr>
                    <td>123</td>
                    <td>ABC</td>
                    <td>4.00</td>
                    <td>100</td>
                  </tr>
                  <tr>
                    <td>321</td>
                    <td>dopa2</td>
                    <td>0.99</td>
                    <td>200</td>
                  </tr>
                </tbody>
              </table>
            -->
        </div> 

        <!-- Modal -->
        <div class="modal fade" id="refillStock" role="dialog">
            <div class="modal-dialog">
    
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Refill Stock</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <p id="stockPara">Stock needed to refill :</p>
                            <input type="number" id="refillStockNumber"/>
                            <button id="refillStockGo" type="button" onclick="addForm()" class="btn btn-primary">Go</button>
                        </div>

                        <div id="repeatForm">

                        </div>

                        <!--<div class="panel panel-default">
                            <div class="panel-body">
                                <div class="form-group">
                                    <label for="refillBarcode">Barcode:</label>
                                    <input type="text" class="form-control" id="refillBarcode" placeholder="eg: 3000212302132" />
                                </div>
                                <div class="form-group">
                                    <label for="refillName">Name:</label>
                                    <input type="text" class="form-control" id="refillName" placeholder="eg:NutriPlus Egg" />
                                </div>
                                <div class="form-group">
                                    <label for="refillAmount">Quantity:</label>
                                    <input type="number" class="form-control" id="refillAmount" />
                                </div>
                            </div>
                         </div>-->

                        <button id="refillConfirm" type="button" onclick="refillStock()" class="btn btn-primary">Confirm</button>

                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
      
            </div>
        </div>

        <!-- Modal -->
        <div class="modal fade" id="addNewStock" role="dialog">
            <div class="modal-dialog">
    
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Add New Stock</h4>
                    </div>
                    <div class="modal-body">

                        <div class="form-group">
                            <label for="addBarcode">Barcode:</label>
                            <input type="text" class="form-control" id="addBarcode" placeholder="eg: 3000212302132" name="addBarcode" />
                        </div>
                        <div class="form-group">
                            <label for="addName">Name:</label>
                            <input type="text" class="form-control" id="addName" placeholder="eg:NutriPlus Egg" name="addName" />
                        </div>
                        <div class="form-group">
                            <label for="addPrice">Price(RM):</label>
                            <input type="text" class="form-control" id="addPrice" placeholder="4.99" name="addPrice"/>
                        </div>
                        <div class="form-group">
                            <label for="addAmount">Quantity:</label>
                            <input type="number" class="form-control" id="addAmount" name="addAmount"/>
                        </div>
                        <div class="form-group">
                            <label for="addStatus">Status:</label>
                            <select class="form-control" id="addStatus">
                                <option>Available</option>
                                <option>Unavailable</option>
                            </select>
                        </div>
                        <button id="addConfirm" type="button" class="btn btn-primary" onclick="addNewStock()">Confirm</button>

                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
      
            </div>
        </div>

        <!-- Modal -->
        <div class="modal fade" id="editStock" role="dialog">
            <div class="modal-dialog">
    
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Edit Stock Detail</h4>
                    </div>
                    <div class="modal-body">

                        <div id="editSearch">
                            <div class="input-group col-xs-12">
                              <input type="text" class="form-control" placeholder="Search" id="editSearchInput"/>
                              <div class="input-group-btn">
                                <button class="btn btn-default" type="button" onclick="searchEditModal()"><i class="glyphicon glyphicon-search"></i></button>
                              </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="editBarcode">Barcode:</label>
                            <input type="text" class="form-control" id="editBarcode" readonly/>
                        </div>
                        <div class="form-group">
                            <label for="editName">Name:</label>
                            <input type="text" class="form-control" id="editName" placeholder="eg:NutriPlus Egg" />
                        </div>
                        <div class="form-group">
                            <label for="editPrice">Price(RM):</label>
                            <input type="text" class="form-control" id="editPrice" placeholder="4.99"/>
                        </div>
                        <div class="form-group">
                            <label for="editStatus">Status:</label>
                            <select class="form-control" id="editStatus">
                                <option value="1">Available</option>
                                <option value="0">Unavailable</option>
                            </select>
                        </div>
                        <button id="editConfirm" type="button" onclick="editStock()" class="btn btn-primary">Confirm</button>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
      
            </div>
        </div>

    </form>
    
</body>
</html>
