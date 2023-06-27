using TesteAutoGlass.Utils.Abstractions.Command.Commands;

namespace TesteAutoGlass.Produtos.Application.Commands
{
    public class ExcluirProdutoCommand : CommandBase
    {
        public int Id { get; set; }
    }
}
