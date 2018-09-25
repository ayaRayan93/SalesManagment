﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;
using DevExpress.XtraTab;

namespace SalesManagment
{
    public partial class QuantityUpdate : DevExpress.XtraEditors.XtraForm
    {
        int rowHandel = 0;
        DataRowView selRow;
        MySqlConnection dbconnection;

        public QuantityUpdate(int rowhandel, DataRowView Selrow)
        {
            InitializeComponent();
            dbconnection = new MySqlConnection(connection.connectionString);
            rowHandel = rowhandel;
            selRow = Selrow;
        }

        private void QuantityUpdate_Load(object sender, EventArgs e)
        {
            try
            {
                dbconnection.Open();
                string query = "select * from store";
                MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                comStore.DataSource = dt;
                comStore.DisplayMember = dt.Columns["Store_Name"].ToString();
                comStore.ValueMember = dt.Columns["Store_ID"].ToString();
                comStore.Text = selRow["المخزن"].ToString();

                txtQuantity.Text = selRow["الكمية"].ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtQuantity.Text != "" && comStore.Text != "")
                {
                    double quantity = 0;
                    if (double.TryParse(txtQuantity.Text, out quantity))
                    {
                        dbconnection.Open();
                        if (!checkQuantityInStore())
                        {
                            MessageBox.Show("لا يوجد كمية كافية من العنصر فى المخزن");
                            dbconnection.Close();
                            return;
                        }

                        if (selRow["الكود"].ToString().Length >= 20)
                        {
                            if (cartonNumCheck())
                            { }
                            else
                            {
                                dbconnection.Close();
                                return;
                            }
                        }

                        dbconnection.Close();
                        Main.BillConfirm.refreshView(rowHandel, quantity, Convert.ToInt16(comStore.SelectedValue.ToString()), comStore.Text);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("الكمية يجب ان تكون عدد");
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
        }

        //functions
        public bool cartonNumCheck()
        {
            try
            {
                string query = "select Carton from data where Code='" + selRow["الكود"].ToString() + "'";
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

        bool checkQuantityInStore()
        {
            double totalMeter = Convert.ToDouble(txtQuantity.Text);
            if (selRow["النوع"].ToString() == "بند")
            {
                string query = "SELECT sum(storage.Total_Meters) as 'الكمية' FROM storage where storage.Store_ID=" + comStore.SelectedValue.ToString() + " and storage.Data_ID=" + selRow["التسلسل"].ToString() + " and storage.Type='بند' group by storage.Data_ID,storage.Store_ID";
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
            else if (selRow["النوع"].ToString() == "طقم")
            {
                string query = "SELECT sum(storage.Total_Meters) as 'الكمية' FROM storage where storage.Store_ID=" + comStore.SelectedValue.ToString() + " and storage.Set_ID=" + selRow["التسلسل"].ToString() + " and storage.Type='طقم' group by storage.Set_ID,storage.Store_ID";
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
            else if (selRow["النوع"].ToString() == "عرض")
            {
                string query = "SELECT sum(storage.Total_Meters) as 'الكمية' FROM storage where storage.Store_ID=" + comStore.SelectedValue.ToString() + " and storage.Offer_ID=" + selRow["التسلسل"].ToString() + " and storage.Type='عرض' group by storage.Offer_ID,storage.Store_ID";
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
            return false;
        }
    }
}