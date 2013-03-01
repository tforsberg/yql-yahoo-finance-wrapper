using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESGI.Entity.Action.Download;
using ESGI.Entity.Interfaces;


namespace ESGI.Entity.Action.Download
{

    public class DownloadRessourceHandler<T> : IRessourceHandler<T,DownloadInfo>
    {

       private DownloadInfo _info;

       public DownloadInfo UserArgs()
       {
           return _info;
       }

       public T Acquire(Uri ressource, RessourceHandler<T> handler)
       {
           _info = new DownloadInfo()
           {
               Url = ressource
           };

           if (!string.IsNullOrEmpty(_info.Url.AbsolutePath))
           {
               DownloadClient<T> webDL = new DownloadClient<T>();
               byte[] downloadedData = webDL.Download(_info);
               return handler(downloadedData);
           }

           throw new UriFormatException("URL connot be null or empty");
       }

       public T Acquire(Uri ressource, DownloadInfo userArgs, RessourceHandler<T> handler)
       {
           _info = userArgs;

           if (!string.IsNullOrEmpty(_info.Url.AbsolutePath))
           {
               DownloadClient<T> webDL = new DownloadClient<T>();
               byte[] downloadedData = webDL.Download(_info);
               return handler(downloadedData);
           }

           throw new UriFormatException("URL connot be null or empty");
       }


       public void AcquireAsync(Uri ressource, RessourceHandler<T> handler, CallbackHandler<T> callback)
       {
           _info = new DownloadInfo()
           {
               Url = ressource
           };

           if (!string.IsNullOrEmpty(_info.Url.AbsolutePath))
           {
               DownloadClient<T> webDL = new DownloadClient<T>();
               webDL.DownloadAsync(_info, handler, callback);
               return;
           }

           throw new UriFormatException("URL cannot be null or empty");
       }

       public void AcquireAsync(Uri ressource, RessourceHandler<T> handler, CallbackHandler<T> callback, DownloadInfo userArgs)
       {
           _info = userArgs;

           if (!string.IsNullOrEmpty(_info.Url.AbsolutePath))
           {
               DownloadClient<T> webDL = new DownloadClient<T>();
               webDL.DownloadAsync(_info, handler, callback);
               return;
           }

           throw new UriFormatException("URL cannot be null or empty");
       }

    }
}
