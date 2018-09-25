﻿using DevExpress.XtraGrid;
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
    public partial class Customer_Report : Form
    {
        MySqlConnection conn;
        XtraTabControl MainTabControlPS;

        public static XtraTabPage MainTabPageAddCustomer;
        Panel panelAddCustomer;
        public static XtraTabPage MainTabPageUpdateCustomer;
        Panel panelUpdateCustomer;
        public static XtraTabPage MainTabPagePrintCustomer;
        Panel panelPrintCustomer;

        public static Customer_Print customerPrint;

        public static GridControl gridcontrol;

        public Customer_Report()
        {
            InitializeComponent();
            conn = new MySqlConnection(connection.connectionString);
            MainTabControlPS = SalesMainForm.tabControlSales;

            MainTabPageAddCustomer = new XtraTabPage();
            panelAddCustomer = new Panel();

            MainTabPageUpdateCustomer = new XtraTabPage();
            panelUpdateCustomer = new Panel();

            MainTabPagePrintCustomer = new XtraTabPage();
            panelPrintCustomer = new Panel();

            gridcontrol = gridControl1;
        }

        private void Delegate_Report_Load(object sender, EventArgs e)
        {
            try
            {
                search();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                XtraTabPage xtraTabPage = getTabPage("tabPageAddCustomer");
                if (xtraTabPage == null)
                {
                    Customer_Record form = new Customer_Record();
                    MainTabPageAddCustomer.Name = "tabPageAddCustomer";
                    MainTabPageAddCustomer.Text = "اضافة عميل";
                    MainTabPageAddCustomer.ImageOptions.Image = null;
                    panelAddCustomer.Name = "panelAddCustomer";
                    panelAddCustomer.Dock = DockStyle.Fill;
                    MainTabPageAddCustomer.Controls.Clear();
                    form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    form.Dock = DockStyle.Fill;
                    form.TopLevel = false;
                    form.Show();
                    panelAddCustomer.Controls.Add(form);
                    MainTabPageAddCustomer.Controls.Add(panelAddCustomer);
                    MainTabControlPS.TabPages.Add(MainTabPageAddCustomer);
                    MainTabControlPS.SelectedTabPage = MainTabPageAddCustomer;
                }
                else if (xtraTabPage.ImageOptions.Image != null)
                {
                    DialogResult dialogResult = MessageBox.Show("هناك تعديلات لم تحفظ بعد..هل انت متاكد انك تريد التجاهل؟", "تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        MainTabPageAddCustomer.ImageOptions.Image = null;
                        Customer_Record form = new Customer_Record();
                        form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                        form.Dock = DockStyle.Fill;
                        form.TopLevel = false;
                        form.Show();
                        MainTabPageAddCustomer.Controls.Clear();
                        panelAddCustomer.Controls.Clear();
                        panelAddCustomer.Controls.Add(form);
                        MainTabPageAddCustomer.Controls.Add(panelAddCustomer);
                        MainTabControlPS.SelectedTabPage = MainTabPageAddCustomer;
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        MainTabControlPS.SelectedTabPage = MainTabPageAddCustomer;
                    }
                }
                else
                {
                    MainTabPageAddCustomer.ImageOptions.Image = null;
                    Customer_Record form = new Customer_Record();
                    form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    form.Dock = DockStyle.Fill;
                    form.TopLevel = false;
                    form.Show();
                    MainTabPageAddCustomer.Controls.Clear();
                    panelAddCustomer.Controls.Clear();
                    panelAddCustomer.Controls.Add(form);
                    MainTabPageAddCustomer.Controls.Add(panelAddCustomer);
                    MainTabControlPS.SelectedTabPage = MainTabPageAddCustomer;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DataRowView selRow = (DataRowView)gridView1.GetRow(gridView1.GetSelectedRows()[0]);
                if (selRow[0].ToString() != "")
                {
                    XtraTabPage xtraTabPage = getTabPage("tabPageUpdateCustomer");
                    if (xtraTabPage == null)
                    {
                        Customer_Update form = new Customer_Update(selRow);
                        MainTabPageUpdateCustomer.Name = "tabPageUpdateCustomer";
                        MainTabPageUpdateCustomer.Text = "تعديل بيانات عميل";
                        MainTabPageUpdateCustomer.ImageOptions.Image = null;
                        panelUpdateCustomer.Name = "panelUpdateCustomer";
                        panelUpdateCustomer.Dock = DockStyle.Fill;
                        MainTabPageUpdateCustomer.Controls.Clear();
                        form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                        form.Dock = DockStyle.Fill;
                        form.TopLevel = false;
                        form.Show();
                        panelUpdateCustomer.Controls.Add(form);
                        MainTabPageUpdateCustomer.Controls.Add(panelUpdateCustomer);
                        MainTabControlPS.TabPages.Add(MainTabPageUpdateCustomer);
                        MainTabControlPS.SelectedTabPage = MainTabPageUpdateCustomer;
                    }
                    else if (xtraTabPage.ImageOptions.Image != null)
                    {
                        DialogResult dialogResult = MessageBox.Show("هناك تعديلات لم تحفظ بعد..هل انت متاكد انك تريد التجاهل؟", "تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dialogResult == DialogResult.Yes)
                        {
                            MainTabPageUpdateCustomer.ImageOptions.Image = null;
                            Customer_Update form = new Customer_Update(selRow);
                            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                            form.Dock = DockStyle.Fill;
                            form.TopLevel = false;
                            form.Show();
                            MainTabPageUpdateCustomer.Controls.Clear();
                            panelUpdateCustomer.Controls.Clear();
                            panelUpdateCustomer.Controls.Add(form);
                            MainTabPageUpdateCustomer.Controls.Add(panelUpdateCustomer);
                            MainTabControlPS.SelectedTabPage = MainTabPageUpdateCustomer;
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            MainTabControlPS.SelectedTabPage = MainTabPageUpdateCustomer;
                        }
                    }
                    else
                    {
                        MainTabPageUpdateCustomer.ImageOptions.Image = null;
                        Customer_Update form = new Customer_Update(selRow);
                        form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                        form.Dock = DockStyle.Fill;
                        form.TopLevel = false;
                        form.Show();
                        MainTabPageUpdateCustomer.Controls.Clear();
                        panelUpdateCustomer.Controls.Clear();
                        panelUpdateCustomer.Controls.Add(form);
                        MainTabPageUpdateCustomer.Controls.Add(panelUpdateCustomer);
                        MainTabControlPS.SelectedTabPage = MainTabPageUpdateCustomer;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                GridView view = gridView1 as GridView;
                delete(view);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                MainTabPagePrintCustomer.Name = "tabPagePrintCustomer";
                MainTabPagePrintCustomer.Text = "طباعة العملاء";
                panelPrintCustomer.Name = "panelPrintCustomer";
                panelPrintCustomer.Dock = DockStyle.Fill;

                panelPrintCustomer.Controls.Clear();
                customerPrint = new Customer_Print();
                customerPrint.Size = new Size(1059, 638);
                customerPrint.TopLevel = false;
                customerPrint.FormBorderStyle = FormBorderStyle.None;
                customerPrint.Dock = DockStyle.Fill;
                panelPrintCustomer.Controls.Add(customerPrint);
                MainTabPagePrintCustomer.Controls.Add(panelPrintCustomer);
                MainTabControlPS.TabPages.Add(MainTabPagePrintCustomer);
                customerPrint.Show();
                MainTabControlPS.SelectedTabPage = MainTabPagePrintCustomer;

                Main.loadedPrintCustomer = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    //GridView view = sender as GridView;
                    GridView view = gridView1 as GridView;
                    delete(view);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
        }

        private void toolTipController1_GetActiveObjectInfo(object sender, DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            try
            {
                if (e.SelectedControl != gridControl1) return;
                GridHitInfo gridhitinfo = gridView1.CalcHitInfo(e.ControlMousePosition);
                object o = gridhitinfo.HitTest.ToString();
                string text = gridhitinfo.HitTest.ToString();
                e.Info = new DevExpress.Utils.ToolTipControlInfo(o, text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //functions
        public void search()
        {
            DataSet sourceDataSet = new DataSet();
            //MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT Delegate_ID,Delegate_Name,Phone,Address,Qualification,Birth_Date,National_ID,Social_Status,Mail,Photo,Start_Date,Target,Salary,Job_Hours,Info,Branch_ID,Branch_Name FROM delegate", conn);
            MySqlDataAdapter adapterCustomer = new MySqlDataAdapter("SELECT customer1.Customer_ID as 'التسلسل',customer1.Customer_Name as 'الاسم',customer1.Customer_Address as 'العنوان',customer1.Customer_Start as 'تاريخ البداية',customer1.Customer_NationalID as 'الرقم القومى',customer1.Customer_Email as 'الايميل',customer1.Customer_Type as 'النوع',customer2.Customer_Name as 'الضامن',customer1.Customer_Info as 'البيان' FROM customer AS customer1 LEFT JOIN custmer_client ON customer1.Customer_ID = custmer_client.Client_ID LEFT JOIN customer AS customer2 ON customer2.Customer_ID = custmer_client.Customer_ID ORDER BY customer1.Customer_ID", conn);

            MySqlDataAdapter adapterPhone = new MySqlDataAdapter("SELECT customer.Customer_ID as 'التسلسل',Phone as 'رقم التليفون' FROM customer_phone inner join customer on customer.Customer_ID=customer_phone.Customer_ID", conn);

            adapterCustomer.Fill(sourceDataSet, "customer");
            adapterPhone.Fill(sourceDataSet,"customer_phone");

            //Set up a master-detail relationship between the DataTables 
            DataColumn keyColumn = sourceDataSet.Tables["customer"].Columns["التسلسل"];
            DataColumn foreignKeyColumn = sourceDataSet.Tables["customer_phone"].Columns["التسلسل"];
            sourceDataSet.Relations.Add("ارقام التليفون", keyColumn, foreignKeyColumn);
            
            gridControl1.DataSource = sourceDataSet.Tables["customer"];

            //gridView1.Columns["التسلسل"].Group();

            /*this.gridView1.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
                new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Min, "IdRandom", "Min = {0}", "Min")});*/

            /*gridView1.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            for (int i = 1; i < gridView1.Columns.Count; i++)
            {
                gridView1.Columns[i].Width = 150;
            }*/
        }

        public XtraTabPage getTabPage(string text)
        {
            for (int i = 0; i < MainTabControlPS.TabPages.Count; i++)
                if (MainTabControlPS.TabPages[i].Name == text)
                {
                    return MainTabControlPS.TabPages[i];
                }
            return null;
        }

        void delete(GridView view)
        {
            int[] selRows = ((GridView)gridControl1.MainView).GetSelectedRows();
            if (selRows.Length > 0)
            {
                if (MessageBox.Show("هل انت متاكد انك تريد الحذف؟", "تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;

                conn.Open();
                for (int i = 0; i < selRows.Length; i++)
                {
                    DataRowView selRow = (DataRowView)(((GridView)gridControl1.MainView).GetRow(selRows[i]));

                    string query = "delete from customer_phone where Customer_ID=" + selRow[0].ToString();
                    MySqlCommand comand = new MySqlCommand(query, conn);
                    comand.ExecuteNonQuery();

                    query = "delete from customer where Customer_ID=" + selRow[0].ToString();
                    comand = new MySqlCommand(query, conn);
                    comand.ExecuteNonQuery();

                    //UserControl.ItemRecord("customer", "حذف", Convert.ToInt16(selRow[0].ToString()), DateTime.Now, textBox.Text, conn);
                }
                conn.Close();
                search();
            }
            else
            {
                MessageBox.Show("يجب ان تختار عنصر للحذف");
            }
        }
    }
}
