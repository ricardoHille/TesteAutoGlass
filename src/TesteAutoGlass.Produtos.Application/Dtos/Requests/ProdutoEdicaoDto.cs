using System.ComponentModel.DataAnnotations;
using TesteAutoGlass.Produtos.Application.Dtos.Requests.Abstraction;

namespace TesteAutoGlass.Produtos.Application.Dtos.Requests
{
    public class ProdutoEdicaoDto : ProdutoDtoBase
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int Id { get; set; }
    }
}
