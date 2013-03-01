using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using ESGI.Entity.Interfaces;

namespace ESGI.Entity.Action.Download
{
    public class DownloadClient<T> 
    {
        DownloadInfo _info;
        RessourceHandler<T> _handler;
        CallbackHandler<T> _callback;

        public byte[] Download(DownloadInfo info)
        {
            _info = info;
           return new WebClient().DownloadData(_info.Url);
        }

        public void DownloadAsync(DownloadInfo info, RessourceHandler<T> handler, CallbackHandler<T> callback)
        {
            _info = info;
            _handler = handler;
            _callback = callback;

            WebClient c = new WebClient();
            c.DownloadDataCompleted += HandleDownloadCompleted;
            c.DownloadDataAsync(_info.Url);
        }

        private void HandleDownloadCompleted(object s, DownloadDataCompletedEventArgs  e)
        {
            _callback.Invoke( _handler.Invoke(e.Result));
        }

    }
}
