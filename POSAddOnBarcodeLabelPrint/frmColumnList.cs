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

namespace POSAddOnBarcodeLabelPrint
{
    public partial class frmColumnList : Form
    {
        public string connectionString;
        public string strTableName;
        public string ReturnValue { get; set; }
        public frmColumnList()
        {
            InitializeComponent();
        }

        private void frmColumnList_Load(object sender, EventArgs e)
        {
            try
            {
                string strSQLString;
                // Set up the DataGridView.
                {
                    var withBlock = this.dataGridView1;
                    // Automatically generate the DataGridView columns.
                    withBlock.AutoGenerateColumns = true;
                    if (strTableName == null || strTableName =="")
                    {
                        strSQLString = " SELECT name FROM sys.tables";
                    }
                    else
                    {
                        strSQLString = @"select COLUMN_NAME from information_schema.columns where table_name = '"+ strTableName + @"'";
                    }

                    bindingSource1.DataSource = GetData(strSQLString);
                    withBlock.DataSource = bindingSource1;

                    // Automatically resize the visible rows.
                    withBlock.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

                    // Set the DataGridView control's border.
                    withBlock.BorderStyle = BorderStyle.Fixed3D;

                    withBlock.ReadOnly = true;
                }


            }


            catch (SqlException ex)
            {
                MessageBox.Show("Comcash Data Connection Error." + ex.Errors, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        public DataTable GetData(string sqlCommand)
        {
            SqlConnection ComcashConnection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand(sqlCommand, ComcashConnection);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = command;

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            adapter.Fill(table);

            return table;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            //this.ReturnValue = dataGridView1.SelectedCells[0].Value.ToString();
            ////this.ReturnValue = dataGridView1.SelectedCells[1].Value.ToString();
            //this.DialogResult = DialogResult.OK;
            //this.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                this.ReturnValue = dataGridView1.SelectedCells[0].Value.ToString();
                //this.ReturnValue = dataGridView1.SelectedCells[1].Value.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            
        }
    }
}
