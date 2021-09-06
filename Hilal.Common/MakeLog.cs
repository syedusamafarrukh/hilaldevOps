using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hilal.Common
{
    public class MakeLog
    {
        private string sLogFormat;
        private string sErrorTime;

        public MakeLog()
        {
            sLogFormat = DateTime.UtcNow.ToShortDateString().ToString() + " " + DateTime.UtcNow.ToLongTimeString().ToString() + " ==> ";

            string sYear = DateTime.UtcNow.Year.ToString();
            string sMonth = DateTime.UtcNow.Month.ToString();
            string sDay = DateTime.UtcNow.Day.ToString();
            sErrorTime = sYear + sMonth + sDay;
        }

        public void ErrorLog(string rootPath, string sPathName, string sErrMsg)
        {
            string path = rootPath + "/Logs/" + sErrorTime + "/" + sPathName;
            CreatDirectory(path);
            try
            {
                StreamWriter sw = new StreamWriter(path, true);
                sw.WriteLine(sLogFormat + sErrMsg);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
            }

        }
        private void CreatDirectory(string path)
        {
            var newPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
        }
    }
}
