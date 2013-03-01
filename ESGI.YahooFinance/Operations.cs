using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using ESGI.Entity.Interfaces;
using ESGI.Entity.Entities;
using ESGI.Entity.Action.Download;
using ESGI.Yahoo.Yql;
using Ninject;

namespace ESGI.Yahoo.Finance
{
    public enum OPERATIONS
    {
        STOCK,
        OPTION,
        HISTORY,
        NEWS
    }

    public enum RENDERS
    {
        XML,
        JSON
    }

    public static class OperationHelper
    {
        public static Boolean IsValidYQLResponse(XDocument xml)
        {
            return
                (xml.XPathSelectElement("/*[1]").Name == "error") ? false : true;
        }

        public static IContainer HandleErrors(XDocument root)
        {
            IContainer container = new GenericContainer();

            foreach (XElement element in root.XPathSelectElements("/*/child::*"))
            {
                container.RegisterProperty<String>(element.Name.LocalName, element.Value);
            }
            return container;
        }
    }

    public class StockInfo : AbstractOperation<IContainer>
    {

        IContainer Result { get; set; }

         [Inject]
        public StockInfo(IContainer container)
            : base("StockInfo", null)
        {
            Result = container;
            base.Set(ExecuteEx);
            base.Arguments = new List<IProperty>()
            {
                new Property ("symbols", typeof(String)),
                new Property ("criterias",typeof(String))
            };
        }

        private IContainer ExecuteEx()
        {
            YQLQuery query = new YQLQuery();

            query.
                Select(Arguments.First(a => (a.Id() == "criterias")).Get<String>()).
                From(Register.Instance.Get("TABLE", "QUOTES")).
                Where().In("symbol", Arguments.First(a => (a.Id() == "symbols")).Get<String>());

          DownloadRessourceHandler<String> dl = new DownloadRessourceHandler<String>();
 
          String result =  dl.Acquire(
                new Uri(Helper.FormatQuery(Register.Instance.Get("URL", "PUBLIC_API"), query.Yield(), Register.Instance.Get("URL", "TABLES"))), Helper.HandleStream);

          XDocument doc = XDocument.Parse(result);

          if (!OperationHelper.IsValidYQLResponse(doc))
              return OperationHelper.HandleErrors(doc);

          foreach (XElement element in doc.XPathSelectElements("/*/*/quote"))
          {
              IContainer container = (IContainer)Activator.CreateInstance(Result.GetType());

              foreach (XElement property in element.XPathSelectElements("child::*"))
              {
                  container.RegisterProperty<String>(property.Name.LocalName,property.Value);
              }

              Result.RegisterProperty<IContainer>(element.Attribute("symbol").Value, container);
          }

          Result.RegisterProperty<String>("script", result);

          return Result;

        }
    }


    public class StockHistoryInfo : AbstractOperation<IContainer>
    {

        IContainer Result { get; set; }

          [Inject]
        public StockHistoryInfo(IContainer container)
            : base("StockHistoryInfo", null)
        {
            Result = container;
            base.Set(ExecuteEx);
            base.Arguments = new List<IProperty>()
            {
                new Property ("symbol",typeof(String)),
            };
        }

        private IContainer ExecuteEx()
        {
            YQLQuery query = new YQLQuery();

            query.
                Select("*").
                From("csv").
                Where("url", Register.Instance.Get("URL", "HISTORIC_API") + Arguments.First(a => (a.Id() == "symbol")).Get<String>()).And("columns", "Date,Open,High,Low,Close,Volume,AdjClose");

            DownloadRessourceHandler<String> dl = new DownloadRessourceHandler<String>();

            String url = Helper.FormatQuery(Register.Instance.Get("URL", "PUBLIC_API"), query.Yield(), Register.Instance.Get("URL", "TABLES"));

            String result = dl.Acquire(
                  new Uri(Helper.FormatQuery(Register.Instance.Get("URL", "PUBLIC_API"), query.Yield(), Register.Instance.Get("URL", "TABLES"))), Helper.HandleStream);

            XDocument doc = XDocument.Parse(result);

            if (!OperationHelper.IsValidYQLResponse(doc))
                return OperationHelper.HandleErrors(doc);

            Result.RegisterProperty<String>("symbol", Arguments.First(a => (a.Id() == "symbol")).Get<String>());

            foreach (XElement element in doc.XPathSelectElements("/*/results/*"))
            {
                IContainer container = (IContainer)Activator.CreateInstance(Result.GetType());

                Boolean header = true;

                foreach (XElement child in element.XPathSelectElements("child::*"))
                {
                    if (!header) container.RegisterProperty<String>(child.Name.LocalName, child.Value);
                    header = false;
                }

                Result.RegisterProperty<IContainer>(element.Element("Date").Value, container);
            }

            Result.RegisterProperty<String>("script", result);

            return Result;
        }
    }

