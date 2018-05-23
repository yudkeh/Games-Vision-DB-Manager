using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace DB_Manager.classes
{
    class dal
    {

        private MySqlConnection connection;
        private string connectionString;

        public string server {get; set;}
        public string port {get; set;}
        public string database {get; set;}
        public string uid {get; set;}
        public string password {get; set;}

        //constructors:
        //empty constructor
        public dal()
        {

        }

        public Boolean OpenConnection()
        {
            if (connection == null)
            {
                connection = new MySqlConnection(connectionString);
                if (connection == null)
                    return false;
            }

            try
            {
                connection.Open();
                MessageBox.Show("Connected to DB");
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                    default:
                        MessageBox.Show("Cannot connect to server.\nContact administrator");
                        break;
                }
                return false;
            }
        }

        public Boolean CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void SetConnectionString()
        {
            connectionString = "SERVER=" + server + ";" + "PORT=" + port + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        }

    }
}
