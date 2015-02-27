using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedRef.Domain.IoC
{
    public interface INamed
    {
        string Name { get; set; }
    }
}
