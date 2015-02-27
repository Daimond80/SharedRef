using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using System.Xml.Serialization;

namespace SharedRef.Data
{
    [Serializable]
    public class DataObject
    {

        protected static Logger _log = LogManager.GetCurrentClassLogger();

        [NonSerialized]
        private DataUnitOfWork _uof;

        public virtual DataUnitOfWork Uof 
        {
            get { return _uof ?? DataUnitOfWork.Current(); }
            set { _uof = value; } 
        }

        public virtual void Save()
        {
            Uof.Save(this);
        }
    }
}
