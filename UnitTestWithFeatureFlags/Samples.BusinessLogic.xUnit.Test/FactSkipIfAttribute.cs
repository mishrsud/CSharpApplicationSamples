using Sample.BusinessLogic;
using Xunit;

namespace Samples.BusinessLogic.xUnit.Test
{
    public class FactSkipIfAttribute : FactAttribute
    {
        private readonly string _flagName;

        private readonly bool _skipWhen;

        private readonly IFeatureFlagProvider _flagProvider;
        
        public FactSkipIfAttribute(string flagName, bool skipWhen)
        {
            _flagName = flagName;
            _skipWhen = skipWhen;
            _flagProvider = new DefaultFeatureFlagProvider();
        }

        public override string Skip
        {
            get
            {
                if (_flagProvider.Enabled(_flagName) == _skipWhen)
                {
                    return $"Skipped as feature flag {_flagName} is configured as {_skipWhen}";
                }

                return "";
            }
            set
            {

            }
        }
    }
}