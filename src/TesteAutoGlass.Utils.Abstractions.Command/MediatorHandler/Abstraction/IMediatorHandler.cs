using FluentValidation.Results;
using System.Threading.Tasks;
using TesteAutoGlass.Utils.Abstractions.Command.Commands;

namespace TesteAutoGlass.Utils.Abstractions.Command.MediatorHandler.Abstraction
{
    public interface IMediatorHandler
    {
        Task<ValidationResult> SendCommandAsync<T>(T command)
            where T : CommandBase;
    }
}
