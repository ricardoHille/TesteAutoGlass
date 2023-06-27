using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TesteAutoGlass.Utils.Abstractions.Pagination.Results.Abstraction;

namespace TesteAutoGlass.Utils.Abstractions.Pagination.Results
{
    [ExcludeFromCodeCoverage]
    public class PagedResult<T> : PagedResultBase
        where T : class
    {
        public IEnumerable<T> Results { get; set; } = new List<T>();
    }
}
