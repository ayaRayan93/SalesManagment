﻿using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraTab;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesSystem
{
    public partial class Bill_Confirm : Form
    {
        MySqlConnection dbconnection, dbconnectionr, dbconnection2, dbconnection3;
        MySqlConnection connectionReader, connectionReader1, connectionReader2;
        string RecivedType = "شحن";
        string Customer_Type = "";
        int CustomerBill_ID = 0;
        bool loaded = false;
        
        int EmpBranchId = 0;
        string EmpBranchName = "";
        string type = "كاش";

        public Bill_Confirm()
        {
            InitializeComponent();
            dbconnection = new MySqlConnection(connection.connectionString);
            dbconnectionr = new MySqlConnection(connection.connectionString);
            dbconnection2 = new MySqlConnection(connection.connectionString);
            dbconnection3 = new MySqlConnection(connection.connectionString);
            connectionReader = new MySqlConnection(connection.connectionString);
            connectionReader1 = new MySqlConnection(connection.connectionString);
            connectionReader2 = new MySqlConnection(connection.connectionString);
        }
        //events
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                this.comClient.AutoCompleteMode = AutoCompleteMode.Suggest;
                this.comClient.AutoCompleteSource = AutoCompleteSource.ListItems;
                this.comEngCon.AutoCompleteMode = AutoCompleteMode.Suggest;
                this.comEngCon.AutoCompleteSource = AutoCompleteSource.ListItems;
                this.comBranch.AutoCompleteMode = AutoCompleteMode.Suggest;
                this.comBranch.AutoCompleteSource = AutoCompleteSource.ListItems;
                this.comDelegate.AutoCompleteMode = AutoCompleteMode.Suggest;
                this.comDelegate.AutoCompleteSource = AutoCompleteSource.ListItems;

                EmpBranchId = UserControl.UserBranch(dbconnection);

                dbconnection.Open();
                string query = "select * from branch";
                MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                comBranch.DataSource = dt;
                comBranch.DisplayMember = dt.Columns["Branch_Name"].ToString();
                comBranch.ValueMember = dt.Columns["Branch_ID"].ToString();
                comBranch.Text = "";

                string qb = "select Branch_Name from branch where Branch_ID=" + EmpBranchId;
                MySqlCommand cqb = new MySqlCommand(qb, dbconnection);
                EmpBranchName = cqb.ExecuteScalar().ToString();

                loaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }

        //check type of customer if engineer,client or contract 
        private void radiotype_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radio = (RadioButton)sender;
            Customer_Type = radio.Text;

            loaded = false; //this is flag to prevent action of SelectedValueChanged event until datasource fill combobox
            try
            {
                if (Customer_Type == "عميل")
                {
                    labelEng.Visible = false;
                    comEngCon.Visible = false;
                    labelClient.Visible = true;
                    comClient.Visible = true;

                    string query = "select * from customer where Customer_Type='" + Customer_Type + "'";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection2);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comClient.DataSource = dt;
                    comClient.DisplayMember = dt.Columns["Customer_Name"].ToString();
                    comClient.ValueMember = dt.Columns["Customer_ID"].ToString();
                    comClient.Text = "";
                    comEngCon.Text = "";
                }
                else
                {
                    labelEng.Visible = true;
                    comEngCon.Visible = true;
                    labelClient.Visible = false;
                    comClient.Visible = false;

                    string query = "select * from customer where Customer_Type='" + Customer_Type + "'";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection2);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comEngCon.DataSource = dt;
                    comEngCon.DisplayMember = dt.Columns["Customer_Name"].ToString();
                    comEngCon.ValueMember = dt.Columns["Customer_ID"].ToString();
                    comClient.Text = "";
                    comEngCon.Text = "";
                }

                loaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection2.Close();
        }

        //when select customer(مهندس,مقاول)display in comCustomer the all clients of th customer 
        private void comEngCon_SelectedValueChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                try
                {
                    labelClient.Visible = true;
                    comClient.Visible = true;
                
                    string query = "select * from customer where Customer_ID in(select Client_ID from custmer_client where Customer_ID=" + comEngCon.SelectedValue + ")";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comClient.DataSource = dt;
                    comClient.DisplayMember = dt.Columns["Customer_Name"].ToString();
                    comClient.ValueMember = dt.Columns["Customer_ID"].ToString();
                    comClient.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        //recived type checked
        private void radRecivedType_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton radio = (RadioButton)sender;
                if (radio.Text == "العميل")
                {
                    RecivedType = "العميل";
                    //comStore.Visible = false;
                }
                else if (radio.Text == "شحن")
                {
                    RecivedType = "شحن";
                    /*string query = "select * from store ";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    //comStore.DataSource = dt;
                    //comStore.DisplayMember = dt.Columns["Store_Name"].ToString();
                    //comStore.ValueMember = dt.Columns["Store_ID"].ToString();
                    //comStore.Visible = true;
                    flag2 = true;
                    //comStore.Text = "";*/
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtBillNo_KeyDown(object sender, KeyEventArgs e)
        {
            MySqlDataReader dataReader = null;
            MySqlDataReader dataReader1 = null;
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    dbconnection.Open();
                    dbconnectionr.Open();

                    if (checkBoxAdd.Checked == false)
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter("select distinct data.Data_ID as 'التسلسل', data.Code as 'الكود', data.Code as 'النوع',product.Product_Name as 'الاسم',color.Color_Name as 'اللون',size.Size_Value as 'المقاس',sort.Sort_Value as 'الفرز',dash_details.Quantity as 'الكمية',sellprice.Price as 'السعر',(sellprice.Price * dash_details.Quantity) as 'الاجمالى',(sellprice.Sell_Discount * dash_details.Quantity) as 'الخصم',(sellprice.Sell_Discount * dash_details.Quantity) as 'النسبة',data.Carton as 'الكرتنة',dash_details.Store_ID,dash_details.Store_Name as 'المخزن',dash.Dash_ID,'added' from dash INNER JOIN dash_details ON dash_details.Dash_ID = dash.Dash_ID inner join data on data.Data_ID=dash_details.Data_ID inner join product on product.Product_ID=data.Product_ID LEFT JOIN color ON color.Color_ID = data.Color_ID LEFT JOIN size ON size.Size_ID = data.Size_ID LEFT JOIN sort ON sort.Sort_ID = data.Sort_ID INNER JOIN sellprice ON sellprice.Data_ID = dash_details.Data_ID INNER JOIN store ON dash_details.Store_ID = store.Store_ID where  dash.Bill_Number=0 and dash.Branch_ID=0", dbconnection);
                        DataSet sourceDataSet = new DataSet();
                        adapter.Fill(sourceDataSet);
                        gridControl1.DataSource = sourceDataSet.Tables[0];
                    }
                    gridView1.Columns[0].Visible = false;
                    gridView1.Columns["الخصم"].Visible = false;
                    gridView1.Columns["النسبة"].Visible = false;
                    gridView1.Columns["Store_ID"].Visible = false;
                    gridView1.Columns["الكرتنة"].Visible = false;
                    gridView1.Columns["Dash_ID"].Visible = false;
                    gridView1.Columns["added"].Visible = false;

                    string query = "SELECT dash.Customer_Name,customer.Customer_Type,delegate.Delegate_Name,dash_details.Data_ID,dash_details.Quantity,dash_details.Type,dash.Dash_ID FROM dash_details INNER JOIN dash ON dash_details.Dash_ID = dash.Dash_ID INNER JOIN customer ON customer.Customer_ID = dash.Customer_ID INNER JOIN delegate ON delegate.Delegate_ID = dash.Delegate_ID where dash.Bill_Number = " + txtBillNo.Text + " and dash.Branch_ID = " + comBranch.SelectedValue.ToString() + "";
                    MySqlCommand com = new MySqlCommand(query, dbconnection);
                    dataReader = com.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["Customer_Type"].ToString() == "عميل")
                            {
                                radClient.Checked = true;
                            }
                            if (dataReader["Customer_Type"].ToString() == "مهندس")
                            {
                                radEng.Checked = true;
                            }
                            if (dataReader["Customer_Type"].ToString() == "مقاول")
                            {
                                radCon.Checked = true;
                            }
                            if (dataReader["Customer_Type"].ToString() == "تاجر")
                            {
                                radDealer.Checked = true;
                            }
                            comClient.Text = dataReader["Customer_Name"].ToString();
                            comDelegate.Text = dataReader["Delegate_Name"].ToString();
                            
                            #region بند
                            if (dataReader["Type"].ToString().Trim() == "بند")
                            {
                                string PriceType = "";
                                query = "SELECT Price_Type FROM sellprice where Data_ID=" + dataReader["Data_ID"].ToString();
                                com = new MySqlCommand(query, dbconnectionr);
                                if (com.ExecuteScalar() != null)
                                {
                                    PriceType = com.ExecuteScalar().ToString();
                                }

                                if (PriceType == "لستة")
                                {
                                    query = "select distinct data.Data_ID, data.Code as 'الكود',product.Product_Name as 'الاسم',color.Color_Name as 'اللون',size.Size_Value as 'المقاس',sort.Sort_Value as 'الفرز',dash_details.Quantity as 'الكمية',dash_details.Store_ID,dash_details.Store_Name as 'المخزن',sellprice.Price as 'السعر',(sellprice.Price * dash_details.Quantity) as 'الاجمالى',(sellprice.Sell_Discount) as 'الخصم',data.Carton as 'الكرتنة' from dash INNER JOIN dash_details ON dash_details.Dash_ID = dash.Dash_ID inner join data on data.Data_ID=dash_details.Data_ID inner join product on product.Product_ID=data.Product_ID LEFT JOIN color ON color.Color_ID = data.Color_ID LEFT JOIN size ON size.Size_ID = data.Size_ID LEFT JOIN sort ON sort.Sort_ID = data.Sort_ID INNER JOIN sellprice ON sellprice.Data_ID = dash_details.Data_ID INNER JOIN store ON dash_details.Store_ID = store.Store_ID where  dash.Bill_Number=" + txtBillNo.Text + " and dash.Branch_ID=" + EmpBranchId + "  and dash_details.Type='بند'";
                                    com = new MySqlCommand(query, dbconnectionr);
                                    dataReader1 = com.ExecuteReader();
                                    while (dataReader1.Read())
                                    {
                                        gridView1.AddNewRow();
                                        int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
                                        if (gridView1.IsNewItemRow(rowHandle))
                                        {
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["التسلسل"], dataReader1[0].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكود"], dataReader1["الكود"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["النوع"], "بند");
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الاسم"], dataReader1["الاسم"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["اللون"], dataReader1["اللون"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المقاس"], dataReader1["المقاس"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الفرز"], dataReader1["الفرز"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], dataReader1["الكمية"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], dataReader1["السعر"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الاجمالى"], dataReader1["الاجمالى"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["النسبة"], dataReader1["الخصم"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الخصم"], Convert.ToDouble(dataReader1["الاجمالى"].ToString()) * (Convert.ToDouble(dataReader1["الخصم"].ToString()) / 100));
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكرتنة"], dataReader1["الكرتنة"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Store_ID"], dataReader1["Store_ID"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المخزن"], dataReader1["المخزن"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Dash_ID"], dataReader["Dash_ID"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["added"], 1);
                                        }
                                    }
                                    dataReader1.Close();
                                }
                                else if (PriceType == "قطعى")
                                {
                                    query = "select distinct data.Data_ID, data.Code as 'الكود',product.Product_Name as 'الاسم',color.Color_Name as 'اللون',size.Size_Value as 'المقاس',sort.Sort_Value as 'الفرز',dash_details.Quantity as 'الكمية',dash_details.Store_ID,dash_details.Store_Name as 'المخزن',sellprice.Sell_Price as 'السعر',(sellprice.Sell_Price * dash_details.Quantity) as 'الاجمالى',data.Carton as 'الكرتنة' from dash INNER JOIN dash_details ON dash_details.Dash_ID = dash.Dash_ID inner join data on data.Data_ID=dash_details.Data_ID inner join product on product.Product_ID=data.Product_ID LEFT JOIN color ON color.Color_ID = data.Color_ID LEFT JOIN size ON size.Size_ID = data.Size_ID LEFT JOIN sort ON sort.Sort_ID = data.Sort_ID INNER JOIN sellprice ON sellprice.Data_ID = dash_details.Data_ID INNER JOIN store ON dash_details.Store_ID = store.Store_ID where  dash.Bill_Number=" + txtBillNo.Text + " and dash.Branch_ID=" + EmpBranchId + "";
                                    com = new MySqlCommand(query, dbconnectionr);
                                    dataReader1 = com.ExecuteReader();
                                    while (dataReader1.Read())
                                    {
                                        gridView1.AddNewRow();
                                        int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
                                        if (gridView1.IsNewItemRow(rowHandle))
                                        {
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["التسلسل"], dataReader1[0].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكود"], dataReader1["الكود"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["النوع"], "بند");
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الاسم"], dataReader1["الاسم"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["اللون"], dataReader1["اللون"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المقاس"], dataReader1["المقاس"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الفرز"], dataReader1["الفرز"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], dataReader1["الكمية"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], dataReader1["السعر"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الاجمالى"], dataReader1["الاجمالى"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["النسبة"], 0);
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الخصم"], 0);
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكرتنة"], dataReader1["الكرتنة"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Store_ID"], dataReader1["Store_ID"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المخزن"], dataReader1["المخزن"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Dash_ID"], dataReader["Dash_ID"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["added"], 1);
                                        }
                                    }
                                    dataReader1.Close();
                                }
                            }
                            #endregion

                            #region طقم
                            else if (dataReader["Type"].ToString().Trim() == "طقم")
                            {
                                query = "select distinct sets.Set_ID,sets.Set_Name as 'الاسم',dash_details.Quantity as 'الكمية',dash_details.Store_ID,dash_details.Store_Name as 'المخزن' FROM dash_details INNER JOIN dash ON dash_details.Dash_ID = dash.Dash_ID INNER JOIN sets ON sets.Set_ID = dash_details.Data_ID  where sets.Set_ID=" + Convert.ToInt16(dataReader["Data_ID"].ToString()) + " and dash.Bill_Number=" + txtBillNo.Text + " and dash.Branch_ID=" + EmpBranchId + "  and dash_details.Type='طقم'";
                                com = new MySqlCommand(query, dbconnectionr);
                                dataReader1 = com.ExecuteReader();
                                while (dataReader1.Read())
                                {
                                    dbconnection3.Open();
                                    double price = 0;
                                    double priceF = 0;
                                    gridView1.AddNewRow();
                                    int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
                                    if (gridView1.IsNewItemRow(rowHandle))
                                    {
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["التسلسل"], dataReader1[0].ToString());
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["النوع"], "طقم");
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الاسم"], dataReader1["الاسم"].ToString());
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], dataReader1["الكمية"].ToString());
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Store_ID"], dataReader1["Store_ID"].ToString());
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المخزن"], dataReader1["المخزن"].ToString());
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Dash_ID"], dataReader["Dash_ID"].ToString());
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["added"], 1);
                                        query = "SELECT  sum(sellprice.Price * set_details.Quantity) as 'السعر',sum(sellprice.Price * dash_details.Quantity * set_details.Quantity) as 'الاجمالى',(sellprice.Sell_Discount) as 'الخصم' FROM sets INNER JOIN set_details ON set_details.Set_ID = sets.Set_ID INNER JOIN dash_details ON sets.Set_ID = dash_details.Data_ID INNER JOIN sellprice ON set_details.Data_ID = sellprice.Data_ID where sellprice.Price_Type='لستة' and sets.Set_ID=" + dataReader1[0] + " and dash_details.Type='طقم' group by set_details.Set_ID";
                                        com = new MySqlCommand(query, dbconnection3);
                                        MySqlDataReader dr1 = com.ExecuteReader();
                                        while (dr1.Read())
                                        {
                                            price += Convert.ToDouble(dr1["السعر"].ToString());
                                            priceF += Convert.ToDouble(dr1["الاجمالى"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["النسبة"], dr1["الخصم"].ToString());
                                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الخصم"], Convert.ToDouble(dr1["الاجمالى"].ToString()) * (Convert.ToDouble(dr1["الخصم"].ToString()) / 100));
                                        }
                                        dr1.Close();

                                        query = "SELECT  sum(set_details.Quantity*sellprice.Sell_Price) as 'السعر',sum(dash_details.Quantity *set_details.Quantity*sellprice.Sell_Price) as 'الاجمالى' FROM sets INNER JOIN set_details ON set_details.Set_ID = sets.Set_ID INNER JOIN sellprice ON set_details.Data_ID = sellprice.Data_ID INNER JOIN dash_details ON sets.Set_ID = dash_details.Data_ID where sellprice.Price_Type='قطعى' and sets.Set_ID=" + dataReader1[0] + " and dash_details.Type='طقم' group by set_details.Set_ID";
                                        com = new MySqlCommand(query, dbconnection3);
                                        MySqlDataReader dr2 = com.ExecuteReader();
                                        while (dr2.Read())
                                        {
                                            price += Convert.ToDouble(dr2["السعر"].ToString());
                                            priceF += Convert.ToDouble(dr2["الاجمالى"].ToString());
                                        }
                                        dr2.Close();

                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], price);
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الاجمالى"], priceF);
                                    }
                                    dbconnection3.Close();
                                }
                                dataReader1.Close();
                            }
                            #endregion

                            #region عرض
                            else if (dataReader["Type"].ToString().Trim() == "عرض")
                            {
                                query = "select distinct offer.Offer_ID, offer.Offer_Name as 'الاسم',dash_details.Quantity as 'الكمية',dash_details.Store_ID,dash_details.Store_Name as 'المخزن',offer.Price as 'السعر',(offer.Price*dash_details.Quantity) as 'الاجمالى' FROM offer INNER JOIN dash_details ON dash_details.Data_ID = offer.Offer_ID INNER JOIN dash ON dash_details.Dash_ID = dash.Dash_ID INNER JOIN store ON dash_details.Store_ID = store.Store_ID where offer.Offer_ID=" + Convert.ToInt16(dataReader["Data_ID"].ToString()) + " and dash.Bill_Number=" + txtBillNo.Text + " and dash.Branch_ID=" + EmpBranchId + " and dash_details.Type='عرض'";
                                com = new MySqlCommand(query, dbconnectionr);
                                dataReader1 = com.ExecuteReader();
                                while (dataReader1.Read())
                                {
                                    gridView1.AddNewRow();
                                    int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
                                    if (gridView1.IsNewItemRow(rowHandle))
                                    {
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["التسلسل"], dataReader1[0].ToString());
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["النوع"], "عرض");
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الاسم"], dataReader1["الاسم"].ToString());
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], dataReader1["الكمية"].ToString());
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], dataReader1["السعر"].ToString());
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الاجمالى"], dataReader1["الاجمالى"].ToString());
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Store_ID"], dataReader1["Store_ID"].ToString());
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المخزن"], dataReader1["المخزن"].ToString());
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Dash_ID"], dataReader["Dash_ID"].ToString());
                                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["added"], 1);
                                    }
                                }
                                dataReader1.Close();
                            }
                            #endregion

                            else
                            {
                                MessageBox.Show("هذه الفاتورة غير موجوده");
                                dataReader.Close();
                                dbconnection.Close();
                                dbconnectionr.Close();
                                dbconnection2.Close();
                                return;
                            }
                        }
                        dataReader.Close();
                        
                        if (gridView1.IsLastVisibleRow)
                        {
                            gridView1.FocusedRowHandle = gridView1.RowCount - 1;
                        }
                    }
                    else
                    {
                        comDelegate.Text = "";
                        comClient.Text = "";
                        comEngCon.Text = "";
                    }

                    double sum = 0;
                    for (int i = 0; i < gridView1.RowCount; i++)
                    {
                        sum += Convert.ToDouble(gridView1.GetRowCellDisplayText(i, "الاجمالى").ToString());
                    }
                    labTotalBillPriceBD.Text = "اجمالى الفاتورة = " + sum.ToString();

                    double discount = 0;
                    for (int i = 0; i < gridView1.RowCount; i++)
                    {
                        if (gridView1.GetRowCellDisplayText(i, "الخصم").ToString() != "")
                        {
                            discount += Convert.ToDouble(gridView1.GetRowCellDisplayText(i, "الخصم").ToString());
                        }
                    }
                    labTotalDiscount.Text = "اجمالى الخصم = " + discount.ToString();

                    labTotalBillPriceAD.Text = "صافى الفاتورة = " + (sum - discount).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                dbconnection.Close();
                dbconnectionr.Close();
                dbconnection2.Close();
            }
        }

        private void rdbSoon_CheckedChanged(object sender, EventArgs e)
        {
            type = "آجل";
        }

        private void rdbCash_CheckedChanged(object sender, EventArgs e)
        {
            type = "كاش";
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("هل انت متاكد انك تريد الحذف؟", "تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;
                GridView view = gridView1 as GridView;
                view.DeleteRow(view.FocusedRowHandle);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if ((comClient.Text != "" || comEngCon.Text != "") && gridView1.RowCount > 0)
                {
                    List<int> pakagesIDs = new List<int>();
                    List<string> pakagesTypes = new List<string>();
                    for (int j = 0; j < gridView1.RowCount; j++)
                    {
                        DataRowView row1 = (DataRowView)gridView1.GetRow(j);
                        for (int i = 0; i < pakagesIDs.Count; i++)
                        {
                            if (row1["التسلسل"].ToString() != "" && row1["النوع"].ToString() != "")
                            {
                                if (pakagesIDs[i] == Convert.ToInt16(row1["التسلسل"].ToString()) && pakagesTypes[i] == row1["النوع"].ToString())
                                {
                                    gridView1.SetRowCellValue(j, "added", 0);
                                    gridView1.UnselectRow(j);
                                    MessageBox.Show("غير مسموح بتكرار عناصر");
                                    return;
                                }
                            }
                        }

                        pakagesIDs.Add(Convert.ToInt16(row1["التسلسل"].ToString()));
                        pakagesTypes.Add(row1["النوع"].ToString());
                    }

                    dbconnection.Open();
                    if (checkQuantityInStore())
                    {
                        string query = "select Branch_BillNumber from customer_bill where Branch_ID=" + EmpBranchId + " order by CustomerBill_ID  Desc limit 1";
                        MySqlCommand com = new MySqlCommand(query, dbconnection);
                        int Branch_BillNumber = Convert.ToInt16(com.ExecuteScalar().ToString()) + 1;

                        query = "insert into customer_bill (RecivedType,Branch_BillNumber,Client_ID,Customer_ID,Total_CostBD,Total_CostAD,Total_Discount,Bill_Date,Type_Buy,Branch_ID,Branch_Name) values (@RecivedType,@Branch_BillNumber,@Client_ID,@Customer_ID,@Total_CostBD,@Total_CostAD,@Total_Discount,@Bill_Date,@Type_Buy,@Branch_ID,@Branch_Name)";
                        com = new MySqlCommand(query, dbconnection);

                        if (comClient.Text != "")
                        {
                            com.Parameters.Add("@Client_ID", MySqlDbType.Int16);
                            com.Parameters["@Client_ID"].Value = Convert.ToInt16(comClient.SelectedValue.ToString());
                        }
                        else
                        {
                            com.Parameters.Add("@Client_ID", MySqlDbType.Int16);
                            com.Parameters["@Client_ID"].Value = null;
                        }


                        if (comEngCon.Text != "")
                        {
                            com.Parameters.Add("@Customer_ID", MySqlDbType.Int16);
                            com.Parameters["@Customer_ID"].Value = Convert.ToInt16(comEngCon.SelectedValue.ToString());
                        }
                        else
                        {
                            com.Parameters.Add("@Customer_ID", MySqlDbType.Int16);
                            com.Parameters["@Customer_ID"].Value = null;
                        }

                        if (type == "كاش")
                        {
                            com.Parameters.Add("@Type_Buy", MySqlDbType.VarChar);
                            com.Parameters["@Type_Buy"].Value = "كاش";
                        }
                        else if (type == "آجل")
                        {
                            com.Parameters.Add("@Type_Buy", MySqlDbType.VarChar);
                            com.Parameters["@Type_Buy"].Value = "آجل";
                        }
                        com.Parameters.Add("@Total_CostBD", MySqlDbType.Double);
                        com.Parameters["@Total_CostBD"].Value = labTotalBillPriceBD.Text.Split('=')[1].Trim();
                        com.Parameters.Add("@Total_CostAD", MySqlDbType.Double);
                        com.Parameters["@Total_CostAD"].Value = labTotalBillPriceAD.Text.Split('=')[1].Trim();
                        com.Parameters.Add("@Total_Discount", MySqlDbType.Double);
                        com.Parameters["@Total_Discount"].Value = labTotalDiscount.Text.Split('=')[1].Trim();
                        com.Parameters.Add("@Bill_Date", MySqlDbType.DateTime);
                        com.Parameters["@Bill_Date"].Value = DateTime.Now; //.ToString("yyyy-MM-dd")
                        com.Parameters.Add("@Branch_ID", MySqlDbType.Int16);
                        com.Parameters["@Branch_ID"].Value = EmpBranchId;
                        com.Parameters.Add("@Branch_Name", MySqlDbType.VarChar);
                        com.Parameters["@Branch_Name"].Value = EmpBranchName;
                        com.Parameters.Add("@Branch_BillNumber", MySqlDbType.Int16);
                        com.Parameters["@Branch_BillNumber"].Value = Branch_BillNumber;
                        com.Parameters.Add("@RecivedType", MySqlDbType.VarChar);
                        com.Parameters["@RecivedType"].Value = RecivedType;
                        com.ExecuteNonQuery();

                        query = "select CustomerBill_ID from customer_bill order by CustomerBill_ID desc limit 1";
                        com = new MySqlCommand(query, dbconnection);

                        if (com.ExecuteScalar() != null)
                        {
                            CustomerBill_ID = (int)com.ExecuteScalar();

                            for (int j = 0; j < gridView1.RowCount; j++)
                            {
                                DataRowView row1 = (DataRowView)gridView1.GetRow(j);

                                query = "insert into product_bill (CustomerBill_ID,Data_ID,Type,Price,Discount,Quantity,Store_ID,Store_Name) values (@CustomerBill_ID,@Data_ID,@Type,@Price,@Discount,@Quantity,@Store_ID,@Store_Name)";
                                com = new MySqlCommand(query, dbconnection);
                                
                                com.Parameters.Add("@CustomerBill_ID", MySqlDbType.Int16);
                                com.Parameters["@CustomerBill_ID"].Value = CustomerBill_ID;
                                com.Parameters.Add("@Data_ID", MySqlDbType.Int16);
                                com.Parameters["@Data_ID"].Value = Convert.ToInt16(row1["التسلسل"].ToString());
                                com.Parameters.Add("@Type", MySqlDbType.VarChar);
                                com.Parameters["@Type"].Value = row1["النوع"].ToString();
                                com.Parameters.Add("@Price", MySqlDbType.Decimal);
                                com.Parameters["@Price"].Value = Convert.ToDouble(row1["السعر"].ToString());
                                if (row1["النسبة"].ToString() != "")
                                {
                                    com.Parameters.Add("@Discount", MySqlDbType.Decimal);
                                    com.Parameters["@Discount"].Value = row1["النسبة"].ToString();
                                }
                                else
                                {
                                    com.Parameters.Add("@Discount", MySqlDbType.Decimal);
                                    com.Parameters["@Discount"].Value = null;
                                }
                                com.Parameters.Add("@Quantity", MySqlDbType.Decimal);
                                com.Parameters["@Quantity"].Value = Convert.ToDouble(row1["الكمية"].ToString());
                                com.Parameters.Add("@Store_ID", MySqlDbType.Int16);
                                com.Parameters["@Store_ID"].Value = row1["Store_ID"].ToString();
                                com.Parameters.Add("@Store_Name", MySqlDbType.VarChar);
                                com.Parameters["@Store_Name"].Value = row1["المخزن"].ToString();
                                com.ExecuteNonQuery();
                                
                            }
                        }

                        DecreaseProductQuantity();

                        UserControl.ItemRecord("customer_bill", "اضافة", CustomerBill_ID, DateTime.Now, "", dbconnection);
                        
                        //printBill();
                        MessageBox.Show("تم");
                    }
                    else
                    {
                        MessageBox.Show("تاكد من تواجد كميات كافية من العناصر");
                    }
                }
                else
                {
                    MessageBox.Show("يجب ادخال البيانات كاملة");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
            connectionReader2.Close();
            connectionReader1.Close();
            connectionReader.Close();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                Products_Report ProductsReport = new Products_Report();
                ProductsReport.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                if (loaded)
                {
                    GridView view = sender as GridView;
                    //if item is repeated in gridview
                    if (view.GetRowCellDisplayText(e.RowHandle, "added") == "0")
                    {
                        e.Appearance.BackColor = Color.Salmon;
                    }

                }
            }
            catch { }
        }
        
        /*private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                DataRowView row1 = (DataRowView)gridView1.GetRow(gridView1.GetRowHandle(e.RowHandle));
                if (loaded)
                {
                    if (e.Column.ToString() == "الكمية")
                    {
                        QuantityUpdate sd = new QuantityUpdate(row1);
                        sd.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }*/

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            if (loaded)
            {
                DXMouseEventArgs ea = e as DXMouseEventArgs;
                GridView view = sender as GridView;
                GridHitInfo info = view.CalcHitInfo(ea.Location);
                if (info.InRow || info.InRowCell)
                {
                    /*string colCaption = info.Column == null ? "N/A" : info.Column.GetCaption();
                    MessageBox.Show(string.Format("DoubleClick on row: {0}, column: {1}.", info.RowHandle, colCaption));*/

                    DataRowView row1 = (DataRowView)gridView1.GetRow(gridView1.GetRowHandle(info.RowHandle));
                    if (info.Column.GetCaption() == "الكمية" || info.Column.GetCaption() == "المخزن")
                    {
                        QuantityUpdate sd = new QuantityUpdate(info.RowHandle, row1);
                        sd.ShowDialog();
                    }
                }
            }
        }

        private void comBranch_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (loaded)
                {
                    string query = "select * from delegate where Branch_ID=" + comBranch.SelectedValue.ToString();
                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comDelegate.DataSource = dt;
                    comDelegate.DisplayMember = dt.Columns["Delegate_Name"].ToString();
                    comDelegate.ValueMember = dt.Columns["Delegate_ID"].ToString();
                    comDelegate.Text = "";

                    txtBillNo.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //functions
        public XtraTabPage getTabPage(string text)
        {
            for (int i = 0; i < Main.tabControlPointSale.TabPages.Count; i++)
                if (Main.tabControlPointSale.TabPages[i].Name == text)
                {
                    return Main.tabControlPointSale.TabPages[i];
                }
            return null;
        }

        //check if there are enough quantity from this item in store
        public bool checkQuantityInStore()
        {
            for (int j = 0; j < gridView1.RowCount; j++)
            {
                DataRowView row1 = (DataRowView)gridView1.GetRow(j);
                double totalMeter = Convert.ToDouble(row1["الكمية"].ToString());

                if (row1["النوع"].ToString() == "بند" && row1["added"].ToString() != "0")
                {
                    string query = "SELECT sum(storage.Total_Meters) as 'الكمية' FROM storage where storage.Store_ID=" + row1["Store_ID"].ToString() + " and storage.Data_ID=" + gridView1.GetRowCellDisplayText(j, "التسلسل") + " and storage.Type='بند' group by storage.Data_ID,storage.Store_ID";
                    MySqlCommand com = new MySqlCommand(query, dbconnection);
                    if (com.ExecuteScalar() != null)
                    {
                        double totalquant = Convert.ToDouble(com.ExecuteScalar().ToString());
                        if (totalMeter <= totalquant)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else if (row1["النوع"].ToString() == "طقم" && row1["added"].ToString() != "0")
                {
                    string query = "SELECT sum(storage.Total_Meters) as 'الكمية' FROM storage where storage.Store_ID=" + row1["Store_ID"].ToString() + " and storage.Set_ID=" + gridView1.GetRowCellDisplayText(j, "التسلسل") + " and storage.Type='طقم' group by storage.Set_ID,storage.Store_ID";
                    MySqlCommand com = new MySqlCommand(query, dbconnection);
                    if (com.ExecuteScalar() != null)
                    {
                        double totalquant = Convert.ToDouble(com.ExecuteScalar().ToString());
                        if (totalMeter <= totalquant)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else if (row1["النوع"].ToString() == "عرض" && row1["added"].ToString() != "0")
                {
                    string query = "SELECT sum(storage.Total_Meters) as 'الكمية' FROM storage where storage.Store_ID=" + row1["Store_ID"].ToString() + " and storage.Offer_ID=" + gridView1.GetRowCellDisplayText(j, "التسلسل") + " and storage.Type='عرض' group by storage.Offer_ID,storage.Store_ID";
                    MySqlCommand com = new MySqlCommand(query, dbconnection);
                    if (com.ExecuteScalar() != null)
                    {
                        double totalquant = Convert.ToDouble(com.ExecuteScalar().ToString());
                        if (totalMeter <= totalquant)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        public void DecreaseProductQuantity()
        {
            connectionReader.Open();
            connectionReader1.Open();
            connectionReader2.Open();
            string q;
            int id;
            double storageQ, productQ;
            
            #region not customer
            string query = "select Data_ID,Type,Quantity,Store_ID from product_bill where CustomerBill_ID=" + CustomerBill_ID;
            MySqlCommand com = new MySqlCommand(query, connectionReader);
            MySqlDataReader dr = com.ExecuteReader();

            while (dr.Read())
            {
                if (dr["Type"].ToString() == "بند")
                {
                    #region بند
                    double quantityInStore = 0;
                    query = "select sum(Total_Meters) from storage where Data_ID=" + dr["Data_ID"].ToString() + " and Store_ID=" + dr["Store_ID"].ToString() + " GROUP BY Store_ID,Data_ID";
                    com = new MySqlCommand(query, connectionReader2);
                    if (com.ExecuteScalar().ToString() != "")
                    {
                        quantityInStore = Convert.ToDouble(com.ExecuteScalar());
                    }
                    productQ = Convert.ToDouble(dr["Quantity"].ToString());
                    query = "select Storage_ID,Total_Meters from storage where Data_ID=" + dr["Data_ID"].ToString() + " and Store_ID=" + dr["Store_ID"].ToString() + "";
                    com = new MySqlCommand(query, connectionReader2);
                    MySqlDataReader dr2 = com.ExecuteReader();
                    while (dr2.Read())
                    {
                        if (productQ > 0)
                        {
                            storageQ = Convert.ToDouble(dr2["Total_Meters"].ToString());

                            if (storageQ >= productQ)
                            {
                                id = Convert.ToInt16(dr2["Storage_ID"].ToString());
                                q = "update storage set Total_Meters=" + (storageQ - productQ) + " where Storage_ID=" + id;
                                MySqlCommand comm = new MySqlCommand(q, dbconnection);
                                comm.ExecuteNonQuery();
                                productQ = 0;
                                //flag = true;
                                break;
                            }
                            else
                            {
                                id = Convert.ToInt16(dr2["Storage_ID"].ToString());
                                q = "update storage set Total_Meters=" + 0 + " where Storage_ID=" + id;
                                MySqlCommand comm = new MySqlCommand(q, dbconnection);
                                comm.ExecuteNonQuery();
                                productQ -= storageQ;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    dr2.Close();
                    #endregion
                }

                else if (dr["Type"].ToString() == "طقم")
                {
                    #region طقم
                    query = "select sum(Total_Meters) from storage where Set_ID=" + dr["Data_ID"].ToString() + " and Store_ID=" + dr["Store_ID"].ToString() + " GROUP BY Store_ID,Set_ID";
                    com = new MySqlCommand(query, connectionReader2);
                    double quantityInStore = 0;
                    if (com.ExecuteScalar().ToString() != "")
                    {
                        quantityInStore = Convert.ToDouble(com.ExecuteScalar());
                    }
                    productQ = Convert.ToDouble(dr["Quantity"].ToString());
                    query = "select Storage_ID,Total_Meters from storage where Set_ID=" + dr["Data_ID"].ToString() + " and Store_ID=" + dr["Store_ID"].ToString() + "";
                    com = new MySqlCommand(query, connectionReader2);
                    MySqlDataReader dr2 = com.ExecuteReader();
                    while (dr2.Read())
                    {
                        if (productQ > 0)
                        {
                            storageQ = Convert.ToDouble(dr2["Total_Meters"].ToString());

                            if (storageQ > productQ)
                            {
                                id = Convert.ToInt16(dr2["Storage_ID"].ToString());
                                q = "update storage set Total_Meters=" + (storageQ - productQ) + " where Storage_ID=" + id;
                                MySqlCommand comm = new MySqlCommand(q, dbconnection);
                                comm.ExecuteNonQuery();
                                productQ = 0;
                                break;
                            }
                            else
                            {
                                id = Convert.ToInt16(dr2["Storage_ID"].ToString());
                                q = "update storage set Total_Meters=" + 0 + " where Storage_ID=" + id;
                                MySqlCommand comm = new MySqlCommand(q, dbconnection);
                                comm.ExecuteNonQuery();
                                productQ -= storageQ;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    dr2.Close();
                    #endregion
                }

                else if (dr["Type"].ToString() == "عرض")
                {
                    #region عرض
                    query = "select sum(Total_Meters) from storage where Offer_ID=" + dr["Data_ID"].ToString() + " and Store_ID=" + dr["Store_ID"].ToString() + " GROUP BY Store_ID,Offer_ID";
                    com = new MySqlCommand(query, connectionReader2);
                    double quantityInStore = 0;
                    if (com.ExecuteScalar().ToString() != "")
                    {
                        quantityInStore = Convert.ToDouble(com.ExecuteScalar());
                    }
                    productQ = Convert.ToDouble(dr["Quantity"].ToString());
                    query = "select Storage_ID,Total_Meters from storage where Offer_ID=" + dr["Data_ID"].ToString() + " and Store_ID=" + dr["Store_ID"].ToString() + "";
                    com = new MySqlCommand(query, connectionReader2);
                    MySqlDataReader dr2 = com.ExecuteReader();
                    while (dr2.Read())
                    {
                        if (productQ > 0)
                        {
                            storageQ = Convert.ToDouble(dr2["Total_Meters"].ToString());

                            if (storageQ > productQ)
                            {
                                id = Convert.ToInt16(dr2["Storage_ID"].ToString());
                                q = "update storage set Total_Meters=" + (storageQ - productQ) + " where Storage_ID=" + id;
                                MySqlCommand comm = new MySqlCommand(q, dbconnection);
                                comm.ExecuteNonQuery();
                                productQ = 0;
                                break;
                            }
                            else
                            {
                                id = Convert.ToInt16(dr2["Storage_ID"].ToString());
                                q = "update storage set Total_Meters=" + 0 + " where Storage_ID=" + id;
                                MySqlCommand comm = new MySqlCommand(q, dbconnection);
                                comm.ExecuteNonQuery();
                                productQ -= storageQ;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    dr2.Close();
                    #endregion
                }
            }

            dr.Close();

            #endregion
            connectionReader2.Close();
            connectionReader1.Close();
            connectionReader.Close();
        }

        public void addItemToView(DataRowView comeRow1, double comeQuantity, int comeStoreId, string comeStore)
        {
            try
            {
                gridView1.AddNewRow();
                int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
                if (gridView1.IsNewItemRow(rowHandle))
                {
                    if (comeRow1["الكود"].ToString().Length >= 20)
                    {
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["التسلسل"], comeRow1[0].ToString());
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكود"], comeRow1["الكود"].ToString());
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["النوع"], "بند");
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الاسم"], comeRow1["الاسم"].ToString().Split(')')[1].Trim());
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["اللون"], comeRow1["اللون"].ToString());
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المقاس"], comeRow1["المقاس"].ToString());
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الفرز"], comeRow1["الفرز"].ToString());
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], comeQuantity);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], comeRow1["السعر"].ToString());
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الاجمالى"], Convert.ToDouble(comeRow1["السعر"].ToString()) * comeQuantity);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["النسبة"], comeRow1["الخصم"].ToString());
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الخصم"], (Convert.ToDouble(comeRow1["السعر"].ToString()) * comeQuantity) * (Convert.ToDouble(comeRow1["الخصم"].ToString()) / 100));
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكرتنة"], comeRow1["الكرتنة"].ToString());
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Store_ID"], comeStoreId);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المخزن"], comeStore);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Dash_ID"], gridView1.GetRowCellDisplayText((gridView1.RowCount - 2), "Dash_ID"));
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["added"], 1);
                    }
                    else if (comeRow1["الاسم"].ToString().Split(')')[0].Split('(')[1] == "طقم")
                    {
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["التسلسل"], comeRow1["الكود"].ToString());
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["النوع"], "طقم");
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الاسم"], comeRow1["الاسم"].ToString().Split(')')[1].Trim());
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], comeQuantity);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], comeRow1["السعر"].ToString());
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الاجمالى"], Convert.ToDouble(comeRow1["السعر"].ToString()) * comeQuantity);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["النسبة"], comeRow1["الخصم"].ToString());
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الخصم"], (Convert.ToDouble(comeRow1["السعر"].ToString()) * comeQuantity) * (Convert.ToDouble(comeRow1["الخصم"].ToString()) / 100));
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Store_ID"], comeStoreId);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المخزن"], comeStore);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Dash_ID"], gridView1.GetRowCellDisplayText((gridView1.RowCount - 2), "Dash_ID"));
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["added"], 1);
                    }
                    else if (comeRow1["الاسم"].ToString().Split(')')[0].Split('(')[1] == "عرض")
                    {
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["التسلسل"], comeRow1["الكود"].ToString());
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["النوع"], "عرض");
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الاسم"], comeRow1["الاسم"].ToString().Split(')')[1].Trim());
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], comeQuantity);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], comeRow1["السعر"].ToString());
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الاجمالى"], Convert.ToDouble(comeRow1["السعر"].ToString()) * comeQuantity);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Store_ID"], comeStoreId);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المخزن"], comeStore);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Dash_ID"], gridView1.GetRowCellDisplayText((gridView1.RowCount - 2), "Dash_ID"));
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["added"], 1);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void refreshView(int rowHandel, double quantity, int storeId, string storeName)
        {
            gridView1.SetRowCellValue(rowHandel, "الكمية", quantity);
            gridView1.SetRowCellValue(rowHandel, "Store_ID", storeId);
            gridView1.SetRowCellValue(rowHandel, "المخزن", storeName);
        }

        void printBill()
        {
            List<Bill_Items> bi = new List<Bill_Items>();
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                Bill_Items item = new Bill_Items() { Code = gridView1.GetRowCellDisplayText(i, gridView1.Columns["الكود"]), Product_Type = gridView1.GetRowCellDisplayText(i, gridView1.Columns["النوع"]), Product_Name = gridView1.GetRowCellDisplayText(i, gridView1.Columns["الاسم"]), Color = gridView1.GetRowCellDisplayText(i, gridView1.Columns["اللون"]), Size = gridView1.GetRowCellDisplayText(i, gridView1.Columns["المقاس"]), Sort = gridView1.GetRowCellDisplayText(i, gridView1.Columns["الفرز"]), Quantity = Convert.ToDouble(gridView1.GetRowCellDisplayText(i, gridView1.Columns["الكمية"])), Cost = Convert.ToDouble(gridView1.GetRowCellDisplayText(i, gridView1.Columns["السعر"])), Total_Cost = Convert.ToDouble(gridView1.GetRowCellDisplayText(i, gridView1.Columns["الاجمالى"])), Store_Name = gridView1.GetRowCellDisplayText(i, gridView1.Columns["المخزن"]) };
                bi.Add(item);
            }

            Print_Bill_Report f = new Print_Bill_Report();
            if (comClient.Text != "")
            {
                f.PrintInvoice(comClient.Text, type, CustomerBill_ID, Convert.ToDouble(labTotalBillPriceBD.Text.Split('=')[1].Trim()), Convert.ToDouble(labTotalBillPriceAD.Text.Split('=')[1].Trim()), Convert.ToDouble(labTotalDiscount.Text.Split('=')[1].Trim()), bi);
            }
            else if (comEngCon.Text != "")
            {
                f.PrintInvoice(comEngCon.Text, type, CustomerBill_ID, Convert.ToDouble(labTotalBillPriceBD.Text.Split('=')[1].Trim()), Convert.ToDouble(labTotalBillPriceAD.Text.Split('=')[1].Trim()), Convert.ToDouble(labTotalDiscount.Text.Split('=')[1].Trim()), bi);
            }
            f.ShowDialog();
        }
    }
}
