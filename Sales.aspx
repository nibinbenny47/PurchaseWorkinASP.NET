<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sales.aspx.cs" Inherits="Purchase_Sales" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             Bill NO:
            <asp:TextBox ID="txtInvoice" runat="server"></asp:TextBox><br />
            
            
                
            
            Date:
            <asp:TextBox ID="txtDate" runat="server" TextMode="Date"></asp:TextBox><br />
            Item:
            <asp:DropDownList ID="ddlItem" runat="server" >

            </asp:DropDownList><br />
            Quantity:
            <asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox><br />
            Rate:
            <asp:TextBox ID="txtRate" runat="server"></asp:TextBox><br />
            <asp:Button  runat="server" ID="btnAdd" Text="Add"  OnClick="btnAdd_Click"/>
            <asp:GridView runat="server" ID="grdPurchase" ShowFooter="true" >
            
            </asp:GridView><br />
            GrandTotal:<asp:TextBox ID="txtGrandTotal" runat="server"></asp:TextBox><br />
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
            <asp:Label runat="server" ID="lblQuantity" Visible="false"></asp:Label>
            <asp:Label runat="server" ID="lblsaleQnty" Visible="false"></asp:Label>
            
            

        
        </div>
       
    </form>
</body>
</html>
