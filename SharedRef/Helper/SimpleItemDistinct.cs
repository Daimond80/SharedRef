using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedRef.Domain;

namespace SharedRef.Helper
{
    class SimpleItemDistinct : IEqualityComparer<SimpleItem>
    {

        public bool Equals(SimpleItem x, SimpleItem y)
        {
            Assert.IsNotNull(x);
            Assert.IsNotNull(y);

            if (x.Values.Count != y.Values.Count)
                return false;

            return x.Values.ToList().Except(y.Values.ToList()).Count() == 0;
        }

        public int GetHashCode(SimpleItem obj)
        {
            int hash = 0;

            foreach (var pair in obj.Values)
            {
                hash ^= pair.Key.GetHashCode();
                hash ^= pair.Value.GetHashCode();
            }

            return hash;
        }
    }
}
