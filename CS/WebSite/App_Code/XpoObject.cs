using System;
using System.Collections.Generic;
using System.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;

namespace PersistentObjects {
    [Persistent("Arts")]
    public class ArtsEntity : XPCustomObject {
        public ArtsEntity(Session session) : base(session) { }
        DateTime lastWriteTime;
        string name;
        int id;
        int pid;
        bool isFolder;

        public DateTime LastWriteTime {
            get { return lastWriteTime; }
            set { SetPropertyValue("LastWriteTime", ref lastWriteTime, value); }
        }
        public string Name {
            get { return name; }
            set { SetPropertyValue("Name", ref name, value); }
        }
        [Persistent("ID"), Key(AutoGenerate = true)]
        public int Id {
            get { return id; }
            set { SetPropertyValue("Id", ref id, value); }
        }
        [Persistent("ParentID")]
        public int Pid {
            get { return pid; }
            set { SetPropertyValue("Pid", ref pid, value); }
        }
        public bool IsFolder {
            get { return isFolder; }
            set { SetPropertyValue("IsFolder", ref isFolder, value); }
        }
        [Delayed(true)]
        public byte[] Data {
            get { return GetDelayedPropertyValue<byte[]>("Data"); }
            set { SetDelayedPropertyValue<byte[]>("Data", value); }
        }
    }
}
