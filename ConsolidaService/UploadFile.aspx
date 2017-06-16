<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UploadFile.aspx.vb" Inherits="ConsolidaService.UploadFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" action="PhotoStore.aspx" enctype="multipart/form-data">
    <div>
    <input type="file" id="file" onchange="preview(this)" />
    <input type="submit" />
    </div>
</form>
</body>
</html>
