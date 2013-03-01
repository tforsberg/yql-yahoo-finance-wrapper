using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESGI.Entity.Interfaces
{
    public delegate V RessourceHandler<V>(byte[] dataAcquired);
    public delegate void CallbackHandler<V>(V dataAcquired);

    public interface IRessourceHandler<T,Y>
    {
        Y UserArgs();
        T Acquire(Uri ressource, RessourceHandler<T> handler);
        T Acquire(Uri ressource, Y userArgs, RessourceHandler<T> handler);
        void AcquireAsync(Uri ressource, RessourceHandler<T> handler, CallbackHandler<T> callback);
        void AcquireAsync(Uri ressource, RessourceHandler<T> handler, CallbackHandler<T> callback, Y userArgs);
    }
}
