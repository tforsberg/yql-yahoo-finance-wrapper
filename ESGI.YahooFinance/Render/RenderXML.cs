using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESGI.Entity.Interfaces;

namespace ESGI.Yahoo.Finance.Render
{
   public class RenderXML : AbstractRender<IContainer,String>
    {
       public RenderXML()
        : base ("XML", RenderEx)
        {
        }

       public static String RenderEx(IContainer container)
        {
            String result = "<CONTAINER>" + Environment.NewLine;

            foreach (IProperty property in container.Properties())
            {
                result += "     <" + property.Id() + ">" + property.Get<Object>().ToString() + "</" +property.Id()  + ">"+ Environment.NewLine;
            }

            result += "</CONTAINER>";

            return result;
        }

    }
}
