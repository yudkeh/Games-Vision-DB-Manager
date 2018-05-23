using DB_Manager.classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_Manager
{
    public partial class Form1 : Form
    {
        private OpenFileDialog _fDialog;
        private String _filePath;
        private dal d;
        private String[] file;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void howToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This App created by: Yehuda Hadad\nfor more information and sources, please contact:\nyuda098q@gmail.com\nor visit my GitHub page:\nhttps://github.com/yudkeh", "About");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void serverSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            file = new String[5];
        }

        private void saveDbBtn_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    file[0] = "dbUrl=" + txtDbUrl.Text;
                    file[1] = "dbPort=" + txtPort.Text;
                    file[2] = "dbName=" + txtDbName.Text;
                    file[3] = "dbUsername=" + txtDbUser.Text;
                    file[4] = "dbPass=" + txtDbPass.Text;
                    System.IO.File.WriteAllLines(@"data\settings.config", file);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Can't save config file.");
                    Console.Write(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Please make sure that the values have been inserted as need.", "Error");
            }

        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            _fDialog = new OpenFileDialog();
            _fDialog.Title = "Choose File";
            DialogResult result = _fDialog.ShowDialog(); // Show the dialog.

            if (result == DialogResult.OK) // Test result.
            {
                _filePath = _fDialog.FileName;  //inserting the file path to the String
                try
                {
                    txtfilePath.Text = _filePath.ToString();    //showing thr file path
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.ToString());  //For debugging use.
                }
            }
            Console.WriteLine(result);  //For debugging use.
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            d = new dal();

            if (ValidateForm())
            {
                FillDal();

                if(txtfilePath.Text.Trim() != String.Empty)
                {

                }
                else
                {
                    MessageBox.Show("Please select file.", "Error");

                }
            }
            else
            {
                MessageBox.Show("Please make sure that the values have been inserted as need.", "Error");
            }
        }

        //validating the form texts
        private Boolean ValidateForm()
        {
            Boolean validated = true;
            int myInt;
            if (txtDbName.Text.Trim() == String.Empty)
                validated = false;
            else if(txtDbPass.Text.Trim()==String.Empty)
                validated = false;
            else if (txtDbUrl.Text.Trim() == String.Empty)
                validated = false;
            else if (txtDbUser.Text.Trim() == String.Empty)
                validated = false;
            else if (txtPort.Text.Trim() == String.Empty)
                validated = false;

            if(validated)
            {
                validated = int.TryParse(txtPort.Text, out myInt);
            }

            return validated;
        }

        //inserting values from the form to the dal object and generating the connection string
        private void FillDal()
        {
            d.server = txtDbUrl.Text;
            d.port = txtPort.Text;
            d.password = txtDbPass.Text;
            d.uid = txtDbUser.Text;
            d.database = txtDbName.Text;
            d.SetConnectionString();
        }
    }
}
