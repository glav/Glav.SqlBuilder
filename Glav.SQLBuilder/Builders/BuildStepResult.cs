using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.SQLBuilder.Builders
{
    public class BuildStepResult
    {
        public BuildStepResult(bool wasSuccessfull, string detail, bool canContinue)
        {
            Successful = wasSuccessfull;
            ResultDetail = detail;
            CanContinue = canContinue;
        }

        public BuildStepResult(bool wasSuccessfull, string detail) : this(wasSuccessfull, detail, wasSuccessfull) { }

        public bool Successful { get; set; }
        public string ResultDetail { get; set; }

        /// <summary>
        /// A build step might not have been successful,but it might not necessarily mean
        /// that everything else has to stop.It might be fine to continue to do things.
        /// </summary>
        public bool CanContinue { get; set; }
    }
}
