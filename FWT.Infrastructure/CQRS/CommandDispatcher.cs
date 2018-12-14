using System.Threading.Tasks;
using Autofac;
using FluentValidation;
using FWTL.Core.CQRS;
using FWTL.Infrastructure.Validation;

namespace FWTL.Infrastructure.CQRS
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IComponentContext _context;

        public CommandDispatcher(IComponentContext context)
        {
            _context = context;
        }

        public async Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand
        {
            AppAbstractValidation<TCommand> validator;
            if (_context.TryResolve(out validator))
            {
                var validationResult = validator.Validate(command);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }

            var handler = _context.Resolve<ICommandHandler<TCommand, TResult>>();
            var result = await handler.ExecuteAsync(command).ConfigureAwait(false);
            return result;
        }

        public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            AppAbstractValidation<TCommand> validator;
            if (_context.TryResolve(out validator))
            {
                var validationResult = validator.Validate(command);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }

            var handler = _context.Resolve<ICommandHandler<TCommand>>();
            await handler.ExecuteAsync(command).ConfigureAwait(false);
        }
    }
}
