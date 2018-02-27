using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Xsl;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

   
        private void button1_Click(object sender, EventArgs e)
        {
            
            
            if ((textBoxBarcode.TextLength > 0) && (textBoxPrice.TextLength > 0))
            {
              
                string[] lines = {
                    "^XSET,CODEPAGE,15",
                    "^Q25,3",
                    "^W40",
                    "^H8",
                    "^P"+numericUpDown.Value.ToString(),
                    "^S3",
                    "^AD",
                    "^C1",
                    "^R8",
                    "~Q-8",
                    "^O0",
                    "^D0",
                    "^E12",
                    "~R255",
                    "^XSET,ROTATION,0",
                    "^L",
                    "Dy2-me-dd",
                    "Th:m:s",
                    "Y223,17,RoundRect1-71",
                    "AE,241,155,1,1,0,3,"+textBoxPrice.Text.ToString()+ "€",
                    "BQ,66,187,3,5,80,3,3,"+textBoxBarcode.Text.ToString(),
                    "AB,185,146,1,1,0,3,25.36.14",
                    "E"
                };
                // WriteAllLines creates a file, writes a collection of strings to the file,
                // and then closes the file.  You do NOT need to call Flush() or Close().

                try
                {
                    System.IO.File.WriteAllLines(@"PrintFiles\\demo.zpl", lines ,Encoding.GetEncoding(1250) );
                    while (true)
                    {
                        if (File.Exists(@"PrintFiles\\demo.zpl"))
                        {
                            System.Diagnostics.Process.Start(@"PrintFiles\\demo.zpl");
                            //den anagorizei to path kai o ektipotis vgazei "file Name not found"
                            break;
                        }
                    }
                }catch(Exception exe)
                {

                }

                



            }
            else
            {
                MessageBox.Show("Τα πεδία barcode και τιμή πρέπει να ναι συμπληρωμένα!! ", "Βιάστηκες!",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }


        public void searchDb()
        {
            SqlConnection con = new SqlConnection("Data Source=LENOVO-PC\\INFLOWSQL;Initial Catalog=inFlow;Integrated Security=True;");
            SqlDataAdapter sda = new SqlDataAdapter("select UnitPrice,Name from dbo.BASE_ItemPrice,dbo.BASE_Product where BarCode = '" + textBoxBarcode.Text + "' and dbo.BASE_ItemPrice.ProdId=dbo.BASE_Product.ProdId;", con);
            DataTable dt = new DataTable();

            sda.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                imgFree.Visible = true;
            }
            else
            {
                string price = dt.Rows[0][0].ToString();
                textBoxPrice.Text = price.Substring(0, price.Length - 3);
                lblName.Text = dt.Rows[0][1].ToString();
            }
        }

        private void textBoxBarcode_TextChanged(object sender, EventArgs e)
        {
            clearAll();
        }

        private void clearAll()
        {
            textBoxPrice.Text = "";
            lblName.Text = "";
            imgFree.Visible = false;
        }


        private void textBoxBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchDb();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }



      
    }
}
