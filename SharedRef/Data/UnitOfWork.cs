using System;
using System.Data;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Context;
using System.Linq;
using NLog;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SharedRef.Data
{
    /// <summary>
    /// Базовый класс для всех единиц работы с БД
    /// </summary>
    public class UnitOfWork : EmptyInterceptor, IDisposable
    {
        protected static Logger _log = LogManager.GetCurrentClassLogger();

        public readonly ISession _session;
        protected ITransaction _transaction;
        protected ISessionFactory _sessionFactory;

        protected Stopwatch _stopwatch;

        public UnitOfWork(ISessionFactory sessionFactory, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            _sessionFactory = sessionFactory;
            _session = _sessionFactory.OpenSession(this);

            if (_session == null)
                throw new ArgumentNullException("session");

            _log.Trace("Присоединение сессии");
            CurrentSessionContext.Bind(_session);
            _transaction = _session.BeginTransaction(isolationLevel);
        }

        public UnitOfWork(ISession session)
        {
            _session = session;
            _log.Trace("Присоединение сессии");
            CurrentSessionContext.Bind(_session);
            _transaction = session.Transaction;
        }

        public TimeSpan Duration { get { return _stopwatch.Elapsed; } }

        public static UnitOfWork Current()
        {
            ISession session = DataHelper.GetSession();

            return new UnitOfWork(session);
        }
        
        #region Сессия

        public IQueryable<T> GetAll<T>() where T : DataObject
        {
            return _session.Query<T>();
        }

        public IQuery GetNamedQuery(string name)
        {
            return _session.GetNamedQuery(name);
        }

        public ISQLQuery CreateSqlQuery(string name)
        {
            return _session.CreateSQLQuery(_session.GetNamedQuery(name).QueryString);
        }

        public void Save(DataObject obj)
        {
            _session.SaveOrUpdate(obj);
        }

        internal void SaveAll(IEnumerable<DataObject> items)
        {
            items.ForEach(i => Save(i));
        }

        public void Commit()
        {
            _log.Trace("Закрываем транзакцию");
            _transaction.Commit();

        }

        #endregion

        #region Окончание жизненного цикла

        public void Dispose()
        {
            try
            {
                if (!_transaction.WasCommitted && !_transaction.WasRolledBack)
                {

                    _transaction.Rollback();
                }
                _transaction.Dispose();
                _transaction = null;
            }
            catch (Exception ex)
            {
                _log.ErrorException("Ошибка", ex);
            }

            CurrentSessionContext.Unbind(_session.SessionFactory);
            _session.Dispose();

            _stopwatch.Stop();
            _log.Trace("Завершение единицы работы. Время выполнения: {0:d\\.hh\\:mm\\:ss}", Duration);
        }

        #endregion
    }
}