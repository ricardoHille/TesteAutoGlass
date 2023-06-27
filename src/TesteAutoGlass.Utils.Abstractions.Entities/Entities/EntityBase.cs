using System.Diagnostics.CodeAnalysis;

namespace TesteAutoGlass.Utils.Abstractions.Entities.Entities
{
    [ExcludeFromCodeCoverage]
    public abstract class EntityBase 
    {
        public int Id { get; set; }
    }
}
