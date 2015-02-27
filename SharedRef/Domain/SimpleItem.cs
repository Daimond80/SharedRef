using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedRef.Data;
using System.Collections;
using SharedRef.Helper;
using SharedRef.Extensions.StringParser;
using SharedRef.Extensions.ObjectHelper;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SharedRef.Domain
{
    [Serializable]
    public class SimpleItem : DataObject, ICloneable
    {
        public SimpleItem ()
	    {
            Values  = new Dictionary<string, object>();
	    }

        public virtual Dictionary<string, object> Values { get; set; }

        public virtual object this[string index]
        {
            get { return Values.ContainsKey(index) ? Values[index] : null; }
            set { Values.Add(index, value); }
        }

        public virtual string Id { get; set; }

        public virtual bool IsTrue(Dictionary<string, string> conditional)
        {
            foreach (var cond in conditional)
            {
                Assert.True(Values.ContainsKey(cond.Key), "Справочник не содержит параметра {0}", cond.Key);

                var func = cond.Value.ParseFunction();

                var values = func.FirstOrDefault()
                    .If(f => f == "IN")
                    .Return(
                        f => new List<string>(func.GetRange(1, func.Count() - 1)), 
                        new List<string>() { cond.Value });

                if (values.Contains(Values[cond.Key].ToString()) == false)
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            return String.Join(string.Empty, Values.Select(x => x.Key + "=" + x.Value + "; "));
        }

        public virtual object Clone()
        {
            BinaryFormatter BF = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            BF.Serialize(memStream, this);
            memStream.Position = 0;

            return (BF.Deserialize(memStream));
        }
    }
}
