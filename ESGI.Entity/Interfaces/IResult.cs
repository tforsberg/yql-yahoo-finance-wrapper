using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESGI.Entity.Interfaces;

namespace ESGI.Entity.Entities
{
    public interface IResult<T>
    {
         T Release();
         //V Execute(String expression);

    }
}
