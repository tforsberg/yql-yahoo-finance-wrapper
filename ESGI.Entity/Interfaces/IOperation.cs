using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESGI.Entity.Entities;

namespace ESGI.Entity.Interfaces
{
    public interface IOperation<T> 
    {
        String Id();
        T Execute();
        IProperty Get(String argument);
        IProperty Next();
    }
}
