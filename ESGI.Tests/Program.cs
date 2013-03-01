using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ESGI.Yahoo.Yql;
using ESGI.Entity.Action.Download;
using ESGI.Entity.Entities;
using ESGI.Entity.Interfaces;
using ESGI.Yahoo.Finance;
using ESGI.Entity.Extension;


namespace ESGI.Tests
{
    class Program
    {
        static String _data = string.Empty;
        static String Handler(byte[] data)
        {
            String str = System.Text.Encoding.ASCII.GetString(data);
            return str;
        }

        static void Callback(String data)
        {
            _data  = data;
        }

        static void Main(string[] args)
        {
            try
            {

                String[] fff = System.IO.File.ReadAllLines(@"C:\Users\t-raynal\Desktop\ticker.txt").Where(v=> (v!= Environment.NewLine || v!= string.Empty)).ToArray();

                IEnumerable<String> value = fff.Where(v => (v.Length>=2 && v.Substring(0, 2) == "GO"));

                foreach (String s in value)
                {
                    Console.Out.WriteLine(s);
                }

                IContainer container = new GenericContainer();
                container.RegisterProperty<String>("str", "ttttt");
                container.RegisterProperty<DateTime>("date", DateTime.Parse("10/01/2001"));
                container.RegisterProperty<DateTime>("date", DateTime.Parse("12/02/2002"));

                foreach(IProperty p in container.Where(p=> (p.Type==typeof(DateTime))))
                {
                    Console.Out.WriteLine(p.TryGetS<DateTime>().Value.ToShortDateString());
                }

                //Console.Out.WriteLine(Type.GetType("ESGI.Yahoo.Finance.StockInfo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null").ToString());
                //ESGI.Yahoo.Finance.StockInfo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
                //GenericContainer container;

                //IOperation<GenericContainer> options = new OptionsInfo();
                //options.Get("criterias").Set<String>("*");
                //options.Get("symbol").Set<String>("GOOG");
                //container = options.Execute();

                //Console.Out.WriteLine("***********************************************");
                //Console.Out.WriteLine(container.ToString());

                //IOperation<GenericContainer> newsInfo = new NewsInfo();
                //newsInfo.Get("symbol").Set<String>("GOOG");
                //container = newsInfo.Execute();

                //Console.Out.WriteLine("***********************************************");
                //Console.Out.WriteLine(container.ToString());

                //IOperation<GenericContainer> historic = new StockHistoryInfo();
                //historic.Get("symbol").Set<String>("GOOG");
                //container = historic.Execute();

                //Console.Out.WriteLine("***********************************************");
                //Console.Out.WriteLine(container.ToString());

                //IOperation<GenericContainer> infos = new StockInfo();
                //infos.Get("criterias").Set<String>("*");
                //infos.Get("symbols").Set<String>("GOOG, AAPL");
                //container = infos.Execute();

                //Console.Out.WriteLine("***********************************************");
                //Console.Out.WriteLine(container.ToString());


            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.ToString());
            }

            return;

        }

        static void DownloadCompleteCallback(byte[] dataDownloaded)
        {
            Console.Out.WriteLine(dataDownloaded.Length);
        }

    }
}
