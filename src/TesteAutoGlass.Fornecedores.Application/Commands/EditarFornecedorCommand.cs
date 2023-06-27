using TesteAutoGlass.Fornecedores.Application.Commands.Abstraction;
using TesteAutoGlass.Utils.Abstractions.Command.Commands;

namespace TesteAutoGlass.Fornecedores.Application.Commands
{
    public class EditarFornecedorCommand : FornecedorCommandBase
    {
        public int Id { get; set; } 
    }
}
