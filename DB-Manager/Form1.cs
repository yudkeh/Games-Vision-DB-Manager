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
            MessageBox.Show("This App created by: Yehuda Hadad\nfor more information and sources, please contact:\nyuda098q@gmail.com\nOr visit my GitHub page:\nhttps://github.com/yudkeh", "About");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void serverSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void saveDbBtn_Click(object sender, EventArgs e)
        {

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

        }
    }
}
