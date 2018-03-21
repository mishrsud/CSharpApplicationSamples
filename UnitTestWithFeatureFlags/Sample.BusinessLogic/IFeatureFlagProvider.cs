namespace Sample.BusinessLogic
{
    public interface IFeatureFlagProvider
    {
        bool Enabled(string featureName);
    }
}