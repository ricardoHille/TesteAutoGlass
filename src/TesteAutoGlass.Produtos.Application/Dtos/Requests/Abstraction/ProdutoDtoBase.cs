using System;
using System.ComponentModel.DataAnnotations;

namespace TesteAutoGlass.Produtos.Application.Dtos.Requests.Abstraction
{
    public abstract class ProdutoDtoBase
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime Fabricacao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime Validade { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(1, 9999, ErrorMessage = "O campo não pode vir vazio.")]
        public int FornecedorId { get; set; }
    }
}
