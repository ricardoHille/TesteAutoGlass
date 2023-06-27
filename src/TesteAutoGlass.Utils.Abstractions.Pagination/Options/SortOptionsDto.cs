using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TesteAutoGlass.Utils.Abstractions.Pagination.Options
{
    [ExcludeFromCodeCoverage]
    public class SortOptionsDto
    {
        public string[] SortColumns { get; set; }
        public bool Ascending { get; set; }

        public bool IsValid() => !SortColumns.Any(x => string.IsNullOrEmpty(x));
    }
}
