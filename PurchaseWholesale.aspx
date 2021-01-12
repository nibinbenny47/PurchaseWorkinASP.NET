<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseWholesale.aspx.cs" Inherits="Purchase_PurchaseWholesale" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
              <div>
            Invoice:
            <asp:TextBox ID="txtInvoice" runat="server"></asp:TextBox><br />
            Supplier:
            <asp:DropDownList runat="server" ID="ddlSupplier" AutoPostBack="true" OnSelectedIndexChanged="ddlSupplier_SelectedIndexChanged">
                
            </asp:DropDownList><br />
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
            
            

        </div>
        </div>
    </form>
</body>
</html>
