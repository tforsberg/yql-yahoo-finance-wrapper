using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections;
using ESGI.Entity.Interfaces;
using ESGI.Entity.Entities;
using Ninject;
using Ninject.Modules;

namespace ESGI.Yahoo.Finance
{
    public class ContainerModule : NinjectModule
    {

        public override void Load()
        {
            Bind<IContainer>().To<GenericContainer>();

            //foreach (XElement render in ((IEnumerable)Register.Instance._doc.XPathEvaluate("/*/INJECTION/RENDERS/child::*")))
            //{
            //    Bind<IRender<IContainer, String>>()
            //        .To(Type.GetType(render.Value))
            //        .When(s => Register.Instance.RENDER_CONTEXT == (RENDERS)Enum.Parse(typeof(RENDERS), render.Attribute("id").Value));
            //}

            //foreach (XElement operation in ((IEnumerable)Register.Instance._doc.XPathEvaluate("/*/INJECTION/OPERATIONS/child::*")))
            //{

            //    Bind<IOperation<IContainer>>()
            //        .To(Type.GetType(operation.Value))
            //        .When(s => Register.Instance.OPERATION_CONTEXT == (OPERATIONS)Enum.Parse(typeof(OPERATIONS), operation.Attribute("id").Value ));
            //}

            Bind<IRender<IContainer, String>>()
                .To<Render.RenderXML>()
                .When(request => Register.Instance.RENDER_CONTEXT == RENDERS.XML);
            Bind<IRender<IContainer, String>>()
                .To<Render.RenderJSON>()
                .When(request => Register.Instance.RENDER_CONTEXT == RENDERS.JSON);

            Bind<IOperation<IContainer>>()
                .To<StockInfo>()
                .When(request => Register.Instance.OPERATION_CONTEXT == OPERATIONS.STOCK);
            Bind<IOperation<IContainer>>()
                .To<NewsInfo>()
                .When(request => Register.Instance.OPERATION_CONTEXT == OPERATIONS.NEWS);
            Bind<IOperation<IContainer>>()
                .To<OptionsInfo>()
                .When(request => Register.Instance.OPERATION_CONTEXT == OPERATIONS.OPTION);
            Bind<IOperation<IContainer>>()
                .To<StockHistoryInfo>()
                .When(request => Register.Instance.OPERATION_CONTEXT == OPERATIONS.HISTORY);
        }
    }

   public class Register
    {
       private static object l = new object();
       private static Register _register;
       internal XDocument _doc;
       protected IKernel _kernel;

       public RENDERS RENDER_CONTEXT { get; set; }

       public OPERATIONS OPERATION_CONTEXT { get; set; }

       public static Register Instance
       {
           get
           {
               lock (l)
               {
                   if (_register == null)
                   {
                       _register = new Register();
                       _register.RENDER_CONTEXT = RENDERS.XML;
                       _register.OPERATION_CONTEXT = OPERATIONS.STOCK;
                       _register._doc = XDocument.Parse(System.IO.File.ReadAllText("Ressources.xml"));
                       _register._kernel = new StandardKernel(new ContainerModule());
                   }
               }
               return _register;
           }
       }

       public IRender<IContainer, String> Get()
       {
           return _kernel.Get<IRender<IContainer, String>>();
       }

       public String Get(String NodeName, String NodeId)
       {
           return 
               ((IEnumerable)_doc.XPathEvaluate("/RESSOURCES/" + NodeName + "S" + "/" + NodeName + "[@id='" + NodeId + "']"))
               .Cast<XElement>()
               .FirstOrDefault()
               .Value;
        }

       public IOperation<IContainer> Get(OPERATIONS operation, String ticker)
       {

           OPERATION_CONTEXT = operation;

           IOperation<IContainer> container = 
               _kernel.Get<IOperation<IContainer>>(new Ninject.Parameters.ConstructorArgument ("container",_kernel.Get<IContainer>())
                   );

           switch (operation)
           {
               case OPERATIONS.STOCK:
                   container.Get("criterias").Set<String>("*");
                   container.Get("symbols").Set<String>(ticker);
                   break;
               case OPERATIONS.OPTION:
                   container.Get("criterias").Set<String>("*");
                   container.Get("symbol").Set<String>(ticker);
                   break;
               case OPERATIONS.HISTORY:
                   container.Get("symbol").Set<String>(ticker);
                   break;
               case OPERATIONS.NEWS:
                   container.Get("symbol").Set<String>(ticker);
                   break;
           }

           return container;
       }
    }
}
