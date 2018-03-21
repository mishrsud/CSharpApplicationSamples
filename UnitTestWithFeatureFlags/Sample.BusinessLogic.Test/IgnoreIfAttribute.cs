using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Sample.BusinessLogic.Test
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class IgnoreIfAttribute : NUnitAttribute, IApplyToTest
    {
        private readonly string _featureName;
        private readonly bool _status;
        private IFeatureFlagProvider _featureFlagProvider;

        public IgnoreIfAttribute(string featureName, bool status)
        {
            _featureName = featureName;
            _status = status;
            _featureFlagProvider = new DefaultFeatureFlagProvider();
        }

        public void ApplyToTest(NUnit.Framework.Internal.Test test)
        {
            if (_featureFlagProvider.Enabled(_featureName) == _status)
            {
                test.RunState = RunState.Ignored;
            }
        }
    }
}
