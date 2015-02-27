using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AopAlliance.Intercept;
using NLog;
using System.Diagnostics;
using SharedRef.Extensions.ObjectHelper;

namespace SharedRef.AOP
{
    /// <summary>
    /// AOP-обертка для времени вызова
    /// </summary>
    class WatchInterceptor : IMethodInterceptor
    {
        static Logger _log = LogManager.GetCurrentClassLogger();

        public object Invoke(IMethodInvocation invocation)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                return invocation.Proceed();
            }
            finally
            {
                _log.Debug("{1}({2}) Длит.: {0:d\\.hh\\:mm\\:ss}",
                    stopWatch.Elapsed,
                    invocation.Method.Name,
                    String.Join(" | ", invocation.Arguments.Return(arg => arg.Select(a => a), new string[] {})));
            }
            
        }
    }
}
