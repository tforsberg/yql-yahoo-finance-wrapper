using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESGI.Entity.Interfaces;


namespace ESGI.Entity.Entities
{
  public class Property : IProperty
    {

      private String _id;
      private Type _type;
      private Object _value;

      public Type Type { get; set; }

      public Property(String id, Type type)
      {
          _id = id;
          Type = type;
      }

        public String Id()
        {
            return _id;
        }

        public T Get<T>()
        {   
            return (T)_value;
        }

        public void Set<T>(T item)
        {
            _type =  typeof(T);
            _value = item;
        }
    }

}
