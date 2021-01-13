using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Purchase_Sales : System.Web.UI.Page
{
    string connectionstring = "Data Source =.; Initial Catalog = db_purchase; Integrated Security = True";
    SqlConnection con;
    SqlCommand cmd;
    SqlCommand cmd1,cmd2;
    public static int qnty, rate, total;
    public static int temp = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fillItem();
            AddDefaultFirstRecord();
        }
    }
    public void fillItem()
    {
        con = new SqlConnection(connectionstring);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        cmd = new SqlCommand("spFillItem", con);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter adp = new SqlDataAdapter();
        adp.SelectCommand = cmd;
        DataTable dt = new DataTable();
        adp.Fill(dt);
        ddlItem.DataSource = dt;
        ddlItem.DataTextField = "item_name";
        ddlItem.DataValueField = "item_id";
        ddlItem.DataBind();
        ddlItem.Items.Insert(0, "--select Item--");


    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {

        qnty = Convert.ToInt32(txtQuantity.Text);
        rate = Convert.ToInt32(txtRate.Text);
        total = qnty * rate;






        temp = temp + total;
        AddNewRecordToGrid();

    }
    private void AddDefaultFirstRecord()
    {
        //creating datatable
        DataTable dt = new DataTable();
        DataRow dr;
        dt.TableName = "sales";
        dt.Columns.Add(new DataColumn("Item", typeof(string)));
        dt.Columns.Add(new DataColumn("Quantity", typeof(string)));
        dt.Columns.Add(new DataColumn("Rate", typeof(string)));
        dt.Columns.Add(new DataColumn("Total", typeof(string)));
        dr = dt.NewRow();
        dt.Rows.Add(dr);
        //saving datatable into viewstate
        ViewState["sales"] = dt;
        Session["sales"] = dt;
        //bind gridview
        grdPurchase.DataSource = dt;
        grdPurchase.DataBind();
    }
    public void AddNewRecordToGrid()
    {

        //check viewstate is not null
        if (ViewState["sales"] != null)
        {
            //get datatable from viewstate
            DataTable dtCurrentTable = (DataTable)ViewState["sales"];
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
                ViewState["sales"] = dtCurrentTable;
                Session["sales"] = dtCurrentTable;
                //bind gridview with latest row
                grdPurchase.DataSource = dtCurrentTable;
                grdPurchase.DataBind();
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        con = new SqlConnection(connectionstring);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        string insQry = "insert into tbl_Sales (sales_date,sales_bill,sales_grandtotal) values('" + txtDate.Text + "','" + txtInvoice.Text + "','" + txtGrandTotal.Text + "')";
        cmd = new SqlCommand(insQry, con);
        cmd.ExecuteNonQuery();


        string selQry = "select max(sales_id) as headID from tbl_Sales";
        SqlDataAdapter adp = new SqlDataAdapter(selQry, con);
        DataTable dt = new DataTable();
        adp.Fill(dt);
        Session["sid"] = dt.Rows[0]["headID"];

        DataTable dt1 = Session["sales"] as DataTable;

        foreach (DataRow dr in dt1.Rows)
        {
            //checking if the item is available in the stock and if so checking if the availble quantity greater than the user requested quantity
            string selQrystock = "select * from tbl_stock where item_id= '" + Convert.ToInt32(dr["Item"]) + "'";
            SqlDataAdapter adp1 = new SqlDataAdapter(selQrystock, con);
            DataTable dt2 = new DataTable();
            adp1.Fill(dt2);
            lblsaleQnty.Text = Convert.ToInt32(dr["Quantity"]).ToString();
            int salesqnty = Convert.ToInt32(lblsaleQnty.Text);


            if (dt2.Rows.Count > 0)
            {
                lblQuantity.Text = dt2.Rows[0]["stock_quantity"].ToString();
                int qnty = Convert.ToInt32(lblQuantity.Text);

                if(salesqnty <= qnty) 
                {
                    string insQry1 = "insert into tbl_salesDetails(sales_id,item_id,sd_quantity,sd_rate) values('" + Session["sid"] + "','" + Convert.ToInt32(dr["Item"]) + "','" + Convert.ToInt32(dr["Quantity"]) + "','" + Convert.ToInt32(dr["Rate"]) + "')";
                    cmd1 = new SqlCommand(insQry1, con);
                    cmd1.ExecuteNonQuery();

                    int availableQnty = Convert.ToInt32(qnty - salesqnty);

                    string upQrystock = "update tbl_stock set stock_quantity= '"+availableQnty+"' where item_id= '"+ Convert.ToInt32(dr["Item"]) + "'";
                    cmd2 = new SqlCommand(upQrystock, con);
                    cmd2.ExecuteNonQuery();
                }
                else
                {
                    Response.Write("<script>alert('sorry out of stock!!')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Sorry !! no such item in the stock')</script>");
            }



            //string insQry1 = "insert into tbl_salesDetails(sales_id,item_id,sd_quantity,sd_rate) values('" + Session["sid"] + "','" + Convert.ToInt32(dr["Item"]) + "','" + Convert.ToInt32(dr["Quantity"]) + "','" + Convert.ToInt32(dr["Rate"]) + "')";
            //cmd1 = new SqlCommand(insQry1, con);
            //cmd1.ExecuteNonQuery();

            //string upQrystock = "update tbl_stock set stock_quantity = '" + Convert.ToInt32(dr["Quantity"]) + "'";
        }
    }
}