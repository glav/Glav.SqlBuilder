using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.SQLBuilder.Configuration
{
    public interface IConfig
    {
        string ServerName { get;  }
        string Username { get;  }
        string Password { get;  }
        string ScriptDirectory { get; }
        string PostScriptToRun { get;  }
        string PreScriptToRun { get;  }
        string DatabaseName { get;  }
        string VersionTableName { get;  }
    }
}
