using NUnit.Framework;

namespace Sample.BusinessLogic.Test
{
    public class JobExecutorShouldv2
    {
        [Test]
        [IgnoreIf("test-feature", true)]
        public void ReturnExpectedMessage_WhenFlagOff()
        {
            IFeatureFlagProvider flagProvider = new DefaultFeatureFlagProvider();
            IOutputWriter outputWriter = new DefaultOutputWriter();
            var sut = new JobExecutor(flagProvider, outputWriter);

            var message = sut.Execute();

            Assert.That(message == "Feature off", Is.True);
        }

        [Test]
        [IgnoreIf("test-feature", false)]
        public void ReturnExpectedMessage_WhenFlagOn()
        {
            IFeatureFlagProvider flagProvider = new DefaultFeatureFlagProvider();
            IOutputWriter outputWriter = new DefaultOutputWriter();
            var sut = new JobExecutor(flagProvider, outputWriter);

            var message = sut.Execute();

            Assert.That(message == "Feature on", Is.True);
        }
    }
}
