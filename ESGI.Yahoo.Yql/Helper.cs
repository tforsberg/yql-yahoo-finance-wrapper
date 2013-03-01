using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;


namespace ESGI.Yahoo.Yql
{
    public class Helper
    {
        public static String HandleStream(byte[] stream)
        {
            return Encoding.UTF8.GetString(stream);
        }

        public static String FormatQuery(String url, String yqlQuery, String tables)
        {
            return String.Format("{0}?q={1}&env={2}", url, yqlQuery, tables);
        }

        public static String FormatSymbol()
        {
            return string.Empty;
        }
    }
}
