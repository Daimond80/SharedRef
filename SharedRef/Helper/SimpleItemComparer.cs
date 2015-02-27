using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedRef.Domain;
using SharedRef.Extensions.StringParser;

namespace SharedRef.Helper
{
    class SimpleItemComparer : IComparer<SimpleItem>
    {
        public SimpleItemComparer(string p)
        {
            _sortParams = ExractParameters(p);
        }

        Dictionary<string, bool> _sortParams;

        public static Dictionary<string, bool> ExractParameters(string sort)
        {
            var sortFields = sort.ParseCommaSeparated();

            Dictionary<string, bool> sortParams = new Dictionary<string, bool>();

            foreach (var sField in sortFields)
            {
                var parts = sField.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Count() > 2)
                    Assert.Fail("Ошибка в параметрах сортировки: {0}", sField);

                var sortOrder = (parts.Count() > 1) ? parts[1].ToLower() : "asc";

                Assert.True(sortOrder.ToLower() == "asc" || sortOrder.ToLower() == "desc", 
                    "Неверно указан порядо сортировки: {0}", sortOrder);

                sortParams.Add(parts[0], sortOrder == "asc");
            }

            return sortParams;
        }

        public int Compare(SimpleItem x, SimpleItem y)
        {
            foreach (var sParam in _sortParams)
            {
                int r = String.Compare(x.Values[sParam.Key].ToString(), y.Values[sParam.Key].ToString());

                if (sParam.Value == false)
                    r = -1 * r;

                if (r != 0)
                    return r;
            }

            return 0;
        }
    }
}