    public class NewsInfo : AbstractOperation<IContainer>
    {
        IContainer Result { get; set; }

         [Inject]
        public NewsInfo(IContainer container)
            : base("NewsInfo", null)
        {
            Result = container;
            base.Set(ExecuteEx);
            base.Arguments = new List<IProperty>()
            {
                new Property ("symbol",typeof(String)),
            };
        }

        private IContainer ExecuteEx()
        {
            YQLQuery query = new YQLQuery();

            query.
                Select("title, link, description, pubDate").
                From("rss").
                Where("url", Register.Instance.Get("URL", "RSS_API") + Arguments.First(a => (a.Id() == "symbol")).Get<String>());

            DownloadRessourceHandler<String> dl = new DownloadRessourceHandler<String>();

         
            String result = dl.Acquire(
                  new Uri(Helper.FormatQuery(Register.Instance.Get("URL", "PUBLIC_API"), query.Yield(), Register.Instance.Get("URL", "TABLES"))), Helper.HandleStream);

       
            XDocument doc = XDocument.Parse(result);

            if (!OperationHelper.IsValidYQLResponse(doc))
                return OperationHelper.HandleErrors(doc);

            foreach (XElement element in doc.XPathSelectElements("/*/*/item"))
            {
                IContainer container = (IContainer)Activator.CreateInstance(Result.GetType());

                foreach (XElement property in element.XPathSelectElements("child::*"))
                {
                    container.RegisterProperty<String>(property.Name.LocalName, property.Value);
                }

                Result.RegisterProperty<IContainer>(element.Element("title").Value, container);
            }

            Result.RegisterProperty<String>("script", result);

            return Result;
        }
    }


    public class OptionsInfo : AbstractOperation<IContainer>
    {
        IContainer Result { get; set; }

        [Inject]
        public OptionsInfo(IContainer container)
              : base("OptionsInfo", null)
          {
              Result = container;
              base.Set(ExecuteEx);
              base.Arguments = new List<IProperty>()
            {
                new Property ("criterias",typeof(String)),
                new Property ("symbol",typeof(String)),
            };
          }

        private IContainer ExecuteEx()
        {
            YQLQuery query = new YQLQuery();

            query.
                Select(Arguments.First(a => (a.Id() == "criterias")).Get<String>()).
                From(Register.Instance.Get("TABLE", "OPTIONS")).
                Where("symbol=\"" + Arguments.First(a => (a.Id() == "symbol")).Get<String>() + "\"");

            DownloadRessourceHandler<String> dl = new DownloadRessourceHandler<String>();

            String result = dl.Acquire(
                  new Uri(Helper.FormatQuery(Register.Instance.Get("URL", "PUBLIC_API"), query.Yield(), Register.Instance.Get("URL", "TABLES"))), Helper.HandleStream);

            XDocument doc = XDocument.Parse(result);

            if (!OperationHelper.IsValidYQLResponse(doc))
                return OperationHelper.HandleErrors(doc);

            foreach (XElement element in doc.XPathSelectElements("/*/*/optionsChain/*"))
            {
                IContainer container = (IContainer)Activator.CreateInstance(Result.GetType());

                foreach (XElement property in element.XPathSelectElements("child::*"))
                {
                    container.RegisterProperty<String>(property.Name.LocalName, property.Value);
                }

                container.RegisterProperty<String>("type", element.Attribute("type").Value);
                Result.RegisterProperty<IContainer>(element.Attribute("symbol").Value, container);
            }

            Result.RegisterProperty<String>("script", result);

            return Result;
        }
    }


}
