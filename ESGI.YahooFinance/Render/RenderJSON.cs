using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESGI.Entity.Interfaces;

namespace ESGI.Yahoo.Finance.Render
{
    public class RenderJSON : AbstractRender<IContainer,String>
    {
        public RenderJSON()
        : base ("JSON", RenderEx)
        {
        }

       public static String RenderEx(IContainer container)
        {
            String result = "CONTAINER" + Environment.NewLine;

            result += "{";

            foreach (IProperty property in container.Properties())
            {
                result += "     {\"" + property.Id() + "\" : \"" + property.Get<Object>().ToString() + "\"}" + Environment.NewLine;
            }

            result += "}";

            return result;
        }

    }
}
