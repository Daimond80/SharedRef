using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AopAlliance.Intercept;
using NLog;

namespace SharedRef.AOP
{
    /// <summary>
    /// AOP-обертка для логгирования исключений
    /// </summary>
    class ExceptionInterceptor : IMethodInterceptor
    {
        static Logger _log = LogManager.GetCurrentClassLogger();

        public object Invoke(IMethodInvocation invocation)
        {
            try
            {
                return invocation.Proceed();
            }
            catch (Exception ex)
            {
                _log.ErrorException(invocation.Method.Name, ex);
                throw new ApplicationException(invocation.Method.Name, ex);
            }
            
        }
    }
}
