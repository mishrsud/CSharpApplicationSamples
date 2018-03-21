using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Sample.BusinessLogic.Test
{
    public class JobExecutorShould
    {
        [Test]
        public void ReturnExpectedMessage_WhenCalled()
        {
            IFeatureFlagProvider flagProvider = new DefaultFeatureFlagProvider();
            IOutputWriter outputWriter = new DefaultOutputWriter();
            var sut = new JobExecutor(flagProvider, outputWriter);

            var message = sut.Execute();

            if (flagProvider.Enabled("test-feature"))
            {
                Assert.That(message == "Feature on", Is.True);
            }
            else
            {
                Assert.That(message == "Feature off", Is.True);
            }
        }
    }
}
