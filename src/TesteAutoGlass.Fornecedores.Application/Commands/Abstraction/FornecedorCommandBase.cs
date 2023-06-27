using TesteAutoGlass.Utils.Abstractions.Command.Commands;

namespace TesteAutoGlass.Fornecedores.Application.Commands.Abstraction
{
    public abstract class FornecedorCommandBase : CommandBase
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
    }
}
