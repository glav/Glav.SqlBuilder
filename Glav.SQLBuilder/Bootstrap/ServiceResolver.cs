using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.SQLBuilder.Logging;
using Glav.SQLBuilder.Configuration;
using Glav.SQLBuilder.SQLManagement;
using Glav.SQLBuilder.Helpers;
using Autofac;
using Glav.SQLBuilder.Builders;

namespace Glav.SQLBuilder.Bootstrap
{
    public class ServiceResolver : IServiceResolver
    {
        Autofac.IContainer _container;

        public ServiceResolver()
        {
            Initialise();
        }
        private void Initialise()
        {
            Autofac.ContainerBuilder builder = new Autofac.ContainerBuilder();
            builder.Register(c => new ServiceResolver()).As<IServiceResolver>().SingleInstance();
            builder.Register(c => new Config()).As<IConfig>().SingleInstance();
            builder.Register(c => new Logger()).As<ILogger>().SingleInstance();
            builder.Register(c => new SQLManagement.SqlServer(c.Resolve<IConfig>(), c.Resolve<ILogger>())).As<ISqlServer>().SingleInstance();
            builder.Register(c => new ScriptCollector()).As<IScriptCollector>();
            builder.Register(c => new PreBuildScriptStep(c.Resolve<IConfig>(), c.Resolve<ILogger>(), c.Resolve<ISqlServer>()));
            builder.Register(c => new PostBuildScriptStep(c.Resolve<IConfig>(), c.Resolve<ILogger>(), c.Resolve<ISqlServer>()));
            builder.Register(c => new DBCreationBuildStep(c.Resolve<ILogger>(),c.Resolve<IConfig>() , c.Resolve<ISqlServer>()));
            builder.Register(c => new VersionTableCheckBuildStep(c.Resolve<ILogger>(), c.Resolve<IConfig>(), c.Resolve<ISqlServer>()));
            builder.Register(c => new SchemaScriptsExecutionStep(c.Resolve<ILogger>(), c.Resolve<IConfig>(), c.Resolve<ISqlServer>(),c.Resolve<IScriptCollector>()));
            builder.Register(c => new DataScriptsExecutionStep(c.Resolve<ILogger>(), c.Resolve<IConfig>(), c.Resolve<ISqlServer>(), c.Resolve<IScriptCollector>()));

            _container = builder.Build();
        }

        public T Get<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
