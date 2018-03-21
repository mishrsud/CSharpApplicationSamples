using System.Collections.Generic;

namespace Sample.BusinessLogic
{
    public class DefaultFeatureFlagProvider : IFeatureFlagProvider
    {
        private readonly Dictionary<string, bool> _featureFlagStore;

        public DefaultFeatureFlagProvider()
        {
            _featureFlagStore = new Dictionary<string, bool>
            {
                { "test-feature", true }
            };
        }

        public bool Enabled(string featureName)
        {
            if (_featureFlagStore.ContainsKey(featureName))
            {
                return _featureFlagStore[featureName];
            }

            return false;
        }
    }
}