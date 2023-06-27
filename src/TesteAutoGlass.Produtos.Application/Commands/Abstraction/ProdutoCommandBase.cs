using System;
using TesteAutoGlass.Utils.Abstractions.Command.Commands;

namespace TesteAutoGlass.Produtos.Application.Commands.Abstraction
{
    public abstract class ProdutoCommandBase : CommandBase
    {
        public string Nome { get; set; }
        public DateTime Fabricacao { get; set; }
        public DateTime Validade { get; set; }
        public int FornecedorId { get; set; }   
    }
}
