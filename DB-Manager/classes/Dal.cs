using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace DB_Manager.classes
{
    class Dal
    {
        Logger _log;
        private MySqlConnection _connection;
        private string _connectionString;
        public Boolean _connectionStatus;

        public string _server {get; set;}
        public string _port {get; set;}
        public string _database {get; set;}
        public string _uid {get; set;}
        public string _password {get; set;}
        public string _ssl { get; set; }

        //constructors:
        //empty constructor
        public Dal()
        {
            _connectionStatus = false;
            _log = new Logger();
        }

        public Boolean OpenConnection()
        {
            if (_connection == null)
            {
                _connection = new MySqlConnection(_connectionString);
                if (_connection == null)
                    return _connectionStatus;
            }

            try
            {
                _connection.Open();
                _connectionStatus = true;
                return _connectionStatus;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again.");
                        break;
                    default:
                        MessageBox.Show("Cannot connect to server.\nContact administrator.");
                        break;
                }
                _log.WriteLog("failed to open connection to the DB : " + ex.ToString());
                return _connectionStatus;
            }
            catch(Exception ex)
            {
                _log.WriteLog("failed to open connection to the DB : " + ex.ToString());
                return _connectionStatus;
            }
        }

        public Boolean CloseConnection()
        {
            Boolean status = true;
            try
            {
                _connection.Close();
            }
            catch (MySqlException ex)
            {
                _log.WriteLog("failed to close connection to the DB : " + ex.ToString());
                status =false;
            }
            return status;
        }

        public void SetConnectionString()
        {
            if(_ssl=="Yes")
                _connectionString = "SERVER=" + _server + ";" + "PORT=" + _port + ";" + "DATABASE=" +
                _database + ";" + "UID=" + _uid + ";" + "PASSWORD=" + _password + ";"+ "Encrypt = false;";
            else
                _connectionString = "SERVER=" + _server + ";" + "PORT=" + _port + ";" + "DATABASE=" +
                _database + ";" + "UID=" + _uid + ";" + "PASSWORD=" + _password + ";"+ "Encrypt=false;";
        }

        public Boolean PushData(HashSet<string> _teams, List<Game> _games, String _division)
        {
            if (_connectionStatus == false)
                return false;

            int i = 0, _gameins = 0, _teamins = 0, _div_ins=0;
            MySqlCommand cmd = _connection.CreateCommand();
            string command;

            //pushing the teams to the DB
            foreach (string t in _teams)
            {
                try
                {
                    cmd = _connection.CreateCommand();
                    command = string.Format("insert into teams (name) VALUES(@name)");
                    cmd.CommandText = command;
                    cmd.Parameters.AddWithValue("@name", t);
                    cmd.ExecuteNonQuery();
                    _teamins++;
                }
                catch (MySqlException ex)
                {
                    _log.WriteLog("MySQL command to add team failed with the error : " + ex.ToString());
                    MessageBox.Show("failed to isnert team to the DB.");
                }
                catch (Exception ex)
                {
                    _log.WriteLog("MySQL command to add team failed with the error : " + ex.ToString());
                    MessageBox.Show("failed to isnert team to the DB.");
                }

            }

            //pushing the games to the DB
            for (i = 0; i < _games.Count; i++)
            {
                try
                {
                    cmd = _connection.CreateCommand();
                    command = string.Format("insert into games (HomeTeam,AwayTeam,FTHG,FTAG,FTR,HTHG,HTAG,HTR,HS,`AS`,HST,AST,HF,AF,HC,AC,HY,AY,HR,AR,`DIV`,DATE) VALUES(@HomeTeam,@AwayTeam,@FTHG,@FTAG,@FTR,@HTHG,@HTAG,@HTR,@HS,@AS,@HST,@AST,@HF,@AF,@HC,@AC,@HY,@AY,@HR,@AR,@DIV,@DATE)");
                    cmd.CommandText = command;
                    cmd.Parameters.AddWithValue("@HomeTeam", _games[i].HomeTeam.ToString());
                    cmd.Parameters.AddWithValue("@AwayTeam", _games[i].AwayTeam.ToString());
                    cmd.Parameters.AddWithValue("@FTHG", Convert.ToInt32(_games[i].FTHG));
                    cmd.Parameters.AddWithValue("@FTAG", Convert.ToInt32(_games[i].FTAG)); ;
                    cmd.Parameters.AddWithValue("@FTR", _games[i].FTR.ToString());
                    cmd.Parameters.AddWithValue("@HTHG", Convert.ToInt32(_games[i].HTHG));
                    cmd.Parameters.AddWithValue("@HTAG", Convert.ToInt32(_games[i].HTAG));
                    cmd.Parameters.AddWithValue("@HTR", _games[i].HTR.ToString());
                    cmd.Parameters.AddWithValue("@HS", Convert.ToInt32(_games[i].HS));
                    cmd.Parameters.AddWithValue("@AS", Convert.ToInt32(_games[i].AS));
                    cmd.Parameters.AddWithValue("@HST", Convert.ToInt32(_games[i].HST));
                    cmd.Parameters.AddWithValue("@AST", Convert.ToInt32(_games[i].AST));
                    cmd.Parameters.AddWithValue("@HF", Convert.ToInt32(_games[i].HF));
                    cmd.Parameters.AddWithValue("@AF", Convert.ToInt32(_games[i].AF));
                    cmd.Parameters.AddWithValue("@HC", Convert.ToInt32(_games[i].HC));
                    cmd.Parameters.AddWithValue("@AC", Convert.ToInt32(_games[i].AC));
                    cmd.Parameters.AddWithValue("@HY", Convert.ToInt32(_games[i].HY));
                    cmd.Parameters.AddWithValue("@AY", Convert.ToInt32(_games[i].AY));
                    cmd.Parameters.AddWithValue("@HR", Convert.ToInt32(_games[i].HR));
                    cmd.Parameters.AddWithValue("@AR", Convert.ToInt32(_games[i].AR));
                    cmd.Parameters.AddWithValue("@DIV", _games[i].DIV.ToString());
                    cmd.Parameters.AddWithValue("@DATE", _games[i].DATE);
                    cmd.ExecuteNonQuery();
                    _gameins++;

                }
                catch (MySqlException ex)
                {
                    _log.WriteLog("MySQL command to add game failed with the error : " + ex.ToString());
                    MessageBox.Show("failed to isnert games to the DB.");
                }
                catch (Exception ex)
                {
                    _log.WriteLog("MySQL command to add game failed with the error : " + ex.ToString());
                    MessageBox.Show("failed to isnert games to the DB.");
                }
            }

            //adding the division
            try
            {
                cmd = _connection.CreateCommand();
                command = string.Format("insert into divisions (sign) VALUES(@sign)");
                cmd.CommandText = command;
                cmd.Parameters.AddWithValue("@sign", _division);
                cmd.ExecuteNonQuery();
                _div_ins++;
            }
            catch (MySqlException ex)
            {
                _log.WriteLog("MySQL command to add division failed with the error : " + ex.ToString());
                MessageBox.Show("failed to isnert division to the DB.");
            }
            catch (Exception ex)
            {
                _log.WriteLog("MySQL command to add division failed with the error : " + ex.ToString());
                MessageBox.Show("failed to isnert division to the DB.");
            }

            String _finish = string.Format("Upload complete.\nTotal records added:\nteams: {0}\nmatches: {1}\ndivisions {2}", _teamins, _gameins, _div_ins);
            MessageBox.Show(_finish);
            String _finishForLog= string.Format("Upload to the DB complete : Total records added:   teams: {0}  ,  matches: {1}  ,  divisions {2}", _teamins, _gameins, _div_ins);
            _log.WriteLog(_finishForLog);
            return true;
        }
    }
}
