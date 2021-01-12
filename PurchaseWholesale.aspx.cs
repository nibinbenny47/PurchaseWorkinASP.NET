using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Purchase_PurchaseWholesale : System.Web.UI.Page
{
    string connectionstring = "Data Source =.; Initial Catalog = db_purchase; Integrated Security = True";
    SqlConnection con;
    SqlCommand cmd;
    SqlCommand cmd1;
    public static int qnty, rate, total;
    public static int temp = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fillSupplier();
            AddDefaultFirstRecord();
            AddNewRecordToGrid();

            // ddlItem.Items.Insert(0, "--select item--");

        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {

        qnty = Convert.ToInt32(txtQuantity.Text);
        rate = Convert.ToInt32(txtRate.Text);
        total = qnty * rate;






        temp = temp + total;
        AddNewRecordToGrid();

    }
    public void fillSupplier()
    {
        con = new SqlConnection(connectionstring);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        cmd = new SqlCommand("spSelSupplier", con);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter adp = new SqlDataAdapter();
        adp.SelectCommand = cmd;
        DataTable dt = new DataTable();
        adp.Fill(dt);
        ddlSupplier.DataSource = dt;
        ddlSupplier.DataTextField = "supplier_name";
        ddlSupplier.DataValueField = "supplier_id";
        ddlSupplier.DataBind();
        ddlSupplier.Items.Insert(0, "--select supplier--");


    }
    public void fillItem()
    {
        //string id = ddlSupplier.SelectedValue;
        //con = new SqlConnection(connectionstring);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        //cmd = new SqlCommand("spFillItem", con);
        //cmd.CommandType = CommandType.StoredProcedure;
        ////cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = ddlSupplier.SelectedValue;
        //cmd.Parameters.AddWithValue("@Id", id);
        ////cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(ddlSupplier.SelectedValue));
        //SqlDataAdapter adp = new SqlDataAdapter(cmd);
        //// adp.SelectCommand = cmd;
        //DataTable dt = new DataTable();
        //adp.Fill(dt);
        //ddlItem.DataSource = dt;
        //ddlItem.DataTextField = "item_name";
        //ddlItem.DataValueField = "item_id";
        //ddlItem.DataBind();

        con = new SqlConnection(connectionstring);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        string sel1 = "select * from tbl_item where supplier_id = '" + ddlSupplier.SelectedValue + "'";

        DataTable dt = new DataTable();
        SqlDataAdapter adp = new SqlDataAdapter(sel1, con);
        adp.Fill(dt);
        ddlItem.DataSource = dt;
        ddlItem.DataTextField = "item_name";
        ddlItem.DataValueField = "item_id";
        ddlItem.DataBind();

    }

    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillItem();
    }
    private void AddDefaultFirstRecord()
    {
        //creating datatable
        DataTable dt = new DataTable();
        DataRow dr;
        dt.TableName = "purchase";
        dt.Columns.Add(new DataColumn("Item", typeof(string)));
        dt.Columns.Add(new DataColumn("Quantity", typeof(string)));
        dt.Columns.Add(new DataColumn("Rate", typeof(string)));
        dt.Columns.Add(new DataColumn("Total", typeof(string)));
        dr = dt.NewRow();
        dt.Rows.Add(dr);
        //saving datatable into viewstate
        ViewState["purchase"] = dt;
        Session["purchase"] = dt;
        //bind gridview
        grdPurchase.DataSource = dt;
        grdPurchase.DataBind();
    }
    public void AddNewRecordToGrid()
    {

        //check viewstate is not null
        if (ViewState["purchase"] != null)
        {
            //get datatable from viewstate
            DataTable dtCurrentTable = (DataTable)ViewState["purchase"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {

                    //addeach row into datatable
                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["Item"] = ddlItem.SelectedValue;
                    drCurrentRow["Quantity"] = txtQuantity.Text;
                    drCurrentRow["Rate"] = txtRate.Text;
                    drCurrentRow["Total"] = total.ToString();
                    //Prints grandtotal in the textbox
                    txtGrandTotal.Text = temp.ToString();





                }
                //remove initial blank row
                if (dtCurrentTable.Rows[0][0].ToString() == "")
                {
                    dtCurrentTable.Rows[0].Delete();
                    dtCurrentTable.AcceptChanges();
                }
                //add created rows into datatable 
                dtCurrentTable.Rows.Add(drCurrentRow);
                //save datatable into viewstate  after creating each row
                ViewState["purchase"] = dtCurrentTable;
                Session["purchase"] = dtCurrentTable;
                //bind gridview with latest row
                grdPurchase.DataSource = dtCurrentTable;
                grdPurchase.DataBind();
            }
        }
    }




    //protected void grdPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if(e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        TextBox txt = (TextBox)e.Row.FindControl("txtGrandTotal");


    //    }
    //}

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        con = new SqlConnection(connectionstring);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        string insQry = "insert into tbl_purchaseHead (ph_date,ph_invoice,ph_grandtotal,supplier_id) values('" + txtDate.Text + "','" + txtInvoice.Text + "','" + txtGrandTotal.Text + "','" + ddlSupplier.SelectedValue + "')";
        cmd = new SqlCommand(insQry, con);
        cmd.ExecuteNonQuery();


        string selQry = "select max(ph_id) as headID from tbl_purchaseHead";
        SqlDataAdapter adp = new SqlDataAdapter(selQry, con);
        DataTable dt = new DataTable();
        adp.Fill(dt);
        Session["phid"] = dt.Rows[0]["headID"];

        DataTable dt1 = Session["purchase"] as DataTable;

        foreach (DataRow dr in dt1.Rows)
        {

            string insQry1 = "insert into tbl_purchaseDetails(ph_id,item_id,pd_quantity,pd_rate) values('" + Session["phid"] + "','" + ddlItem.SelectedValue + "','" + txtQuantity.Text + "','" + txtRate.Text + "')";
            cmd1 = new SqlCommand(insQry1, con);
            cmd1.ExecuteNonQuery();
        }
    }
}