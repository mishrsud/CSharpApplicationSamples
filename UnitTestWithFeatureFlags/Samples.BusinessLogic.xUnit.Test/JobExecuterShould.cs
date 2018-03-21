using Sample.BusinessLogic;
using Xunit;

namespace Samples.BusinessLogic.xUnit.Test
{
    public class JobExecuterShould
    {
        [FactSkipIf(flagName: "test-feature", skipWhen: true)]
        public void ReturnExpectedMessage_WhenFlagOff()
        {
            IFeatureFlagProvider flagProvider = new DefaultFeatureFlagProvider();
            IOutputWriter outputWriter = new DefaultOutputWriter();
            var sut = new JobExecutor(flagProvider, outputWriter);

            var message = sut.Execute();

            Assert.True(message == "Feature off");
        }

        [FactSkipIf(flagName: "test-feature", skipWhen: false)]
        public void ReturnExpectedMessage_WhenFlagOn()
        {
            IFeatureFlagProvider flagProvider = new DefaultFeatureFlagProvider();
            IOutputWriter outputWriter = new DefaultOutputWriter();
            var sut = new JobExecutor(flagProvider, outputWriter);

            var message = sut.Execute();

            Assert.True(message == "Feature on");
        }
    }
}