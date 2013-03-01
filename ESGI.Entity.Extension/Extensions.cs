using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESGI.Entity.Interfaces;
using ESGI.Entity.Entities;
using ESGI.Entity.Extension;

namespace ESGI.Entity.Extension
{
    public static class Extensions
    {
        public static IEnumerable<IProperty> Where(this IContainer container, Func<IProperty,Boolean> predicate)
        {
            List<IProperty> results = new List<IProperty>();

            foreach (IProperty property in container.Properties())
            {
                if (predicate.Invoke(property))
                {
                    results.Add(property);
                }
            }
            return results;
        }

        public static Nullable<T> TryGetS<T>(this IContainer container, Func<IProperty, Boolean> predicate) where T : struct
        {
            Nullable<T> result = null;

            foreach (IProperty property in container.Properties())
            {
                if (predicate.Invoke(property))
                {
                    if (property.TryGetS<T>().HasValue)
                    {
                        result = property.TryGetS<T>().Value;
                    }
                    break;
                }
            }

            return result;
        }

        public static Nullable<T> TryGetS<T>(this IProperty property) where T : struct
        {
            Nullable<T> result = null;

            if (typeof(T) == property.Type && null != property.Get<Object>())
            {
                result = property.Get<T>();
            }

            return result;
        }

        public static T TryGet<T> (this IContainer container, Func<IProperty, Boolean> predicate) where T: class
        {
           T result = null;

            foreach (IProperty property in container.Properties())
            {
                if (predicate.Invoke(property))
                {
                    result = property.TryGet<T>();
                    break;
                }
            }

            return result;
        }

        public static T TryGet<T>(this IProperty property) where T : class
        {
            T result = null;

            if (typeof(T) == property.Type && null != property.Get<Object>())
            {
                result = property.Get<T>();
            }

            return result;
        }

    }
}
