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
        private Dal _d;
        private String[] _file;
        private FileReader _fd;
        Logger _log = new Logger();
        private Boolean _runnig;
        private Boolean _uploadStatus;

        public Form1()
        {
            InitializeComponent();
            lblPb.Visible = false;
            pbUpload.Visible = false;
            _runnig = false;
        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_runnig)
            {
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Cannot close the application while it reading the file and insert it values to the dtabase.", "Error");
            }
        }

        private void HowToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Currently the application only support MySQL as the DB.\nIn order to push the data to the DB, please enter the DB inforamtion and select the excel file, afterwards, just click on the 'Upload Data!' button.\nIf any error occur, please check the log file for more info.", "How to use?");
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
            _file = new String[5];
            LoadSettings();
        }

        private void SaveDbBtn_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                _file = new string[6];
                try
                {
                    _file[0] = "dbUrl=" + txtDbUrl.Text;
                    _file[1] = "dbPort=" + txtPort.Text;
                    _file[2] = "dbName=" + txtDbName.Text;
                    _file[3] = "dbUsername=" + txtDbUser.Text;
                    _file[4] = "dbPass=" + txtDbPass.Text;
                    _file[5] = "dbSsl=" + ddSsl.SelectedItem.ToString();
                    System.IO.File.WriteAllLines(@"data\settings.config", _file);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Can't save config file.");
                    _log.WriteLog("failed to save configuration : "+ex.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can't save config file.");
                    _log.WriteLog("failed to save configuration : " + ex.ToString());
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
                    _log.WriteLog("failed to select file : " + ex.ToString());
                }
                catch (Exception ex)
                {
                    _log.WriteLog("failed to select file : " + ex.ToString());
                }
            }
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            _d = new Dal();
            pbUpload.Value = 0; //resetting the progres bar value
            _runnig = true;
            _uploadStatus = false;
            btnUpload.Enabled = false;

            //validating the form
            if (ValidateForm())
            {
                FillDal();

                //validating that we have select file
                if(txtfilePath.Text.Trim() != String.Empty)
                {
                    lblPb.Visible = true;
                    pbUpload.Visible = true;
                    lblPb.Text = "Connecting to the DB";

                    //trying to connect to the DB
                    if (_d.OpenConnection())
                    {
                        pbUpload.Value = 15;
                        _fd = new FileReader(txtfilePath.Text);
                        lblPb.Text = "Reading the file";

                        //trying to read the file.
                        if (_fd.ReadData())
                        {
                            pbUpload.Value = 50;
                            lblPb.Text = "Uplaoding the data, please wait..";

                            backgroundWorker.WorkerReportsProgress = true;
                            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
                            backgroundWorker.RunWorkerAsync();

                        }
                        else
                        {
                            _runnig = false;
                            lblPb.Visible = false;
                            pbUpload.Visible = false;
                            MessageBox.Show("Cannot read the file.", "Error");
                        }

                    }
                    else
                    {
                        _runnig = false;
                        lblPb.Visible = false;
                        pbUpload.Visible = false;
                        //MessageBox.Show("Could not conenct to the database.", "Error");
                        //not priniting error since it can come due to few reason, therefore the error will be printed in the Dal class.
                    }

                }
                else
                {
                    _runnig = false;
                    lblPb.Visible = false;
                    pbUpload.Visible = false;
                    MessageBox.Show("Please select file.", "Error");
                }
            }
            else
            {
                _runnig = false;
                lblPb.Visible = false;
                pbUpload.Visible = false;
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
            else if(ddSsl.SelectedItem == null)
                validated = false;

            if (validated)
            {
                validated = int.TryParse(txtPort.Text, out myInt);
            }

            return validated;
        }

        //inserting values from the form to the dal object and generating the connection string
        private void FillDal()
        {
            _d._server = txtDbUrl.Text;
            _d._port = txtPort.Text;
            _d._password = txtDbPass.Text;
            _d._uid = txtDbUser.Text;
            _d._database = txtDbName.Text;
            _d._ssl = ddSsl.SelectedItem.ToString();
            _d.SetConnectionString();
        }

        //loading app settings from configration file
        private void LoadSettings()
        {
            try
            {
                _file = System.IO.File.ReadAllLines(@"data\settings.config");
                txtDbUrl.Text = _file[0];
                txtPort.Text = _file[1];
                txtDbName.Text = _file[2];
                txtDbUser.Text = _file[3];
                txtDbPass.Text = _file[4];

                ddSsl.SelectedItem = _file[5].Replace("dbSsl=", "");
                txtDbUrl.Text = txtDbUrl.Text.Replace("dbUrl=", "");
                txtPort.Text = txtPort.Text.Replace("dbPort=", "");
                txtDbName.Text = txtDbName.Text.Replace("dbName=", "");
                txtDbUser.Text = txtDbUser.Text.Replace("dbUsername=", "");
                txtDbPass.Text = txtDbPass.Text.Replace("dbPass=", "");

            }
            catch (IOException ex)
            {
                    MessageBox.Show("Can't load configuration file.");
                    _log.WriteLog("failed to load configration file : " + ex.ToString());
            }
            catch(Exception ex)
            {
                    //MessageBox.Show("Can't load configuration file.");
                    _log.WriteLog("failed to load configration file : " + ex.ToString());
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _uploadStatus = _d.PushData(_fd._teams, _fd._games, _fd._division);
            }
            catch (Exception ex)
            {
                _log.WriteLog("failure in the thread executer : " + ex.ToString());
                btnUpload.Enabled = true;
                _runnig = false;
            }

        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _runnig = false;
            pbUpload.Value = 100;
            lblPb.Text = "Upload completed";
            _d.CloseConnection();
            btnUpload.Enabled = true;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
