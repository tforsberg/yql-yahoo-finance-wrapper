using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESGI.Entity.Interfaces
{
   public interface IRender<C,V>
    {
       V Render(C entity);
    }
}
