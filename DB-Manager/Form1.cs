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

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void HowToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Currently the application only support MySQL as the DB.\nIn order to push the data to the DB, please enter the DB inforamtion and select the excel file, afterwards, just click on the 'Upload Data!' button.", "How to use?");
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This App created by: Yehuda Hadad\nfor more information and sources, please contact:\nyuda098q@gmail.com\nor visit my GitHub page:\nhttps://github.com/yudkeh", "About");
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void ServerSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            file = new String[5];
            LoadSettings();
        }

        private void SaveDbBtn_Click(object sender, EventArgs e)
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
                    Console.Write("Can't save config file : "+ex.ToString());
                }
                catch (Exception ex1)
                {
                    Console.Write("Exception occured : " + e.ToString());
                }
            }
            else
            {
                MessageBox.Show("Please make sure that the values have been inserted as need.", "Error");
            }

        }

        private void BtnSelectFile_Click(object sender, EventArgs e)
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

        private void BtnUpload_Click(object sender, EventArgs e)
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

        //loading app settings from configration file
        private void LoadSettings()
        {
            try
            {
                file = System.IO.File.ReadAllLines(@"data\settings.config");
                txtDbUrl.Text = file[0];
                txtPort.Text = file[1];
                txtDbName.Text = file[2];
                txtDbUser.Text = file[3];
                txtDbPass.Text = file[4];

                txtDbUrl.Text = txtDbUrl.Text.Replace("dbUrl=", "");
                txtPort.Text = txtPort.Text.Replace("dbPort=", "");
                txtDbName.Text = txtDbName.Text.Replace("dbName=", "");
                txtDbUser.Text = txtDbUser.Text.Replace("dbUsername=", "");
                txtDbPass.Text = txtDbPass.Text.Replace("dbPass=", "");

            }
            catch (IOException ex)
            {
                Console.Write("Can't load config file : "+ ex.ToString());
            }
            catch(Exception e)
            {
                Console.Write("Can't load file info : " + e.ToString());
            }
        }
    }
}
