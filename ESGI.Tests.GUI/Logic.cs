using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ESGI.Entity.Entities;
using ESGI.Entity.Interfaces;
using ESGI.Yahoo.Finance;
using ESGI.Entity.Action.Download;
using ESGI.Yahoo.Yql;

namespace ESGI.Tests.GUI
{
    public class Logic : IDisposable
    {
        private static Logic _logic;
        private Form1 _form;
        private static object l = new object();
        private System.Timers.Timer _tape;

        public String[] TICKER { get; set; }

        public void Init(Form1 form, System.Timers.ElapsedEventHandler tape_handler)
        {
            _form = form;
            _tape = new System.Timers.Timer(10000);
            _tape.Elapsed += tape_handler;
            _tape.Start();
            if (System.IO.File.Exists("ticker"))
            {
                TICKER = System.IO.File.ReadAllLines("ticker").Where(v=> (v!= Environment.NewLine || v!= string.Empty)).ToArray();
            }
        }


        public List<String> Find(String value)
        {
            List<String> result = new List<String>();

            if (null != TICKER && null != string.Empty)
            {
                result =
                    TICKER.Where(v => (v.Length >= value.Length && v.Substring(0, value.Length) == value)).ToList();
            }

            return result;
        }

        public IContainer Execute(String ticker, OPERATIONS operation)
        {
            return Register.Instance.Get(operation, ticker)
                           .Execute();
        }

        public void Render(String value)
        {
            Register.Instance.RENDER_CONTEXT = (RENDERS)Enum.Parse(typeof(RENDERS), value);
        }

        public string Render(IContainer container)
        {
        
            return
                Register.Instance.Get().Render(container);
        }

        public static Logic Instance
        {
            get
            {
                lock (l)
                {
                    if (_logic == null)
                    {
                        _logic = new Logic();
                    }

                }
                return _logic;

            }
        }

        public void Dispose()
        {
            _tape.Stop();
            GC.WaitForFullGCComplete();
        }
    }
}
