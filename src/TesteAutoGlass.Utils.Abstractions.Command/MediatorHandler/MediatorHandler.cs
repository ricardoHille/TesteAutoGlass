using FluentValidation.Results;
using MediatR;
using System.Threading.Tasks;
using TesteAutoGlass.Utils.Abstractions.Command.Commands;
using TesteAutoGlass.Utils.Abstractions.Command.MediatorHandler.Abstraction;

namespace TesteAutoGlass.Utils.Abstractions.Command.MediatorHandler
{
    public class MediatorHandler : IMediatorHandler
    {
        private IMediator _mediator;

        public MediatorHandler()
        {

        }

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ValidationResult> SendCommandAsync<T>(T command)
            where T : CommandBase =>
            await _mediator.Send(command);
    }
}
