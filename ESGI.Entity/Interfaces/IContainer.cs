using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESGI.Entity.Interfaces
{
    public interface IContainer
    {
        IEnumerable<IProperty> Properties();
        IProperty Property(String id);
        void RegisterProperty<T>(String id);
        void RegisterProperty<T>(String id, T value);
    }
}
