using TesteAutoGlass.Utils.Abstractions.Entities.Entities;
using TesteAutoGlass.Utils.Validators.TextValidator;

namespace TesteAutoGlass.Fornecedores.Domain.Entities
{
    public class Fornecedor : EntityBase
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public bool Ativo { get; set; }

        public bool CnpjValido()
        {
            return CnpjValidator.CnpjValido(Cnpj);
        }

        public void Inativar() =>
            Ativo = false;
    }
}
