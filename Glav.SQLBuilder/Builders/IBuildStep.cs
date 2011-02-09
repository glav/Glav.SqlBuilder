using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Glav.SQLBuilder.Builders
{
    public interface IBuildStep
    {
        BuildStepResult ExecuteBuildStep();
        string Name { get;  }
    }
}
