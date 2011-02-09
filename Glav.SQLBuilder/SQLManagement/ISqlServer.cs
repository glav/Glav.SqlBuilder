using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace Glav.SQLBuilder.SQLManagement
{
    public interface ISqlServer
    {
        Server DatabaseServer { get; }
        bool EnsureConnection();
    }
}
