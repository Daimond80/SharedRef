using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedRef.Data;
using SharedRef.Helper;
using NLog;
using NHibernate;
using NHibernate.Transform;
using System.Collections;
using SharedRef.Extensions.StringParser;

namespace SharedRef.Domain.IoC
{

    public class Reference : INamed
    {
        protected static Logger _log = LogManager.GetCurrentClassLogger();

        public string Name { get; set; }

        public Database Database { get; set; }

        public string QueryNameForList { get; set; }

        public string QueryNameForId { get; set; }

        public SimpleItem GetById(string id)
        {
            if (string.IsNullOrEmpty(QueryNameForId))
                Assert.Fail("Справочник {0} не поддерживает получение данных по Id", Name);

            using (var uof = UnitOfWorkFactory.Data())
            {
                _log.Info("Загрузка данных: {0}", Name);

                var sqlQuery = uof
                    .CreateSqlQuery(QueryNameForId)
                    .SetParameter("Id", id)
                    .SetResultTransformer(new DictionaryResultTransformer());

                return sqlQuery.UniqueResult<SimpleItem>();
            }
        }

        private void SetParameters(string parentId, string startWith, IQuery query)
        {
            if (query.NamedParameters.Contains("StartWith") == false && startWith == null)
                Assert.Fail("В запросе для справочника {0} должен быть параметр StartWith", Name);

            if (query.NamedParameters.Contains("ParentId") == false && string.IsNullOrEmpty(parentId) == false)
                Assert.Fail("В запросе для справочника {0} должен быть параметр ParentId", Name);

            if (query.NamedParameters.Contains("StartWith"))
                query.SetString("StartWith", (startWith ?? "") + "%");

            if (query.NamedParameters.Contains("ParentId"))
                query.SetParameter("ParentId", parentId);
        }

        public List<SimpleItem> GetItems(string parentId, string startWith, int count, string sort)
        {
            if (string.IsNullOrEmpty(QueryNameForList))
                Assert.Fail("Справочник {0} не поддерживает получение списка", Name);

            using (var uof = UnitOfWorkFactory.Data())
            {
                _log.Info("Загрузка данных: {0}", Name);

                var sqlQuery =
                    uof.CreateSqlQuery(QueryNameForList)
                    .SetResultTransformer(new DictionaryResultTransformer());

                if (count > 0)
                    sqlQuery.SetMaxResults(count);

                SetParameters(parentId, startWith, sqlQuery);

                var l = sqlQuery.List<SimpleItem>().ToList();

                if (string.IsNullOrEmpty(sort) == false)
                    l.Sort(new SimpleItemComparer(sort));

                return l;
            }
        }
    }
}
