using System;
using TesteAutoGlass.Fornecedores.Domain.Entities;
using TesteAutoGlass.Utils.Abstractions.Entities.Entities;

namespace TesteAutoGlass.Produtos.Domain.Entities
{
    public class Produto : EntityBase
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public DateTime Fabricacao { get; set; }
        public DateTime Validade { get; set; }
        public int FornecedorId { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }

        public void Inativar() =>
            Ativo = false;

        public bool DataFabricacaoValida() =>
            Fabricacao < Validade;
    }
}
