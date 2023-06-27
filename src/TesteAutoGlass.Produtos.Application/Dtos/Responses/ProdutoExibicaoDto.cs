using TesteAutoGlass.Produtos.Application.Dtos.Responses.Abstraction;

namespace TesteAutoGlass.Produtos.Application.Dtos.Responses
{
    public class ProdutoExibicaoDto : ProdutoExibicaoDtoBase
    {
        public int FornecedorId { get; set; }
    }
}
