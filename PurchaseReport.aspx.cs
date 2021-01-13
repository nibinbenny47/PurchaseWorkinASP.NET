using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class Purchase_PurchaseReport : System.Web.UI.Page
{
    string connectionstring = "Data Source =.; Initial Catalog = db_purchase; Integrated Security = True";
    SqlConnection con;
    SqlCommand cmd;
    public static int id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //MultiView1.ActiveViewIndex = 0;
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the runtime error "  
        //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        con = new SqlConnection(connectionstring);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        string selQry = "select * from tbl_purchaseHead where ph_date between '"+txtFromDate.Text+"' and '"+txtToDate.Text+"'";
        SqlDataAdapter adp = new SqlDataAdapter(selQry, con);
        DataTable dt = new DataTable();
        adp.Fill(dt);
        grdPurchaseReport.DataSource = dt;
        grdPurchaseReport.DataBind();
     
    }

    protected void grdPurchaseReport_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        con = new SqlConnection(connectionstring);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        id = Convert.ToInt32(e.CommandArgument.ToString());
        Session["phid"] = id;
        if(e.CommandName == "invoiceNumber")
        {
            MultiView1.ActiveViewIndex = 1;
            fillGrid();
        }
    }
    protected void fillGrid()
    {
        string selQryphDetails = "select * from tbl_purchaseDetails p inner join tbl_item i on p.item_id=i.item_id where ph_id='"+id+"'";
        SqlDataAdapter adp2 = new SqlDataAdapter(selQryphDetails, con);
        DataTable dt1 = new DataTable();
        adp2.Fill(dt1);
        grdPurchaseDetails.DataSource = dt1;
        grdPurchaseDetails.DataBind();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "ExcelDemo" + DateTime.Now + ".xls";
        StringWriter strwriter = new StringWriter();
        HtmlTextWriter htmltextwriter = new HtmlTextWriter(strwriter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        grdPurchaseDetails.GridLines = GridLines.Both;
        grdPurchaseDetails.HeaderStyle.Font.Bold = true;
        grdPurchaseDetails.RenderControl(htmltextwriter);
        Response.Write(strwriter.ToString());
        Response.End();
    }
}