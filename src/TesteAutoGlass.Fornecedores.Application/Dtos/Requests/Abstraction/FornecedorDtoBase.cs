using System.ComponentModel.DataAnnotations;

namespace TesteAutoGlass.Fornecedores.Application.Dtos.Requests.Abstraction
{
    public abstract class FornecedorDtoBase
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(14, ErrorMessage = "O campo {0} deve conter entre {2} caracteres.", MinimumLength = 14)]
        public string Cnpj { get; set; }
    }
}
