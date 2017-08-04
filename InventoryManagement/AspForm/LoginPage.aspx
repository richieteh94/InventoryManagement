<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="InventoryManagement.LoginPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="Content/bootstrap.min.css" />
    <script src="Scripts/jquery-1.10.2.min.js" ></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link rel="stylesheet" href="Content/PageCSS/LoginPage.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="loginForm">
            <div class="input-group col-xs-10">
                <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                <input id="username" type="text" class="form-control" name="username" placeholder="Username"/>
            </div>
            <div class="input-group col-xs-10">
                <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                <input id="password" type="password" class="form-control" name="password" placeholder="Password"/>
            </div>
        </div>
        <div id="buttonBlock">
            <button type="button" class="btn " runat="server" onserverclick="Login">Login</button>
        </div>
    </form>
</body>
</html>

