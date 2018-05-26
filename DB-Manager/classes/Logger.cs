using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Manager.classes
{
    class Logger
    {
        
        //empty constructor
        public Logger()
        {

        }

        public void WriteLog(String logLine)
        {
            try
            {
                File.AppendAllText(@"data\log.txt", DateTime.Now.ToString() + " --- "  + logLine + Environment.NewLine);

            }
            catch (IOException e)
            {

            }
            catch (Exception e)
            {

            }
        }

    }
}
