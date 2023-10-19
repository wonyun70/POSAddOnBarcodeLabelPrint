using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Xml;
using System.Drawing.Printing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using BarcodeStandard;
using Type = BarcodeStandard.Type;

// ** https://www.codeguru.com/dotnet/creating-a-most-recently-used-menu-list-in-net/

namespace POSAddOnBarcodeLabelPrint
{
    public partial class frmMain : Form
    {


        private Queue<string> MRUlist = new Queue<string>();
        public string connectionString;
        public string CurrentFileName;
        Barcode _b = new Barcode();


        private void SaveRecentFile(string strPath)
        {
            recentToolStripMenuItem.DropDownItems.Clear();

            LoadRecentList();

            if (!(MRUlist.Contains(strPath)))

                MRUlist.Enqueue(strPath);

            while (MRUlist.Count > 5)

                MRUlist.Dequeue();

            foreach (string strItem in MRUlist)
            {
                ToolStripMenuItem tsRecent = new
                   ToolStripMenuItem(strItem, null, RecentFileClickEvent);

                recentToolStripMenuItem.DropDownItems.Add(tsRecent);
            }

            StreamWriter stringToWrite = new
               StreamWriter(System.Environment.CurrentDirectory +
               @"\Recent.txt");

            foreach (string item in MRUlist)

                stringToWrite.WriteLine(item);

            stringToWrite.Flush();

            stringToWrite.Close();
        }

