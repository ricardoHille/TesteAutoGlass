using FluentValidation.Results;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using TesteAutoGlass.Infraestruture.Data.Context.Abstraction;

namespace TesteAutoGlass.Utils.Abstractions.Command.CommandHandlers
{
    [ExcludeFromCodeCoverage]
    public abstract class CommandHandlerBase
    {
        protected ValidationResult ValidationResult;

        protected CommandHandlerBase() =>
            ValidationResult = new ValidationResult();

        protected void AddError(string message) =>
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));

        protected async Task<ValidationResult> SaveData(IUnitOfWork unitOfWork)
        {
            try
            {
                await unitOfWork.Commit();
            }
            catch (Exception exeption)
            {
                AddError("Não foi possível salvar as operações no banco de dados");
            }   

            return ValidationResult;
        }
    }
}
