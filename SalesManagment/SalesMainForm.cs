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
using DevExpress.XtraTab;
using DevExpress.XtraGrid;
using DevExpress.XtraTab.ViewInfo;
using DevExpress.XtraNavBar;

namespace SalesManagment
{
    public partial class SalesMainForm : DevExpress.XtraEditors.XtraForm
    {
        XtraTabPage StoreTP;
        bool flag = false;
        public static XtraTabControl tabControlSales;
        public SalesMainForm()
        {
            try
            {
                InitializeComponent();
                StoreTP = xtraTabPageSales;
                xtraTabControlMainContainer.TabPages.Remove(xtraTabPageSales);

                tabControlSales = xtraTabControlSalesContent;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void btnSales_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                if (flag == false)
                {
                    xtraTabControlMainContainer.TabPages.Insert(1, StoreTP);
                    flag = true;
                }
                xtraTabControlMainContainer.SelectedTabPage = xtraTabControlMainContainer.TabPages[1];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void navBarItemCustomers_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                restForeColorOfNavBarItem();
                NavBarItem navBarItem = (NavBarItem)sender;
                navBarItem.Appearance.ForeColor = Color.Blue;

                if (!xtraTabControlSalesContent.Visible)
                    xtraTabControlSalesContent.Visible = true;

                XtraTabPage xtraTabPage = getTabPage("العملاء");
                if (xtraTabPage == null)
                {
                    xtraTabControlSalesContent.TabPages.Add("العملاء");
                    xtraTabPage = getTabPage("العملاء");
                }
                xtraTabPage.Controls.Clear();

                xtraTabControlSalesContent.SelectedTabPage = xtraTabPage;
                bindDisplayCustomersForm(xtraTabPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void navBarItemConfirmBill_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                restForeColorOfNavBarItem();
                NavBarItem navBarItem = (NavBarItem)sender;
                navBarItem.Appearance.ForeColor = Color.Blue;

                if (!xtraTabControlSalesContent.Visible)
                    xtraTabControlSalesContent.Visible = true;

                XtraTabPage xtraTabPage = getTabPage("تاكيد الفاتورة");
                if (xtraTabPage == null)
                {
                    xtraTabControlSalesContent.TabPages.Add("تاكيد الفاتورة");
                    xtraTabPage = getTabPage("تاكيد الفاتورة");
                }
                xtraTabPage.Controls.Clear();

                xtraTabControlSalesContent.SelectedTabPage = xtraTabPage;
                bindDisplayConfirmBillForm(xtraTabPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnProductSellPrice_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                restForeColorOfNavBarItem();
                NavBarItem navBarItem = (NavBarItem)sender;
                navBarItem.Appearance.ForeColor = Color.Blue;

                if (!xtraTabControlSalesContent.Visible)
                    xtraTabControlSalesContent.Visible = true;

                XtraTabPage xtraTabPage = getTabPage("البنود");
                if (xtraTabPage == null)
                {
                    xtraTabControlSalesContent.TabPages.Add("البنود");
                    xtraTabPage = getTabPage("البنود");
                }
                xtraTabPage.Controls.Clear();

                xtraTabControlSalesContent.SelectedTabPage = xtraTabPage;
                bindDisplayProductsSellPriceForm(xtraTabPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOffers_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                restForeColorOfNavBarItem();
                NavBarItem navBarItem = (NavBarItem)sender;
                navBarItem.Appearance.ForeColor = Color.Blue;

                if (!xtraTabControlSalesContent.Visible)
                    xtraTabControlSalesContent.Visible = true;

                XtraTabPage xtraTabPage = getTabPage("العروض");
                if (xtraTabPage == null)
                {
                    xtraTabControlSalesContent.TabPages.Add("العروض");
                    xtraTabPage = getTabPage("العروض");
                }
                xtraTabPage.Controls.Clear();

                xtraTabControlSalesContent.SelectedTabPage = xtraTabPage;
                bindDisplayOffersForm(xtraTabPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void xtraTabControlStoresContent_CloseButtonClick(object sender, EventArgs e)
        {
            try
            {
                ClosePageButtonEventArgs arg = e as ClosePageButtonEventArgs;
                XtraTabPage xtraTabPage = (XtraTabPage)arg.Page;
                if (xtraTabPage.ImageOptions.Image != null)
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to Close this page without save?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {

                        xtraTabControlSalesContent.TabPages.Remove(arg.Page as XtraTabPage);
                    }
                    else if (dialogResult == DialogResult.No)
                    {

                    }
                }
                else
                {
                    xtraTabControlSalesContent.TabPages.Remove(arg.Page as XtraTabPage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void xtraTabControlMainContainer_CloseButtonClick(object sender, EventArgs e)
        {
            try
            {
                ClosePageButtonEventArgs arg = e as ClosePageButtonEventArgs;
                XtraTabPage xtraTabPage = (XtraTabPage)arg.Page;
                if (!IsTabPageSave())
                {
                    DialogResult dialogResult = MessageBox.Show("There are unsave Pages To you wound close anyway?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        xtraTabControlMainContainer.TabPages.Remove(arg.Page as XtraTabPage);
                        flag = false;
                    }
                    else if (dialogResult == DialogResult.No)
                    {

                    }
                }
                else
                {
                    xtraTabControlMainContainer.TabPages.Remove(arg.Page as XtraTabPage);
                    flag = false;
                }
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
     
        private void SalesMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsTabPageSave())
            {
                DialogResult dialogResult = MessageBox.Show("There are unsave Pages To you wound close anyway?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    Environment.Exit(0);
                }
                else if (dialogResult == DialogResult.No)
                {
                    e.Cancel = (dialogResult == DialogResult.No);
                }
            }
            else
            {
                Environment.Exit(0);
            }
        }

        //functions
        //Products sell price
        public void bindDisplayProductsSellPriceForm(XtraTabPage xtraTabPage)
        {
            ProductsSellPriceForm objForm = new ProductsSellPriceForm(this);
            objForm.TopLevel = false;

            xtraTabPage.Controls.Add(objForm);
            objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            objForm.Dock = DockStyle.Fill;
            objForm.Show();
        }
        //record sell price 
        public void bindRecordSellPriceForm(ProductsSellPriceForm productsSellPriceForm)
        {
            if (!xtraTabControlSalesContent.Visible)
                xtraTabControlSalesContent.Visible = true;

            XtraTabPage xtraTabPage = getTabPage("تسجيل اسعار البنود");
            if (xtraTabPage == null)
            {
                xtraTabControlSalesContent.TabPages.Add("تسجيل اسعار البنود");
                xtraTabPage = getTabPage("تسجيل اسعار البنود");
            }
            xtraTabPage.Controls.Clear();

            xtraTabControlSalesContent.SelectedTabPage = xtraTabPage;
            SetSellPrice objForm = new SetSellPrice(productsSellPriceForm,xtraTabControlSalesContent);
            objForm.TopLevel = false;

            xtraTabPage.Controls.Add(objForm);
            objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            objForm.Dock = DockStyle.Fill;
            objForm.Show();
        }
        //update sell price 
        public void bindUpdateSellPriceForm(List<DataRowView> rows,ProductsSellPriceForm productsSellPriceForm,String query)
        {
            if (!xtraTabControlSalesContent.Visible)
                xtraTabControlSalesContent.Visible = true;

            XtraTabPage xtraTabPage = getTabPage("تعديل اسعار البنود");

            if (xtraTabPage == null)
            {
                xtraTabControlSalesContent.TabPages.Add("تعديل اسعار البنود");
                xtraTabPage = getTabPage("تعديل اسعار البنود");
            }
            xtraTabPage.Controls.Clear();

            xtraTabControlSalesContent.SelectedTabPage = xtraTabPage;

            UpdateSellPriceForm objForm = new UpdateSellPriceForm(rows,productsSellPriceForm, query,xtraTabControlSalesContent);
            objForm.TopLevel = false;

            xtraTabPage.Controls.Add(objForm);
            objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            objForm.Dock = DockStyle.Fill;
            objForm.Show();
        }
        //report sell price 
        public void bindReportSellPriceForm( GridControl gridControl)
        {
            if (!xtraTabControlSalesContent.Visible)
                xtraTabControlSalesContent.Visible = true;

            XtraTabPage xtraTabPage = getTabPage("تقرير اسعار البنود");

            if (xtraTabPage == null)
            {
                xtraTabControlSalesContent.TabPages.Add("تقرير اسعار البنود");
                xtraTabPage = getTabPage("تقرير اسعار البنود");
            }
            xtraTabPage.Controls.Clear();

            xtraTabControlSalesContent.SelectedTabPage = xtraTabPage;
            ProductSellPricesReport objForm = new ProductSellPricesReport(gridControl);
            objForm.TopLevel = false;

            xtraTabPage.Controls.Add(objForm);
            objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            objForm.Dock = DockStyle.Fill;
            objForm.Show();
        }

        //offers
        public void bindDisplayOffersForm(XtraTabPage xtraTabPage)
        {
            //OffersSellPrice objForm = new OffersSellPrice();
            //objForm.TopLevel = false;

            //xtraTabPage.Controls.Add(objForm);
            //objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //objForm.Dock = DockStyle.Fill;
            //objForm.Show();
        }

        //customers
        public void bindDisplayCustomersForm(XtraTabPage xtraTabPage)
        {
            Customer_Report objForm = new Customer_Report();
            objForm.TopLevel = false;

            xtraTabPage.Controls.Add(objForm);
            objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            objForm.Dock = DockStyle.Fill;
            objForm.Show();
        }
        //confirm bill
        public void bindDisplayConfirmBillForm(XtraTabPage xtraTabPage)
        {
            Bill_Confirm objForm = new Bill_Confirm();
            objForm.TopLevel = false;

            xtraTabPage.Controls.Add(objForm);
            objForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            objForm.Dock = DockStyle.Fill;
            objForm.Show();
        }

        public XtraTabPage getTabPage(string text)
        {
            for (int i = 0; i < xtraTabControlSalesContent.TabPages.Count; i++)
                if (xtraTabControlSalesContent.TabPages[i].Text == text)
                {
                    return xtraTabControlSalesContent.TabPages[i];
                }
            return null;
        }

        public bool IsTabPageSave()
        {
            for (int i = 0; i < xtraTabControlSalesContent.TabPages.Count; i++)
                if (xtraTabControlSalesContent.TabPages[i].ImageOptions.Image!=null)
                {
                    return false;
                }
            return true;
        }

        public void restForeColorOfNavBarItem()
        {
            foreach (NavBarItem item in navBarControl1.Items)
            {
                item.Appearance.ForeColor = Color.Black;
            }
        }

      
    }

    public static class connection
    {
       public static string connectionString = "SERVER=192.168.1.200;DATABASE=cccs;user=Devccc;PASSWORD=rootroot;CHARSET=utf8";
      //   public static string connectionString = "SERVER=localhost;DATABASE=cccLocal;user=root;PASSWORD=root;CHARSET=utf8";

    }
}