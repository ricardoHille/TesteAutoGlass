using System.Diagnostics.CodeAnalysis;

namespace TesteAutoGlass.Utils.Abstractions.Pagination.Options
{
    [ExcludeFromCodeCoverage]
    public abstract class PageOptionsDto
    {
        public int Size { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public SortOptionsDto SortOptions { get; set; }

        public bool isValid() =>
            Size > 0 && Page > 0 && (SortOptions == null || SortOptions.IsValid());
    }
}
