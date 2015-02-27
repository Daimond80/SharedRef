using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedRef.Domain.IoC
{
    public class Database
    {
        public string ConnectionString { get; set; }

        public string Provider { get; set; }
    }
}
