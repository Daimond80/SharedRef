using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Caching;
using NLog;
using SharedRef.Helper;
using SharedRef.Data;
using System.Text.RegularExpressions;
using SharedRef.Extensions.StringParser;

namespace SharedRef.Domain.IoC
{
    public class Matrix : INamed
    {
        protected static Logger _log = LogManager.GetCurrentClassLogger();

        public string Name { get; set; }

        public Database Database { get; set; }

        public string Query { get; set; }

        private List<SimpleItem> CacheValues = null;

        public List<SimpleItem> Get(string conditions, string returns)
        {
            if (CacheValues == null)
                Load();

            var cond = conditions.ParseNameValues(';');

            List<SimpleItem> filtered = CacheValues
                .Where(s => s.IsTrue(cond))
                .Select(v => (SimpleItem)v.Clone())
                .ToList();

            if (string.IsNullOrEmpty(returns) == false)
            {
                var col = returns.ParseCommaSeparated();

                foreach (var item in filtered)
                {
                    item.Values.Where(v => col.Contains(v.Key) == false).ToList().ForEach(v => item.Values.Remove(v.Key));
                }

                filtered = filtered.Distinct(new SimpleItemDistinct()).ToList();
            }

            return filtered;
        }

        private void Load()
        {
            _log.Info("Загрузка данных: {0}", Name);

            if (string.IsNullOrEmpty(Query))
                Assert.Fail("Для справочника {0} не заполнен запрос", Name);

            using (var uof = UnitOfWorkFactory.Data())
            {
                var sqlQuery =
                    uof.CreateSqlQuery(Query)
                    .SetResultTransformer(new DictionaryResultTransformer());

                var l = sqlQuery.List<SimpleItem>();

                CacheValues = l.ToList();
            }
        }
    }
}
