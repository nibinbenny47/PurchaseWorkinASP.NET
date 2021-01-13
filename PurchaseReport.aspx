<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseReport.aspx.cs" Inherits="Purchase_PurchaseReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
         <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                    <asp:View ID="View1" runat="server">
                     From Date: <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date"></asp:TextBox><br />
            To Date:<asp:TextBox ID="txtToDate" runat="server" TextMode="Date"></asp:TextBox><br />
            <asp:Button  ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click"/>
                   
            <asp:GridView ID="grdPurchaseReport" runat="server" AutoGenerateColumns="false" OnRowCommand="grdPurchaseReport_RowCommand">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                             <asp:LinkButton ID="lkbtnInvoice" runat="server" CommandName="invoiceNumber" CommandArgument='<%# Eval("ph_id") %>'><%# Eval("ph_invoice") %></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField HeaderText="Invoice NO" DataField="ph_invoice" />--%>
                    <asp:BoundField HeaderText="Date" DataField="ph_date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField HeaderText="Grand Total" DataField="ph_grandtotal" />
                    
                </Columns>
            </asp:GridView>
              </asp:View>
                       <asp:View ID="View2" runat="server">
                    <asp:GridView ID="grdPurchaseDetails" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField HeaderText="Item" DataField="item_name"/>
                            <asp:BoundField HeaderText="Quantity" DataField="pd_quantity"/>
                            <asp:BoundField HeaderText="Rate" DataField="pd_rate"/>
                            
                        </Columns>
                    </asp:GridView>
                           <asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click" />
                </asp:View>

                    
                </asp:MultiView>
          
        </div>
    </form>
</body>
</html>

