using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESGI.Entity.Interfaces;

namespace ESGI.Entity.Entities
{
    public class GenericContainer : IContainer
    {
       List<IProperty> _properties;

       public GenericContainer()
       {
           _properties = new List<IProperty>();
       }

       public override string ToString()
       {
           String result = string.Empty;

           foreach (IProperty property in _properties)
           {
               result += "    " + property.Id() + " : " +property.Get<Object>().ToString()+Environment.NewLine;  
           }

           return result;
       }

       public IEnumerable<IProperty> Properties()
       {
           return _properties;
       }
      public IProperty Property (String id)
       {
           return _properties.Find(c => (c.Id() == id));
       }
      public void RegisterProperty<T>(String id)
       {
           _properties.Add(new Property(id, typeof(T)));
       }

      public void RegisterProperty<T>(string id, T value)
      {
          Property property = new Property(id, typeof(T));
          property.Set<T>(value);
          _properties.Add(property);
      }

    }
}
