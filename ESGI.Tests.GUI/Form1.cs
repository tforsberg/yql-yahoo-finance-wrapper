using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

using ComponentFactory.Krypton.Toolkit;
using ComponentFactory.Krypton.Ribbon;
using System.Threading;
using ESGI.Yahoo.Finance;


namespace ESGI.Tests.GUI
{
    public partial class Form1 : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public Form1()
        {
            InitializeComponent();
            Logic.Instance.Init(this, MajTape);
        }

        private void AsyncEx(object data)
        {
            String ticker = ((List<Object>)data)[0].ToString();
            OPERATIONS action = (OPERATIONS)(((List<Object>)data)[1]);

                ESGI.Entity.Interfaces.IContainer container = Logic.Instance.Execute(
                   ticker, action);
                container.RegisterProperty<String>("c_id", ticker + action.ToString());
                AsyncCallback(container);
        }

        private void AsyncCallback(ESGI.Entity.Interfaces.IContainer container)
        {

            KryptonRibbonRecentDoc rDoc = new KryptonRibbonRecentDoc()
            {
                Text = container.Property("c_id").Get<String>(),
                Tag = container
            };

            rDoc.Click += new EventHandler(ClickWork);

            kryptonRibbon.RibbonAppButton.AppButtonRecentDocs.Add(rDoc);
        }

        private void ClickWork(Object item, EventArgs args)
        {

            String product = string.Empty;

            Logic.Instance.Render(kryptonRibbonGroupComboBox2.Text);

            foreach (ESGI.Entity.Interfaces.IProperty property in (
                (ESGI.Entity.Interfaces.IContainer)((KryptonRibbonRecentDoc)item).Tag).Properties())
            {
                if (!(property.Id() == "script"))
                    product += "    " + property.Id() + " : " + property.Get<Object>().ToString() + Environment.NewLine;
            }

            String result =
                ((ESGI.Entity.Interfaces.IContainer)((KryptonRibbonRecentDoc)item).Tag).Property("script").Get<String>();

            kryptonTextBox1.Text = product;
            kryptonTextBox2.Text = (result.Length>2000)? result.Substring(0,2000) +"[TRUNCATED]" : result;

            result =Logic.Instance.Render((ESGI.Entity.Interfaces.IContainer)((KryptonRibbonRecentDoc)item).Tag);

            kryptonTextBox3.Text = (result.Length>2000)? result.Substring(0,2000) +"[TRUNCATED]" : result;
        }

        private void kryptonRibbonGroupButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncEx),
                    new List<Object> { kryptonRibbonGroupTextBox1.Text, (OPERATIONS)Enum.Parse(typeof(OPERATIONS),
                        kryptonRibbonGroupComboBox1.Text) });
            }
            catch (Exception ex)
            {
                kryptonTextBox1.Text = string.Empty;
                kryptonTextBox2.Text = ex.ToString();
            }
        }

        public void MajTape(Object sender, EventArgs e)
        {
            if (Logic.Instance.TICKER != null && Logic.Instance.TICKER.Length > 0)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        String ticker =  Logic.Instance.TICKER[(int)new Random().Next(0,Logic.Instance.TICKER.Length)];
                        ESGI.Entity.Interfaces.IContainer container = Logic.Instance.Execute(
                           ticker,
                           OPERATIONS.STOCK);

                        kryptonRibbonGroupTextBox3.Clear();
                        kryptonRibbonGroupTextBox3.Text = String.Format("{0}:{1}", ticker, container.Property(ticker).
                            Get<ESGI.Entity.Interfaces.IContainer>().
                            Property("Ask").Get<String>());

                    }
                    catch (Exception ex)
                    {
                        kryptonRibbonGroupTextBox3.Text = "Service unavailable";
                    }
                });
            }
        }


        private void AsyncTickerList(object value)
        {
              this.Invoke((MethodInvoker)delegate
                {
                    foreach (String s in Logic.Instance.Find((string)value))
                    {
                        kryptonRibbonGroupTextBox2.AppendText(s + Environment.NewLine);
                
                    }
                }); 
        }

        private void kryptonRibbonGroupTextBox1_TextChanged(object sender, EventArgs e)
        {
            kryptonRibbonGroupTextBox2.Clear();
            ThreadPool.QueueUserWorkItem(AsyncTickerList, (object)kryptonRibbonGroupTextBox1.Text);
        }


    }
}