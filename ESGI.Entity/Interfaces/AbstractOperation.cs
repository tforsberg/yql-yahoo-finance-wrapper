using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESGI.Entity.Interfaces
{
    public class AbstractOperation<T> : IOperation<T> 
    {
        String _id;
        public List<IProperty> Arguments { get; set; }

        public delegate T ExecutionStrategy();
        ExecutionStrategy _strategy;

        public AbstractOperation(String id, ExecutionStrategy strategy)
        {
            _id = id;
            _strategy = strategy;
            Arguments = new List<IProperty>();
        }

        public string Id()
        {
            return _id;
        }

        public void Set(ExecutionStrategy strategy)
        {
            _strategy = strategy;
        }

        public T Execute()
        {
            return _strategy();
        }

        public IProperty Get(string argument)
        {
            return Arguments.Find(a => (a.Id() == argument));
        }

        public IProperty Next()
        {
            IProperty result = null;

            foreach (IProperty arg in Arguments)
            {
                if (arg.Get<String>() == null)
                {
                    result = arg;
                    break;
                }
            }

            return result;
        }


    }
}
