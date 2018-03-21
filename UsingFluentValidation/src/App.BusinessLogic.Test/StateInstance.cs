using System;
using Xunit;

namespace App.BusinessLogic.Test
{
    public class StateInstance
    {
        [Fact]
        public void ShouldThrowException_WhenStateIsInvalid()
        {
            var state = new State();
            state.WithHost("test");

            var exception = Record.Exception(() => state.Build());

            Assert.True(exception != null);
            Assert.IsAssignableFrom<ApplicationException>(exception);
        }

        [Fact]
        public void ShouldSetValidState_WhenAllPropertiesAreSet()
        {
            var state = new State();
            state.WithHost("localhost");
            state.WithName("my-node");

            var exception = Record.Exception(() => state.Build());

            Assert.True(exception == null);
        }
    }
}
