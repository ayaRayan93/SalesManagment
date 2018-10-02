using DevExpress.XtraGrid.Views.Grid;
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
    public partial class AdvancedEditForm : EditFormUserControl
    {
        MySqlConnection dbconnection;
        bool loaded = false;
        string Customer_Type;
        DataGridViewRow row;
        private int[] addedRecordIDs;
        int recordCount = 0;
        private bool Added = false;
        int id;

        public AdvancedEditForm()
        {
            InitializeComponent();
            dbconnection = new MySqlConnection(connection.connectionString);
            addedRecordIDs = new int[100];

            this.SetBoundFieldName(txtRequestNum, "رقم الطلب");
            this.SetBoundPropertyName(txtRequestNum, "EditValue");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
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

                query = "select * from factory";
                da = new MySqlDataAdapter(query, dbconnection);
                dt = new DataTable();
                da.Fill(dt);
                comFactory.DataSource = dt;
                comFactory.DisplayMember = dt.Columns["Factory_Name"].ToString();
                comFactory.ValueMember = dt.Columns["Factory_ID"].ToString();
                comFactory.Text = "";

                query = "select * from groupo";
                da = new MySqlDataAdapter(query, dbconnection);
                dt = new DataTable();
                da.Fill(dt);
                comGroup.DataSource = dt;
                comGroup.DisplayMember = dt.Columns["Group_Name"].ToString();
                comGroup.ValueMember = dt.Columns["Group_ID"].ToString();
                comGroup.Text = "";

                query = "select * from product";
                da = new MySqlDataAdapter(query, dbconnection);
                dt = new DataTable();
                da.Fill(dt);
                comProduct.DataSource = dt;
                comProduct.DisplayMember = dt.Columns["Product_Name"].ToString();
                comProduct.ValueMember = dt.Columns["Product_ID"].ToString();
                comProduct.Text = "";

                query = "select * from delegate";
                da = new MySqlDataAdapter(query, dbconnection);
                dt = new DataTable();
                da.Fill(dt);
                comDelegate.DataSource = dt;
                comDelegate.DisplayMember = dt.Columns["Delegate_Name"].ToString();
                comDelegate.ValueMember = dt.Columns["Delegate_ID"].ToString();
                comDelegate.Text = "";

                query = "select * from supplier";
                da = new MySqlDataAdapter(query, dbconnection);
                dt = new DataTable();
                da.Fill(dt);
                comSupplier.DataSource = dt;
                comSupplier.DisplayMember = dt.Columns["Supplier_Name"].ToString();
                comSupplier.ValueMember = dt.Columns["Supplier_ID"].ToString();
                comSupplier.Text = "";
                
                groupBox2.Visible = false;
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
            groupBox2.Visible = true;
            loaded = false;//this is flag to prevent action of SelectedValueChanged event until datasource fill combobox
            try
            {
                if (Customer_Type == "عميل")
                {
                    label3.Visible = false;
                    comEngCon.Visible = false;
                    label9.Visible = true;
                    comCustomer.Visible = true;

                    string query = "select * from customer where Customer_Type='" + Customer_Type + "'";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comCustomer.DataSource = dt;
                    comCustomer.DisplayMember = dt.Columns["Customer_Name"].ToString();
                    comCustomer.ValueMember = dt.Columns["Customer_ID"].ToString();
                    comCustomer.Text = "";
                }
                else
                {
                    label3.Visible = true;
                    comEngCon.Visible = true;
                    label9.Visible = false;
                    comCustomer.Visible = false;


                    string query = "select * from customer where Customer_Type='" + Customer_Type + "'";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comEngCon.DataSource = dt;
                    comEngCon.DisplayMember = dt.Columns["Customer_Name"].ToString();
                    comEngCon.ValueMember = dt.Columns["Customer_ID"].ToString();
                    comEngCon.Text = "";
                }


                loaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }

        //when select customer(مهندس,مقاول)display in comCustomer the all clients of th customer 
        private void comEngCon_SelectedValueChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                label9.Visible = true;
                comCustomer.Visible = true;

                try
                {
                    string query = "select * from customer where Customer_ID in(select Client_ID from custmer_client where Customer_ID=" + comEngCon.SelectedValue + ")";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comCustomer.DataSource = dt;
                    comCustomer.DisplayMember = dt.Columns["Customer_Name"].ToString();
                    comCustomer.ValueMember = dt.Columns["Customer_ID"].ToString();
                    comCustomer.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

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
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            row = (DataGridViewRow)gridView1.GetRow(gridView1.GetSelectedRows()[0]);

            string value = row.Cells[0].Value.ToString();
            txtCode.Text = value;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            double x = 0;
            if (!Added && row != null && double.TryParse(txtPrice.Text,out x))
            {
                addedRecordIDs[recordCount] = gridView1.GetSelectedRows()[0];
                /*gridView1.Rows[gridView1.SelectedCells[0].RowIndex].DefaultCellStyle.BackColor = Color.Silver;
                recordCount++;
                /*int n = gridView2.AddNewRow();*/
                int n=0;
                foreach (DataGridViewColumn item in gridView1.Columns)
                {
                    gridView2.SetRowCellValue(n,item.Index.ToString(), gridView1.GetRowCellDisplayText(row.Index,item.Index.ToString()));

                }
                /*gridView2.Rows[n].Cells[10].Value = txtTotalMeters.Text;
                gridView2.Rows[n].Cells[11].Value = x;*/

                //clear fields
                txtCode.Text = txtPrice.Text = txtTotalMeters.Text = "";
            }
            else
            {
                MessageBox.Show("insert all values with correct format");
                return;
            }
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            int branchId = 0;
            /*&&comBranch.Text!=""&& int.TryParse(txtBranchID.Text,out branchId)&&comStore.Text!=""&&txtStoreID.Text!=""*/
            if ( comDelegate.Text != "" && txtEmployee.Text!="")
            {
                dbconnection.Open();

                int branchBillNumber = 1;
                string query = "select BranchBillNumber from requests where Branch_ID="+branchId+" order by BranchBillNumber desc limit 1";
                MySqlCommand com = new MySqlCommand(query,dbconnection);
                if (com.ExecuteScalar()!= null)
                {
                    branchBillNumber = Convert.ToInt16(com.ExecuteScalar())+1;
                }
                try
                {
                    query = "insert into requests (Supplier_ID,Supplier_Name,Store_ID,Store_Name,Branch_ID,Branch_Name,BranchBillNumber,Customer_ID,Client_ID,Delegate_ID,Employee_Name,Request_Date,Recive_Date,PaidMoney)values(@Supplier_ID,@Supplier_Name,@Store_ID,@Store_Name,@Branch_ID,@Branch_Name,@BranchBillNumber,@Customer_ID,@Client_ID,@Delegate_ID,@Employee_Name,@Request_Date,@Recive_Date,@PaidMoney)";
                    com = new MySqlCommand(query,dbconnection);
                    com.Parameters.Add("@Client_ID", MySqlDbType.Int16);
                    com.Parameters["@Client_ID"].Value = comCustomer.SelectedValue;
                    com.Parameters.Add("@Customer_ID", MySqlDbType.Int16);
                    com.Parameters["@Customer_ID"].Value = comEngCon.SelectedValue;
                    com.Parameters.Add("@Delegate_ID", MySqlDbType.Int16);
                    com.Parameters["@Delegate_ID"].Value = comDelegate.SelectedValue;
                    com.Parameters.Add("@Employee_Name", MySqlDbType.VarChar);
                    com.Parameters["@Employee_Name"].Value = txtEmployee.Text;
                    com.Parameters.Add("@Request_Date", MySqlDbType.Date);
                    com.Parameters["@Request_Date"].Value = dateTimePicker1.Value.Date;             
                    com.Parameters.Add("@Recive_Date", MySqlDbType.Date);
                    com.Parameters["@Recive_Date"].Value = dateTimePicker2.Value.Date;
                    int res2;
                    if (int.TryParse(comSupplier.SelectedValue.ToString(), out res2))
                    {
                        com.Parameters.Add("@Supplier_ID", MySqlDbType.Int16);
                        com.Parameters["@Supplier_ID"].Value = res2;
                    }
                    else
                    {
                        MessageBox.Show("insert correct value please :)");
                        dbconnection.Close();
                        return;
                    }
                    com.Parameters.Add("@Supplier_Name", MySqlDbType.VarChar);
                    com.Parameters["@Supplier_Name"].Value = comSupplier.Text;
                    /*int res1;
                    if (int.TryParse(txtStoreID.Text, out res1))
                    {
                        com.Parameters.Add("@Store_ID", MySqlDbType.Int16);
                        com.Parameters["@Store_ID"].Value = res1;
                    }
                    else
                    {
                        MessageBox.Show("insert correct value please :)");
                        dbconnection.Close();
                        return;
                    }*/
                    com.Parameters.Add("@Store_Name", MySqlDbType.VarChar);
                    /*com.Parameters["@Store_Name"].Value = comStore.Text;
                    int res;*/
            /*if (int.TryParse(txtBranchID.Text, out res))
            {
                com.Parameters.Add("@Branch_ID", MySqlDbType.Int16);
                com.Parameters["@Branch_ID"].Value = res;
            }
            else
            {
                MessageBox.Show("insert correct value please :)");
                dbconnection.Close();
                return;
            }*/
            com.Parameters.Add("@Branch_Name", MySqlDbType.VarChar);
                    /*com.Parameters["@Branch_Name"].Value = comBranch.Text;*/
                    com.Parameters.Add("@BranchBillNumber", MySqlDbType.Int16);
                    com.Parameters["@BranchBillNumber"].Value = branchBillNumber;
                    /*double y = 0;

                    if ( double.TryParse(txtPaidMoney.Text, out y))
                    {
                       
                        com.Parameters.Add("@PaidMoney", MySqlDbType.Double);
                        com.Parameters["@PaidMoney"].Value = y;
                    }
                    else
                    {
                        MessageBox.Show("insert correct value please :)");
                        dbconnection.Close();
                        return;
                    }*/
                    com.ExecuteNonQuery();

                    query = "select Request_ID from requests order by Request_ID desc limit 1";
                    com = new MySqlCommand(query, dbconnection);

                    if (com.ExecuteScalar() != null)
                    {
                        id = (int)com.ExecuteScalar();


                        /*foreach (DataGridViewRow row in dataGridView2.Rows)
                        {

                            query = "insert into request_details (Request_ID,Code,Quantity,Price,Supplier_ID) values (@Request_ID,@Code,@Quantity,@Price,@Supplier_ID)";
                            com = new MySqlCommand(query, dbconnection);

                            com.Parameters.Add("@Request_ID", MySqlDbType.Int16);
                            com.Parameters["@Request_ID"].Value = id;
                            com.Parameters.Add("@Code", MySqlDbType.VarChar);
                            com.Parameters["@Code"].Value = row.Cells["Code"].Value;
                            com.Parameters.Add("@Quantity", MySqlDbType.Double);
                            com.Parameters["@Quantity"].Value = row.Cells["Total_Meter"].Value; 
                            com.Parameters.Add("@Supplier_ID", MySqlDbType.VarChar);
                            com.Parameters["@Supplier_ID"].Value = comSupplier.SelectedValue;
                            com.Parameters.Add("@Price", MySqlDbType.Double);
                            com.Parameters["@Price"].Value = row.Cells["Price"].Value;
                          
                            com.ExecuteNonQuery();
                            
                        }*/
                        MessageBox.Show("Done");
                        dbconnection.Close();

                        //لعرض التقرير
                        /*&& txtPaidMoney.Text != ""*/
                        if (comDelegate.Text != "" && txtEmployee.Text != "" )
                        {
                            DataTable dt = new DataTable();
                            /*for (int i = 0; i < dataGridView2.Columns.Count; i++)
                            {
                                dt.Columns.Add(dataGridView2.Columns[i].Name.ToString());
                            }
                            for (int i = 0; i < dataGridView2.Rows.Count; i++)
                            {
                                DataRow dr = dt.NewRow();
                                for (int j = 0; j < dataGridView2.Columns.Count; j++)
                                {
                                    dr[dataGridView2.Columns[j].Name.ToString()] = dataGridView2.Rows[i].Cells[j].Value;
                                }

                                dt.Rows.Add(dr);
                            }*/

                            if (comCustomer.Text != "")
                            {
                                dbconnection.Open();
                                string q = "select Customer_Phone from customer where Customer_ID=" + comCustomer.SelectedValue.ToString();
                                MySqlCommand comand = new MySqlCommand(q, dbconnection);
                                string phone = comand.ExecuteScalar().ToString();
                                dbconnection.Close();

                                dbconnection.Open();
                                q = "select Customer_Address from customer where Customer_ID=" + comCustomer.SelectedValue.ToString();
                                comand = new MySqlCommand(q, dbconnection);
                                string address = comand.ExecuteScalar().ToString();
                                dbconnection.Close();

                                /*Form1_CrystalReport f = new Form1_CrystalReport(comStore.Text,comBranch.Text, branchBillNumber, comCustomer.Text, phone, address, dateTimePicker1.Value.Date, dateTimePicker2.Value.Date, comDelegate.Text, txtPaidMoney.Text, txtEmployee.Text, dt);
                                f.Show();
                                this.Hide();*/
                            }
                            else if (comEngCon.Text != "")
                            {
                                dbconnection.Open();
                                string q = "select Customer_Phone from customer where Customer_ID=" + comEngCon.SelectedValue.ToString();
                                MySqlCommand comand = new MySqlCommand(q, dbconnection);
                                string phone = comand.ExecuteScalar().ToString();
                                dbconnection.Close();

                                dbconnection.Open();
                                q = "select Customer_Address from customer where Customer_ID=" + comEngCon.SelectedValue.ToString();
                                comand = new MySqlCommand(q, dbconnection);
                                string address = comand.ExecuteScalar().ToString();
                                dbconnection.Close();

                                /*Form1_CrystalReport f = new Form1_CrystalReport(comStore.Text,comBranch.Text, branchBillNumber, comEngCon.Text, phone, address, dateTimePicker1.Value.Date, dateTimePicker2.Value.Date, comDelegate.Text, txtPaidMoney.Text, txtEmployee.Text, dt);
                                f.Show();
                                this.Hide();*/
                            }
                            else
                            {
                                MessageBox.Show("fill requird fields");
                            }
                        }
                        else
                        {
                            MessageBox.Show("fill requird fields");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                dbconnection.Close();
            }
            else
            {
                MessageBox.Show("fill requird fields");
            }
        }

        private void btnMainForm_Click(object sender, EventArgs e)
        {
            /*MainForm f = new MainForm();
            f.Show();
            this.Hide();*/
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comBranch_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (loaded)
                {
                    /*txtBranchID.Text = comBranch.SelectedValue.ToString();*/
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
