using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Manager.classes
{
    class FileReader
    {
        private string _filePath;
        public List<Game> _games { get; set; }
        public String _division { get; set; }
        public HashSet<string> _teams { get; set; }
        public Boolean _status;
        Logger _log = new Logger();

        //empty constructor
        public FileReader()
        {

        }
    
        //constructor with values
        public FileReader(string filePath)
        {
            _filePath = filePath;
        }

        public Boolean ReadData()
        {
            _status = true;
            Game game;
            Boolean first = true;

            StreamReader reader = new StreamReader(File.OpenRead(_filePath));
            string line = reader.ReadLine(); //skipping te first line of the file which is the file header

            try
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    if (!String.IsNullOrWhiteSpace(line))
                    {
                        string[] values = line.Split(',');
                        game = new Game();
                        game.HomeTeam = values[2];
                        game.AwayTeam = values[3];
                        game.FTHG = values[4];
                        game.FTAG = values[5];
                        game.FTR = values[6];
                        game.HTHG = values[7];
                        game.HTAG = values[8];
                        game.HTR = values[9];
                        game.HS = values[10];
                        game.AS = values[11];
                        game.HST = values[12];
                        game.AST = values[13];
                        game.HF = values[14];
                        game.AF = values[15];
                        game.HC = values[16];
                        game.AC = values[17];
                        game.HY = values[18];
                        game.AY = values[19];
                        game.HR = values[20];
                        game.AR = values[21];
                        game.DIV = values[0];
                        game.DATE = Convert.ToDateTime(values[1]);
                        _games.Add(game);
                        _teams.Add(game.HomeTeam);
                        _teams.Add(game.AwayTeam);
                        if (first)
                        {
                            _division = values[0];
                            first = false;
                        }

                    }
                }
            }
            catch (IOException e)
            {
                _log.WriteLog("error occurred while trying to read the file : " + e.ToString());
                _status = false;
            }
            catch (Exception e)
            {
                _log.WriteLog("error occurred while trying to read the file : " + e.ToString());
                _status = false;
            }

            return _status;

        }
    }
}
