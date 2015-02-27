using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Context;
using Common;


namespace SharedRef.Data
{
    /// <summary>
    /// Единица работы, хранящая сессию и позволяющая управлять бизнес-объектами базы
    /// </summary>
    public class DataUnitOfWork : UnitOfWork
    {
        public DataUnitOfWork(ISessionFactory sessionFactory, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            : base(sessionFactory, isolationLevel)
        {
            
        }

        public DataUnitOfWork(ISession session)
            : base(session)
        {

        }

        public static new DataUnitOfWork Current()
        {
            ISession session = DataHelper.GetSession();

            return new DataUnitOfWork(session) { _transaction = session.Transaction };
        }
        
        #region Модель

        internal void Clear()
        {
            _sessionFactory.EvictQueries();
            foreach (var collectionMetadata in _sessionFactory.GetAllCollectionMetadata())
                _sessionFactory.EvictCollection(collectionMetadata.Key);
            foreach (var classMetadata in _sessionFactory.GetAllClassMetadata())
                _sessionFactory.EvictEntity(classMetadata.Key);
        }

        #endregion

        #region EmptyInterceptor

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            var result = base.OnSave(entity, id, state, propertyNames, types);
            ((DataObject)entity).Uof = this;
            return result;
        }

        public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            ((DataObject)entity).Uof = this;
            return base.OnLoad(entity, id, state, propertyNames, types);
        }

        #endregion
    }
}