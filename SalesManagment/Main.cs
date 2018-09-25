using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTab.ViewInfo;
using DevExpress.XtraTab;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraGrid;
using DevExpress.XtraNavBar;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace SalesManagment
{
    public partial class Main : DevExpress.XtraEditors.XtraForm
    {
        MySqlConnection conn;
        public static XtraTabControl tabControlPointSale;

        public static bool loadedPrintDelegateReport = false;
        public static bool loadedPrintCustomer = false;

        public static Customer_Report DelegateReportShow;
        public static Bill_Confirm BillConfirm;
        
        XtraTabPage tabPageBillConfirm;
        Panel panelBillConfirm;
        XtraTabPage tabPageDelegateReport;
        Panel panelDelegateReport;

        public static int delegateID = -1;
        public static int billNum = 0;
        bool flag = false;

        public Main()
        {
            InitializeComponent();
            conn = new MySqlConnection(connection.connectionString);
            
            tabPageBillConfirm = new XtraTabPage();
            panelBillConfirm = new Panel();
            tabPageDelegateReport = new XtraTabPage();
            panelDelegateReport = new Panel();

            xtraTabControlMain.TabPages.Remove(xtraTabPagePointSale);

            tabControlPointSale = xtraTabControlPointSale;
        }

        private void xtraTabControlMain_CloseButtonClick(object sender, EventArgs e)
        {
            try
            {
                ClosePageButtonEventArgs arg = e as ClosePageButtonEventArgs;
                XtraTabPage xtraTabPage = (XtraTabPage)arg.Page;
                if (!IsTabPageSave())
                {
                    DialogResult dialogResult = MessageBox.Show("هل متاكد انك تريد غلق النافذة بدون حفظ التعديلات؟", "تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        xtraTabControlMain.TabPages.Remove(arg.Page as XtraTabPage);
                        flag = false;
                    }
                    else if (dialogResult == DialogResult.No)
                    {

                    }
                }
                else
                {
                    xtraTabControlMain.TabPages.Remove(arg.Page as XtraTabPage);
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void xtraTabControlPointSale_CloseButtonClick(object sender, EventArgs e)
        {
            try
            {
                ClosePageButtonEventArgs arg = e as ClosePageButtonEventArgs;
                XtraTabPage xtraTabPage = (XtraTabPage)arg.Page;
                if (xtraTabPage.ImageOptions.Image != null)
                {
                    DialogResult dialogResult = MessageBox.Show("هل متاكد انك تريد غلق النافذة بدون حفظ التعديلات؟", "تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {

                        xtraTabControlPointSale.TabPages.Remove(arg.Page as XtraTabPage);
                    }
                    else if (dialogResult == DialogResult.No)
                    {

                    }
                }
                else
                {
                    xtraTabControlPointSale.TabPages.Remove(arg.Page as XtraTabPage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void xtraTabControlPointSale_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            try
            {
                //if (xtraTabControlPointSale.SelectedTabPage == tabPageProductsReport)
                //{
                //    ProductsReport.search();
                //}

                //else if (xtraTabControlBank.SelectedTabPage == Bank_Report.MainTabPagePrintingBank)
                //{
                //    if (loadedPrintBank)
                //    {
                //        Bank_Report.bankPrint.display();
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void navBarItemShow_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                restForeColorOfNavBarItem();
                NavBarItem navBarItem = (NavBarItem)sender;
                navBarItem.Appearance.ForeColor = Color.FromArgb(54, 70, 151);

                XtraTabPage xtraTabPage = getTabPage("tabPageCustomerReport");
                if (xtraTabPage == null)
                {
                    tabPageDelegateReport.Name = "tabPageCustomerReport";
                    tabPageDelegateReport.Text = "عرض العملاء";
                    panelDelegateReport.Name = "panelCustomerReport";
                    panelDelegateReport.Dock = DockStyle.Fill;

                    DelegateReportShow = new Customer_Report();
                    DelegateReportShow.Size = new Size(1109, 660);
                    DelegateReportShow.TopLevel = false;
                    DelegateReportShow.FormBorderStyle = FormBorderStyle.None;
                    DelegateReportShow.Dock = DockStyle.Fill;
                }
                panelDelegateReport.Controls.Clear();
                panelDelegateReport.Controls.Add(DelegateReportShow);
                tabPageDelegateReport.Controls.Add(panelDelegateReport);
                xtraTabControlPointSale.TabPages.Add(tabPageDelegateReport);
                DelegateReportShow.Show();
                xtraTabControlPointSale.SelectedTabPage = tabPageDelegateReport;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //functions
        public void restForeColorOfNavBarItem()
        {
            foreach (NavBarItem item in navBarControlPointSaleReport.Items)
            {
                item.Appearance.ForeColor = Color.Black;
            }
        }

        public XtraTabPage getTabPage(string text)
        {
            for (int i = 0; i < Main.tabControlPointSale.TabPages.Count; i++)
                if (Main.tabControlPointSale.TabPages[i].Name == text)
                {
                    return Main.tabControlPointSale.TabPages[i];
                }
            return null;
        }

        private void tileItemPointSale_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                if (flag == false)
                {
                    xtraTabControlMain.TabPages.Add(xtraTabPagePointSale);
                    flag = true;
                }
                xtraTabControlMain.SelectedTabPage = xtraTabPagePointSale;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void test(int DelegateId, int BillNum)
        {
            int count = 0;
            conn.Open();
            string query = "SELECT dash_details.Code FROM dash_details INNER JOIN dash ON dash.Dash_ID = dash_details.Dash_ID where dash.Bill_Number=" + BillNum + " and dash.Delegate_ID=" + DelegateId;
            MySqlCommand com = new MySqlCommand(query, conn);
            MySqlDataReader dr = com.ExecuteReader();
            while(dr.Read())
            {
                count++;
            }
            labelNotify.Text = (count).ToString();
            if (Convert.ToInt16(labelNotify.Text) > 0)
            {
                labelNotify.Visible = true;
                delegateID = DelegateId;
                billNum = BillNum;
            }
            else
            {
                labelNotify.Visible = false;
                delegateID = 0;
                billNum = 0;
            }
            dr.Close();
            conn.Close();
        }

        private void navBarItemConfirmBill_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                restForeColorOfNavBarItem();
                NavBarItem navBarItem = (NavBarItem)sender;
                navBarItem.Appearance.ForeColor = Color.FromArgb(54, 70, 151);

                XtraTabPage xtraTabPage = getTabPage("tabPageBillConfirm");
                if (xtraTabPage == null)
                {
                    tabPageBillConfirm.Name = "tabPageBillConfirm";
                    tabPageBillConfirm.Text = "تاكيد فاتورة";
                    panelBillConfirm.Name = "panelBillConfirm";
                    panelBillConfirm.Dock = DockStyle.Fill;

                    BillConfirm = new Bill_Confirm();
                    BillConfirm.Size = new Size(1109, 660);
                    BillConfirm.TopLevel = false;
                    BillConfirm.FormBorderStyle = FormBorderStyle.None;
                    BillConfirm.Dock = DockStyle.Fill;
                }
                panelBillConfirm.Controls.Clear();
                panelBillConfirm.Controls.Add(BillConfirm);
                tabPageBillConfirm.Controls.Add(panelBillConfirm);
                xtraTabControlPointSale.TabPages.Add(tabPageBillConfirm);
                BillConfirm.Show();
                xtraTabControlPointSale.SelectedTabPage = tabPageBillConfirm;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool IsTabPageSave()
        {
            for (int i = 0; i < xtraTabControlPointSale.TabPages.Count; i++)
                if (xtraTabControlPointSale.TabPages[i].ImageOptions.Image != null)
                {
                    return false;
                }
            return true;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!IsTabPageSave())
                {
                    DialogResult dialogResult = MessageBox.Show("هل متاكد انك تريد غلق النافذة بدون حفظ التعديلات؟", "تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        e.Cancel = (dialogResult == DialogResult.No);
                    }
                }
                else
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }


}