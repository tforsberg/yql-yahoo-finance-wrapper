using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESGI.Entity.Interfaces
{
    public interface IProperty
    {
        String Id();
        T Get<T>();
        Type Type { get; set; }
        void Set<T>(T item); 
    }
}
