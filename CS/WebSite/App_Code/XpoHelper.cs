using System;
using System.Collections.Generic;
using System.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using System.Threading;

public class FileManagerDemoXpoHelper {
    public static Session GetNewSession() {
        return new Session(DataLayer);
    }

    readonly static object _lockObject = new object();
    static object _dataLayer;
    static IDataLayer DataLayer {
        get {
            if(_dataLayer == null) {
                lock(_lockObject) {
                    if(Thread.VolatileRead(ref _dataLayer) == null) {
                        Thread.VolatileWrite(ref _dataLayer, GetDataLayer());
                    }
                }
            }
            return (IDataLayer)_dataLayer;
        }
    }

    static IDataLayer GetDataLayer() {
        XpoDefault.Session = null;
        string conn = AccessConnectionProvider.GetConnectionString(@"|DataDirectory|\Data.mdb");
        XPDictionary dict = new DevExpress.Xpo.Metadata.ReflectionDictionary();
        IDataStore store = XpoDefault.GetConnectionProvider(conn, AutoCreateOption.SchemaAlreadyExists);
        dict.GetDataStoreSchema(typeof(PersistentObjects.ArtsEntity).Assembly);

        IDataLayer dl = new ThreadSafeDataLayer(dict, store);
        return dl;
    }
}
