using FluentValidation.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TesteAutoGlass.Utils.Abstractions.Command.Commands
{
    [ExcludeFromCodeCoverage]
    public abstract class CommandBase : IRequest<ValidationResult>
    {
        public ValidationResult ValidationResult { get; set; }
    }
}
