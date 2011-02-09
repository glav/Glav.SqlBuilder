using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Glav.SQLBuilder.Logging;
using Glav.SQLBuilder.Builders;
using Glav.SQLBuilder.Helpers;

namespace Glav.SQLBuilder.Bootstrap
{
    public class AppStart
    {
        IServiceResolver _resolver;
        ILogger _logger;
        List<IBuildStep> _buildSteps = new List<IBuildStep>();

        public AppStart(IServiceResolver resolver)
        {
            _resolver = resolver;
            _logger = resolver.Get<ILogger>();
            ConstructBuildSteps();
        }

        public bool PerformBuild()
        {
            _logger.LogMessage(">> Establishing Transaction Scope");
            bool buildStepsCompletedOK = true;

                try
                {
                    foreach (var step in _buildSteps)
                    {
                        _logger.LogMessage(">> Starting build step '{0}'", step.Name);
                        var result = step.ExecuteBuildStep();
                        if (result != null && result.Successful)
                        {
                            _logger.LogMessage(">> Completed build step '{0}' successfully.", step.Name);
                        }
                        else
                        {
                            _logger.LogMessage(">>>! Build step '{0}' was not successfull. Reason: '{1}'.", step.Name, result != null ? result.ResultDetail : string.Empty);
                            if (result != null && result.CanContinue)
                            {
                                _logger.LogMessage(">>>! Build step '{0}' was not successfull but execution can continue.", step.Name);
                            }
                            else
                            {
                                buildStepsCompletedOK = false;
                                break;
                            }
                        }
                    }

                    if (buildStepsCompletedOK)
                    {
                        _logger.LogMessage(">> Build steps completed successfully. Completing Transaction.");
                    }
                    else
                    {
                        _logger.LogMessage(">>! Build steps DID NOT complete successfully. Transaction disposed/not commpleted.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogMessage(">>! There was a major exception thrown.[{0}] [{1}]", ex.GetFullExceptionMessages(), ex.StackTrace);
                    buildStepsCompletedOK = false;
                }

            return buildStepsCompletedOK;
        }

        private void ConstructBuildSteps()
        {
            // Add the steps in the order they should be executed
            _buildSteps.Add(_resolver.Get<PreBuildScriptStep>());
            
            _buildSteps.Add(_resolver.Get<DBCreationBuildStep>());
            _buildSteps.Add(_resolver.Get<VersionTableCheckBuildStep>());
            _buildSteps.Add(_resolver.Get<SchemaScriptsExecutionStep>());
            _buildSteps.Add(_resolver.Get<DataScriptsExecutionStep>());
            
            _buildSteps.Add(_resolver.Get<PostBuildScriptStep>());
        }
    }
}
