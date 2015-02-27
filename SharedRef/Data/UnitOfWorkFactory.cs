using System.Data;
using NLog;

namespace SharedRef.Data
{
    /// <summary>
    /// Фабрика для получения единиц работы
    /// </summary>
    static public class UnitOfWorkFactory
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        static public DataUnitOfWork Data(IsolationLevel isolationLevel)
        {
            _log.Trace("Создание ед. работы");
            return new DataUnitOfWork(DataHelper.SessionFactory, isolationLevel);
        }

        static public DataUnitOfWork Data()
        {
            return Data(IsolationLevel.ReadCommitted);
        }
    }
}