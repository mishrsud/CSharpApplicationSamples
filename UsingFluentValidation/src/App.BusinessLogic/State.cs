using System;
using System.Linq;
using FluentValidation;

namespace App.BusinessLogic
{
    public class State
    {
        private string _name;
        private string _hostName;

        public State WithName(string name)
        {
            _name = name;
            return this;
        }

        public State WithHost(string hostName)
        {
            _hostName = hostName;
            return this;
        }

        public void Build()
        {
            var validator = new StateValidator();
            var validationResult = validator.Validate(this);

            if (!validationResult.IsValid)
            {
                var data = validationResult.Errors.Select(failure => failure.ToString());
                var errorMessages = string.Join(System.Environment.NewLine, data);
                throw new ApplicationException(errorMessages);
            }
        }

        private class StateValidator : AbstractValidator<State>
        {
            internal StateValidator()
            {
                RuleFor(state => state._name).NotNull().NotEmpty()
                    .WithMessage(state => $"Have you called {nameof(State.WithName)}?");
                RuleFor(state => state._hostName).NotNull().NotEmpty()
                    .WithMessage(state => $"Have you called {nameof(State.WithHost)}?");
            }
        }
    }
}
