using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using System.IO;
using SharedRef.Helper;
using NLog;


namespace SharedRef.Data
{
    /// <summary>
    /// Управление сессией для базы
    /// </summary>
    public static class DataHelper
    {
        private static readonly object _lockObject = new object();
        private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;

        private static Logger _log = LogManager.GetCurrentClassLogger();

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    lock (_lockObject)
                    {
                        _log.Trace("Создание фабрики");
                        if (_sessionFactory == null)
                            _sessionFactory = Configuration.BuildSessionFactory();
                    }
                }

                return _sessionFactory;
            }
        }

        private static Configuration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    lock (_lockObject)
                    {
                        if (_configuration == null)
                        {
                            _log.Trace("Создание конфигурации");
                            _configuration = new Configuration();
                            _configuration.SetProperty(NHibernate.Cfg.Environment.ConnectionString, AppSettings.ConnetionString);
                            _configuration.Configure(Path.Combine(AppSettings.ConfigsPath, "_Technical", "database.mssql.cfg.xml"));
                            _configuration.AddFile(Path.Combine(AppSettings.ConfigsPath, "_Technical", "SimpleItem.hbm.xml"));
                            _configuration.AddDirectory(new DirectoryInfo(Path.Combine(AppSettings.ConfigsPath, "SQL")));
                        }
                    }
                }

                return _configuration;
            }
        }

        public static ISession GetSession()
        {
            if (CurrentSessionContext.HasBind(SessionFactory))
                return SessionFactory.GetCurrentSession();

            throw new InvalidOperationException("Единица доступа к базе данных не может быть использована, если сессия не присоединена.");
        }
    }
}