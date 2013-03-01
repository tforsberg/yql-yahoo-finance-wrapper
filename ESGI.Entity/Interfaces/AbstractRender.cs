using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESGI.Entity.Interfaces
{
    public class AbstractRender<C, V> : AbstractOperation<V>, IRender<C, V> where C : IContainer 
    {
        public delegate V RenderingStrategy(C entity);
        C _entity;

        RenderingStrategy renderingStrategy;

        public AbstractRender(String id, RenderingStrategy strategy)
            : base(id, null)
        {
            base.Set(Render);
            renderingStrategy = strategy;
        }

        public V Render()
        {
            return Render(_entity);
        }

        public V Render(C entity)
        {
            _entity = entity;
            return renderingStrategy(entity);
        }
    }
}
