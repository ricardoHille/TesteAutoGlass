using TesteAutoGlass.Produtos.Application.Commands.Abstraction;

namespace TesteAutoGlass.Produtos.Application.Commands
{
    public class EditarProdutoCommand : ProdutoCommandBase
    {
        public int Id { get; set; }
    }
}
