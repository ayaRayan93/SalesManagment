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

namespace Filteration
{
    public partial class Form1 : Form
    {
        MySqlConnection conn;
        string fQuery = "";
        public Form1()
        {
            InitializeComponent();
            conn = new MySqlConnection(connection.connectionString);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = "select * from type";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = dt.Columns["Type_Name"].ToString();
                comboBox1.ValueMember = dt.Columns["Type_ID"].ToString();
                comboBox1.Text = "";
                textBox1.Text = "";

                query = "select * from factory";
                da = new MySqlDataAdapter(query, conn);
                dt = new DataTable();
                da.Fill(dt);
                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = dt.Columns["Factory_Name"].ToString();
                comboBox2.ValueMember = dt.Columns["Factory_ID"].ToString();
                comboBox2.Text = "";
                textBox2.Text = "";

                query = "select * from groupo";
                da = new MySqlDataAdapter(query, conn);
                dt = new DataTable();
                da.Fill(dt);
                comboBox4.DataSource = dt;
                comboBox4.DisplayMember = dt.Columns["Group_Name"].ToString();
                comboBox4.ValueMember = dt.Columns["Group_ID"].ToString();
                comboBox4.Text = "";
                textBox4.Text = "";

                query = "select * from product";
                da = new MySqlDataAdapter(query, conn);
                dt = new DataTable();
                da.Fill(dt);
                comboBox3.DataSource = dt;
                comboBox3.DisplayMember = dt.Columns["Product_Name"].ToString();
                comboBox3.ValueMember = dt.Columns["Product_ID"].ToString();
                comboBox3.Text = "";
                textBox3.Text = "";

               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox1.SelectedValue.ToString();
        }
        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = comboBox2.SelectedValue.ToString();
        }
        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            textBox3.Text = comboBox3.SelectedValue.ToString();
        }
        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            textBox4.Text = comboBox4.SelectedValue.ToString();
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {

                    if (textBox1.Text != "")
                    {
                        conn.Open();
                        string query = "select Type_Name from type where Type_ID='" + textBox1.Text + "'";
                        MySqlCommand com = new MySqlCommand(query, conn);
                        string TypeName = (string)com.ExecuteScalar();
                        comboBox1.Text = TypeName;
                        textBox2.Focus();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                conn.Close();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {

                    if (textBox2.Text != "")
                    {
                        conn.Open();
                        string query = "select Factory_Name from factory where Factory_ID='" + textBox2.Text + "'";
                        MySqlCommand com = new MySqlCommand(query, conn);
                        string TypeName = (string)com.ExecuteScalar();
                        comboBox2.Text = TypeName;
                        textBox4.Focus();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                conn.Close();
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {

                    if (textBox4.Text != "")
                    {
                        conn.Open();
                        string query = "select Group_Name from groupo where Group_ID='" + textBox4.Text + "'";
                        MySqlCommand com = new MySqlCommand(query, conn);
                        string TypeName = (string)com.ExecuteScalar();
                        comboBox4.Text = TypeName;
                        textBox3.Focus();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                conn.Close();
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {

                    if (textBox3.Text != "")
                    {
                        conn.Open();
                        string query = "select Product_Name from product where Product_ID='" + textBox3.Text + "'";
                        MySqlCommand com = new MySqlCommand(query, conn);
                        string TypeName = (string)com.ExecuteScalar();
                        comboBox3.Text = TypeName;
                        textBox1.Focus();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string q1, q2, q3, q4;
            if (textBox1.Text == "")
            {
                q1 = "select Type_ID from data";
            }
            else
            {
                q1 = textBox1.Text;
            }
            if (textBox2.Text == "")
            {
                q2 = "select Factory_ID from data";
            }
            else
            {
                q2 = textBox2.Text;
            }
            if (textBox3.Text == "")
            {
                q3 = "select Product_ID from data";
            }
            else
            {
                q3 = textBox3.Text;
            }
            if (textBox4.Text == "")
            {
                q4 = "select Group_ID from data";
            }
            else
            {
                q4 = textBox4.Text;
            }
            if (textBox5.Text != "")
            {
                fQuery += "and data.Size='" + textBox5.Text + "'";
            }
            if (textBox6.Text != "")
            {
                fQuery += "and data.Classification='" + textBox6.Text + "'";
            }
            if (textBox7.Text != "")
            {
                fQuery += "and data.Colour='" + textBox7.Text + "'";
            }
            if (textBox8.Text != "")
            {
                fQuery += "and data.Sort='" + textBox8.Text + "'";
            }
           
            string query = "select data.Code as 'كود' , type.Type_Name as 'النوع', factory.Factory_Name as 'المصنع' ,groupo.Group_Name as 'المجموعة', product.Product_Name as 'المنتج' , data.Colour as 'لون', data.Size as 'حجم', data.Sort as 'الفرز',data.Classification as 'التصنيف', data.Description as 'الوصف' from data INNER JOIN type ON type.Type_ID = data.Type_ID INNER JOIN product ON product.Product_ID = data.Product_ID INNER JOIN factory ON data.Factory_ID = factory.Factory_ID INNER JOIN groupo ON data.Group_ID = groupo.Group_ID   where data.Type_ID IN(" + q1 + ") and  data.Factory_ID  IN(" + q2 + ") and data.Group_ID IN (" + q4 + ") and data.Product_ID IN (" + q3 + ")"+fQuery;
           
            MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            fQuery = "";
           
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
              
                textBox6.Focus();
            }
        }
        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               
                textBox7.Focus();
            }
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               
                textBox8.Focus();
            }
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox5.Focus();
            }
        }

        
    }
    public static class connection
    {
        public static string connectionString = "SERVER=192.168.1.200;DATABASE=ccc;user=Devccc;PASSWORD=rootroot;CHARSET=utf8";
    }

       
    }
