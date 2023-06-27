namespace TesteAutoGlass.Fornecedores.Application.Dtos.Responses.Abstraction
{
    public abstract class FornecedorExibicaoDtoBase
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
    }
}
