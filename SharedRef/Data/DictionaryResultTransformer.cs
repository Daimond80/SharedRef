using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Transform;
using System.Collections;
using SharedRef.Domain;

namespace SharedRef.Data
{
    [Serializable]
    public class DictionaryResultTransformer : IResultTransformer
    {

        public DictionaryResultTransformer()
        {

        }

        #region IResultTransformer Members

        public IList TransformList(IList collection)
        {
            return collection;
        }

        public object TransformTuple(object[] tuple, string[] aliases)
        {
            var o = new SimpleItem();
            o.Id = tuple[aliases.ToList().IndexOf("Id")].ToString();

            for (int i = 0; i < aliases.Length; i++)
            {
                o[aliases[i]] = tuple[i];
            }

            return o;
        }

        #endregion
    }
}
