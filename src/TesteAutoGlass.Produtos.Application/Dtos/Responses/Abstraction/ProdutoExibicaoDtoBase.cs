namespace TesteAutoGlass.Produtos.Application.Dtos.Responses.Abstraction
{
    public abstract class ProdutoExibicaoDtoBase
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Fabricacao { get; set; }
        public string Validade { get; set; }
        public string Fornecedor { get; set; }
    }
}
