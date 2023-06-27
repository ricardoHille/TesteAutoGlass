using TesteAutoGlass.Utils.Abstractions.Command.Commands;

namespace TesteAutoGlass.Fornecedores.Application.Commands
{
    public class ExcluirFornecedorCommand : CommandBase
    {
        public int Id { get; set; }
    }
}