        private void LoadRecentList()
        {
            MRUlist.Clear();

            try
            {
                StreamReader srStream = new StreamReader
                   (Environment.CurrentDirectory + @"\Recent.txt");

                string strLine = "";

                while ((InlineAssignHelper(ref strLine,
                      srStream.ReadLine())) != null)

                    MRUlist.Enqueue(strLine);

                srStream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        private static T InlineAssignHelper<T>(ref T target, T value)
        {
            target = value;
            return value;
        }
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadRecentList();

            foreach (string item in MRUlist)
            {
                ToolStripMenuItem fileRecent = new ToolStripMenuItem(item, null, RecentFileClickEvent);
                //fileRecent.Click += EventHandler(RecentFileClickEvent);
                recentToolStripMenuItem.DropDownItems.Add(fileRecent);
            }

            //string barcodeType = @"UpcA, UpcE, UpcSupplemental2Digit, UpcSupplemental5Digit, Ean13, Ean8, Interleaved2Of5, Interleaved2Of5Mod10, Standard2Of5, Standard2Of5Mod10, Industrial2Of5, Industrial2Of5Mod10, Code39, Code39Extended, Code39Mod43, Codabar, PostNet, Bookland, Isbn, Jan13, MsiMod10, Msi2Mod10, MsiMod11, MsiMod11Mod10, ModifiedPlessey, Code11, Usd8, Ucc12, Ucc13, Logmars, Code128, Code128A, Code128B, Code128C, Itf14, Code93, Telepen, Fim, Pharmacode";            
            //cboBacodeType.DataSource = barcodeType.Split(',');
            BindingSource bs = new BindingSource();
            bs.DataSource = new List<string> { "Left", "Center", "Right" };
            cboNameAlignment.DataSource = bs;
            BindingSource bsPrice = new BindingSource();
            bsPrice.DataSource = new List<string> { "Left", "Center", "Right" };
            cboPriceAlignment.DataSource = bsPrice;
        }
        private void RecentFileClickEvent(object sender, EventArgs e)
        {
            loadXml((sender as ToolStripMenuItem).Text);
        }

        private void loadXml(string xmlFile)
        {
            try
            {

                //  Create an Xml document instance and load XML data.
                XmlDocument doc = new XmlDocument();
                //string xmlFile = (Directory.GetCurrentDirectory() + "\\DBServer.xml");
                if (File.Exists(xmlFile))
                {
                    CurrentFileName = xmlFile;

                    doc.Load(xmlFile);
                    XmlNode node = doc.SelectSingleNode("//Company");
                    txtServerName.Text = node.SelectSingleNode("SERVERNAME").InnerText;
                    txtDBName.Text = node.SelectSingleNode("DBNAME").InnerText;
                    txtUserName.Text = node.SelectSingleNode("USERNAME").InnerText;
                    txtPassword.Text = node.SelectSingleNode("PASSWORD").InnerText;
                    txtTableName.Text = node.SelectSingleNode("TABLENAME").InnerText;
                    txtColumnName.Text = node.SelectSingleNode("NAME").InnerText;
                    txtColumnPrice.Text = node.SelectSingleNode("PRICE").InnerText;
                    txtColumnBarcode.Text = node.SelectSingleNode("BARCODE").InnerText;
                    txtPrinter.Text = node.SelectSingleNode("PRINTER").InnerText;
                    txtPaperSizeWidth.Text = node.SelectSingleNode("PAPERWIDTH").InnerText;
                    txtPaperSizeHeight.Text = node.SelectSingleNode("PAPERHEIGHT").InnerText;
                    txtPaperTopMargin.Text = node.SelectSingleNode("PAPERTOP").InnerText;
                    txtPaperLeftMargin.Text = node.SelectSingleNode("PAPERLEFT").InnerText;
                    ckLandscape.Checked = Convert.ToBoolean(node.SelectSingleNode("LANDSCAPE").InnerText);
                    cboBacodeType.SelectedIndex = cboBacodeType.FindStringExact(node.SelectSingleNode("BARCODETYPE").InnerText);
                    txtBarcodeWidth.Text = node.SelectSingleNode("BARCODEWIDTH").InnerText;
                    txtBarcodeHeight.Text = node.SelectSingleNode("BARCODEHEIGHT").InnerText;
                    txtBarcodeX.Text = node.SelectSingleNode("BARCODEX").InnerText;
                    txtBarcodeY.Text = node.SelectSingleNode("BARCODEY").InnerText;
                    txtBarwidth.Text = node.SelectSingleNode("BARWIDTH").InnerText;
                    ckIncludeLabel.Checked = Convert.ToBoolean(node.SelectSingleNode("INCLUDELABEL").InnerText);
                    txtBarcodeLabelFont.Text = node.SelectSingleNode("BARCODELABELFONT").InnerText;

                    txtNameFont.Text = node.SelectSingleNode("NameFont").InnerText;
                    txtNameFontSize.Text = node.SelectSingleNode("NameFontSize").InnerText;
                    ckNameBold.Checked = Convert.ToBoolean(node.SelectSingleNode("NameFontBold").InnerText);
                    txtNameX.Text = node.SelectSingleNode("NameX").InnerText;
                    txtNameY.Text = node.SelectSingleNode("NameY").InnerText;
                    cboNameAlignment.SelectedIndex = cboNameAlignment.FindStringExact(node.SelectSingleNode("NameAlignment").InnerText);
                    txtPriceFont.Text = node.SelectSingleNode("PriceFont").InnerText;
                    txtPriceFontSize.Text = node.SelectSingleNode("PriceFontSize").InnerText;
                    ckPriceBold.Checked = Convert.ToBoolean(node.SelectSingleNode("PriceFontBold").InnerText);
                    txtPriceX.Text = node.SelectSingleNode("PriceX").InnerText;
                    txtPriceY.Text = node.SelectSingleNode("PriceY").InnerText;
                    cboPriceAlignment.SelectedIndex = cboPriceAlignment.FindStringExact(node.SelectSingleNode("PriceAlignment").InnerText);


                    // doc.Save(Path)
                    //SaveRecentFile(CurrentFileName); //Save RecentFile Path

                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show(("Data Connection Error." + ex.Errors), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //  System.Threading.Thread.CurrentThread.Abort()
            }
        }
        private void SaveToXML(string Path)
        {

            try
            {
                // Create an Xml document instance and load XML data.
                XmlDocument doc = new XmlDocument();
                //string Path = Directory.GetCurrentDirectory() + @"\DBServer.xml";
                bool filenameExists;

                filenameExists = File.Exists(Path);
                if (filenameExists)
                    File.Delete(Path);
                if (true)
                {
                    XmlTextWriter createFile = new XmlTextWriter(Path, Encoding.Unicode);
                    createFile.WriteStartDocument(false);
                    // createFile.WriteDocType("Invoice", Nothing, Nothing, Nothing)
                    createFile.WriteComment("This file represents POSAddOn Barcode Label Printer config info");
                    createFile.WriteStartElement("POSAddOn");
                    createFile.Flush();
                    createFile.Close();

                    doc.Load(Path);
                    XmlElement newElem = doc.CreateElement("Company");

                    XmlAttribute newAttr = doc.CreateAttribute("info");
                    newAttr.Value = "POSAddOn.com";
                    newElem.Attributes.Append(newAttr);

                    // Create the child nodes. The following example shows various ways to add child nodes.
                    string strInnerXml;
                    strInnerXml = "<SERVERNAME></SERVERNAME>";
                    strInnerXml = strInnerXml + "<DBNAME></DBNAME>";
                    strInnerXml = strInnerXml + "<USERNAME></USERNAME>";
                    strInnerXml = strInnerXml + "<PASSWORD></PASSWORD>";
                    strInnerXml = strInnerXml + "<PRINTER></PRINTER>";
                    strInnerXml = strInnerXml + "<TABLENAME></TABLENAME>";
                    strInnerXml = strInnerXml + "<NAME></NAME>";
                    strInnerXml = strInnerXml + "<PRICE></PRICE>";
                    strInnerXml = strInnerXml + "<BARCODE></BARCODE>";
                    strInnerXml = strInnerXml + "<PAPERWIDTH></PAPERWIDTH>";
                    strInnerXml = strInnerXml + "<PAPERHEIGHT></PAPERHEIGHT>";
                    strInnerXml = strInnerXml + "<PAPERTOP></PAPERTOP>";
                    strInnerXml = strInnerXml + "<PAPERLEFT></PAPERLEFT>";
                    strInnerXml = strInnerXml + "<LANDSCAPE></LANDSCAPE>";
                    strInnerXml = strInnerXml + "<BARCODETYPE></BARCODETYPE>";
                    strInnerXml = strInnerXml + "<BARCODEWIDTH></BARCODEWIDTH>";
                    strInnerXml = strInnerXml + "<BARCODEHEIGHT></BARCODEHEIGHT>";
                    strInnerXml = strInnerXml + "<BARCODEX></BARCODEX>";
                    strInnerXml = strInnerXml + "<BARCODEY></BARCODEY>";
                    strInnerXml = strInnerXml + "<BARWIDTH></BARWIDTH>";
                    strInnerXml = strInnerXml + "<INCLUDELABEL></INCLUDELABEL>";
                    strInnerXml = strInnerXml + "<BARCODELABELFONT></BARCODELABELFONT>";
                    strInnerXml = strInnerXml + "<NameFont></NameFont>";
                    strInnerXml = strInnerXml + "<NameFontSize></NameFontSize>";
                    strInnerXml = strInnerXml + "<NameFontBold></NameFontBold>";
                    strInnerXml = strInnerXml + "<NameX></NameX>";
                    strInnerXml = strInnerXml + "<NameY></NameY>";
                    strInnerXml = strInnerXml + "<NameAlignment></NameAlignment>";
                    strInnerXml = strInnerXml + "<PriceFont></PriceFont>";
                    strInnerXml = strInnerXml + "<PriceFontSize></PriceFontSize>";
                    strInnerXml = strInnerXml + "<PriceFontBold></PriceFontBold>";
                    strInnerXml = strInnerXml + "<PriceX></PriceX>";
                    strInnerXml = strInnerXml + "<PriceY></PriceY>";
                    strInnerXml = strInnerXml + "<PriceAlignment></PriceAlignment>";
                    strInnerXml = strInnerXml + "<Help></Help>";

                    newElem.InnerXml = strInnerXml;

                    doc.DocumentElement.AppendChild(newElem);
                    doc.PreserveWhitespace = true;
                    XmlTextWriter wrtr = new XmlTextWriter(Path, Encoding.Unicode);
                    doc.WriteTo(wrtr);
                    // doc.WriteTo(CryptoZ.Encrypt(strInnerXml, "MyKey"))
                    wrtr.Close();
                }
                doc.Load(Path);
                XmlNode node = doc.SelectSingleNode("//Company");


                node.SelectSingleNode("SERVERNAME").InnerText = txtServerName.Text.Trim();
                node.SelectSingleNode("DBNAME").InnerText = txtDBName.Text.Trim();
                node.SelectSingleNode("USERNAME").InnerText = txtUserName.Text.Trim();
                node.SelectSingleNode("PASSWORD").InnerText = txtPassword.Text.Trim();
                node.SelectSingleNode("TABLENAME").InnerText = txtTableName.Text.Trim();
                node.SelectSingleNode("NAME").InnerText = txtColumnName.Text.Trim();
                node.SelectSingleNode("PRICE").InnerText = txtColumnPrice.Text.Trim();
                node.SelectSingleNode("BARCODE").InnerText = txtColumnBarcode.Text.Trim();
                node.SelectSingleNode("PRINTER").InnerText = txtPrinter.Text.Trim();
                node.SelectSingleNode("PAPERWIDTH").InnerText = txtPaperSizeWidth.Text.Trim();
                node.SelectSingleNode("PAPERHEIGHT").InnerText = txtPaperSizeHeight.Text.Trim();
                node.SelectSingleNode("PAPERTOP").InnerText = txtPaperTopMargin.Text.Trim();
                node.SelectSingleNode("PAPERLEFT").InnerText = txtPaperLeftMargin.Text.Trim();
                node.SelectSingleNode("LANDSCAPE").InnerText = ckLandscape.Checked.ToString();
                node.SelectSingleNode("BARCODETYPE").InnerText = cboBacodeType.SelectedItem.ToString();
                node.SelectSingleNode("BARCODEWIDTH").InnerText = txtBarcodeWidth.Text.ToString();
                node.SelectSingleNode("BARCODEHEIGHT").InnerText = txtBarcodeHeight.Text.ToString();
                node.SelectSingleNode("BARCODEX").InnerText = txtBarcodeX.Text.ToString();
                node.SelectSingleNode("BARCODEY").InnerText = txtBarcodeY.Text.ToString();
                node.SelectSingleNode("BARWIDTH").InnerText = txtBarwidth.Text.ToString();
                node.SelectSingleNode("INCLUDELABEL").InnerText = ckIncludeLabel.Checked.ToString();
                node.SelectSingleNode("BARCODELABELFONT").InnerText = txtBarcodeLabelFont.Text.ToString();
                node.SelectSingleNode("NameFont").InnerText = txtNameFont.Text.ToString();
                node.SelectSingleNode("NameFontSize").InnerText = txtNameFontSize.Text.ToString();
                node.SelectSingleNode("NameFontBold").InnerText = ckNameBold.Checked.ToString();
                node.SelectSingleNode("NameX").InnerText = txtNameX.Text.ToString();
                node.SelectSingleNode("NameY").InnerText = txtNameY.Text.ToString();
                node.SelectSingleNode("NameAlignment").InnerText = cboNameAlignment.SelectedItem.ToString();
                node.SelectSingleNode("PriceFont").InnerText = txtPriceFont.Text.ToString();
                node.SelectSingleNode("PriceFontSize").InnerText = txtPriceFontSize.Text.ToString();
                node.SelectSingleNode("PriceFontBold").InnerText = ckPriceBold.Checked.ToString();
                node.SelectSingleNode("PriceX").InnerText = txtPriceX.Text.ToString();
                node.SelectSingleNode("PriceY").InnerText = txtPriceY.Text.ToString();
                node.SelectSingleNode("PriceAlignment").InnerText = cboPriceAlignment.SelectedItem.ToString();

                doc.Save(Path);
                SaveRecentFile(Path); //Save RecentFile Path
            }
            catch (XmlException xmlex)
            {
                MessageBox.Show(xmlex.Message);
            }
            // UserLoad_From_SQL = False
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void IsServerConnected()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("SQL Connection successful.");
                }
                catch (SqlException)
                {
                    MessageBox.Show("Connection fail");
                }
            }
        }
        private void btnConnectionTest_Click(object sender, EventArgs e)
        {
            createConnectionString();
            IsServerConnected();
        }
        private void createConnectionString()
        {
            connectionString = @"Data Source=" + txtServerName.Text + @";Initial Catalog=" + txtDBName.Text.Trim() + ";Persist Security Info=True;User ID=" + txtUserName.Text.Trim() + @";Password=" + txtPassword.Text.Trim() + @";";
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "XML File|*.xml";
            openFileDialog1.Title = "Open a config file";
            openFileDialog1.InitialDirectory = System.Environment.CurrentDirectory;
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != null)
            {
                loadXml(openFileDialog1.FileName);
                createConnectionString();
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentFileName == null || CurrentFileName == "")
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                SaveToXML(CurrentFileName);
            }

        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "XML File|*.xml";
            saveFileDialog1.Title = "Save as a config file";
            saveFileDialog1.InitialDirectory = System.Environment.CurrentDirectory;
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                CurrentFileName = saveFileDialog1.FileName;
                SaveToXML(CurrentFileName);
            }
        }

        private void selectList(System.Windows.Forms.TextBox txBox)
        {
            if (txtTableName.Text == "" && txBox.Name != "txtTableName")
            {
                MessageBox.Show("Please select table first!!");

            }
            else
            {
                if (txBox.Name == "txtTableName")
                {
                    txtTableName.Text = "";
                }
                using (frmColumnList form = new frmColumnList())
                {
                    if (connectionString == null || connectionString == "")
                    {
                        createConnectionString();
                    }
                    form.connectionString = connectionString;
                    form.strTableName = txtTableName.Text;
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        string val = form.ReturnValue;            //values preserved after close
                        txBox.Text = val;
                    }
                }
            }

        }
        private void btnTable_Click(object sender, EventArgs e)
        {
            selectList(txtTableName);
        }

        private void btnTitle_Click(object sender, EventArgs e)
        {
            selectList(txtColumnName);
        }

        private void btnPrice_Click(object sender, EventArgs e)
        {
            selectList(txtColumnPrice);
        }

        private void btnBarcode_Click(object sender, EventArgs e)
        {
            selectList(txtColumnBarcode);
        }

        private void btnPrinter_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog1 = new PrintDialog();
            var result = printDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtPrinter.Text = printDialog1.PrinterSettings.PrinterName;
            }
        }

        private void btnBarcodeLabelFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog1 = new FontDialog();
            var result = fontDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                txtBarcodeLabelFont.Text = fontDialog1.Font.Name.ToString();
            }
        }

        private void btnNameFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog1 = new FontDialog();
            var result = fontDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtNameFont.Text = fontDialog1.Font.Name.ToString();
                txtNameFontSize.Text = fontDialog1.Font.Size.ToString();
                ckNameBold.Checked = fontDialog1.Font.Bold;
            }
        }

        private void btnPriceFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog1 = new FontDialog();
            var result = fontDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtPriceFont.Text = fontDialog1.Font.Name.ToString();
                txtPriceFontSize.Text = fontDialog1.Font.Size.ToString();
                ckPriceBold.Checked = fontDialog1.Font.Bold;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                int PaperWidth = 0, PaperHeight = 0;
                PaperWidth= (int)(double.Parse(txtPaperSizeWidth.Text)*100);
                PaperHeight= (int)(double.Parse(txtPaperSizeHeight.Text)*100);
                Bitmap b = new Bitmap(PaperWidth, PaperHeight);
                Graphics g = Graphics.FromImage(b);
                g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, PaperWidth, PaperHeight)); // i used this code to make the background color white

                //Draw Barcode
                var barCode = new Barcode();
                barCode.IncludeLabel = ckIncludeLabel.Checked;
                barCode.Alignment = AlignmentPositions.Center;
                //BarcodeLib barcodeType = cboBacodeType.SelectedItem.ToString();
                Image imageBarcode = Image.FromStream(barCode.Encode(GetTypeSelected(), txtDataBarcode.Text.Trim(), _b.ForeColor, _b.BackColor,500,150).Encode().AsStream());
                g.DrawImage(imageBarcode, Single.Parse(txtBarcodeX.Text), Single.Parse(txtBarcodeY.Text),int.Parse(txtBarcodeWidth.Text), int.Parse(txtBarcodeHeight.Text));

                //Draw Name
                Font NameFont = new Font(txtNameFont.Text, Single.Parse(txtNameFontSize.Text));
                if (ckNameBold.Checked)
                {
                    NameFont = new Font(txtNameFont.Text, Single.Parse(txtNameFontSize.Text), FontStyle.Bold);
                }
                g.DrawString(txtDataname.Text.ToString(), NameFont, new SolidBrush(Color.Black), new PointF(Single.Parse(txtNameX.Text), Single.Parse(txtNameY.Text)));


                //Draw Price
                Font PriceFont = new Font(txtPriceFont.Text, Single.Parse(txtPriceFontSize.Text));
                if (ckPriceBold.Checked)
                {
                    PriceFont = new Font(txtPriceFont.Text, Single.Parse(txtPriceFontSize.Text), FontStyle.Bold);
                }
                g.DrawString(txtDataPrice.Text.ToString(), PriceFont, new SolidBrush(Color.Black), new PointF(Single.Parse(txtPriceX.Text), Single.Parse(txtPriceY.Text)));


                

                pbBacodeLabel.Image = b;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private Type GetTypeSelected()
        {
            var type = Type.Unspecified;
            string barcodeType = cboBacodeType.SelectedItem.ToString().Trim();
            switch (barcodeType)
            {
                case "UPC-A": type = Type.UpcA; break;
                case "UPC-E": type = Type.UpcE; break;
                case "UPC 2 Digit Ext.": type = Type.UpcSupplemental2Digit; break;
                case "UPC 5 Digit Ext.": type = Type.UpcSupplemental5Digit; break;
                case "EAN-13": type = Type.Ean13; break;
                case "JAN-13": type = Type.Jan13; break;
                case "EAN-8": type = Type.Ean8; break;
                case "ITF-14": type = Type.Itf14; break;
                case "Codabar": type = Type.Codabar; break;
                case "PostNet": type = Type.PostNet; break;
                case "Bookland/ISBN": type = Type.Bookland; break;
                case "Code 11": type = Type.Code11; break;
                case "Code 39": type = Type.Code39; break;
                case "Code 39 Extended": type = Type.Code39Extended; break;
                case "Code 39 Mod 43": type = Type.Code39Mod43; break;
                case "Code 93": type = Type.Code93; break;
                case "LOGMARS": type = Type.Logmars; break;
                case "MSI Mod 10": type = Type.MsiMod10; break;
                case "MSI Mod 11": type = Type.MsiMod11; break;
                case "MSI 2 Mod 10": type = Type.Msi2Mod10; break;
                case "MSI Mod 11 Mod 10": type = Type.MsiMod11Mod10; break;
                case "Interleaved 2 of 5": type = Type.Interleaved2Of5; break;
                case "Interleaved 2 of 5 Mod 10": type = Type.Interleaved2Of5Mod10; break;
                case "Standard 2 of 5": type = Type.Standard2Of5; break;
                case "Standard 2 of 5 Mod 10": type = Type.Standard2Of5Mod10; break;
                case "Code 128": type = Type.Code128; break;
                case "Code 128-A": type = Type.Code128A; break;
                case "Code 128-B": type = Type.Code128B; break;
                case "Code 128-C": type = Type.Code128C; break;
                case "Telepen": type = Type.Telepen; break;
                case "FIM": type = Type.Fim; break;
                case "Pharmacode": type = Type.Pharmacode; break;
                default: MessageBox.Show(@"Please specify the encoding type."); break;
            }//switch

            return type;
        }
    }

    
}
