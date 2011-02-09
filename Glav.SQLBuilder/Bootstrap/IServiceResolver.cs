using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.SQLBuilder.Bootstrap
{
    public interface IServiceResolver
    {
        T Get<T>();
    }
}
