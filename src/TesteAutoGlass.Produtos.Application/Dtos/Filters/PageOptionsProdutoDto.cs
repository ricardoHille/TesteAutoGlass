using System.Linq;
using TesteAutoGlass.Utils.Abstractions.Pagination.Options;

namespace TesteAutoGlass.Produtos.Application.Dtos.Filters
{
    public class PageOptionsProdutoDto : PageOptionsDto
    {
        public ProdutoFiltroDto Filtro { get; set; }

        public PageOptionsProdutoDto()
        {
            if (SortOptions == null)
            {
                SortOptions = new SortOptionsDto
                {
                    Ascending = false,
                    SortColumns = new string[] { "Nome" }
                };
            }
        }
    }
}
