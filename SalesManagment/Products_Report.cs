﻿using DevExpress.Utils;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Preview;
using DevExpress.XtraTab;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagment
{
    public partial class Products_Report : Form
    {
        MySqlConnection dbconnection;
        MySqlConnection dbconnection2;
        MySqlConnection dbconnection3;
        MySqlConnection dbconnection4;
        MySqlConnection dbconnection5;
        XtraTabControl MainTabControlSales;
        bool loaded = false;
        XtraTabPage xtraTabPage;
        public static GridControl gridcontrol;
        
        List<string> arr;
        DataRow row1;
        public static TipImage tipImage = null;
        int EmpBranchId = 0;

        string code1 = "0000";
        string code2 = "0000";
        string code3 = "0000";
        string code4 = "0000";
        string code5 = "0000";
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;
        bool flag4 = false;
        bool flag5 = false;

        public Products_Report()
        {
            InitializeComponent();
            dbconnection = new MySqlConnection(connection.connectionString);
            dbconnection2 = new MySqlConnection(connection.connectionString);
            dbconnection3 = new MySqlConnection(connection.connectionString);
            dbconnection4 = new MySqlConnection(connection.connectionString);
            dbconnection5 = new MySqlConnection(connection.connectionString);
            MainTabControlSales = SalesMainForm.tabControlSales;

            xtraTabPage = new XtraTabPage();

            gridcontrol = gridControl1;
            arr = new List<string>();

            comType.AutoCompleteMode = AutoCompleteMode.Suggest;
            comType.AutoCompleteSource = AutoCompleteSource.ListItems;
            comFactory.AutoCompleteMode = AutoCompleteMode.Suggest;
            comFactory.AutoCompleteSource = AutoCompleteSource.ListItems;
            comGroup.AutoCompleteMode = AutoCompleteMode.Suggest;
            comGroup.AutoCompleteSource = AutoCompleteSource.ListItems;
            comProduct.AutoCompleteMode = AutoCompleteMode.Suggest;
            comProduct.AutoCompleteSource = AutoCompleteSource.ListItems;

            comSet.AutoCompleteMode = AutoCompleteMode.Suggest;
            comSet.AutoCompleteSource = AutoCompleteSource.ListItems;
            comOffer.AutoCompleteMode = AutoCompleteMode.Suggest;
            comOffer.AutoCompleteSource = AutoCompleteSource.ListItems;

            /*ScrollBar hScrollBar1 = new HScrollBar();
            hScrollBar1.Dock = DockStyle.Bottom;
            hScrollBar1.Scroll += (sender, e) => { panel2.HorizontalScroll.Value = hScrollBar1.Value; };
            panel2.Controls.Add(hScrollBar1);*/

            panel2.AutoScroll = false;
            panel2.VerticalScroll.Enabled = false;
            panel2.VerticalScroll.Visible = false;
            panel2.VerticalScroll.Maximum = 0;
            panel2.AutoScroll = true;
        }

        private void SearchProduct_Load(object sender, EventArgs e)
        {
            try
            {
                EmpBranchId = UserControl.UserBranch(dbconnection);
                search();
                foreach (GridColumn column in gridView1.Columns)
                    column.OptionsColumn.AllowSort = DefaultBoolean.False;
                loaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string q1, q2, q3, q4, qall = "";
                if (comType.Text == "")
                {
                    q1 = "select Type_ID from type";
                }
                else
                {
                    q1 = comType.SelectedValue.ToString();
                }
                if (comFactory.Text == "")
                {
                    q2 = "select Factory_ID from factory";
                }
                else
                {
                    q2 = comFactory.SelectedValue.ToString();
                }
                if (comGroup.Text == "")
                {
                    q3 = "select Group_ID from groupo";
                }
                else
                {
                    q3 = comGroup.SelectedValue.ToString();
                }
                if (comProduct.Text == "")
                {
                    q4 = "select Product_ID from product";
                }
                else
                {
                    q4 = comProduct.SelectedValue.ToString();
                }

                if (comSort.Text != "")
                {
                    qall += " and data.Sort_ID=" + comSort.SelectedValue.ToString();
                }
                if (comSize.Text != "")
                {
                    qall += " and data.Size_ID=" + comSize.SelectedValue.ToString();
                } 
                if (comColor.Text != "")
                {
                    qall += " and data.Color_ID=" + comColor.SelectedValue.ToString();
                }

                dbconnection.Open();
                //AND size.Factory_ID = factory.Factory_ID AND groupo.Factory_ID = factory.Factory_ID  AND product.Group_ID = groupo.Group_ID  AND factory.Type_ID = type.Type_ID AND color.Type_ID = type.Type_ID
                string query = "select data.Data_ID,data.Code as 'الكود',CONCAT('(بند) ',product.Product_Name) as 'الاسم',type.Type_Name 'النوع',factory.Factory_Name as 'المصنع',groupo.Group_Name as 'المجموعة',color.Color_Name as 'اللون',size.Size_Value as 'المقاس',sort.Sort_Value as 'الفرز',data.Classification as 'التصنيف',data.Carton as 'الكرتنة',data.Description as 'البيان',sellprice.Price as 'السعر',sellprice.Sell_Discount as 'الخصم',sum(storage.Total_Meters) as 'الكمية' FROM data LEFT JOIN color ON color.Color_ID = data.Color_ID LEFT JOIN size ON size.Size_ID = data.Size_ID LEFT JOIN sort ON sort.Sort_ID = data.Sort_ID INNER JOIN groupo ON data.Group_ID = groupo.Group_ID INNER JOIN factory ON factory.Factory_ID = data.Factory_ID  INNER JOIN product ON product.Product_ID = data.Product_ID  INNER JOIN type ON type.Type_ID = data.Type_ID  INNER JOIN sellprice ON sellprice.Data_ID = data.Data_ID LEFT JOIN storage ON storage.Data_ID = data.Data_ID where  data.Type_ID IN(" + q1 + ") and  data.Factory_ID  IN(" + q2 + ") and data.Group_ID IN (" + q4 + ") and data.Data_ID=0 group by data.Data_ID";
                MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gridControl1.DataSource = dt;
                gridView1.Columns[0].Visible = false;

                query = "SELECT data.Data_ID,data.Code as 'الكود',CONCAT('(بند) ',product.Product_Name) as 'الاسم',type.Type_Name 'النوع',factory.Factory_Name as 'المصنع',groupo.Group_Name as 'المجموعة',color.Color_Name as 'اللون',size.Size_Value as 'المقاس',sort.Sort_Value as 'الفرز',data.Classification as 'التصنيف',data.Carton as 'الكرتنة',data.Description as 'البيان',sellprice.Price as 'السعر',sellprice.Sell_Discount as 'الخصم',sellprice.Sell_Price as 'بعد الخصم',sum(storage.Total_Meters) as 'الكمية',sellprice.Price_Type FROM data LEFT JOIN color ON color.Color_ID = data.Color_ID LEFT JOIN size ON size.Size_ID = data.Size_ID LEFT JOIN sort ON sort.Sort_ID = data.Sort_ID INNER JOIN groupo ON data.Group_ID = groupo.Group_ID INNER JOIN factory ON factory.Factory_ID = data.Factory_ID  INNER JOIN product ON product.Product_ID = data.Product_ID  INNER JOIN type ON type.Type_ID = data.Type_ID  INNER JOIN sellprice ON sellprice.Data_ID = data.Data_ID LEFT JOIN storage ON storage.Data_ID = data.Data_ID where data.Type_ID IN(" + q1 + ") and data.Factory_ID IN(" + q2 + ") and data.Group_ID IN (" + q3 + ") and data.Product_ID IN (" + q4 + ") " + qall + " group by data.Data_ID";
                MySqlCommand comand = new MySqlCommand(query, dbconnection);
                MySqlDataReader dr = comand.ExecuteReader();
                while (dr.Read())
                {
                    gridView1.AddNewRow();
                    int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
                    if (gridView1.IsNewItemRow(rowHandle))
                    {
                        if (dr["Price_Type"].ToString() == "لستة")
                        {
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[0], dr["Data_ID"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[1], dr["الكود"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[2], dr["الاسم"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[3], dr["النوع"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[4], dr["المصنع"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[5], dr["المجموعة"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["اللون"], dr["اللون"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المقاس"], dr["المقاس"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الفرز"], dr["الفرز"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["التصنيف"], dr["التصنيف"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكرتنة"], dr["الكرتنة"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["البيان"], dr["البيان"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], dr["السعر"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الخصم"], dr["الخصم"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["بعد الخصم"], dr["بعد الخصم"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], dr["الكمية"]);

                        }
                        else if (dr["Price_Type"].ToString() == "قطعى")
                        {
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[0], dr["Data_ID"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[1], dr["الكود"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[2], dr["الاسم"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[3], dr["النوع"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[4], dr["المصنع"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[5], dr["المجموعة"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["اللون"], dr["اللون"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المقاس"], dr["المقاس"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الفرز"], dr["الفرز"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["التصنيف"], dr["التصنيف"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكرتنة"], dr["الكرتنة"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["البيان"], dr["البيان"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], dr["بعد الخصم"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الخصم"], "");
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["بعد الخصم"], dr["بعد الخصم"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], dr["الكمية"]);

                        }
                    }
                }
                dr.Close();
                if (gridView1.IsLastVisibleRow)
                {
                    gridView1.FocusedRowHandle = gridView1.RowCount - 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            try
            {
                //"SELECT data.Data_ID,data.Code as 'الكود',product.Product_Name as 'المنتج',type.Type_Name 'النوع',factory.Factory_Name as 'المصنع',groupo.Group_Name as 'المجموعة',color.Color_Name as 'اللون',size.Size_Value as 'المقاس',sort.Sort_Value as 'الفرز',data.Classification as 'التصنيف',data.Carton as 'الكرتنة',data.Description as 'البيان',price.Price as 'السعر',sum(storage.Total_Meters) as 'الكمية' FROM data INNER JOIN color ON color.Color_ID = data.Color_ID INNER JOIN size ON size.Size_ID = data.Size_ID INNER JOIN sort ON sort.Sort_ID = data.Sort_ID INNER JOIN groupo ON data.Group_ID = groupo.Group_ID INNER JOIN factory ON factory.Factory_ID = data.Factory_ID AND size.Factory_ID = factory.Factory_ID AND groupo.Factory_ID = factory.Factory_ID INNER JOIN product ON product.Product_ID = data.Product_ID AND product.Group_ID = groupo.Group_ID INNER JOIN type ON type.Type_ID = data.Type_ID AND factory.Type_ID = type.Type_ID AND color.Type_ID = type.Type_ID INNER JOIN price ON price.Code = data.Code LEFT JOIN storage ON storage.Code = data.Code group by storage.Code", dbconnection);

                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT data.Data_ID,data.Code as 'الكود',CONCAT('(بند) ',product.Product_Name) as 'الاسم',type.Type_Name 'النوع',factory.Factory_Name as 'المصنع',groupo.Group_Name as 'المجموعة',color.Color_Name as 'اللون',size.Size_Value as 'المقاس',sort.Sort_Value as 'الفرز',data.Classification as 'التصنيف',data.Carton as 'الكرتنة',data.Description as 'البيان',sellprice.Price as 'السعر',sellprice.Sell_Discount as 'الخصم',sellprice.Sell_Price as 'بعد الخصم',sum(storage.Total_Meters) as 'الكمية' FROM data LEFT JOIN color ON color.Color_ID = data.Color_ID LEFT JOIN size ON size.Size_ID = data.Size_ID LEFT JOIN sort ON sort.Sort_ID = data.Sort_ID INNER JOIN groupo ON data.Group_ID = groupo.Group_ID INNER JOIN factory ON factory.Factory_ID = data.Factory_ID  INNER JOIN product ON product.Product_ID = data.Product_ID  INNER JOIN type ON type.Type_ID = data.Type_ID  INNER JOIN sellprice ON sellprice.Data_ID = data.Data_ID LEFT JOIN storage ON storage.Data_ID = data.Data_ID where data.Data_ID=0 group by data.Data_ID", dbconnection);
                DataTable dtf = new DataTable();
                adapter.Fill(dtf);
                gridControl1.DataSource = dtf;
                gridView1.Columns[0].Visible = false;

                dbconnection.Open();
                #region بند
                string query = "SELECT data.Data_ID,data.Code as 'الكود',CONCAT('(بند) ',product.Product_Name) as 'الاسم',type.Type_Name 'النوع',factory.Factory_Name as 'المصنع',groupo.Group_Name as 'المجموعة',color.Color_Name as 'اللون',size.Size_Value as 'المقاس',sort.Sort_Value as 'الفرز',data.Classification as 'التصنيف',data.Carton as 'الكرتنة',data.Description as 'البيان',sellprice.Price as 'السعر',sellprice.Sell_Discount as 'الخصم',sellprice.Sell_Price as 'بعد الخصم',sum(storage.Total_Meters) as 'الكمية',sellprice.Price_Type FROM data LEFT JOIN color ON color.Color_ID = data.Color_ID LEFT JOIN size ON size.Size_ID = data.Size_ID LEFT JOIN sort ON sort.Sort_ID = data.Sort_ID INNER JOIN groupo ON data.Group_ID = groupo.Group_ID INNER JOIN factory ON factory.Factory_ID = data.Factory_ID  INNER JOIN product ON product.Product_ID = data.Product_ID  INNER JOIN type ON type.Type_ID = data.Type_ID  INNER JOIN sellprice ON sellprice.Data_ID = data.Data_ID LEFT JOIN storage ON storage.Data_ID = data.Data_ID group by data.Data_ID";
                MySqlCommand comand = new MySqlCommand(query, dbconnection);
                MySqlDataReader dr = comand.ExecuteReader();
                while (dr.Read())
                {
                    gridView1.AddNewRow();
                    int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
                    if (gridView1.IsNewItemRow(rowHandle))
                    {
                        if (dr["Price_Type"].ToString() == "لستة")
                        {
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[0], dr["Data_ID"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[1], dr["الكود"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[2], dr["الاسم"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[3], dr["النوع"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[4], dr["المصنع"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[5], dr["المجموعة"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["اللون"], dr["اللون"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المقاس"], dr["المقاس"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الفرز"], dr["الفرز"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["التصنيف"], dr["التصنيف"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكرتنة"], dr["الكرتنة"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["البيان"], dr["البيان"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], dr["السعر"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الخصم"], dr["الخصم"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["بعد الخصم"], dr["بعد الخصم"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], dr["الكمية"]);

                        }
                        else if (dr["Price_Type"].ToString() == "قطعى")
                        {
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[0], dr["Data_ID"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[1], dr["الكود"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[2], dr["الاسم"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[3], dr["النوع"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[4], dr["المصنع"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[5], dr["المجموعة"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["اللون"], dr["اللون"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المقاس"], dr["المقاس"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الفرز"], dr["الفرز"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["التصنيف"], dr["التصنيف"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكرتنة"], dr["الكرتنة"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["البيان"], dr["البيان"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], dr["بعد الخصم"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الخصم"], "");
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["بعد الخصم"], dr["بعد الخصم"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], dr["الكمية"]);

                        }
                    }
                }
                dr.Close();
                #endregion

                #region طقم
                query = "SELECT sets.Set_ID as 'الكود',CONCAT('(طقم) ',sets.Set_Name) as 'الاسم',sets.Description as 'البيان' FROM sets";
                comand = new MySqlCommand(query, dbconnection);
                dr = comand.ExecuteReader();
                while (dr.Read())
                {
                    dbconnection3.Open();
                    double price = 0;
                    double priceF = 0;
                    double totalQuanity = 0;
                    gridView1.AddNewRow();
                    int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
                    if (gridView1.IsNewItemRow(rowHandle))
                    {
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns[1], dr["الكود"]);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns[2], dr["الاسم"]);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["البيان"], dr["البيان"]);
                        
                        query = "SELECT sum(set_details.Quantity*sellprice.Price) as 'السعر',(sellprice.Sell_Discount) as 'الخصم',sum(set_details.Quantity*sellprice.Sell_Price) as 'بعد الخصم',sum(storage.Total_Meters)/COUNT(set_details.Data_ID) as 'الكمية' FROM sets INNER JOIN set_details ON set_details.Set_ID = sets.Set_ID INNER JOIN sellprice ON set_details.Data_ID = sellprice.Data_ID LEFT JOIN storage ON storage.Set_ID = sets.Set_ID where sellprice.Price_Type='لستة' and sets.Set_ID=" + dr["الكود"] + " group by set_details.Set_ID, storage.Set_ID";
                        comand = new MySqlCommand(query, dbconnection3);
                        MySqlDataReader dr1 = comand.ExecuteReader();
                        while (dr1.Read())
                        {
                            price += Convert.ToDouble(dr1["السعر"].ToString());
                            priceF += Convert.ToDouble(dr1["بعد الخصم"].ToString());
                            if (dr1["الكمية"].ToString() != "")
                            {
                                totalQuanity += Convert.ToDouble(dr1["الكمية"].ToString());
                            }
                            else
                            {
                                totalQuanity += 0;
                            }
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الخصم"], dr1["الخصم"]);
                        }
                        dr1.Close();

                        query = "SELECT sum(set_details.Quantity*sellprice.Sell_Price) as 'السعر',sum(set_details.Quantity*sellprice.Sell_Price) as 'بعد الخصم',sum(storage.Total_Meters)/COUNT(set_details.Data_ID) as 'الكمية',sets.Description as 'البيان' FROM sets INNER JOIN set_details ON set_details.Set_ID = sets.Set_ID INNER JOIN sellprice ON set_details.Data_ID = sellprice.Data_ID LEFT JOIN storage ON storage.Set_ID = sets.Set_ID where sellprice.Price_Type='قطعى' and sets.Set_ID=" + dr["الكود"] + " group by set_details.Set_ID, storage.Set_ID";
                        comand = new MySqlCommand(query, dbconnection3);
                        MySqlDataReader dr2 = comand.ExecuteReader();
                        while (dr2.Read())
                        {
                            price += Convert.ToDouble(dr2["السعر"].ToString());
                            priceF += Convert.ToDouble(dr2["بعد الخصم"].ToString());
                            if (dr2["الكمية"].ToString() != "")
                            {
                                totalQuanity += Convert.ToDouble(dr2["الكمية"].ToString());
                            }
                            else
                            {
                                totalQuanity += 0;
                            }
                        }
                        dr2.Close();

                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], price);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["بعد الخصم"], priceF);
                        if (totalQuanity != 0)
                        {
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], totalQuanity);
                        }
                        else
                        {
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], "");
                        }
                    }
                    dbconnection3.Close();
                }
                dr.Close();
                #endregion

                #region عرض
                query = "select offer.Offer_ID as 'الكود',CONCAT('(عرض) ',Offer_Name) as 'الاسم',Price as 'السعر',sum(storage.Total_Meters) as 'الكمية' from offer LEFT JOIN storage ON storage.Offer_ID = offer.Offer_ID group by storage.Offer_ID";
                comand = new MySqlCommand(query, dbconnection);
                dr = comand.ExecuteReader();
                while (dr.Read())
                {
                    gridView1.AddNewRow();
                    int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
                    if (gridView1.IsNewItemRow(rowHandle))
                    {
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns[1], dr["الكود"]);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns[2], dr["الاسم"]);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], dr["السعر"]);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["بعد الخصم"], dr["السعر"]);
                        gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], dr["الكمية"]);
                    }
                }
                dr.Close();
                #endregion

                dbconnection.Close();

                gridView1.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[1].Width = 150;
                gridView1.Columns[0].Visible = false;
                for (int i = 2; i < gridView1.Columns.Count; i++)
                {
                    gridView1.Columns[i].Width = 100;
                }
                gridView1.Columns["الفرز"].Width = 50;
                gridView1.Columns["الكرتنة"].Width = 60;
                //gridView1.Columns["بعد الخصم"].Width = 130;
                //gridView1.Columns["الكمية"].Width = 120;

                if (gridView1.IsLastVisibleRow)
                {
                    gridView1.FocusedRowHandle = gridView1.RowCount - 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection3.Close();
            dbconnection.Close();
        }

        private void txtCodeSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dbconnection.Open();
                    string query = "select data.Data_ID,data.Code as 'الكود',CONCAT('(بند) ',product.Product_Name) as 'الاسم',type.Type_Name 'النوع',factory.Factory_Name as 'المصنع',groupo.Group_Name as 'المجموعة',color.Color_Name as 'اللون',size.Size_Value as 'المقاس',sort.Sort_Value as 'الفرز',data.Classification as 'التصنيف',data.Carton as 'الكرتنة',data.Description as 'البيان',sellprice.Price as 'السعر',sellprice.Sell_Discount as 'الخصم',sum(storage.Total_Meters) as 'الكمية' FROM data LEFT JOIN color ON color.Color_ID = data.Color_ID LEFT JOIN size ON size.Size_ID = data.Size_ID LEFT JOIN sort ON sort.Sort_ID = data.Sort_ID INNER JOIN groupo ON data.Group_ID = groupo.Group_ID INNER JOIN factory ON factory.Factory_ID = data.Factory_ID  INNER JOIN product ON product.Product_ID = data.Product_ID  INNER JOIN type ON type.Type_ID = data.Type_ID  INNER JOIN sellprice ON sellprice.Data_ID = data.Data_ID LEFT JOIN storage ON storage.Data_ID = data.Data_ID where data.Code='" + code1 + code2 + code3 + code4 + code5 + "' and data.Data_ID=0 group by data.Data_ID";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gridControl1.DataSource = dt;
                    gridView1.Columns[0].Visible = false;

                    query = "SELECT data.Data_ID,data.Code as 'الكود',CONCAT('(بند) ',product.Product_Name) as 'الاسم',type.Type_Name 'النوع',factory.Factory_Name as 'المصنع',groupo.Group_Name as 'المجموعة',color.Color_Name as 'اللون',size.Size_Value as 'المقاس',sort.Sort_Value as 'الفرز',data.Classification as 'التصنيف',data.Carton as 'الكرتنة',data.Description as 'البيان',sellprice.Price as 'السعر',sellprice.Sell_Discount as 'الخصم',sellprice.Sell_Price as 'بعد الخصم',sum(storage.Total_Meters) as 'الكمية',sellprice.Price_Type FROM data LEFT JOIN color ON color.Color_ID = data.Color_ID LEFT JOIN size ON size.Size_ID = data.Size_ID LEFT JOIN sort ON sort.Sort_ID = data.Sort_ID INNER JOIN groupo ON data.Group_ID = groupo.Group_ID INNER JOIN factory ON factory.Factory_ID = data.Factory_ID  INNER JOIN product ON product.Product_ID = data.Product_ID  INNER JOIN type ON type.Type_ID = data.Type_ID  INNER JOIN sellprice ON sellprice.Data_ID = data.Data_ID LEFT JOIN storage ON storage.Data_ID = data.Data_ID where data.Code='" + code1 + code2 + code3 + code4 + code5 + "' group by data.Data_ID";
                    MySqlCommand comand = new MySqlCommand(query, dbconnection);
                    MySqlDataReader dr = comand.ExecuteReader();
                    while (dr.Read())
                    {
                        gridView1.AddNewRow();
                        int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
                        if (gridView1.IsNewItemRow(rowHandle))
                        {
                            if (dr["Price_Type"].ToString() == "لستة")
                            {
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[0], dr["Data_ID"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[1], dr["الكود"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[2], dr["الاسم"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[3], dr["النوع"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[4], dr["المصنع"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[5], dr["المجموعة"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["اللون"], dr["اللون"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المقاس"], dr["المقاس"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الفرز"], dr["الفرز"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["التصنيف"], dr["التصنيف"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكرتنة"], dr["الكرتنة"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["البيان"], dr["البيان"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], dr["السعر"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الخصم"], dr["الخصم"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["بعد الخصم"], dr["بعد الخصم"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], dr["الكمية"]);

                            }
                            else if (dr["Price_Type"].ToString() == "قطعى")
                            {
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[0], dr["Data_ID"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[1], dr["الكود"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[2], dr["الاسم"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[3], dr["النوع"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[4], dr["المصنع"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[5], dr["المجموعة"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["اللون"], dr["اللون"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المقاس"], dr["المقاس"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الفرز"], dr["الفرز"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["التصنيف"], dr["التصنيف"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكرتنة"], dr["الكرتنة"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["البيان"], dr["البيان"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], dr["بعد الخصم"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الخصم"], "");
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["بعد الخصم"], dr["بعد الخصم"]);
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], dr["الكمية"]);

                            }
                        }
                    }
                    dr.Close();
                    if (gridView1.IsLastVisibleRow)
                    {
                        gridView1.FocusedRowHandle = gridView1.RowCount - 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }

        private void comBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                ComboBox comBox = (ComboBox)sender;

                switch (comBox.Name)
                {
                    case "comType":
                        {
                            txtCodeSearch1.Text = comType.SelectedValue.ToString();
                            string query = "select * from color where color.Type_ID=" + comType.SelectedValue.ToString();
                            MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            comColor.DataSource = dt;
                            comColor.DisplayMember = dt.Columns["Color_Name"].ToString();
                            comColor.ValueMember = dt.Columns["Color_ID"].ToString();
                            comColor.Text = "";

                            string qall = "";
                            if (comType.Text != "")
                            {
                                qall += " and product.Type_ID=" + comType.SelectedValue.ToString();
                            }
                            if (comFactory.Text != "")
                            {
                                qall += " and product.Factory_ID=" + comFactory.SelectedValue.ToString();
                            }
                            if (comGroup.Text != "")
                            {
                                qall += " and product.Product_ID IN (select Product_ID from product_group where Group_ID=" + comGroup.SelectedValue.ToString() + ")";
                            }
                            query = "select * from product where product.Product_ID is not null " + qall;
                            da = new MySqlDataAdapter(query, dbconnection);
                            dt = new DataTable();
                            da.Fill(dt);
                            comProduct.DataSource = dt;
                            comProduct.DisplayMember = dt.Columns["Product_Name"].ToString();
                            comProduct.ValueMember = dt.Columns["Product_ID"].ToString();
                            comProduct.Text = "";
                        }
                        break;
                    case "comFactory":
                        {
                            txtCodeSearch2.Text = comFactory.SelectedValue.ToString();
                            string query = "select * from size where size.Factory_ID=" + comFactory.SelectedValue.ToString();
                            MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            comSize.DataSource = dt;
                            comSize.DisplayMember = dt.Columns["Size_Value"].ToString();
                            comSize.ValueMember = dt.Columns["Size_ID"].ToString();
                            comSize.Text = "";

                            string qall = "";
                            if (comType.Text != "")
                            {
                                qall += " and product.Type_ID=" + comType.SelectedValue.ToString();
                            }
                            if (comFactory.Text != "")
                            {
                                qall += " and product.Factory_ID=" + comFactory.SelectedValue.ToString();
                            }
                            if (comGroup.Text != "")
                            {
                                qall += " and product.Product_ID IN (select Product_ID from product_group where Group_ID=" + comGroup.SelectedValue.ToString() + ")";
                            }
                            query = "select * from product where product.Product_ID is not null " + qall;
                            da = new MySqlDataAdapter(query, dbconnection);
                            dt = new DataTable();
                            da.Fill(dt);
                            comProduct.DataSource = dt;
                            comProduct.DisplayMember = dt.Columns["Product_Name"].ToString();
                            comProduct.ValueMember = dt.Columns["Product_ID"].ToString();
                            comProduct.Text = "";
                        }
                        break;
                    case "comGroup":
                        {
                            txtCodeSearch3.Text = comGroup.SelectedValue.ToString();
                            string qall = "";
                            if (comType.Text != "")
                            {
                                qall += " and product.Type_ID=" + comType.SelectedValue.ToString();
                            }
                            if (comFactory.Text != "")
                            {
                                qall += " and product.Factory_ID=" + comFactory.SelectedValue.ToString();
                            }
                            if (comGroup.Text != "")
                            {
                                qall += " and product.Product_ID IN (select Product_ID from product_group where Group_ID=" + comGroup.SelectedValue.ToString() + ")";
                            }
                            string query = "select * from product where product.Product_ID is not null " + qall;
                            MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            comProduct.DataSource = dt;
                            comProduct.DisplayMember = dt.Columns["Product_Name"].ToString();
                            comProduct.ValueMember = dt.Columns["Product_ID"].ToString();
                            comProduct.Text = "";
                        }
                        break;
                    case "comProduct":
                        {
                            txtCodeSearch4.Text = comProduct.SelectedValue.ToString();
                        }
                        break;
                }
            }
        }

        private void txtBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            string query;
            MySqlCommand com;
            string Name;
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (txtBox.Text != "")
                    {
                        dbconnection.Open();
                        switch (txtBox.Name)
                        {
                            case "txtCodeSearch1":
                                query = "select Type_Name from type where Type_ID='" + txtCodeSearch1.Text + "'";
                                com = new MySqlCommand(query, dbconnection);
                                if (com.ExecuteScalar() != null)
                                {
                                    Name = (string)com.ExecuteScalar();
                                    comType.Text = Name;
                                    txtCodeSearch2.Focus();
                                }
                                else
                                {
                                    MessageBox.Show("there is no item with this id");
                                    dbconnection.Close();
                                    return;
                                }
                                break;
                            case "txtCodeSearch2":
                                query = "select Factory_Name from factory where Factory_ID='" + txtCodeSearch2.Text + "'";
                                com = new MySqlCommand(query, dbconnection);
                                if (com.ExecuteScalar() != null)
                                {
                                    Name = (string)com.ExecuteScalar();
                                    comFactory.Text = Name;
                                    txtCodeSearch3.Focus();
                                }
                                else
                                {
                                    MessageBox.Show("there is no item with this id");
                                    dbconnection.Close();
                                    return;
                                }
                                break;
                            case "txtCodeSearch3":
                                query = "select Group_Name from groupo where Group_ID='" + txtCodeSearch3.Text + "'";
                                com = new MySqlCommand(query, dbconnection);
                                if (com.ExecuteScalar() != null)
                                {
                                    Name = (string)com.ExecuteScalar();
                                    comGroup.Text = Name;
                                    txtCodeSearch4.Focus();
                                }
                                else
                                {
                                    MessageBox.Show("there is no item with this id");
                                    dbconnection.Close();
                                    return;
                                }
                                break;
                            case "txtCodeSearch4":
                                query = "select Product_Name from product where Product_ID='" + txtCodeSearch4.Text + "'";
                                com = new MySqlCommand(query, dbconnection);
                                if (com.ExecuteScalar() != null)
                                {
                                    Name = (string)com.ExecuteScalar();
                                    comProduct.Text = Name;
                                    txtCodeSearch5.Focus();

                                    query = "select data.Data_ID,data.Code as 'الكود',CONCAT('(بند) ',product.Product_Name) as 'الاسم',type.Type_Name 'النوع',factory.Factory_Name as 'المصنع',groupo.Group_Name as 'المجموعة',color.Color_Name as 'اللون',size.Size_Value as 'المقاس',sort.Sort_Value as 'الفرز',data.Classification as 'التصنيف',data.Carton as 'الكرتنة',data.Description as 'البيان',sellprice.Price as 'السعر',sellprice.Sell_Discount as 'الخصم',sum(storage.Total_Meters) as 'الكمية' FROM data LEFT JOIN color ON color.Color_ID = data.Color_ID LEFT JOIN size ON size.Size_ID = data.Size_ID LEFT JOIN sort ON sort.Sort_ID = data.Sort_ID INNER JOIN groupo ON data.Group_ID = groupo.Group_ID INNER JOIN factory ON factory.Factory_ID = data.Factory_ID  INNER JOIN product ON product.Product_ID = data.Product_ID  INNER JOIN type ON type.Type_ID = data.Type_ID  INNER JOIN sellprice ON sellprice.Data_ID = data.Data_ID LEFT JOIN storage ON storage.Data_ID = data.Data_ID where data.Code like '" + code1 + code2 + code3 + code4 + "%' and data.Data_ID=0 group by data.Data_ID";
                                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                                    DataTable dt = new DataTable();
                                    da.Fill(dt);
                                    gridControl1.DataSource = dt;
                                    gridView1.Columns[0].Visible = false;

                                    query = "SELECT data.Data_ID,data.Code as 'الكود',CONCAT('(بند) ',product.Product_Name) as 'الاسم',type.Type_Name 'النوع',factory.Factory_Name as 'المصنع',groupo.Group_Name as 'المجموعة',color.Color_Name as 'اللون',size.Size_Value as 'المقاس',sort.Sort_Value as 'الفرز',data.Classification as 'التصنيف',data.Carton as 'الكرتنة',data.Description as 'البيان',sellprice.Price as 'السعر',sellprice.Sell_Discount as 'الخصم',sellprice.Sell_Price as 'بعد الخصم',sum(storage.Total_Meters) as 'الكمية',sellprice.Price_Type FROM data LEFT JOIN color ON color.Color_ID = data.Color_ID LEFT JOIN size ON size.Size_ID = data.Size_ID LEFT JOIN sort ON sort.Sort_ID = data.Sort_ID INNER JOIN groupo ON data.Group_ID = groupo.Group_ID INNER JOIN factory ON factory.Factory_ID = data.Factory_ID  INNER JOIN product ON product.Product_ID = data.Product_ID  INNER JOIN type ON type.Type_ID = data.Type_ID  INNER JOIN sellprice ON sellprice.Data_ID = data.Data_ID LEFT JOIN storage ON storage.Data_ID = data.Data_ID where data.Code like '" + code1 + code2 + code3 + code4 + "%' group by data.Data_ID";
                                    MySqlCommand comand = new MySqlCommand(query, dbconnection);
                                    MySqlDataReader dr = comand.ExecuteReader();
                                    while (dr.Read())
                                    {
                                        gridView1.AddNewRow();
                                        int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
                                        if (gridView1.IsNewItemRow(rowHandle))
                                        {
                                            if (dr["Price_Type"].ToString() == "لستة")
                                            {
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[0], dr["Data_ID"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[1], dr["الكود"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[2], dr["الاسم"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[3], dr["النوع"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[4], dr["المصنع"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[5], dr["المجموعة"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["اللون"], dr["اللون"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المقاس"], dr["المقاس"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الفرز"], dr["الفرز"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["التصنيف"], dr["التصنيف"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكرتنة"], dr["الكرتنة"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["البيان"], dr["البيان"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], dr["السعر"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الخصم"], dr["الخصم"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["بعد الخصم"], dr["بعد الخصم"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], dr["الكمية"]);

                                            }
                                            else if (dr["Price_Type"].ToString() == "قطعى")
                                            {
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[0], dr["Data_ID"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[1], dr["الكود"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[2], dr["الاسم"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[3], dr["النوع"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[4], dr["المصنع"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[5], dr["المجموعة"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["اللون"], dr["اللون"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["المقاس"], dr["المقاس"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الفرز"], dr["الفرز"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["التصنيف"], dr["التصنيف"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكرتنة"], dr["الكرتنة"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["البيان"], dr["البيان"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], dr["بعد الخصم"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الخصم"], "");
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["بعد الخصم"], dr["بعد الخصم"]);
                                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], dr["الكمية"]);

                                            }
                                        }
                                    }
                                    dr.Close();
                                    if (gridView1.IsLastVisibleRow)
                                    {
                                        gridView1.FocusedRowHandle = gridView1.RowCount - 1;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("there is no item with this id");
                                    dbconnection.Close();
                                    return;
                                }
                                break;
                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                dbconnection.Close();
            }
        }

        private void comSet_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (loaded)
                {
                    comOffer.Text = "";
                    checkEditOffers.Checked = false;

                    checkEditSets.Checked = false;
                    dbconnection.Open();
                    string query = "SELECT sets.Set_ID as 'الكود',CONCAT('(طقم) ',sets.Set_Name) as 'الاسم',type.Type_Name as 'النوع',factory.Factory_Name as 'المصنع',groupo.Group_Name as 'المجموعة',(set_details.Quantity*sellprice.Price) as 'السعر',(sellprice.Sell_Discount) as 'الخصم',(set_details.Quantity*sellprice.Price) as 'بعد الخصم',sum(storage.Total_Meters) as 'الكمية',sets.Description as 'البيان' FROM sets INNER JOIN factory ON factory.Factory_ID = sets.Factory_ID INNER JOIN type ON type.Type_ID = sets.Type_ID INNER JOIN groupo ON groupo.Group_ID = sets.Group_ID INNER JOIN set_details ON set_details.Set_ID = sets.Set_ID INNER JOIN sellprice ON set_details.Data_ID = sellprice.Data_ID LEFT JOIN storage ON storage.Set_ID = sets.Set_ID where sets.Set_ID=0 group by set_details.Set_ID";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gridControl1.DataSource = dt;

                    query = "SELECT sets.Set_ID as 'الكود',CONCAT('(طقم) ',sets.Set_Name) as 'الاسم',sets.Description as 'البيان' FROM sets where sets.Set_ID=" + comSet.SelectedValue.ToString();
                    MySqlCommand comand = new MySqlCommand(query, dbconnection);
                    MySqlDataReader dr = comand.ExecuteReader();
                    while (dr.Read())
                    {
                        dbconnection3.Open();
                        double price = 0;
                        double priceF = 0;
                        double totalQuanity = 0;
                        gridView1.AddNewRow();
                        int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
                        if (gridView1.IsNewItemRow(rowHandle))
                        {
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[1], dr["الكود"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[2], dr["الاسم"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["البيان"], dr["البيان"]);

                            query = "SELECT sum(set_details.Quantity*sellprice.Price) as 'السعر',(set_details.Quantity*sellprice.Sell_Discount) as 'الخصم',sum(set_details.Quantity*sellprice.Sell_Price) as 'بعد الخصم',sum(storage.Total_Meters)/COUNT(set_details.Data_ID) as 'الكمية' FROM sets INNER JOIN set_details ON set_details.Set_ID = sets.Set_ID INNER JOIN sellprice ON set_details.Data_ID = sellprice.Data_ID LEFT JOIN storage ON storage.Set_ID = sets.Set_ID where sellprice.Price_Type='لستة' and sets.Set_ID=" + dr["الكود"] + " group by set_details.Set_ID, storage.Set_ID";
                            comand = new MySqlCommand(query, dbconnection3);
                            MySqlDataReader dr1 = comand.ExecuteReader();
                            while (dr1.Read())
                            {
                                price += Convert.ToDouble(dr1["السعر"].ToString());
                                priceF += Convert.ToDouble(dr1["بعد الخصم"].ToString());
                                if (dr1["الكمية"].ToString() != "")
                                {
                                    totalQuanity += Convert.ToDouble(dr1["الكمية"].ToString());
                                }
                                else
                                {
                                    totalQuanity += 0;
                                }
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الخصم"], dr1["الخصم"]);
                            }
                            dr1.Close();

                            query = "SELECT sum(set_details.Quantity*sellprice.Sell_Price) as 'السعر',sum(set_details.Quantity*sellprice.Sell_Price) as 'بعد الخصم',sum(storage.Total_Meters)/COUNT(set_details.Data_ID) as 'الكمية' FROM sets INNER JOIN set_details ON set_details.Set_ID = sets.Set_ID INNER JOIN sellprice ON set_details.Data_ID = sellprice.Data_ID LEFT JOIN storage ON storage.Set_ID = sets.Set_ID where sellprice.Price_Type='قطعى' and sets.Set_ID=" + dr["الكود"] + " group by set_details.Set_ID, storage.Set_ID";
                            comand = new MySqlCommand(query, dbconnection3);
                            MySqlDataReader dr2 = comand.ExecuteReader();
                            while (dr2.Read())
                            {
                                price += Convert.ToDouble(dr2["السعر"].ToString());
                                priceF += Convert.ToDouble(dr2["بعد الخصم"].ToString());
                                if (dr2["الكمية"].ToString() != "")
                                {
                                    totalQuanity += Convert.ToDouble(dr2["الكمية"].ToString());
                                }
                                else
                                {
                                    totalQuanity += 0;
                                }
                            }
                            dr2.Close();

                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], price);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["بعد الخصم"], priceF);
                            if (totalQuanity != 0)
                            {
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], totalQuanity);
                            }
                            else
                            {
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], "");
                            }
                        }
                        dbconnection3.Close();
                    }
                    dr.Close();
                    if (gridView1.IsLastVisibleRow)
                    {
                        gridView1.FocusedRowHandle = gridView1.RowCount - 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection3.Close();
            dbconnection.Close();
        }

        private void comOffer_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (loaded)
                {
                    comSet.Text = "";
                    checkEditSets.Checked = false;

                    checkEditOffers.Checked = false;
                    string query = "select offer.Offer_ID as 'الكود',CONCAT('(عرض) ',Offer_Name) as 'الاسم',Price as 'السعر',Price as 'بعد الخصم',sum(storage.Total_Meters) as 'الكمية' from offer LEFT JOIN storage ON storage.Offer_ID = offer.Offer_ID where offer.Offer_ID=" + comOffer.SelectedValue.ToString()+ "  group by storage.Offer_ID";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gridControl1.DataSource = dt;

                    if (gridView1.IsLastVisibleRow)
                    {
                        gridView1.FocusedRowHandle = gridView1.RowCount - 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }

        private void txtCodeSearch1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                if (txtCodeSearch1.Text != "")
                {
                    if (int.TryParse(txtCodeSearch1.Text, out result))
                    {
                        if (!flag1)
                        {
                            if (txtCodeSearch1.TextLength == 1)
                            {
                                code1 = "000" + txtCodeSearch1.Text;
                            }
                            else if (txtCodeSearch1.TextLength == 2)
                            {
                                code1 = "00" + txtCodeSearch1.Text;
                            }
                            else if (txtCodeSearch1.TextLength == 3)
                            {
                                code1 = "0" + txtCodeSearch1.Text;
                            }
                            else if (txtCodeSearch1.TextLength == 4)
                            {
                                code1 = txtCodeSearch1.Text;
                            }
                            else if (txtCodeSearch1.TextLength > 4)
                            {
                                txtCodeSearch1.Text = code1;
                            }
                            else if (txtCodeSearch1.TextLength == 0)
                            {
                                code1 = "0000";
                            }
                        }
                    }
                    else
                    {
                        flag1 = true;
                        if (txtCodeSearch1.TextLength == 1)
                        {
                            txtCodeSearch1.Text = "";
                        }
                        else if (txtCodeSearch1.TextLength > 1)
                        {
                            for (int i = 0; i < txtCodeSearch1.TextLength - 1; i++)
                            {
                                txtCodeSearch1.Text = txtCodeSearch1.Text[i].ToString();
                            }
                        }
                        MessageBox.Show("يمكنك ادخال ارقام فقط");
                        flag1 = false;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtCodeSearch2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                if (txtCodeSearch2.Text != "")
                {
                    if (int.TryParse(txtCodeSearch2.Text, out result))
                    {
                        if (!flag2)
                        {
                            if (txtCodeSearch2.TextLength == 1)
                            {
                                code2 = "000" + txtCodeSearch2.Text;
                            }
                            else if (txtCodeSearch2.TextLength == 2)
                            {
                                code2 = "00" + txtCodeSearch2.Text;
                            }
                            else if (txtCodeSearch2.TextLength == 3)
                            {
                                code2 = "0" + txtCodeSearch2.Text;
                            }
                            else if (txtCodeSearch2.TextLength == 4)
                            {
                                code2 = txtCodeSearch2.Text;
                            }
                            else if (txtCodeSearch2.TextLength > 4)
                            {
                                txtCodeSearch2.Text = code2;
                            }
                            else if (txtCodeSearch2.TextLength == 0)
                            {
                                code2 = "0000";
                            }
                        }
                    }
                    else
                    {
                        flag2 = true;
                        if (txtCodeSearch2.TextLength == 1)
                        {
                            txtCodeSearch2.Text = "";
                        }
                        else if (txtCodeSearch2.TextLength > 1)
                        {
                            for (int i = 0; i < txtCodeSearch2.TextLength - 1; i++)
                            {
                                txtCodeSearch2.Text = txtCodeSearch2.Text[i].ToString();
                            }
                        }
                        MessageBox.Show("يمكنك ادخال ارقام فقط");
                        flag2 = false;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtCodeSearch3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                if (txtCodeSearch3.Text != "")
                {
                    if (int.TryParse(txtCodeSearch3.Text, out result))
                    {
                        if (!flag3)
                        {
                            if (txtCodeSearch3.TextLength == 1)
                            {
                                code3 = "000" + txtCodeSearch3.Text;
                            }
                            else if (txtCodeSearch3.TextLength == 2)
                            {
                                code3 = "00" + txtCodeSearch3.Text;
                            }
                            else if (txtCodeSearch3.TextLength == 3)
                            {
                                code3 = "0" + txtCodeSearch3.Text;
                            }
                            else if (txtCodeSearch3.TextLength == 4)
                            {
                                code3 = txtCodeSearch3.Text;
                            }
                            else if (txtCodeSearch3.TextLength > 4)
                            {
                                txtCodeSearch3.Text = code3;
                            }
                            else if (txtCodeSearch3.TextLength == 0)
                            {
                                code3 = "0000";
                            }
                        }
                    }
                    else
                    {
                        flag3 = true;
                        if (txtCodeSearch3.TextLength == 1)
                        {
                            txtCodeSearch3.Text = "";
                        }
                        else if (txtCodeSearch3.TextLength > 1)
                        {
                            for (int i = 0; i < txtCodeSearch3.TextLength - 1; i++)
                            {
                                txtCodeSearch3.Text = txtCodeSearch3.Text[i].ToString();
                            }
                        }
                        MessageBox.Show("يمكنك ادخال ارقام فقط");
                        flag3 = false;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtCodeSearch4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                if (txtCodeSearch4.Text != "")
                {
                    if (int.TryParse(txtCodeSearch4.Text, out result))
                    {
                        if (!flag4)
                        {
                            if (txtCodeSearch4.TextLength == 1)
                            {
                                code4 = "000" + txtCodeSearch4.Text;
                            }
                            else if (txtCodeSearch4.TextLength == 2)
                            {
                                code4 = "00" + txtCodeSearch4.Text;
                            }
                            else if (txtCodeSearch4.TextLength == 3)
                            {
                                code4 = "0" + txtCodeSearch4.Text;
                            }
                            else if (txtCodeSearch4.TextLength == 4)
                            {
                                code4 = txtCodeSearch4.Text;
                            }
                            else if (txtCodeSearch4.TextLength > 4)
                            {
                                txtCodeSearch4.Text = code4;
                            }
                            else if (txtCodeSearch4.TextLength == 0)
                            {
                                code4 = "0000";
                            }
                        }
                    }
                    else
                    {
                        flag4 = true;
                        if (txtCodeSearch4.TextLength == 1)
                        {
                            txtCodeSearch4.Text = "";
                        }
                        else if (txtCodeSearch4.TextLength > 1)
                        {
                            for (int i = 0; i < txtCodeSearch4.TextLength - 1; i++)
                            {
                                txtCodeSearch4.Text = txtCodeSearch4.Text[i].ToString();
                            }
                        }
                        MessageBox.Show("يمكنك ادخال ارقام فقط");
                        flag4 = false;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtCodeSearch5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                if (txtCodeSearch5.Text != "")
                {
                    if (int.TryParse(txtCodeSearch5.Text, out result))
                    {
                        if (!flag5)
                        {
                            if (txtCodeSearch5.TextLength == 1)
                            {
                                code5 = "000" + txtCodeSearch5.Text;
                            }
                            else if (txtCodeSearch5.TextLength == 2)
                            {
                                code5 = "00" + txtCodeSearch5.Text;
                            }
                            else if (txtCodeSearch5.TextLength == 3)
                            {
                                code5 = "0" + txtCodeSearch5.Text;
                            }
                            else if (txtCodeSearch5.TextLength == 4)
                            {
                                code5 = txtCodeSearch5.Text;
                            }
                            else if (txtCodeSearch5.TextLength > 4)
                            {
                                txtCodeSearch5.Text = code5;
                            }
                            else if (txtCodeSearch5.TextLength == 0)
                            {
                                code5 = "0000";
                            }
                        }
                    }
                    else
                    {
                        flag5 = true;
                        if (txtCodeSearch5.TextLength == 1)
                        {
                            txtCodeSearch5.Text = "";
                        }
                        else if (txtCodeSearch5.TextLength > 1)
                        {
                            for (int i = 0; i < txtCodeSearch5.TextLength - 1; i++)
                            {
                                txtCodeSearch5.Text = txtCodeSearch5.Text[i].ToString();
                            }
                        }
                        MessageBox.Show("يمكنك ادخال ارقام فقط");
                        flag5 = false;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkEditSets_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkEditSets.Checked)
                {
                    comOffer.Text = "";
                    checkEditOffers.Checked = false;

                    comSet.Text = "";
                    dbconnection.Open();
                    string query = "SELECT sets.Set_ID as 'الكود',CONCAT('(طقم) ',sets.Set_Name) as 'الاسم',type.Type_Name as 'النوع',factory.Factory_Name as 'المصنع',groupo.Group_Name as 'المجموعة',(set_details.Quantity*sellprice.Price) as 'السعر',sum(sellprice.Sell_Discount) as 'الخصم',(set_details.Quantity*sellprice.Price) as 'بعد الخصم',sum(storage.Total_Meters) as 'الكمية',sets.Description as 'البيان' FROM sets INNER JOIN factory ON factory.Factory_ID = sets.Factory_ID INNER JOIN type ON type.Type_ID = sets.Type_ID INNER JOIN groupo ON groupo.Group_ID = sets.Group_ID INNER JOIN set_details ON set_details.Set_ID = sets.Set_ID INNER JOIN sellprice ON set_details.Data_ID = sellprice.Data_ID LEFT JOIN storage ON storage.Set_ID = sets.Set_ID where sets.Set_ID=0 group by set_details.Set_ID";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gridControl1.DataSource = dt;

                    query = "SELECT sets.Set_ID as 'الكود',CONCAT('(طقم) ',sets.Set_Name) as 'الاسم',sets.Description as 'البيان' FROM sets";
                    MySqlCommand comand = new MySqlCommand(query, dbconnection);
                    MySqlDataReader dr = comand.ExecuteReader();
                    while (dr.Read())
                    {
                        dbconnection3.Open();
                        double price = 0;
                        double priceF = 0;
                        double totalQuanity = 0;
                        gridView1.AddNewRow();
                        int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
                        if (gridView1.IsNewItemRow(rowHandle))
                        {
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[1], dr["الكود"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns[2], dr["الاسم"]);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["البيان"], dr["البيان"]);

                            query = "SELECT sum(set_details.Quantity*sellprice.Price) as 'السعر',(set_details.Quantity*sellprice.Sell_Discount) as 'الخصم',sum(set_details.Quantity*sellprice.Sell_Price) as 'بعد الخصم',sum(storage.Total_Meters)/COUNT(set_details.Data_ID) as 'الكمية' FROM sets INNER JOIN set_details ON set_details.Set_ID = sets.Set_ID INNER JOIN sellprice ON set_details.Data_ID = sellprice.Data_ID LEFT JOIN storage ON storage.Set_ID = sets.Set_ID where sellprice.Price_Type='لستة' and sets.Set_ID=" + dr["الكود"] + " group by set_details.Set_ID, storage.Set_ID";
                            comand = new MySqlCommand(query, dbconnection3);
                            MySqlDataReader dr1 = comand.ExecuteReader();
                            while (dr1.Read())
                            {
                                price += Convert.ToDouble(dr1["السعر"].ToString());
                                priceF += Convert.ToDouble(dr1["بعد الخصم"].ToString());
                                if (dr1["الكمية"].ToString() != "")
                                {
                                    totalQuanity += Convert.ToDouble(dr1["الكمية"].ToString());
                                }
                                else
                                {
                                    totalQuanity += 0;
                                }
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الخصم"], dr1["الخصم"]);
                            }
                            dr1.Close();

                            query = "SELECT sum(set_details.Quantity*sellprice.Sell_Price) as 'السعر',sum(set_details.Quantity*sellprice.Sell_Price) as 'بعد الخصم',sum(storage.Total_Meters)/COUNT(set_details.Data_ID) as 'الكمية' FROM sets INNER JOIN set_details ON set_details.Set_ID = sets.Set_ID INNER JOIN sellprice ON set_details.Data_ID = sellprice.Data_ID LEFT JOIN storage ON storage.Set_ID = sets.Set_ID where sellprice.Price_Type='قطعى' and sets.Set_ID=" + dr["الكود"] + " group by set_details.Set_ID, storage.Set_ID";
                            comand = new MySqlCommand(query, dbconnection3);
                            MySqlDataReader dr2 = comand.ExecuteReader();
                            while (dr2.Read())
                            {
                                price += Convert.ToDouble(dr2["السعر"].ToString());
                                priceF += Convert.ToDouble(dr2["بعد الخصم"].ToString());
                                if (dr2["الكمية"].ToString() != "")
                                {
                                    totalQuanity += Convert.ToDouble(dr2["الكمية"].ToString());
                                }
                                else
                                {
                                    totalQuanity += 0;
                                }
                            }
                            dr2.Close();

                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["السعر"], price);
                            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["بعد الخصم"], priceF);
                            if (totalQuanity != 0)
                            {
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], totalQuanity);
                            }
                            else
                            {
                                gridView1.SetRowCellValue(rowHandle, gridView1.Columns["الكمية"], "");
                            }
                        }
                        dbconnection3.Close();
                    }
                    dr.Close();
                    if (gridView1.IsLastVisibleRow)
                    {
                        gridView1.FocusedRowHandle = gridView1.RowCount - 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection3.Close();
            dbconnection.Close();
        }

        private void checkEditOffers_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkEditOffers.Checked)
                {
                    comSet.Text = "";
                    checkEditSets.Checked = false;

                    comOffer.Text = "";
                    string query = "select offer.Offer_ID as 'الكود',CONCAT('(عرض) ',Offer_Name) as 'الاسم',Price as 'السعر',Price as 'بعد الخصم',sum(storage.Total_Meters) as 'الكمية' from offer LEFT JOIN storage ON storage.Offer_ID = offer.Offer_ID group by storage.Offer_ID";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gridControl1.DataSource = dt;

                    if (gridView1.IsLastVisibleRow)
                    {
                        gridView1.FocusedRowHandle = gridView1.RowCount - 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                row1 = gridView1.GetDataRow(gridView1.GetRowHandle(e.RowHandle));
                //row1.Selected = true;
                if (loaded)
                {
                    if (e.Column.ToString() == "الكود")
                    {
                        if (tipImage == null)
                        {
                            if (row1["الكود"].ToString().Length >= 20)
                            {
                                tipImage = new TipImage(row1["Data_ID"].ToString(), "بند");
                            }
                            else if (row1["الاسم"].ToString().Split(')')[0].Split('(')[1] == "طقم")
                            {
                                tipImage = new TipImage(row1["الكود"].ToString(), "طقم");
                            }
                            else if (row1["الاسم"].ToString().Split(')')[0].Split('(')[1] == "عرض")
                            {
                                tipImage = new TipImage(row1["الكود"].ToString(), "عرض");
                            }
                            tipImage.Location = new Point(Cursor.Position.X, Cursor.Position.Y);
                            tipImage.Show();
                        }
                        else
                        {
                            tipImage.Close();
                            if (row1["الكود"].ToString().Length >= 20)
                            {
                                tipImage = new TipImage(row1["Data_ID"].ToString(), "بند");
                            }
                            else if (row1["الاسم"].ToString().Split(')')[0].Split('(')[1] == "طقم")
                            {
                                tipImage = new TipImage(row1["الكود"].ToString(), "طقم");
                            }
                            else if (row1["الاسم"].ToString().Split(')')[0].Split('(')[1] == "عرض")
                            {
                                tipImage = new TipImage(row1["الكود"].ToString(), "عرض");
                            }
                            tipImage.Location = new Point(Cursor.Position.X, Cursor.Position.Y);
                            tipImage.Show();
                        }
                    }

                    else if (e.Column.ToString() == "الكمية")
                    {
                        if (row1["الكود"].ToString().Length >= 20)
                        {
                            StoresDetails sd = new StoresDetails(row1["Data_ID"].ToString());
                            sd.ShowDialog();
                        }
                    }

                    else if (e.Column.ToString() == "الاسم")
                    {
                        if (row1["الكود"].ToString().Length >= 20)
                        {

                        }
                        else if (row1["الاسم"].ToString().Split(')')[0].Split('(')[1] == "طقم")
                        {
                            ItemDetails itemD = new ItemDetails(row1["الكود"].ToString(), "طقم");
                            itemD.ShowDialog();
                        }
                        else if (row1["الاسم"].ToString().Split(')')[0].Split('(')[1] == "عرض")
                        {
                            ItemDetails itemD = new ItemDetails(row1["الكود"].ToString(), "عرض");
                            itemD.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                dbconnection.Open();

                if (gridView1.SelectedRowsCount > 0)
                {
                    if (gridView1.GetRowCellDisplayText(gridView1.GetSelectedRows()[0], "الكمية").ToString() != "" && gridView1.GetRowCellDisplayText(gridView1.GetSelectedRows()[0], "الكمية").ToString() != "0")
                    {
                        if (txtQuantity.Text != "" && comStore.Text != "")
                        {
                            double TotalMeters = 0;
                            if (double.TryParse(txtQuantity.Text, out TotalMeters))
                            {
                                if (checkQuantityInStore(TotalMeters))
                                {
                                    if (gridView1.GetRowCellDisplayText(gridView1.GetSelectedRows()[0], "الكود").Length >= 20)
                                    {
                                        if (cartonNumCheck())
                                        { }
                                        else
                                        {
                                            dbconnection.Close();
                                            return;
                                        }
                                    }

                                    /*****************************************************************************************/
                                    DataRowView row1 = (DataRowView)gridView1.GetRow(gridView1.GetSelectedRows()[0]);
                                    SalesMainForm.objFormBillConfirm.addItemToView(row1, TotalMeters, Convert.ToInt16(comStore.SelectedValue.ToString()), comStore.Text);
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("لا يوجد كمية كافية");
                                }
                            }
                            else
                            {
                                MessageBox.Show("الكمية يجب ان تكون عدد");
                            }
                        }
                        else
                        {
                            MessageBox.Show("برجاء ادخال البيانات كاملة");
                        }
                    }
                    else
                    {
                        MessageBox.Show("لا يوجد اى كمية من هذا العنصر");
                    }
                }
                else
                {
                    MessageBox.Show("برجاء اختيار عنصر للاضافة");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }
    
        /*private void txtBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                xtraTabPage = getTabPage("tabPageProductsReport");
                if (!IsClear())
                {
                    xtraTabPage.ImageOptions.Image = Properties.Resources.unsave;
                }
                else
                {
                    xtraTabPage.ImageOptions.Image = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }*/

        //functions
        public void search()
        {
            dbconnection.Open();

            string query = "select * from type";
            MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comType.DataSource = dt;
            comType.DisplayMember = dt.Columns["Type_Name"].ToString();
            comType.ValueMember = dt.Columns["Type_ID"].ToString();
            comType.Text = "";
            //txtType.Text = "";

            query = "select * from factory";
            da = new MySqlDataAdapter(query, dbconnection);
            dt = new DataTable();
            da.Fill(dt);
            comFactory.DataSource = dt;
            comFactory.DisplayMember = dt.Columns["Factory_Name"].ToString();
            comFactory.ValueMember = dt.Columns["Factory_ID"].ToString();
            comFactory.Text = "";
            //txtFactory.Text = "";

            query = "select * from groupo";
            da = new MySqlDataAdapter(query, dbconnection);
            dt = new DataTable();
            da.Fill(dt);
            comGroup.DataSource = dt;
            comGroup.DisplayMember = dt.Columns["Group_Name"].ToString();
            comGroup.ValueMember = dt.Columns["Group_ID"].ToString();
            comGroup.Text = "";
            //txtGroup.Text = "";

            query = "select * from product";
            da = new MySqlDataAdapter(query, dbconnection);
            dt = new DataTable();
            da.Fill(dt);
            comProduct.DataSource = dt;
            comProduct.DisplayMember = dt.Columns["Product_Name"].ToString();
            comProduct.ValueMember = dt.Columns["Product_ID"].ToString();
            comProduct.Text = "";
            //txtProduct.Text = "";

            query = "select * from sets";
            da = new MySqlDataAdapter(query, dbconnection);
            dt = new DataTable();
            da.Fill(dt);
            comSet.DataSource = dt;
            comSet.DisplayMember = dt.Columns["Set_Name"].ToString();
            comSet.ValueMember = dt.Columns["Set_ID"].ToString();
            comSet.Text = "";

            query = "select * from offer";
            da = new MySqlDataAdapter(query, dbconnection);
            dt = new DataTable();
            da.Fill(dt);
            comOffer.DataSource = dt;
            comOffer.DisplayMember = dt.Columns["Offer_Name"].ToString();
            comOffer.ValueMember = dt.Columns["Offer_ID"].ToString();
            comOffer.Text = "";

            query = "select * from store";
            da = new MySqlDataAdapter(query, dbconnection);
            dt = new DataTable();
            da.Fill(dt);
            comStore.DataSource = dt;
            comStore.DisplayMember = dt.Columns["Store_Name"].ToString();
            comStore.ValueMember = dt.Columns["Store_ID"].ToString();
            comStore.Text = "";

            query = "select * from size";
            da = new MySqlDataAdapter(query, dbconnection);
            dt = new DataTable();
            da.Fill(dt);
            comSize.DataSource = dt;
            comSize.DisplayMember = dt.Columns["Size_Value"].ToString();
            comSize.ValueMember = dt.Columns["Size_ID"].ToString();
            comSize.Text = "";

            query = "select * from color";
            da = new MySqlDataAdapter(query, dbconnection);
            dt = new DataTable();
            da.Fill(dt);
            comColor.DataSource = dt;
            comColor.DisplayMember = dt.Columns["Color_Name"].ToString();
            comColor.ValueMember = dt.Columns["Color_ID"].ToString();
            comColor.Text = "";

            query = "select * from sort";
            da = new MySqlDataAdapter(query, dbconnection);
            dt = new DataTable();
            da.Fill(dt);
            comSort.DataSource = dt;
            comSort.DisplayMember = dt.Columns["Sort_Value"].ToString();
            comSort.ValueMember = dt.Columns["Sort_ID"].ToString();
            comSort.Text = "";

            //search2();
            dbconnection.Close();
        }
        
        public bool checkQuantityInStore(double totalMeter)
        {
            if (row1["الاسم"].ToString().Split(')')[0].Split('(')[1] == "بند")
            {
                string query = "SELECT sum(storage.Total_Meters) as 'الكمية' FROM storage where storage.Store_ID=" + comStore.SelectedValue.ToString() + " and storage.Data_ID=" + gridView1.GetRowCellDisplayText(gridView1.GetSelectedRows()[0], "Data_ID") + " group by storage.Data_ID";
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
            else if (row1["الاسم"].ToString().Split(')')[0].Split('(')[1] == "طقم")
            {
                string query = "SELECT sum(storage.Total_Meters) as 'الكمية' FROM storage where storage.Store_ID=" + comStore.SelectedValue.ToString() + " and storage.Set_ID=" + gridView1.GetRowCellDisplayText(gridView1.GetSelectedRows()[0], "الكود") + " group by storage.Set_ID";
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
            else if (row1["الاسم"].ToString().Split(')')[0].Split('(')[1] == "عرض")
            {
                double totalquant = Convert.ToDouble(gridView1.GetRowCellDisplayText(gridView1.GetSelectedRows()[0], "الكمية"));
                if (totalMeter <= totalquant)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public bool cartonNumCheck()
        {
            try
            {
                string query = "select Carton from data where Code='" + gridView1.GetRowCellDisplayText(gridView1.GetSelectedRows()[0], "الكود") + "'";
                MySqlCommand com = new MySqlCommand(query, dbconnection);
                if (com.ExecuteScalar() != null)
                {
                    double Carton = Convert.ToDouble(com.ExecuteScalar());
                    double totalMeters = double.Parse(txtQuantity.Text);
                    if (totalMeters % Carton == 0)
                    {
                        MessageBox.Show("تحتاج " + totalMeters / Carton + " كرتونة");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("تحتاج " + totalMeters / Carton + " كرتونة و " + totalMeters % Carton + " متر");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        public XtraTabPage getTabPage(string text)
        {
            for (int i = 0; i < SalesMainForm.tabControlSales.TabPages.Count; i++)
                if (SalesMainForm.tabControlSales.TabPages[i].Name == text)
                {
                    return SalesMainForm.tabControlSales.TabPages[i];
                }
            return null;
        }

        //clear function
        public void clear()
        {
            foreach (Control co in this.panel1.Controls)
            {
                //if (co is GroupBox)
                //{
                foreach (Control item in co.Controls)
                {
                    if (item is ComboBox)
                    {
                        item.Text = "";
                    }
                    else if (item is TextBox)
                    {
                        item.Text = "";
                    }
                }
                //}
            }
        }

        public bool IsClear()
        {
            bool flag5 = false;
            foreach (Control co in this.panel1.Controls)
            {
                foreach (Control item in co.Controls)
                {
                    if (item is ComboBox)
                    {
                        if (item.Text == "")
                            flag5 = true;
                        else
                            return false;
                    }
                    else if (item is TextBox)
                    {
                        if (item.Text == "")
                            flag5 = true;
                        else
                            return false;
                    }
                }
            }

            return flag5;
        }
    }
}
