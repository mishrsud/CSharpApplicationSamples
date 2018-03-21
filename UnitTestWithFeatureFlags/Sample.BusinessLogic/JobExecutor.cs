using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.BusinessLogic
{
    public class JobExecutor
    {
        private readonly IFeatureFlagProvider _featureFlagProvider;
        private readonly IOutputWriter _outputWriter;

        public JobExecutor(IFeatureFlagProvider featureFlagProvider, IOutputWriter outputWriter)
        {
            _featureFlagProvider = featureFlagProvider;
            _outputWriter = outputWriter;
        }

        public string Execute()
        {
            if (_featureFlagProvider.Enabled("test-feature"))
            {
                return "Feature on";
                
            }

            return "Feature off";
        }
    }
}
